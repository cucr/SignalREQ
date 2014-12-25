namespace WinformServer
{
    partial class CISFeedServer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ServerStatus = new System.Windows.Forms.Label();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.chb_UDPServer = new System.Windows.Forms.CheckBox();
            this.tb_UDPBroadcastPort = new System.Windows.Forms.TextBox();
            this.lb_UDPData = new System.Windows.Forms.Label();
            this.gb_MDS = new System.Windows.Forms.GroupBox();
            this.gb_UDP = new System.Windows.Forms.GroupBox();
            this.gb_Command = new System.Windows.Forms.GroupBox();
            this.flp_Command = new System.Windows.Forms.FlowLayoutPanel();
            this.cmb_Match = new System.Windows.Forms.ComboBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.gb_Preview = new System.Windows.Forms.GroupBox();
            this.dgv_Preview1 = new System.Windows.Forms.DataGridView();
            this.dgv_Preview2 = new System.Windows.Forms.DataGridView();
            this.gb_MDS.SuspendLayout();
            this.gb_UDP.SuspendLayout();
            this.gb_Command.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.gb_Preview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Preview1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Preview2)).BeginInit();
            this.SuspendLayout();
            // 
            // ServerStatus
            // 
            this.ServerStatus.AutoSize = true;
            this.ServerStatus.Location = new System.Drawing.Point(6, 24);
            this.ServerStatus.Name = "ServerStatus";
            this.ServerStatus.Size = new System.Drawing.Size(77, 12);
            this.ServerStatus.TabIndex = 3;
            this.ServerStatus.Text = "ServerStatus";
            // 
            // txtIP
            // 
            this.txtIP.Font = new System.Drawing.Font("SimSun", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtIP.Location = new System.Drawing.Point(6, 26);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(180, 38);
            this.txtIP.TabIndex = 9;
            this.txtIP.Text = "127.0.0.1";
            // 
            // btnConnect
            // 
            this.btnConnect.Font = new System.Drawing.Font("SimSun", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnConnect.Location = new System.Drawing.Point(233, 26);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(124, 38);
            this.btnConnect.TabIndex = 8;
            this.btnConnect.Text = "ConnectMDS";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // chb_UDPServer
            // 
            this.chb_UDPServer.AutoSize = true;
            this.chb_UDPServer.Font = new System.Drawing.Font("SimSun", 15F);
            this.chb_UDPServer.Location = new System.Drawing.Point(229, 26);
            this.chb_UDPServer.Name = "chb_UDPServer";
            this.chb_UDPServer.Size = new System.Drawing.Size(128, 24);
            this.chb_UDPServer.TabIndex = 61;
            this.chb_UDPServer.Text = "ConnectUDP";
            this.chb_UDPServer.UseVisualStyleBackColor = true;
            this.chb_UDPServer.CheckedChanged += new System.EventHandler(this.chb_UDPServer_CheckedChanged);
            // 
            // tb_UDPBroadcastPort
            // 
            this.tb_UDPBroadcastPort.Font = new System.Drawing.Font("SimSun", 20F);
            this.tb_UDPBroadcastPort.Location = new System.Drawing.Point(6, 18);
            this.tb_UDPBroadcastPort.Name = "tb_UDPBroadcastPort";
            this.tb_UDPBroadcastPort.Size = new System.Drawing.Size(180, 38);
            this.tb_UDPBroadcastPort.TabIndex = 60;
            this.tb_UDPBroadcastPort.TextChanged += new System.EventHandler(this.tb_UDPBroadcastPort_TextChanged);
            // 
            // lb_UDPData
            // 
            this.lb_UDPData.AutoSize = true;
            this.lb_UDPData.Location = new System.Drawing.Point(6, 68);
            this.lb_UDPData.Name = "lb_UDPData";
            this.lb_UDPData.Size = new System.Drawing.Size(47, 12);
            this.lb_UDPData.TabIndex = 62;
            this.lb_UDPData.Text = "UDPData";
            // 
            // gb_MDS
            // 
            this.gb_MDS.Controls.Add(this.txtIP);
            this.gb_MDS.Controls.Add(this.btnConnect);
            this.gb_MDS.Location = new System.Drawing.Point(8, 55);
            this.gb_MDS.Name = "gb_MDS";
            this.gb_MDS.Size = new System.Drawing.Size(400, 120);
            this.gb_MDS.TabIndex = 63;
            this.gb_MDS.TabStop = false;
            this.gb_MDS.Text = "MDS";
            // 
            // gb_UDP
            // 
            this.gb_UDP.Controls.Add(this.tb_UDPBroadcastPort);
            this.gb_UDP.Controls.Add(this.chb_UDPServer);
            this.gb_UDP.Controls.Add(this.lb_UDPData);
            this.gb_UDP.Location = new System.Drawing.Point(427, 55);
            this.gb_UDP.Name = "gb_UDP";
            this.gb_UDP.Size = new System.Drawing.Size(400, 120);
            this.gb_UDP.TabIndex = 64;
            this.gb_UDP.TabStop = false;
            this.gb_UDP.Text = "UDP";
            // 
            // gb_Command
            // 
            this.gb_Command.Controls.Add(this.flp_Command);
            this.gb_Command.Controls.Add(this.cmb_Match);
            this.gb_Command.Dock = System.Windows.Forms.DockStyle.Top;
            this.gb_Command.Location = new System.Drawing.Point(3, 3);
            this.gb_Command.Name = "gb_Command";
            this.gb_Command.Size = new System.Drawing.Size(837, 127);
            this.gb_Command.TabIndex = 65;
            this.gb_Command.TabStop = false;
            this.gb_Command.Text = "Command";
            // 
            // flp_Command
            // 
            this.flp_Command.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flp_Command.Font = new System.Drawing.Font("SimSun", 15F);
            this.flp_Command.Location = new System.Drawing.Point(3, 53);
            this.flp_Command.Name = "flp_Command";
            this.flp_Command.Size = new System.Drawing.Size(831, 71);
            this.flp_Command.TabIndex = 10;
            // 
            // cmb_Match
            // 
            this.cmb_Match.Font = new System.Drawing.Font("SimSun", 15F);
            this.cmb_Match.FormattingEnabled = true;
            this.cmb_Match.Location = new System.Drawing.Point(6, 20);
            this.cmb_Match.Name = "cmb_Match";
            this.cmb_Match.Size = new System.Drawing.Size(277, 28);
            this.cmb_Match.TabIndex = 0;
            this.cmb_Match.SelectedIndexChanged += new System.EventHandler(this.cmb_Match_SelectedIndexChanged);
            this.cmb_Match.MouseClick += new System.Windows.Forms.MouseEventHandler(this.cmb_Match_MouseClick);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(851, 562);
            this.tabControl1.TabIndex = 66;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.ServerStatus);
            this.tabPage1.Controls.Add(this.gb_UDP);
            this.tabPage1.Controls.Add(this.gb_MDS);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(843, 536);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Connect";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.gb_Preview);
            this.tabPage2.Controls.Add(this.gb_Command);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(843, 536);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "SCB";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // gb_Preview
            // 
            this.gb_Preview.Controls.Add(this.dgv_Preview1);
            this.gb_Preview.Controls.Add(this.dgv_Preview2);
            this.gb_Preview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gb_Preview.Location = new System.Drawing.Point(3, 130);
            this.gb_Preview.Name = "gb_Preview";
            this.gb_Preview.Size = new System.Drawing.Size(837, 403);
            this.gb_Preview.TabIndex = 66;
            this.gb_Preview.TabStop = false;
            this.gb_Preview.Text = "Preview";
            // 
            // dgv_Preview1
            // 
            this.dgv_Preview1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_Preview1.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgv_Preview1.Location = new System.Drawing.Point(3, 17);
            this.dgv_Preview1.Name = "dgv_Preview1";
            this.dgv_Preview1.RowTemplate.Height = 23;
            this.dgv_Preview1.Size = new System.Drawing.Size(831, 73);
            this.dgv_Preview1.TabIndex = 1;
            // 
            // dgv_Preview2
            // 
            this.dgv_Preview2.AllowDrop = true;
            this.dgv_Preview2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_Preview2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dgv_Preview2.Location = new System.Drawing.Point(3, 102);
            this.dgv_Preview2.Name = "dgv_Preview2";
            this.dgv_Preview2.RowTemplate.Height = 23;
            this.dgv_Preview2.Size = new System.Drawing.Size(831, 298);
            this.dgv_Preview2.TabIndex = 0;
            this.dgv_Preview2.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgv_Preview2_CellMouseDown);
            this.dgv_Preview2.CellMouseMove += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgv_Preview2_CellMouseMove);
            this.dgv_Preview2.SelectionChanged += new System.EventHandler(this.dgv_Preview2_SelectionChanged);
            this.dgv_Preview2.DragDrop += new System.Windows.Forms.DragEventHandler(this.dgv_Preview2_DragDrop);
            this.dgv_Preview2.DragEnter += new System.Windows.Forms.DragEventHandler(this.dgv_Preview2_DragEnter);
            // 
            // CISFeedServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(851, 562);
            this.Controls.Add(this.tabControl1);
            this.Name = "CISFeedServer";
            this.Text = "CISFeedServer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CISFeedServer_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CISFeedServer_FormClosed);
            this.gb_MDS.ResumeLayout(false);
            this.gb_MDS.PerformLayout();
            this.gb_UDP.ResumeLayout(false);
            this.gb_UDP.PerformLayout();
            this.gb_Command.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.gb_Preview.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Preview1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Preview2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label ServerStatus;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.CheckBox chb_UDPServer;
        private System.Windows.Forms.TextBox tb_UDPBroadcastPort;
        private System.Windows.Forms.Label lb_UDPData;
        private System.Windows.Forms.GroupBox gb_MDS;
        private System.Windows.Forms.GroupBox gb_UDP;
        private System.Windows.Forms.GroupBox gb_Command;
        private System.Windows.Forms.ComboBox cmb_Match;
        private System.Windows.Forms.FlowLayoutPanel flp_Command;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox gb_Preview;
        private System.Windows.Forms.DataGridView dgv_Preview2;
        private System.Windows.Forms.DataGridView dgv_Preview1;
    }
}

