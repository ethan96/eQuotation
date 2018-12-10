using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace Advantech.Myadvantech.DataAccess
{
    public class PISDAL
    {

        private string ConnStr_PIS  = "Data Source=ACLSTNR12;database=PIS;User Id =PISApp;Password =pisuser;Application Name=PISLibrary";
        private string ConnStr_PISBackend = "Data Source=ACLSTNR12;database=PISBackend;User Id =PISApp;Password =pisuser;Application Name=PISLibrary";
        private Boolean IsUsePISBackend = false;
        private SqlConnection sqlConn = null;


    //      Public Sub SetConnectionString(ByVal PISConnectionstr As String)
    //    Me.ConnStr_PIS = PISConnectionstr
    //    IsUsePISBackend = False
    //End Sub

    //Public Sub ConnectToPISBackend(ByVal IsUseBackendDB As Boolean)
    //    Me.IsUsePISBackend = IsUseBackendDB
    //End Sub



        internal void OpenConnection()
        {
            if (this.IsUsePISBackend)
            {
                this.sqlConn = new SqlConnection(ConnStr_PISBackend);
            }
            else
            {
                this.sqlConn = new SqlConnection(ConnStr_PIS);
            }

            if (this.sqlConn.State != ConnectionState.Open) this.sqlConn.Open();
        }

        internal void CloseConnection()
        {
            try {
                if (this.sqlConn != null)
                {
                    this.sqlConn.Close();
                }
            }
            catch { 
            }
        }
        internal DataTable ExccuteDataTable(SqlCommand mSQLCommand, ref SqlConnection conn)
        {

             

                      
                if(string.IsNullOrEmpty(mSQLCommand.CommandText)) return null;

                if (conn.State != ConnectionState.Open) conn.Open();

                mSQLCommand.Connection = conn;

                SqlDataReader _SqlDataReader = mSQLCommand.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(_SqlDataReader);

                return dt;

            }





            internal DataTable GetModelListByPart(String partNo)
            {

            StringBuilder sql = new StringBuilder();
            sql.Append(" Select model_name  ");
            sql.Append(" From model  ");
            SqlCommand sqlCmd = new SqlCommand(sql.ToString(),this.sqlConn);
                //sqlCmd.Parameters.AddWithValue
            DataTable _dt = null;

            using (SqlDataReader dr = sqlCmd.ExecuteReader())
            {
                _dt = new DataTable();
                _dt.Load(dr);
            }

            return _dt;
        }

    }
}
