using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advantech.Myadvantech.DataAccess
{
   public class SqlProvider
    {
        public static DataTable dbGetDataTable(string ConnectionName, string strSqlCmd)
        {
            SqlConnection g_adoConn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConnectionName].ConnectionString);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(strSqlCmd, g_adoConn);
            da.SelectCommand.CommandTimeout = 5 * 60;
            try
            {
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                g_adoConn.Close();
                throw new Exception(ex.ToString() + System.Environment.NewLine + "sql:" + strSqlCmd);
            }
            g_adoConn.Close();
            g_adoConn = null;
            return dt;
        }

        public static IAsyncResult dbGetReaderAsync(string ConnectionName, string strSqlCmd, ref SqlConnection g_adoConn, ref SqlCommand dbCmd)
        {
            g_adoConn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConnectionName].ConnectionString);
            dbCmd = g_adoConn.CreateCommand();
            dbCmd.Connection = g_adoConn;
            dbCmd.CommandText = strSqlCmd;
            g_adoConn.Open();
            return dbCmd.BeginExecuteReader();
        }

        public static int dbExecuteNoQuery2(string ConnectionStringName, string strSqlCmd, SqlParameter[] Parameters = null)
        {
            SqlConnection g_adoConn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString);
            System.Data.SqlClient.SqlCommand dbCmd = g_adoConn.CreateCommand();
            dbCmd.Connection = g_adoConn;
            dbCmd.CommandText = strSqlCmd;
            int retInt = -1;
            if (Parameters != null && Parameters.Length > 0)
            {
                dbCmd.Parameters.AddRange(Parameters);
            }
            for (int i = 0; i <= 3; i++)
            {
                try
                {
                    g_adoConn.Open();
                    break; // TODO: might not be correct. Was : Exit For
                }
                catch (SqlException ex)
                {
                    if (i == 3)
                        throw ex;
                    System.Threading.Thread.Sleep(100);
                }
            }
            try
            {
                retInt = dbCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                g_adoConn.Close();
                throw new Exception(ex.ToString() + System.Environment.NewLine + "sql:" + strSqlCmd);
            }
            g_adoConn.Close();
            return retInt;
        }
        public static int dbExecuteNoQuery(string ConnectionStringName, string strSqlCmd)
        {
            SqlConnection g_adoConn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString);
            System.Data.SqlClient.SqlCommand dbCmd = g_adoConn.CreateCommand();
            dbCmd.Connection = g_adoConn;
            dbCmd.CommandText = strSqlCmd;
            int retInt = -1;
            for (int i = 0; i <= 3; i++)
            {
                try
                {
                    g_adoConn.Open();
                    break; // TODO: might not be correct. Was : Exit For
                }
                catch (SqlException ex)
                {
                    if (i == 3)
                        throw ex;
                    System.Threading.Thread.Sleep(100);
                }
            }
            //Using tran As SqlTransaction = g_adoConn.BeginTransaction
            try
            {
                //dbCmd.Transaction = tran
                retInt = dbCmd.ExecuteNonQuery();
                //tran.Commit()
            }
            catch (Exception ex)
            {
                //tran.Rollback()
                g_adoConn.Close();
                throw new Exception(ex.ToString() + " sql:" + strSqlCmd);
            }
            //End Using
            g_adoConn.Close();
            return retInt;
        }
        //Nada 20131215 to avoid deadlock
        public static int dbExecuteNoQueryShareConn(SqlConnection Connection, string strSqlCmd)
        {
            SqlConnection g_adoConn = Connection;
            System.Data.SqlClient.SqlCommand dbCmd = g_adoConn.CreateCommand();
            dbCmd.Connection = g_adoConn;
            dbCmd.CommandText = strSqlCmd;
            int retInt = -1;
            for (int i = 0; i <= 3; i++)
            {
                try
                {
                    if (!(g_adoConn.State == ConnectionState.Open))
                        g_adoConn.Open();
                    break; // TODO: might not be correct. Was : Exit For
                }
                catch (SqlException ex)
                {
                    if (i == 3)
                        throw ex;
                    System.Threading.Thread.Sleep(100);
                }
            }
            //Using tran As SqlTransaction = g_adoConn.BeginTransaction
            try
            {
                //dbCmd.Transaction = tran
                retInt = dbCmd.ExecuteNonQuery();
                //tran.Commit()
            }
            catch (Exception ex)
            {
                //tran.Rollback()
                g_adoConn.Close();
                throw new Exception(ex.ToString() + " sql:" + strSqlCmd);
            }
            //End Using
            return retInt;
        }

        public static IAsyncResult dbExecuteNoQueryAsync(string ConnectionStringName, string strSqlCmd, ref SqlConnection g_adoConn, ref SqlCommand dbCmd)
        {
            g_adoConn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString);
            dbCmd = g_adoConn.CreateCommand();
            dbCmd.Connection = g_adoConn;
            dbCmd.CommandText = strSqlCmd;
            IAsyncResult ar = null;
            g_adoConn.Open();
            return dbCmd.BeginExecuteNonQuery();
        }
        public static object dbExecuteScalar(string ConnectionName, string strSqlCmd)
        {
            SqlConnection g_adoConn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConnectionName].ConnectionString);
            for (int i = 0; i <= 3; i++)
            {
                try
                {
                    g_adoConn.Open();
                    break; // TODO: might not be correct. Was : Exit For
                }
                catch (SqlException ex)
                {
                    if (i == 3)
                        throw ex;
                    System.Threading.Thread.Sleep(100);
                }
            }
            System.Data.SqlClient.SqlCommand dbCmd = g_adoConn.CreateCommand();
            dbCmd.CommandType = CommandType.Text;
            dbCmd.CommandText = strSqlCmd;
            dbCmd.CommandTimeout = 5 * 60;
            object retObj = null;
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
    }
}
