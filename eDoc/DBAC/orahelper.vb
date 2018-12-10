Imports Microsoft.VisualBasic
Imports Oracle.DataAccess.Client
Imports System.Configuration
Public Class OraDbUtil
    Public Shared Function dbGetDataTable( _
   ByVal ConnectionName As String, _
   ByVal strSqlCmd As String) As DataTable
        'Dim aa As New Oracle.DataAccess.Client.OracleConnection 
        Dim g_adoConn As New OracleConnection(ConfigurationManager.ConnectionStrings(ConnectionName).ConnectionString)
        Dim dt As New DataTable
        Dim da As New OracleDataAdapter(strSqlCmd, g_adoConn)
        Threading.Thread.CurrentThread.Priority = Threading.ThreadPriority.BelowNormal
        Try
            da.Fill(dt)
        Catch ex As Exception
            g_adoConn.Close() : g_adoConn.Dispose()
            Threading.Thread.CurrentThread.Priority = Threading.ThreadPriority.Normal
            Throw ex
        End Try
        g_adoConn.Close() : g_adoConn = Nothing
        Threading.Thread.CurrentThread.Priority = Threading.ThreadPriority.Normal
        Return dt
    End Function
    Public Shared Function dbExecuteNoQuery( _
   ByVal ConnectionStringName As String, _
    ByVal strSqlCmd As String) As Integer
        Dim g_adoConn As New OracleConnection(ConfigurationManager.ConnectionStrings(ConnectionStringName).ConnectionString)
        Dim dbCmd As OracleCommand = g_adoConn.CreateCommand()
        dbCmd.Connection = g_adoConn : dbCmd.CommandText = strSqlCmd
        Dim retInt As Integer = -1
        g_adoConn.Open()
        'Using tran As OracleTransaction = g_adoConn.BeginTransaction
        Try
            'dbCmd.Transaction = tran
            retInt = dbCmd.ExecuteNonQuery()
            'tran.Commit()
        Catch ex As Exception
            'tran.Rollback()
            g_adoConn.Close() : g_adoConn.Dispose() : Throw ex
        End Try
        'End Using
        g_adoConn.Close() : g_adoConn.Dispose() : Return retInt
    End Function
    Public Shared Function dbExecuteScalar(ByVal ConnectionName As String, ByVal strSqlCmd As String) As Object
        Dim g_adoConn As New OracleConnection(ConfigurationManager.ConnectionStrings(ConnectionName).ConnectionString)
        g_adoConn.Open()
        Dim dbCmd As OracleCommand = g_adoConn.CreateCommand()
        dbCmd.CommandType = CommandType.Text : dbCmd.CommandText = strSqlCmd
        Dim retObj As Object = Nothing
        Try
            retObj = dbCmd.ExecuteScalar()
        Catch ex As Exception
            g_adoConn.Close() : Throw New Exception(strSqlCmd + "." + ex.ToString())
        End Try
        g_adoConn.Close() : Return retObj
    End Function
End Class
