using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Text.RegularExpressions;

namespace WinformServer
{
    static class MessageParse
    {
        //private Dictionary<string type,>
        public static List<NotifyMsg> ParseMessagePack(string strXml)
        {
            XmlDocument xx = new XmlDocument();
            try { 
                xx.LoadXml(strXml);//加载xml
            }catch(Exception e)
            {
                return null;
            }
            List<NotifyMsg> notifyMsgList = GetNotifyTypeFromXml(xx);
            return notifyMsgList;
        }

        public static List<string> ProcessNotify(NotifyMsg notifyMsg)
        {
            List<string> invokeList = new List<string>();
            string strMessage = ConfigurationManager.GetSettingString("/Configuration/NotitySettings/", notifyMsg.StrType);
            if(strMessage.Length==0)
                return null;
            //CIS_MatchResult,CIS_Schedule
            string[] messages = strMessage.Split(new char[] { ',' });
            foreach(string message in messages)
            {
                string strProc = ConfigurationManager.GetSettingString("/Configuration/MessageSettings/", message);
                //Proc_CIS_TT_GetMatchResult @MatchID,@MatchSplitID,@LanguageCode
                string[] proc = Regex.Split(strProc, @"\s+");
                if(proc.Length<=0)
                    continue;
                //@" {""CISDataType"":""MatchResult"",""ProcParas"":{""@MatchID"":84,""@MatchSplitID"":-1,""@LanguageCode"":""ENG""}}"
                string invokeItem = "{\"CISDataType\":\""+message+"\",\"ProcParas\":{";
                //存储过程带参数
                if(proc.Length>1)
                {
                    for(int i=1;i<proc.Length;i++)
                    {
                        string prcParaValue=null;
                        if(proc[i]=="@LanguageCode")
                            prcParaValue = "\"" + ConfigurationManager.GetSettingString("/Configuration/UserSettings/", "LanguageCode")+"\"";
                        else
                            prcParaValue = GetModelValue(proc[i].Replace("@","Str"),notifyMsg);
                        invokeItem = invokeItem+"\""+proc[i]+"\":"+prcParaValue+(i==proc.Length-1?"":",");
                    }
                    invokeItem.TrimEnd(',');
                }
                invokeItem = invokeItem + "}}";
                invokeList.Add(invokeItem);

            }
                return invokeList;
            
        }

        public static List<NotifyMsg> GetNotifyTypeFromXml(XmlDocument xmlDoc)
        {
            List<NotifyMsg> notifyMsgList = new List<NotifyMsg>();
            if(xmlDoc.GetElementsByTagName("Message").Count!=1)
                return null;
            // Ensure message type is "NOTIFY"
            if (xmlDoc.SelectSingleNode("/Message").Attributes["Type"].Value != "NOTIFY")
                return null;
            foreach (XmlNode node in xmlDoc.SelectNodes("/Message/Item"))
            {
                NotifyMsg notifyMsg = new NotifyMsg();
                notifyMsg.StrRSCCode = xmlDoc.SelectSingleNode("/Message").Attributes["RSC"].Value;
                notifyMsg.StrDate = xmlDoc.SelectSingleNode("/Message").Attributes["Date"].Value;
                notifyMsg.StrGenderCode = xmlDoc.SelectSingleNode("/Message").Attributes["Gender"].Value;
                notifyMsg.StrDisciplineCode = xmlDoc.SelectSingleNode("/Message").Attributes["Discipline"].Value;
                notifyMsg.StrPhaseCode = xmlDoc.SelectSingleNode("/Message").Attributes["Phase"].Value;
                notifyMsg.StrUnitCode = xmlDoc.SelectSingleNode("/Message").Attributes["Unit"].Value;
                notifyMsg.StrType = node.Attributes["NotifyType"].Value;
                notifyMsg.StrDisciplineID = node.Attributes["DisciplineID"].Value;
                notifyMsg.StrEventID = node.Attributes["EventID"].Value;
                notifyMsg.StrPhaseID = node.Attributes["PhaseID"].Value;
                notifyMsg.StrMatchID = node.Attributes["MatchID"].Value;
                notifyMsg.StrSessionID = node.Attributes["SessionID"].Value;
                notifyMsg.StrCourtID = node.Attributes["CourtID"].Value;
                notifyMsgList.Add(notifyMsg);
            }
            return notifyMsgList;

        }

        public static string GetModelValue(string FieldName, object obj)
        {
            try
            {
                Type Ts = obj.GetType();
                object o = Ts.GetProperty(FieldName).GetValue(obj, null);
                string Value = Convert.ToString(o);
                if (string.IsNullOrEmpty(Value)) return null;
                return Value;
            }
            catch
            {
                return null;
            }
        }
    }

    class NotifyMsg
    {
        private String strRSCCode;

        public String StrRSCCode
        {
            get { return strRSCCode; }
            set { strRSCCode = value; }
        }
        private String strDate;

        public String StrDate
        {
            get { return strDate; }
            set { strDate = value; }
        }
        private String strVenueCode;

        public String StrVenueCode
        {
            get { return strVenueCode; }
            set { strVenueCode = value; }
        }
        private String strGenderCode;

        public String StrGenderCode
        {
            get { return strGenderCode; }
            set { strGenderCode = value; }
        }

        private String strDisciplineCode;

        public String StrDisciplineCode
        {
            get { return strDisciplineCode; }
            set { strDisciplineCode = value; }
        }
        private String strEventCode;

        public String StrEventCode
        {
            get { return strEventCode; }
            set { strEventCode = value; }
        }
        private String strPhaseCode;

        public String StrPhaseCode
        {
            get { return strPhaseCode; }
            set { strPhaseCode = value; }
        }
        private String strUnitCode;

        public String StrUnitCode
        {
            get { return strUnitCode; }
            set { strUnitCode = value; }
        }

        private String strType;

        public String StrType
        {
            get { return strType; }
            set { strType = value; }
        }
        private String strDisciplineID;

        public String StrDisciplineID
        {
            get { return strDisciplineID; }
            set { strDisciplineID = value; }
        }
        private String strEventID;

        public String StrEventID
        {
            get { return strEventID; }
            set { strEventID = value; }
        }
        private String strPhaseID;

        public String StrPhaseID
        {
            get { return strPhaseID; }
            set { strPhaseID = value; }
        }
        private String strMatchID;

        public String StrMatchID
        {
            get { return strMatchID; }
            set { strMatchID = value; }
        }
        private String strSessionID;

        public String StrSessionID
        {
            get { return strSessionID; }
            set { strSessionID = value; }
        }
        private String strCourtID;

        public String StrCourtID
        {
            get { return strCourtID; }
            set { strCourtID = value; }
        }
    };

    public enum MatchStatus
    {
        Available = 10,
        Configured = 20,
        Scheduled = 30,
        Startlist = 40,
        Running = 50,
        Suspend = 60,
        Unofficial = 100,
        Official = 110,
        Revision = 120,
        Canceled = 130
    }

    public enum Event
    {
        Men = 1,
        Women = 2,
        MixTeam = 3
    }

    public enum EventType
    {
        Single = 1,
        Team = 3
    }
}
