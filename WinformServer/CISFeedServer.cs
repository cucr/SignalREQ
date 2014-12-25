using System;
using System.Windows.Forms;
using Microsoft.Owin.Hosting;
using Microsoft.AspNet.SignalR.Client;
using System.Configuration;
using System.Text;
using System.Collections.Generic;
using Stimulsoft.Report;
using System.IO;
using System.Diagnostics;
using System.Net;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Drawing;

namespace WinformServer
{
    public delegate void UDPReceivedDelegate(string bib, string clock, string correct, string timepen, string fencepen, string totalpen, string order, string noc, string rider, string horse, string timeallowed);
    public partial class CISFeedServer : Form
    {
        IDisposable MySignalR { get; set; }

        private string localIP = ConfigurationSettings.AppSettings["LocalAddressIp"];
        private string templateAddress = ConfigurationSettings.AppSettings["SummaryTemplateAddress"];
        private string templateSingle = ConfigurationSettings.AppSettings["SingleTemplateName"];
        private string templateTeam = ConfigurationSettings.AppSettings["TeamTemplateName"];
        private string imageSaveAddree = ConfigurationSettings.AppSettings["ImageSaveAddress"];
        private string connectionString = ConfigurationSettings.AppSettings["DBStr"];
        private string MDSAddress = ConfigurationSettings.AppSettings["MDSAddress"];
        private string UDPServerPort = ConfigurationSettings.AppSettings["UDPServerPort"];
        private string SCBCommand = ConfigurationSettings.AppSettings["SCBCommand"];

        private readonly HubConnection _hubConnection;
        private readonly IHubProxy _buildingApiHubProxy;

        private TcpClientEx m_tcpClient;
        public static int ClientsCount = 0;

        private StiReport m_stiReportRender;
        Stimulsoft.Report.Export.StiPngExportSettings stiPngSettings;

        //UDP输入
        private NetUdp m_Udp;
        private int m_iUDPBroadcastPort;
        //有效数据帧长度
        private const int MaxFrameLength = 500;
        private Byte[] _frameBuffer = new Byte[MaxFrameLength];
        private int _frameBufferIndex = 0;
        //UDP
        private UDPReceivedDelegate updateClientFromUDP;

        //多线程定时器，定时检测MDS是否断开，并重连
        private System.Threading.Timer Thread_Time;

        //当前选择的比赛
        private int m_iMatchID;
        private string m_strMatchName;
        private System.Data.DataTable dtMatch;
        private DataSet dsPreview;
        private int m_iSelectionIdx;

        public CISFeedServer()
        {
            InitializeComponent();

            // Load Configuration
            ConfigurationManager.LoadConfiguration(System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\CISFeedServer.cfg");

            //如果配置文件localIP为空，使用本地ip
            string strHostName = Dns.GetHostName();  //得到本机的主机名
            IPHostEntry ipEntry = Dns.GetHostByName(strHostName); //取得本机IP
            string strAddr = ipEntry.AddressList[0].ToString(); //假设本地主机为单网卡
            if (localIP.Equals(""))
                localIP = "http://" + strAddr + ":8080/";

            StartServer();

            m_stiReportRender = new StiReport();
            stiPngSettings = new Stimulsoft.Report.Export.StiPngExportSettings();
            stiPngSettings.ImageResolution = 500;

            _hubConnection = new HubConnection(localIP + "signalr");
            _buildingApiHubProxy = _hubConnection.CreateHubProxy("myHub");
            _hubConnection.Start().Wait();

            //连接MDS
            this.txtIP.Text = MDSAddress;
            m_tcpClient = new TcpClientEx();
            m_tcpClient.OnReceiveData += new Action<object, byte[]>(m_tcpClient_OnReceiveData);
            m_tcpClient.OnConnectionChanged += new Action<object, bool>(m_tcpClient_OnConnectionChanged);

            //初始化UDP
            //此UDP只接收，不发送
            m_iUDPBroadcastPort = Helper.Str2Int(UDPServerPort);
            tb_UDPBroadcastPort.Text = UDPServerPort;
            m_Udp = new NetUdp();
            m_Udp.eventHandlerDataRecv += new EventHandler<eventArgsDataRecv>(Udp_eventHandlerDataRecv);
            this.updateClientFromUDP = AsyncUpdateClientFromUDP;

            //终止记时器：Thread_Time.Change(Timeout.Infinite,50);
            //重新启动记时器：Thread_Time.Change(0,50);
            Thread_Time = new System.Threading.Timer(Thread_Timer_Method, null, System.Threading.Timeout.Infinite, 2000);

        }

        private void StartServer()
        {
            try
            {
                //string url = "http://127.0.0.1:8080/";

                MySignalR = WebApp.Start<Startup>(localIP);



                if (this.MySignalR == null)
                    ServerStatus.Text = "web app started failed !";
                else
                    ServerStatus.Text = "web app started! Try to visit " + localIP + "signalr/hubs";

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void m_tcpClient_OnReceiveData(object arg1, byte[] arg2)
        {
            string strData = Encoding.UTF8.GetString(arg2);
            if (strData.Length <= 0)
                return;
            string strXml = strData.Substring(strData.IndexOf("<?xml"));
            List<NotifyMsg> notifyMsgList = MessageParse.ParseMessagePack(strXml);
            if (notifyMsgList != null)
                foreach (NotifyMsg notifyMsg in notifyMsgList)
                {
                    List<string> invokeList = MessageParse.ProcessNotify(notifyMsg);
                    //触发页面刷新
                    if (invokeList != null)
                        foreach (string invokeItem in invokeList)
                        {
                            if (_hubConnection.State == Microsoft.AspNet.SignalR.Client.ConnectionState.Connected)
                            {
                                _buildingApiHubProxy.Invoke("FeedClients", invokeItem);
                            }
                        }

                    int matchID = Convert.ToInt32(notifyMsg.StrMatchID);

                    //触发生成summaryreport的图片
                    if (matchID < 1)
                        return;

                    if (notifyMsg.StrType == "MatchStatus")
                    {
                        MatchStatus matchStatus = (MatchStatus)DataBaseAccess.GetStatusByMatchID(matchID);

                        if (matchStatus == MatchStatus.Official || matchStatus == MatchStatus.Unofficial)
                        {
                            GeneratorSummaryResultImage(matchID);
                        }

                    }


                }
            //if (_hubConnection.State == Microsoft.AspNet.SignalR.Client.ConnectionState.Connected)
            //{
            //    _buildingApiHubProxy.Invoke("FeedClients",
            //              @" {""CISDataType"":""MatchResult"",""ProcName"":""Proc_SCB_TT_GetMatchResult"",""ProcParas"":{""@MatchID"":84,""@LanguageCode"":""ENG""}}");
            //}

        }

        private void GeneratorSummaryResultImage(int matchID)
        {
            int eventID = DataBaseAccess.GetEventIDByMatchID(matchID);
            int phaseID = DataBaseAccess.GetPhaseIDByMatchID(matchID);
            int eventType = DataBaseAccess.GetEventTypeByMatchID(matchID);

            string template = "";
            if (eventType == (int)EventType.Single)
            {
                template = templateSingle;
            }
            else if (eventType == (int)EventType.Team)
            {
                template = templateTeam;
            }

            if (template.Length == 0)
                return;
            string[] templateNames = template.Split(new char[] { ',' });
            string templateName = "";
            foreach (string name in templateNames)
            {
                templateName = templateAddress + name + @".mrt";
                m_stiReportRender.Load(templateName);
                foreach (Stimulsoft.Report.Dictionary.StiDatabase stiDb in m_stiReportRender.Dictionary.Databases)
                {
                    if (stiDb.Name == "ReportDB")
                    {
                        ((Stimulsoft.Report.Dictionary.StiSqlDatabase)stiDb).ConnectionString = connectionString;
                    }
                }
                if (m_stiReportRender.Dictionary.Variables.Contains("EventID"))
                    m_stiReportRender.Dictionary.Variables["EventID"].Value = eventID.ToString();
                if (m_stiReportRender.Dictionary.Variables.Contains("PhaseID"))
                    m_stiReportRender.Dictionary.Variables["PhaseID"].Value = phaseID.ToString();
                m_stiReportRender.Render(false);
                string strRptFullName = imageSaveAddree + Enum.GetName(typeof(Event), eventID) + @"_" + name + @".png";
                m_stiReportRender.ExportDocument(StiExportFormat.ImagePng, strRptFullName, stiPngSettings);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //DataBaseAccess.GetProcData("Proc_CIS_TT_GetSchedule", @" {'@DisciplineID':1,'@LanguageCode':'ENG'}");
            //@" {""@DisciplineID"":1,""@LanguageCode"":""ENG""}"
            if (_hubConnection.State == Microsoft.AspNet.SignalR.Client.ConnectionState.Connected)
            {
                //schedule test
                //_buildingApiHubProxy.Invoke("FeedClients",
                //    @" {""CISDataType"":""Schedule"",""ProcName"":""Proc_CIS_TT_GetSchedule"",""ProcParas"":{""@DisciplineID"":1,""@LanguageCode"":""ENG""}}");
                //matchResult test
                _buildingApiHubProxy.Invoke("FeedClients",
                      @" {""CISDataType"":""MatchResult"",""ProcName"":""Proc_SCB_TT_GetMatchResult"",""ProcParas"":{""@MatchID"":84,""@MatchSplitID"":-1,""@LanguageCode"":""ENG""}}");


            }
        }

        private void CISFeedServer_FormClosed(object sender, FormClosedEventArgs e)
        {
            ConfigurationManager.SaveConfiguration();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                bool bRes = m_tcpClient.Connect(txtIP.Text.Trim(), 5000, 2000);

                if (bRes)
                {
                    btnConnect.Enabled = false;
                }
                else
                {
                    btnConnect.Enabled = true;
                }
            }
            catch (Exception ee)
            {
                Helper.Log.Error(ee.Message);
            }
        }

        #region UDP Server

        private void UnInitUDPServer()
        {
            if (m_Udp != null)
            {
                m_Udp.Close();
            }
        }

        private void chb_UDPServer_CheckedChanged(object sender, EventArgs e)
        {
            if (chb_UDPServer.Checked)
            {
                if (tb_UDPBroadcastPort.Text.Length < 1 || Helper.Str2Int(tb_UDPBroadcastPort.Text) == 0)
                {
                    chb_UDPServer.CheckState = CheckState.Unchecked;
                    return;
                }
                //开启服务
                if (!m_Udp.Open(m_iUDPBroadcastPort))
                {
                    MessageBox.Show(m_Udp.LastErrorMsg);
                    return;
                }
            }
            else
            {
                if (m_Udp != null)
                {
                    m_Udp.Close();
                }
            }
        }

        private void tb_UDPBroadcastPort_TextChanged(object sender, EventArgs e)
        {
            try
            {
                m_iUDPBroadcastPort = System.Convert.ToInt32(this.tb_UDPBroadcastPort.Text);
            }
            catch (System.Exception ex)
            {
                m_iUDPBroadcastPort = 0;
                MessageBox.Show(ex.Message);
            }
        }

        private void Udp_eventHandlerDataRecv(object sender, eventArgsDataRecv e)
        {
            byte[] buffer = e.m_bytes;
            if (buffer == null)
            {
                Debug.Assert(false);
                return;
            }
            try
            {
                //对buffer的数据进行解析
                for (int i = 0; i < buffer.Length; i++)
                {
                    //顺序读取buffer[i]到_frameBuffer
                    if (this._frameBufferIndex < MaxFrameLength)
                    {
                        this._frameBuffer[this._frameBufferIndex] = buffer[i];
                    }
                    this._frameBufferIndex++;
                    //如果发现结束标志位0x0d,则返回_frameBuffer对应的string
                    if (buffer[i] == 0x0d)
                    {
                        string frame = Encoding.UTF8.GetString(this._frameBuffer, 0, this._frameBufferIndex - 1);
                        //使用'|'切割字符串
                        string[] fields = frame.Split(new char[] { '|' });
                        //第3个切割内容为滚动时钟
                        if (fields.Length >= 6)
                        {
                            //触发外部更新
                            if (this.updateClientFromUDP != null)
                                this.updateClientFromUDP(fields[0], fields[1], fields[2], fields[3], fields[4], fields[5], fields[6], fields[7], fields[8], fields[9], fields[10]);
                        }
                        //清除缓存
                        Array.Clear(this._frameBuffer, 0, MaxFrameLength);
                        this._frameBufferIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void AsyncUpdateClientFromUDP(string bib, string clock, string correct, string timepen, string fencepen, string totalpen, string order, string noc, string rider, string horse, string timeallowed)
        {
            this.BeginInvoke(new UDPReceivedDelegate(updateClientFromUDPMethod), new object[] { bib, clock, correct, timepen, fencepen, totalpen, order, noc, rider, horse, timeallowed });
        }

        private void updateClientFromUDPMethod(string bib, string clock, string correct, string timepen, string fencepen, string totalpen, string order, string noc, string rider, string horse, string timeallowed)
        {
            string jsonstr = "{\"bib\" : " + "\"" + bib + "\","
                + "\"clock\" : \"" + clock + "\","
                + "\"correct\" : \"" + correct + "\","
                + "\"timepen\" : \"" + timepen + "\","
                + "\"fencepen\" : \"" + fencepen + "\","
                + "\"totalpen\" : \"" + totalpen + "\","
                + "\"order\" : \"" + order + "\","
                + "\"noc\" : \"" + noc + "\","
                + "\"rider\" : \"" + rider + "\","
                + "\"horse\" : \"" + horse + "\","
                + "\"timeallowed\" : \"" + timeallowed + "\"}";
            lb_UDPData.Text = jsonstr;
            _buildingApiHubProxy.Invoke("FeedClientsFromUDP", jsonstr);

        }

        #endregion

        private void CISFeedServer_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Are you sure to exit the window？", "Eixt", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                // 关闭所有的线
                this.Dispose();
                this.Close();

            }
        }

        private void m_tcpClient_OnConnectionChanged(object ob, bool bConnected)
        {
            if (bConnected)
            {
                //this.Invoke(new Action(() => {btnConnect.Enabled = false;timer1.Enabled = false;}));
                this.Invoke(new Action(() => { btnConnect.Enabled = false; Thread_Time.Change(System.Threading.Timeout.Infinite, 2000); }));

            }
            else
            {
                //this.Invoke(new Action(() => { btnConnect.Enabled = true; timer1.Enabled = true; }));
                this.Invoke(new Action(() => { btnConnect.Enabled = true; Thread_Time.Change(0, 2000); }));
            }

        }

        private void Thread_Timer_Method(object o)
        {
            if (!m_tcpClient.Connected)
            {
                this.Invoke(new Action(() => { btnConnect.Enabled = true; }));
                //btnConnect.Enabled = true;

                bool bRes = m_tcpClient.Connect(txtIP.Text.Trim(), 5000, 2000);

                if (bRes)
                {
                    this.Invoke(new Action(() => { btnConnect.Enabled = false; }));
                    //btnConnect.Enabled = false;
                    //timer1.Enabled = false;
                }
            }
            //Console.WriteLine("{0 :T}:tick", System.DateTime.Now);    // 11:43:34
            //System.Threading.Thread.Sleep(3000);
        }

        private void cmb_Match_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_iMatchID = Helper.Str2Int(cmb_Match.SelectedValue.ToString());
            if (m_iMatchID != -1)
            {
                m_strMatchName = cmb_Match.Text;
                InitCommandButton();
            }
        }

        private void InitCommandButton()
        {
            this.flp_Command.Controls.Clear();
            String[] Commands = SCBCommand.Split(new char[] { ',' });
            Int32 iBtnWidth = 100;
            Int32 iBtnHeight = 50;
            foreach (String Command in Commands)
            {
                Button btnCommand = new Button();
                btnCommand.Visible = true;
                btnCommand.Name = String.Format("btnCommand{0}", Command);
                btnCommand.Text = Command;
                btnCommand.Font = new System.Drawing.Font("SimSun", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                btnCommand.Size = new System.Drawing.Size(iBtnWidth, iBtnHeight);
                btnCommand.Click += new EventHandler(this.btn_Command_Click);
                btnCommand.Tag = Command;

                this.flp_Command.Controls.Add(btnCommand);
            }

        }

        private void btn_Command_Click(object sender, EventArgs e)
        {
            Button btnCommand = (Button)sender;
            String Command = (String)btnCommand.Tag;
            //更新浏览器数据
            //String SCBStr = DataBaseAccess.GetProcData2JsonStr("Proc_CIS_EQ_GetSCB", "{\"@MatchID\":" + m_iMatchID.ToString() + ",\"@SCBType\":\"" + Command + "\"}");
            //_buildingApiHubProxy.Invoke("FeedClientsSCB", SCBStr);
            //预览数据
            dsPreview = DataBaseAccess.GetProcData2DataSet("Proc_CIS_EQ_GetSCB", "{\"@MatchID\":" + m_iMatchID.ToString() + ",\"@SCBType\":\"" + Command + "\"}");
            dgv_Preview1.DataSource = dsPreview.Tables[0];
            dgv_Preview2.DataSource = dsPreview.Tables[1];
            //设置dgv样式
            Helper.SetDataGridViewStyle(dgv_Preview1);
            Helper.SetDataGridViewStyle(dgv_Preview2);

        }

        private void cmb_Match_MouseClick(object sender, MouseEventArgs e)
        {
            //比赛列表显示
            this.cmb_Match.SelectedIndexChanged -= new System.EventHandler(this.cmb_Match_SelectedIndexChanged);
            InitCmbMatch();
            this.cmb_Match.SelectedIndexChanged += new System.EventHandler(this.cmb_Match_SelectedIndexChanged);
        }

        public void InitCmbMatch()
        {
            dtMatch = DataBaseAccess.GetMatchList();
            cmb_Match.DataSource = dtMatch;
            cmb_Match.DisplayMember = "F_MatchLongName";
            cmb_Match.ValueMember = "F_MatchID";
        }

        //处理按键
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            //如果是场地障碍，对上下键做特殊处理
            if (tabControl1.SelectedTab.Text=="SCB")
            {
                if (dgv_Preview1 != null && dgv_Preview2 != null)
                {
                    //加4分
                    if (keyData == Keys.Space)
                    {
                        String SCBStr = JsonConvert.SerializeObject(dsPreview, Formatting.Indented);
                        _buildingApiHubProxy.Invoke("FeedClientsSCB", SCBStr);
                        return true;
                    }
                    //减4分
                    if (keyData == Keys.Escape)
                    {
                        _buildingApiHubProxy.Invoke("FeedClientsSCB", "");
                        return true;
                    }
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private int GetRowFromPoint(int x, int y)
        {
            for (int i = 0; i < dgv_Preview2.RowCount; i++)
            {
                Rectangle rec = dgv_Preview2.GetRowDisplayRectangle(i, false);

                if (dgv_Preview2.RectangleToScreen(rec).Contains(x, y))
                    return i;
            }

            return -1;
        }

        private void dgv_Preview2_SelectionChanged(object sender, EventArgs e)
        {
            if (dgv_Preview2.Rows.Count > 0 && m_iSelectionIdx > -1 && m_iSelectionIdx < dgv_Preview2.Rows.Count - 1)// (dgv.SelectedRows.Count > 0))
            {

                if (dgv_Preview2.Rows.Count <= m_iSelectionIdx)
                    m_iSelectionIdx = dgv_Preview2.Rows.Count - 1;
                dgv_Preview2.Rows[m_iSelectionIdx].Selected = true;
            }
        }

        private void dgv_Preview2_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
                m_iSelectionIdx = e.RowIndex;
        }

        private void dgv_Preview2_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ((e.Clicks < 2) && (e.Button == MouseButtons.Left))
            {
                if ((e.ColumnIndex == -1) && (e.RowIndex > -1))
                    dgv_Preview2.DoDragDrop(dgv_Preview2.Rows[e.RowIndex], DragDropEffects.Move);
            }
        }

        private void dgv_Preview2_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void dgv_Preview2_DragDrop(object sender, DragEventArgs e)
        {
            int idx = GetRowFromPoint(e.X, e.Y);
            //if (idx < 0) return;
            if (e.Data.GetDataPresent(typeof(DataGridViewRow)))
            {
                DataGridViewRow row = (DataGridViewRow)e.Data.GetData(typeof(DataGridViewRow));
                DataRow ddr = ((DataRowView)row.DataBoundItem).Row;
                //如果idx=-1，即拖动到行头，删除本行
                if(idx<0)
                {
                    ((DataTable)dgv_Preview2.DataSource).Rows.Remove(ddr);
                }
                //复制需要拖动的行，插入到拖动的位置，删除原行
                else
                { 
                    DataRow nr = ((DataTable)dgv_Preview2.DataSource).NewRow();

                    for (int i = 0; i < row.Cells.Count; i++)
                    {
                        nr[i] = row.Cells[i].Value;
                    }
                    m_iSelectionIdx = idx;
                    ((DataTable)dgv_Preview2.DataSource).Rows.InsertAt(nr, idx);
                    ((DataTable)dgv_Preview2.DataSource).Rows.Remove(ddr);
                }
                dgv_Preview2.ClearSelection();
            }
        }

    }
}
