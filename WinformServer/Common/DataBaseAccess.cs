using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;

namespace WinformServer
{
    public static class DataBaseAccess
    {
        public static string DataBaseString = ConfigurationSettings.AppSettings["DBStr"];
        public static string LangStr = ConfigurationSettings.AppSettings["LangStr"];

        /// <summary>
        /// 执行带参存储过程
        /// </summary>
        public static String GetProcData2JsonStr(string proName, string procParas)
        {
            if (proName == null)
                return null;
            String jsonStr="";
            SqlConnection conn = new SqlConnection(DataBaseString);
            try {
                    if (conn.State == System.Data.ConnectionState.Closed)
                        conn.Open();
                    SqlCommand comm = new SqlCommand();
                    comm.Connection = conn;
                    comm.CommandText = proName;
                    comm.CommandType = CommandType.StoredProcedure;

                    Dictionary<string, string> values = JsonConvert.DeserializeObject<Dictionary<string, string>>(procParas.ToString());
                    if(values.Count>0)
                    { 
                        foreach (string key in values.Keys)
                        {//key是属性名,values[key]是属性的值
                            comm.Parameters.Add(new SqlParameter(key, values[key]));
                        }
                    }
                    //JObject jo = (JObject)JsonConvert.DeserializeObject(procParas);
                    //Console.WriteLine(jo.ToString());

                    SqlDataAdapter da = new SqlDataAdapter(comm);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    jsonStr = JsonConvert.SerializeObject(ds, Formatting.Indented);
                }
            catch (Exception ee)
            {
                Helper.Log.Error(ee.Message);
            }
            finally
            {
                conn.Close();
            }
            return jsonStr;
        }

        public static DataSet GetProcData2DataSet(string proName, string procParas)
        {
            if (proName == null)
                return null;
            DataSet ds = new DataSet();
            SqlConnection conn = new SqlConnection(DataBaseString);
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                SqlCommand comm = new SqlCommand();
                comm.Connection = conn;
                comm.CommandText = proName;
                comm.CommandType = CommandType.StoredProcedure;

                Dictionary<string, string> values = JsonConvert.DeserializeObject<Dictionary<string, string>>(procParas.ToString());
                if (values.Count > 0)
                {
                    foreach (string key in values.Keys)
                    {//key是属性名,values[key]是属性的值
                        comm.Parameters.Add(new SqlParameter(key, values[key]));
                    }
                }

                SqlDataAdapter da = new SqlDataAdapter(comm);
                da.Fill(ds);
            }
            catch (Exception ee)
            {
                Helper.Log.Error(ee.Message);
            }
            finally
            {
                conn.Close();
            }
            return ds;
        }

        public static int GetStatusByMatchID(int matchID)
        {
            int iStatus = 0;
            string commText = "select F_MatchStatusID from TS_Match where F_MatchID=" + matchID.ToString();
            SqlConnection conn = new SqlConnection(DataBaseString);
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                SqlCommand comm = new SqlCommand(commText, conn);
                iStatus = Convert.ToInt32(comm.ExecuteScalar());
            }
            catch (Exception ee)
            {
                Helper.Log.Error(ee.Message);
            }
            finally
            {
                conn.Close();
            }

            return iStatus;
        }

        public static int GetEventIDByMatchID(int matchID)
        {
            int iEventId = 0;
            string commText = "select P.F_EventID from TS_Match AS M LEFT JOIN TS_Phase As P ON P.F_PhaseID=M.F_PhaseID  where F_MatchID=" + matchID.ToString();
            SqlConnection conn = new SqlConnection(DataBaseString);
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                SqlCommand comm = new SqlCommand(commText, conn);
                iEventId = Convert.ToInt32(comm.ExecuteScalar());
            }
            catch (Exception ee)
            {
                Helper.Log.Error(ee.Message);
            }
            finally
            {
                conn.Close();
            }

            return iEventId;
        }

        public static int GetEventTypeByMatchID(int matchID)
        {
            int iEventId = 0;
            string commText = "select E.F_PlayerRegTypeID from TS_Match AS M LEFT JOIN TS_Phase As P ON P.F_PhaseID=M.F_PhaseID LEFT JOIN TS_Event AS E ON E.F_EventID = P.F_EventID  where F_MatchID=" + matchID.ToString();
            SqlConnection conn = new SqlConnection(DataBaseString);
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                SqlCommand comm = new SqlCommand(commText, conn);
                iEventId = Convert.ToInt32(comm.ExecuteScalar());
            }
            catch (Exception ee)
            {
                Helper.Log.Error(ee.Message);
            }
            finally
            {
                conn.Close();
            }

            return iEventId;
        }

        public static int GetPhaseIDByMatchID(int matchID)
        {
            int iEventId = 0;
            string commText = "select P.F_FatherPhaseID from TS_Match AS M LEFT JOIN TS_Phase As P ON P.F_PhaseID=M.F_PhaseID  where F_MatchID=" + matchID.ToString();
            SqlConnection conn = new SqlConnection(DataBaseString);
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                SqlCommand comm = new SqlCommand(commText, conn);
                iEventId = Convert.ToInt32(comm.ExecuteScalar());
            }
            catch (Exception ee)
            {
                Helper.Log.Error(ee.Message);
            }
            finally
            {
                conn.Close();
            }

            return iEventId;
        }

        public static DataTable GetMatchList()
        {
            System.Data.SqlClient.SqlDataAdapter da = new SqlDataAdapter();
            System.Data.DataTable dt = new DataTable();

            try
            {
                SqlConnection conn = new SqlConnection(DataBaseString);
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                SqlCommand cmdSelect = new SqlCommand("Proc_EQ_TS_GetMatchList", conn);
                cmdSelect.CommandType = CommandType.StoredProcedure;
                cmdSelect.Parameters.Add(new SqlParameter("@LanguageCode", LangStr));
                cmdSelect.UpdatedRowSource = UpdateRowSource.None;
                da.SelectCommand = cmdSelect;
                dt.Clear();
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return dt;
        }


    }



}
