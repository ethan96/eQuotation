Imports System.Data.SqlClient
Imports System.Configuration

Public Class dbUtil
    Public Shared Function dbGetDataTable( _
    ByVal ConnectionName As String, _
    ByVal strSqlCmd As String) As DataTable
        Dim g_adoConn As New SqlConnection(ConfigurationManager.ConnectionStrings(ConnectionName).ConnectionString)
        Dim dt As New DataTable
        Dim da As New SqlDataAdapter(strSqlCmd, g_adoConn)
        da.SelectCommand.CommandTimeout = 5 * 60
        Try
            da.Fill(dt)
        Catch ex As Exception
            g_adoConn.Close() : Throw New Exception(ex.ToString() + vbTab + "sql:" + strSqlCmd)
        End Try
        g_adoConn.Close() : g_adoConn = Nothing
        Return dt
    End Function

    Public Shared Function dbExecuteNoQuery( _
    ByVal ConnectionStringName As String, _
    ByVal strSqlCmd As String) As Integer
        Dim g_adoConn As New SqlConnection(ConfigurationManager.ConnectionStrings(ConnectionStringName).ConnectionString)
        Dim dbCmd As SqlClient.SqlCommand = g_adoConn.CreateCommand()
        dbCmd.Connection = g_adoConn : dbCmd.CommandText = strSqlCmd
        Dim retInt As Integer = -1
        For i As Integer = 0 To 3
            Try
                g_adoConn.Open()
                Exit For
            Catch ex As SqlException
                If i = 3 Then Throw ex
                Threading.Thread.Sleep(100)
            End Try
        Next
        'Using tran As SqlTransaction = g_adoConn.BeginTransaction
        Try
            'dbCmd.Transaction = tran
            retInt = dbCmd.ExecuteNonQuery()
            'tran.Commit()
        Catch ex As Exception
            'tran.Rollback()
            g_adoConn.Close() : Throw New Exception(ex.ToString + " sql:" + strSqlCmd)
        End Try
        'End Using
        g_adoConn.Close() : Return retInt
    End Function

    Public Shared Function dbExecuteScalar(ByVal ConnectionName As String, ByVal strSqlCmd As String) As Object
        Dim g_adoConn As New SqlConnection(ConfigurationManager.ConnectionStrings(ConnectionName).ConnectionString)
        For i As Integer = 0 To 3
            Try
                g_adoConn.Open()
                Exit For
            Catch ex As SqlException
                If i = 3 Then Throw ex
                Threading.Thread.Sleep(100)
            End Try
        Next
        Dim dbCmd As SqlClient.SqlCommand = g_adoConn.CreateCommand()
        dbCmd.CommandType = CommandType.Text : dbCmd.CommandText = strSqlCmd : dbCmd.CommandTimeout = 5 * 60
        Dim retObj As Object = Nothing
        Try
            retObj = dbCmd.ExecuteScalar()
        Catch ex As Exception
            g_adoConn.Close() : Throw ex
        End Try
        g_adoConn.Close() : Return retObj
    End Function
End Class
