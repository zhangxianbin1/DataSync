using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DataSync.Common;

namespace DataSync
{
    class Program
    {
        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        extern static IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", EntryPoint = "GetSystemMenu")]
        extern static IntPtr GetSystemMenu(IntPtr hWnd, IntPtr bRevert);
        [DllImport("user32.dll", EntryPoint = "RemoveMenu")]
        extern static IntPtr RemoveMenu(IntPtr hMenu, uint uPosition, uint uFlags);
        static void closebtn()
        {
            IntPtr windowHandle = FindWindow(null, "DataSync");
            IntPtr closeMenu = GetSystemMenu(windowHandle, IntPtr.Zero);
            uint SC_CLOSE = 0xF060;
            RemoveMenu(closeMenu, SC_CLOSE, 0x0);
        }

        protected static void CloseConsole(object sender, ConsoleCancelEventArgs e)
        {
            Environment.Exit(0);
        }
        static log4net.ILog log = log4net.LogManager.GetLogger("logData");
        static string isRunDay = ConfigurationManager.AppSettings["isRunDay"].ToString();
        static string isRunHour = ConfigurationManager.AppSettings["isRunHour"].ToString();
        static int startHour = int.Parse(ConfigurationManager.AppSettings["startHour"]);
        static int startMinute = int.Parse(ConfigurationManager.AppSettings["startMinute"]);
        static string limitNum = ConfigurationManager.AppSettings["limitNum"].ToString();
        static void Main(string[] args)
        {
            Console.Title = "DataSync";
            closebtn();
            Console.CancelKeyPress += new ConsoleCancelEventHandler(CloseConsole);
            if ("1".Equals(isRunDay))
            {
                Thread xicheng_RunDayThread = new Thread(new ThreadStart(RunDay));
                xicheng_RunDayThread.Start();
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 已开启按天同步线程");
            }
            if ("1".Equals(isRunHour))
            {
                Thread xicheng_RunHourThread = new Thread(new ThreadStart(RunHour));
                xicheng_RunHourThread.Start();
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 已开启按小时同步线程");
            }
        }
        #region 公用方法
        /// <summary>
        /// 按天跑
        /// </summary>
        public static void RunDay()
        {
            while (true)
            {
                try
                {
                    if (DateTime.Now.Hour == startHour)
                    {
                        execute();
                        Thread.Sleep(10000);//20小时
                    }
                }
                catch (Exception ex)
                {
                    log.Warn("RunDay()：" + ex.Message);
                }
                finally
                {
                    Thread.Sleep(1800000);//半小时
                }
            }
        }

        /// <summary>
        /// 按小时跑
        /// </summary>
        public static void RunHour()
        {
            while (true)
            {
                try
                {
                    if (DateTime.Now.Minute == startMinute)
                    {
                        execute();
                        Thread.Sleep(1800000);//30分钟
                    }
                }
                catch (Exception ex)
                {
                    log.Warn("RunHour()：" + ex.Message);
                }
                finally
                {
                    Thread.Sleep(30000);//30秒
                }
            }
        }
        public static void execute()
        {
            //cesmysql();
            cesoracle();
            //cessql();
        }
        public static void cesmysql()
        {
            try
            {
                string sql = "select * from ces";
                DataSet ds = MySqlHelper.SqlByAdo(sql);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    Console.WriteLine("query success");
                    log.Info("query success");
                }
                sql = "insert into ces(code,cname,parent,valid)values(2,'测试',1,1)";
                int result = MySqlHelper.ExcuteSQL(sql);
                if (result > 0)
                {
                    Console.WriteLine("execute success");
                    log.Info("execute success");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("cesmysql " + ex.Message);
                log.Info("cesmysql " + ex.Message);
            }
        }
        public static void cesoracle()
        {
            try
            {
                string sql = "select * from PW_AJGK";
                DataSet ds = DBhelper.GetDataSet(sql);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    Console.WriteLine("oracle query success");
                    log.Info("oracle query success");
                }
                sql = "insert into PW_AJGK(\"ajOverviewId\",\"ajlx\",\"ajsl\")values('3','测试',1)";
                int result = DBhelper.ExecuteCommand(sql);
                if (result > 0)
                {
                    Console.WriteLine("oracle execute success");
                    log.Info("oracle execute success");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("cesoracle " + ex.Message);
                log.Info("cesoracle " + ex.Message);
            }
        }
        public static void cessql()
        {
            try
            {
                string sql = "select * from ces";
                DataSet ds = MsSqlHelper.SqlByAdo(sql);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    Console.WriteLine("sqlserver query success");
                    log.Info("sqlserver query success");
                }
                sql = "insert into ces(code,cname)values(2,'测试')";
                int result = MsSqlHelper.ExcuteSQL(sql);
                if (result > 0)
                {
                    Console.WriteLine("sqlserver execute success");
                    log.Info("sqlserver execute success");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("cessqlserver " + ex.Message);
                log.Info("cessqlserver " + ex.Message);
            }
        }
        #endregion
    }
}
