using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Text.RegularExpressions;

namespace WinformServer
{
    public class MyHub : Hub
    {
        public MyHub()
        {
            if (this.Context != null)
                Helper.Log.Info(this.Context.Request.Url.Host);
        }

        public String GetProcData(string CISDataType,string procParas)
        {
            if (this.Context != null)
                Helper.Log.Info(this.Context.Request.Url.Host + " " + CISDataType + " " + procParas);

            string strProc = ConfigurationManager.GetSettingString("/Configuration/MessageSettings/", CISDataType);
            string[] proc = Regex.Split(strProc, @"\s+");
            string procName = null;
            if(proc.Length>0)
                procName = proc[0];
            return DataBaseAccess.GetProcData2JsonStr(procName, procParas);
        }

        public void FeedClients(string queryStr)
        {
            if (this.Context != null)
                Helper.Log.Info(this.Context.Request.Url.Host + " FeedClients");
            Clients.All.updateClients(queryStr);
        }

        public void FeedClientsFromUDP(string UDPStr)
        {
            if (this.Context != null)
                Helper.Log.Info(this.Context.Request.Url.Host + " FeedClientsFromUDP");
            Clients.All.updateClientsFromUDP(UDPStr);
        }

        public void FeedClientsSCB(string SCBStr)
        {
            if (this.Context != null)
                Helper.Log.Info(this.Context.Request.Url.Host + " FeedClientsSCB");
            Clients.All.updateClientsSCB(SCBStr);
        }
       

    }
}
