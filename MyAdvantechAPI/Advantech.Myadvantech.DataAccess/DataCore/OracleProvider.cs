using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Advantech.Myadvantech.DataAccess
{
    public  class OracleProvider 
    {
        private static  OracleConnection conn = null;

        /// <summary>
        /// Get Oracle db connection
        /// </summary>
        /// <param name="ConnectionName"></param>
        /// <returns></returns>
        private static OracleConnection GetConnection(string ConnectionName)
        {
            return new OracleConnection(ConfigurationManager.ConnectionStrings[ConnectionName].ConnectionString);
        }

        public static  DataTable GetDataTable(string ConnectionName, string strSqlCmd)
        {
            conn = GetConnection(ConnectionName);
            DataTable dt = new DataTable();
            OracleDataAdapter da = new OracleDataAdapter(strSqlCmd, conn);
            System.Threading.Thread.CurrentThread.Priority = System.Threading.ThreadPriority.BelowNormal;
            try
            {
                da.Fill(dt);
            }
            catch 
            {
                throw;
            }
            finally
            {
                System.Threading.Thread.CurrentThread.Priority = System.Threading.ThreadPriority.Normal;
                if (conn != null)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }
            return dt;
        }

        public static object ExecuteScalar(string ConnectionName, string strSqlCmd)
        {
            OracleConnection conn = GetConnection(ConnectionName);
            conn.Open();
            OracleCommand dbCmd = conn.CreateCommand();
            dbCmd.CommandType = CommandType.Text;
            dbCmd.CommandText = strSqlCmd;
            object retObj = null;
            try
            {
                retObj = dbCmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                conn.Close();
                throw new Exception(strSqlCmd + "." + ex.ToString());
            }
            conn.Close();
            return retObj;
        }

      
    }
}
