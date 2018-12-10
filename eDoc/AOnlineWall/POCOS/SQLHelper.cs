
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections;


namespace AOnlineWall.POCOS
{

	/// <summary>
	/// 通用数据库操作类
	/// </summary>
    public class SQLHelper
    {
		//连接字符串
        private static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["AonlineWall"].ConnectionString;
		
        #region ExecuteNonQuery

        public static int ExecuteNonQuery(SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn;
            if (trans == null)
            {
                conn = new SqlConnection(ConnectionString);
            }
            else
            {
                conn = trans.Connection;
            }
            PrepareCommand(cmd, conn, trans, cmdType, cmdText, cmdParms);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }


        public static int ExecuteNonQuery(CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {

            return ExecuteNonQuery(null, cmdType, cmdText, cmdParms);
        }

        public static int ExecuteNonQuery(string cmdText, params SqlParameter[] cmdParms)
        {
            return ExecuteNonQuery(null,CommandType.Text, cmdText, cmdParms);

        }

        public static int ExecuteNonQuery(string cmdText)
        {
            return ExecuteNonQuery(null, CommandType.Text, cmdText, null);

        }

       
        #endregion
        

        #region ExecuteReader
        public static SqlDataReader ExecuteReader(CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(ConnectionString);

            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return rdr;
            }
            catch
            {
                conn.Close();
                throw;
            }
        }

        public static SqlDataReader ExecuteReader(string cmdText, params SqlParameter[] cmdParms)
        {
            return ExecuteReader(CommandType.Text, cmdText, cmdParms);
        }
        public static SqlDataReader ExecuteReader(string cmdText)
        {
            return ExecuteReader(CommandType.Text, cmdText, null);
        } 
        #endregion
        
        #region ExecuteScalar
        public static object ExecuteScalar(CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return val;
            }
        }

        public static object ExecuteScalar(string cmdText, params SqlParameter[] cmdParms)
        {
            return ExecuteScalar(CommandType.Text, cmdText, cmdParms);
        }

        public static object ExecuteScalar(string cmdText)
        {
            return ExecuteScalar(CommandType.Text, cmdText, null);
        } 
        #endregion

        #region DataTable
        /// <summary>
        /// 这种返回DataTable  主要用来可以判断这个DataTable里面有没有这个属性
        /// foreach (DataRow row in table.Rows){if (table.Columns.Contains("Id"))}  如果有就获得
        /// </summary>
        /// <param name="safeSql"></param>
        /// <returns></returns>
         public static DataTable GetDataSet(CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(ConnectionString);
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds.Tables[0];
            }
            catch
            {
                conn.Close();
                throw;
            }
        }

         public static DataTable GetDataSet(string cmdText, params SqlParameter[] cmdParms)
        {
            return GetDataSet(CommandType.Text, cmdText, cmdParms);
        }
         public static DataTable GetDataSet(string cmdText)
        {
            return GetDataSet(CommandType.Text, cmdText, null);
        }
        #endregion

        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] cmdParms) {

			if (conn.State != ConnectionState.Open)
				conn.Open();

			cmd.Connection = conn;
			cmd.CommandText = cmdText;

			if (trans != null)
				cmd.Transaction = trans;

			cmd.CommandType = cmdType;

			if (cmdParms != null) {
				foreach (SqlParameter parm in cmdParms)
					cmd.Parameters.Add(parm);
			}
		}
	}
}