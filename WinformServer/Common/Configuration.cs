using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WinformServer
{
    public static class ConfigurationManager
    {
        private static string m_strToken = "1.1.1005";
        private static string m_strConfigurationFile;
        private static bool m_bLoaded = false;
        private static System.Xml.XmlDocument m_xmlDoc = new System.Xml.XmlDocument();

#pragma warning disable 0168
        public static bool LoadConfiguration(string strConfigurationFile)
        {
            if (m_strConfigurationFile == strConfigurationFile)
                return true;

            bool bReset = false;
            string strDiscCode = null;
            string strPrtPath = null;
            string strCurCul = null;
            string strConnectionString = null;

            try
            {
                m_xmlDoc.Load(strConfigurationFile);
                if (m_xmlDoc.DocumentElement.Name != "Configuration")
                {
                    m_bLoaded = false;
                    return false;
                }

                m_strConfigurationFile = strConfigurationFile;
                m_bLoaded = true;

                System.Xml.XmlNodeList ndLstMsg = m_xmlDoc.SelectNodes("Configuration/ConfigSections/Token/self::*");
                if (ndLstMsg.Count != 1 || ndLstMsg[0].InnerText != m_strToken)
                {
                    strDiscCode = GetSettingString("/Configuration/UserSettings/","DiscCode");
                    strPrtPath = GetSettingString("/Configuration/UserSettings/","PrtPath");
                    strCurCul = GetSettingString("/Configuration/UserSettings/","CultureName");
                    strConnectionString = GetSettingString("/Configuration/UserSettings/","ConnectionString");
                    bReset = true;

                    throw (new System.Exception());
                }
            }
            catch (System.Exception ex)
            {
                System.Text.StringBuilder strXML = new System.Text.StringBuilder();
                strXML.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                strXML.AppendLine("<Configuration>");
                strXML.AppendLine(" <ConfigSections>");
                strXML.AppendLine(String.Format("   <Token>{0}</Token>", m_strToken));
                strXML.AppendLine(" </ConfigSections>");

                strXML.AppendLine(" <UserSettings>");
                strXML.AppendLine("     <CultureName></CultureName>");
                strXML.AppendLine("     <ConnectionString></ConnectionString>");
                strXML.AppendLine("     <DiscCode></DiscCode>");
                strXML.AppendLine("     <DCEnable>1</DCEnable>");
                strXML.AppendLine("     <IsServer>0</IsServer>");
                strXML.AppendLine("     <Server></Server>");
                strXML.AppendLine("     <Port>5000</Port>");
                strXML.AppendLine("     <Venue>NONE</Venue>");
                strXML.AppendLine("     <TplPath>Default</TplPath>");
                strXML.AppendLine("     <RptPath>Default</RptPath>");
                strXML.AppendLine("     <PrtPath></PrtPath>");
                strXML.AppendLine("     <ShowSysVar>0</ShowSysVar>");
                strXML.AppendLine("     <ShowAllTpl>0</ShowAllTpl>");
                strXML.AppendLine("     <PdfImageQuality>1.0</PdfImageQuality>");
                strXML.AppendLine("     <PdfImageResolution>500.0</PdfImageResolution>");
                strXML.AppendLine("     <StampCorrected>Corrected</StampCorrected>");
                strXML.AppendLine("     <StampTest>Test</StampTest>");
                strXML.AppendLine("     <TplCommunication>Official Communication</TplCommunication>");
                strXML.AppendLine("     <TplOnDuty>Good Morning</TplOnDuty>");
                strXML.AppendLine("     <TplOffDuty>Good Night</TplOffDuty>");
                strXML.AppendLine("     <RptWndSize>245, 400</RptWndSize>");
                strXML.AppendLine("     <RptWndSplit>147</RptWndSplit>");
                strXML.AppendLine("     <LanguageCode>ENG</LanguageCode>");
                strXML.AppendLine(" </UserSettings>");

                strXML.AppendLine(" <NotitySettings>");
                strXML.AppendLine("     <LangActive></LangActive>");
                strXML.AppendLine("     <SportActive></SportActive>");
                strXML.AppendLine("     <SportInfo></SportInfo>");
                strXML.AppendLine("     <DisciplineActive></DisciplineActive>");
                strXML.AppendLine("     <DisciplineInfo></DisciplineInfo>");
                strXML.AppendLine("     <EventAdd></EventAdd>");
                strXML.AppendLine("     <EventDel></EventDel>");
                strXML.AppendLine("     <EventInfo></EventInfo>");
                strXML.AppendLine("     <EventModel></EventModel>");
                strXML.AppendLine("     <EventStatus></EventStatus>");
                strXML.AppendLine("     <EventResult></EventResult>");
                strXML.AppendLine("     <PhaseAdd></PhaseAdd>");
                strXML.AppendLine("     <PhaseDel></PhaseDel>");
                strXML.AppendLine("     <PhaseInfo></PhaseInfo>");
                strXML.AppendLine("     <PhaseModel></PhaseModel>");
                strXML.AppendLine("     <PhaseStatus></PhaseStatus>");
                strXML.AppendLine("     <PhaseResult></PhaseResult>");
                strXML.AppendLine("     <PhaseProgress></PhaseProgress>");
                strXML.AppendLine("     <PhaseDraw></PhaseDraw>");
                strXML.AppendLine("     <MatchAdd></MatchAdd>");
                strXML.AppendLine("     <MatchDel></MatchDel>");
                strXML.AppendLine("     <MatchInfo></MatchInfo>");
                strXML.AppendLine("     <MatchOrderInSession></MatchOrderInSession>");
                strXML.AppendLine("     <MatchWeather></MatchWeather>");
                strXML.AppendLine("     <MatchModel></MatchModel>");
                strXML.AppendLine("     <MatchStatus></MatchStatus>");
                strXML.AppendLine("     <MatchDate></MatchDate>");
                strXML.AppendLine("     <MatchResult>CIS_Result</MatchResult>");
                strXML.AppendLine("     <MatchProgress></MatchProgress>");
                strXML.AppendLine("     <MatchStatistic></MatchStatistic>");
                strXML.AppendLine("     <MatchOfficials></MatchOfficials>");
                strXML.AppendLine("     <MatchCompetitor></MatchCompetitor>");
                strXML.AppendLine("     <MatchCompetitorMember></MatchCompetitorMember>");
                strXML.AppendLine("     <MatchSessionSet></MatchSessionSet>");
                strXML.AppendLine("     <MatchSessionReset></MatchSessionReset>");
                strXML.AppendLine("     <MatchCourtSet></MatchCourtSet>");
                strXML.AppendLine("     <MatchCourtReset></MatchCourtReset>");
                strXML.AppendLine("     <SplitAdd></SplitAdd>");
                strXML.AppendLine("     <SplitDel></SplitDel>");
                strXML.AppendLine("     <SplitInfo></SplitInfo>");
                strXML.AppendLine("     <SplitCompetitorMember></SplitCompetitorMember>");
                strXML.AppendLine("     <SplitCompetitor></SplitCompetitor>");
                strXML.AppendLine("     <SplitResult></SplitResult>");
                strXML.AppendLine("     <DateInfo></DateInfo>");
                strXML.AppendLine("     <DateAdd></DateAdd>");
                strXML.AppendLine("     <DateDel></DateDel>");
                strXML.AppendLine("     <SessionInfo></SessionInfo>");
                strXML.AppendLine("     <SessionAdd></SessionAdd>");
                strXML.AppendLine("     <SessionDel></SessionDel>");
                strXML.AppendLine("     <VenueInfo></VenueInfo>");
                strXML.AppendLine("     <VenueAdd></VenueAdd>");
                strXML.AppendLine("     <VenueDel></VenueDel>");
                strXML.AppendLine("     <CourtInfo></CourtInfo>");
                strXML.AppendLine("     <CourtAdd></CourtAdd>");
                strXML.AppendLine("     <CourtDel></CourtDel>");
                strXML.AppendLine("     <RegisterAdd></RegisterAdd>");
                strXML.AppendLine("     <RegisterDel></RegisterDel>");
                strXML.AppendLine("     <RegisterModify></RegisterModify>");
                strXML.AppendLine("     <RegisterInscription></RegisterInscription>");
                strXML.AppendLine("     <DelegationModify></DelegationModify>");
                strXML.AppendLine("     <OfficialComAdd></OfficialComAdd>");
                strXML.AppendLine("     <OfficialComDel></OfficialComDel>");
                strXML.AppendLine("     <OfficialComModify></OfficialComModify>");
                strXML.AppendLine(" </NotitySettings>");

                strXML.AppendLine(" <MessageSettings>");
                strXML.AppendLine("     <CIS_Result>Proc_CIS_EQ_GetMatchResultList @MatchID</CIS_Result>");
                strXML.AppendLine("     <CIS_Match>Proc_CIS_EQ_GetMatchList</CIS_Match>");
                strXML.AppendLine(" </MessageSettings>");

                strXML.AppendLine("</Configuration>");

                m_xmlDoc.LoadXml(strXML.ToString());

                m_strConfigurationFile = strConfigurationFile;
                m_bLoaded = true;

                if (bReset)
                {
                    SetSettingString("/Configuration/UserSettings/","DiscCode", strDiscCode);
                    SetSettingString("/Configuration/UserSettings/","ConnectionString", strConnectionString);
                    SetSettingString("/Configuration/UserSettings/","CultureName", strCurCul);
                    SetSettingString("/Configuration/UserSettings/","PrtPath", strPrtPath);
                }
            }
            return true;
        }
#pragma warning restore 0168

        public static bool SaveConfiguration()
        {
            if (!m_bLoaded) return true;

            try
            {
                m_xmlDoc.Save(m_strConfigurationFile);
            }
            catch (System.Exception)
            {
                return false;
            }
            return true;
        }

        public static string GetSettingString(string strSel,string strItem)
        {
            if (!m_bLoaded) return "";
            //strSel = "/Configuration/UserSettings/"
            strSel = strSel + strItem + "/self::*";
            System.Xml.XmlNodeList ndLstMsg = m_xmlDoc.SelectNodes(strSel);

            if (ndLstMsg.Count < 1) return "";

            return ndLstMsg[0].InnerText;
        }

        public static void SetSettingString(string strSel, string strItem, string strValue)
        {
            if (!m_bLoaded) return;

            strSel = strSel + strItem + "/self::*";
            System.Xml.XmlNodeList ndLstMsg = m_xmlDoc.SelectNodes(strSel);

            if (ndLstMsg.Count == 1)
                ndLstMsg[0].InnerText = strValue;
        }

        public static Dictionary<string,string> GetNotifyList()
        {
            Dictionary<string, string> notifyDic = new Dictionary<string, string>();
            foreach (XmlNode node in m_xmlDoc.SelectSingleNode("Configuration/NotitySettings"))
            {
                notifyDic.Add(node.Name,node.Value);
            }
            return notifyDic;
        }
    }

}
