Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Public NotInheritable Class sqlhelper

    Private Shared parmCache As Hashtable = Hashtable.Synchronized(New Hashtable())

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
