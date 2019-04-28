using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace DataSync.Common
{
    public class DBhelper
    { 
        /// <summary>
        /// string 查询返回单行单列的Oracle语句。（ 例：select count(*) from 表名或 select name from 表名 ）。
        /// </summary>
        /// <param name="Oracle"></param>
        /// <returns></returns>
        public static DataSet GetDataSet(string Oracle)
        {
            OracleConnection cnn = null;
            DataSet ds = null;
            try
            {
                string cnnstr = ConfigurationManager.ConnectionStrings["OracleConnectionString"].ConnectionString;
                cnn = new OracleConnection(cnnstr);
                cnn.Open();
                OracleCommand cmm = new OracleCommand(Oracle, cnn);
                OracleDataAdapter adapter = new OracleDataAdapter();
                adapter.SelectCommand = cmm;
                ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cnn.Close();
                cnn.Dispose();
            }
            return ds;
        }
        public static int ExecuteCommand(string Oracle)
        {
            OracleConnection cnn = null;
            int intResult = 0;
            try
            {
                string cnnstr = ConfigurationManager.ConnectionStrings["OracleConnectionString"].ConnectionString;
                cnn = new OracleConnection(cnnstr);
                cnn.Open();
                OracleCommand cmm = new OracleCommand(Oracle, cnn);
                intResult = cmm.ExecuteNonQuery();
                cmm.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cnn.Close();
                cnn.Dispose();
            }
            return intResult;
        }
       
    }
}