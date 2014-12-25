using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Windows.Forms;

namespace WinformServer
{
    class Helper
    {
        public static Int32 Str2Int(Object strObj)
        {
            if (strObj == null) return 0;
            if (strObj.ToString().Length == 0) return 0;

            try
            {
                return Convert.ToInt32(strObj);
            }
            catch (System.Exception errorFmt)
            {
                //MessageBox.Show(errorFmt.ToString());
            }
            return 0;
        }

        public static float Str2float(Object strObj)
        {
            if (strObj == null) return 0.0f;
            if (strObj.ToString().Length == 0) return 0.0f;

            try
            {
                return Convert.ToSingle(strObj);
            }
            catch (System.Exception errorFmt)
            {
                //MessageBox.Show(errorFmt.ToString());
            }
            return 0;
        }

        public static double Str2Double(Object strObj)
        {
            if (strObj == null) return 0.0;
            if (strObj.ToString().Length == 0) return 0.0;

            try
            {
                return Convert.ToDouble(strObj);
            }
            catch (System.Exception errorFmt)
            {
                //MessageBox.Show(errorFmt.ToString());
            }
            return 0;
        }

        public static string Millisec2RunningTime(int t)
        {
            int sec = t / 1000;
            int msec = t - sec * 1000;
            return sec.ToString() + "." + String.Format("{0,1}",(msec / 100).ToString()); 
            //return String.Format("{0:.0}",Math.Round((decimal)t / 1000,1).ToString()); 四舍五入
        }

        public static string Millisec2UsedTime(int t)
        {
            int sec = t / 1000;
            int msec = t - sec * 1000;
            return sec.ToString() + "." + String.Format("{0,2}",(msec / 10).ToString());
        }

        public static readonly log4net.ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void SetDataGridViewStyle(DataGridView dgv)
        {
            if (dgv == null) return;
            dgv.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(230, 239, 248);
            dgv.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            for (int i = 0; i < dgv.Columns.Count; i++)
            {
                dgv.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

    }
}
