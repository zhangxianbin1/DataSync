using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Configuration;

namespace DataSync.Common
{
    public class MySqlHelper
    {
        //连接字符串
        static string strConn = ConfigurationManager.AppSettings["MySqlConnectionString"].ToString();
        /// <summary>
        /// 使用ADO连接数据库公用 返回DataSet
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static DataSet SqlByAdo(string sql)
        {
            MySqlConnection conn = null;
            DataSet ds = null;
            try
            {
                conn = new MySqlConnection(strConn);
                conn.Open();
                var command = conn.CreateCommand();
                command.CommandText = sql;
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                adapter.SelectCommand = command;
                ds = new DataSet();
                adapter.Fill(ds);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
            return ds;
        }
        #region 执行非查询存储过程和SQL语句
        public static int ExcuteProc(string ProcName)
        {
            return ExcuteSQL(ProcName, null, CommandType.StoredProcedure);
        }

        public static int ExcuteProc(string ProcName, MySqlParameter[] pars)
        {
            return ExcuteSQL(ProcName, pars, CommandType.StoredProcedure);
        }

        public static int ExcuteSQL(string strSQL)
        {
            return ExcuteSQL(strSQL, null);
        }

        public static int ExcuteSQL(string strSQL, MySqlParameter[] paras)
        {
            return ExcuteSQL(strSQL, paras, CommandType.Text);
        }

        /// 执行非查询存储过程和SQL语句
        /// 增、删、改
        /// </summary>
        /// <param name="strSQL">要执行的SQL语句</param>
        /// <param name="paras">参数列表，没有参数填入null</param>
        /// <param name="cmdType">Command类型</param>
        /// <returns>返回影响行数</returns>
        public static int ExcuteSQL(string strSQL, MySqlParameter[] paras, CommandType cmdType)
        {
            int i = 0;
            using (MySqlConnection conn = new MySqlConnection(strConn))
            {
                MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                cmd.CommandType = cmdType;
                if (paras != null)
                {
                    cmd.Parameters.AddRange(paras);
                }
                conn.Open();
                i = cmd.ExecuteNonQuery();
                conn.Close();
            }
            return i;

        }
        #endregion
    }
}
