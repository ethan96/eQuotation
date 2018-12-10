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
        dbCmd.CommandTimeout = 5 * 60
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
Public NotInheritable Class dbUtil2

    Private Shared parmCache As Hashtable = Hashtable.Synchronized(New Hashtable())

    'Nada : for batch options share one connection, need manually cloce conn
    Public Overloads Shared Function ExecuteNonQuery(ByVal conn As SqlClient.SqlConnection _
                                                     , ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters() As SqlParameter) As Integer
        Try
            Dim cmd As SqlCommand = New SqlCommand()
            PrepareCommand(cmd, Nothing, cmdType, cmdText, commandParameters)
            If conn.State <> ConnectionState.Open Then
                conn.Open()
            End If
            cmd.Connection = conn
            Dim val As Integer = cmd.ExecuteNonQuery()
            cmd.Parameters.Clear()
            Return val
        Catch ex As Exception
            If Not IsNothing(conn) Then
                conn.Close()
            End If
            Throw ex
        End Try
    End Function
    'Nada : for batch options share one connection, need manually cloce conn
    Public Overloads Shared Function ExecuteScalar(ByVal conn As SqlClient.SqlConnection, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters() As SqlParameter) As Object
        Dim cmd As SqlCommand = New SqlCommand()
        Try
            PrepareCommand(cmd, Nothing, cmdType, cmdText, commandParameters)
            If conn.State <> ConnectionState.Open Then
                conn.Open()
            End If
            cmd.Connection = conn
            Dim val As Object = cmd.ExecuteScalar()
            cmd.Parameters.Clear()
            Return val
        Catch ex As Exception
            If Not IsNothing(conn) Then
                conn.Close()
            End If
            Throw ex
        End Try
    End Function
    'Nada : for batch options share one connection, need manually cloce conn
    Public Overloads Shared Function getDT(ByVal conn As SqlClient.SqlConnection, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters() As SqlParameter) As DataTable
        Dim cmd As SqlCommand = New SqlCommand()
        Try
            Dim dt As New DataTable
            PrepareCommand(cmd, Nothing, cmdType, cmdText, commandParameters)
            If conn.State <> ConnectionState.Open Then
                conn.Open()
            End If
            cmd.Connection = conn
            Dim da As New SqlClient.SqlDataAdapter(cmd)
            da.Fill(dt)
            cmd.Parameters.Clear()
            Return dt
        Catch ex As Exception
            If Not IsNothing(conn) Then
                conn.Close()
            End If
            Throw ex
        End Try
    End Function

    Public Overloads Shared Function ExecuteNonQuery(ByVal connectionName As String, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters() As SqlParameter) As Integer
        Dim cmd As SqlCommand = New SqlCommand()
        Using conn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings(connectionName).ConnectionString)
            PrepareCommand(cmd, Nothing, cmdType, cmdText, commandParameters)
            If conn.State <> ConnectionState.Open Then
                conn.Open()
            End If
            cmd.Connection = conn
            Dim val As Integer = cmd.ExecuteNonQuery()
            cmd.Parameters.Clear()
            Return val
        End Using
    End Function
    Public Overloads Shared Function ExecuteScalar(ByVal connectionName As String, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters() As SqlParameter) As Object
        Dim cmd As SqlCommand = New SqlCommand()
        Using connection As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings(connectionName).ConnectionString)
            PrepareCommand(cmd, Nothing, cmdType, cmdText, commandParameters)
            If connection.State <> ConnectionState.Open Then
                connection.Open()
            End If
            cmd.Connection = connection
            Dim val As Object = cmd.ExecuteScalar()
            cmd.Parameters.Clear()
            Return val
        End Using
    End Function
    'Public Overloads Shared Function ExecuteNonQuery(ByVal connection As SqlConnection, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters() As SqlParameter) As Integer
    '    Dim cmd As SqlCommand = New SqlCommand()
    '    PrepareCommand(cmd, connection, Nothing, cmdType, cmdText, commandParameters)
    '    Dim val As Integer = cmd.ExecuteNonQuery()
    '    cmd.Parameters.Clear()
    '    Return val
    'End Function
    'Public Overloads Shared Function ExecuteNonQuery(ByVal trans As SqlTransaction, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters() As SqlParameter) As Integer
    '    Dim cmd As SqlCommand = New SqlCommand()
    '    PrepareCommand(cmd, trans.Connection, Nothing, cmdType, cmdText, commandParameters)
    '    Dim val As Integer = cmd.ExecuteNonQuery()
    '    cmd.Parameters.Clear()
    '    Return val
    'End Function

    Public Overloads Shared Function ExecuteReader(ByVal connectionName As String, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters() As SqlParameter) As SqlDataReader
        Dim cmd As SqlCommand = New SqlCommand()
        Dim conn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings(connectionName).ConnectionString)
        Try
            PrepareCommand(cmd, Nothing, cmdType, cmdText, commandParameters)
            If conn.State <> ConnectionState.Open Then
                conn.Open()
            End If
            cmd.Connection = conn
            Dim rdr As SqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            cmd.Parameters.Clear()
            Return rdr
        Catch ex As Exception
            If Not IsNothing(conn) Then
                conn.Close()
            End If
            Throw ex
        End Try
    End Function
    'Public Overloads Shared Function ExecuteTb(ByVal connectionName As String, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters() As SqlParameter) As SqlDataReader
    '    Dim cmd As SqlCommand = New SqlCommand()
    '    Dim conn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings(connectionName).ConnectionString)
    '    Try
    '        PrepareCommand(cmd, Nothing, cmdType, cmdText, commandParameters)
    '        Dim rdr As SqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
    '        cmd.Parameters.Clear()
    '        Return rdr
    '    Catch
    '        conn.Close()
    '        Throw
    '    End Try
    'End Function

    Public Overloads Shared Function getDT(ByVal connectionName As String, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters() As SqlParameter) As DataTable
        Dim cmd As SqlCommand = New SqlCommand()
        Dim conn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings(connectionName).ConnectionString)
        Try
            Dim dt As New DataTable
            PrepareCommand(cmd, Nothing, cmdType, cmdText, commandParameters)
            If conn.State <> ConnectionState.Open Then
                conn.Open()
            End If
            cmd.Connection = conn
            Dim da As New SqlClient.SqlDataAdapter(cmd)
            da.Fill(dt)
            cmd.Parameters.Clear()
            Return dt
        Catch ex As Exception
            Throw ex
        Finally
            If Not IsNothing(conn) Then
                conn.Close()
            End If
        End Try
    End Function

    'Public Overloads Shared Function ExecuteScalar(ByVal connection As SqlConnection, ByVal cmdType As CommandType, ByVal cmdText As String, ByVal ParamArray commandParameters() As SqlParameter) As Object
    '    Dim cmd As SqlCommand = New SqlCommand()
    '    PrepareCommand(cmd, connection, Nothing, cmdType, cmdText, commandParameters)
    '    Dim val As Object = cmd.ExecuteScalar()
    '    cmd.Parameters.Clear()
    '    Return val
    'End Function



    Private Shared Sub PrepareCommand(ByVal cmd As SqlCommand, ByVal trans As SqlTransaction, _
                                       ByVal cmdType As CommandType, ByVal cmdText As String, ByVal cmdParms() As SqlParameter)
        cmd.CommandText = cmdText
        If Not (trans Is Nothing) Then
            cmd.Transaction = trans
        End If
        cmd.CommandType = cmdType
        If Not (cmdParms Is Nothing) Then
            cmd.Parameters.AddRange(cmdParms)
        End If
    End Sub

End Class