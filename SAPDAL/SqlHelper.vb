Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Public NotInheritable Class sqlhelper
    'Public Shared ReadOnly ConnectionStringProfile As String = "Data Source=172.21.128.5;Initial Catalog=nadatest;Persist Security Info=True;User ID=b2bsa;Password=2222;async=true;Connect Timeout=180;pooling='true'"
    Private Shared parmCache As Hashtable = Hashtable.Synchronized(New Hashtable())




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
        Catch
            conn.Close()
            Throw
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
        Catch
            conn.Close()
            Throw
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
