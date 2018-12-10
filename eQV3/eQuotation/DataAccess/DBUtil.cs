using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Web;

namespace eQuotation.DataAccess
{
    public class DBUtil
    {
        public static Object dbExecuteScalar(string connectionName, string strSqlCmd)
        {
            SqlConnection g_adoConn = new SqlConnection(ConfigurationManager.ConnectionStrings[connectionName].ConnectionString);
            for (int i = 0; i <= 3; i++)
            {
                try
                {
                    g_adoConn.Open();
                    break;
                }
                catch (Exception ex)
                {
                    if (i == 3)
                        throw ex;
                    Thread.Sleep(100);
                }
            }
            SqlCommand dbCmd = g_adoConn.CreateCommand();
            dbCmd.CommandType = CommandType.Text;
            dbCmd.CommandText = strSqlCmd;
            dbCmd.CommandTimeout = 5 * 60;
            Object retObj = null;
            try
            {
                retObj = dbCmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                g_adoConn.Close();
                throw ex;
            }
            g_adoConn.Close();
            return retObj;

        }

        public static DataTable dbGetDataTable(string connectionName, string sql)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[connectionName].ConnectionString);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(sql, conn);
            da.SelectCommand.CommandTimeout = 300;
            try
            {
                da.Fill(dt);
            }
            catch
            {
                da.Dispose();
                conn.Close();
                conn.Dispose();
            }
            return dt;
        }
    }
}