Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports System.Reflection
Imports System.Configuration
Imports System.IO

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

    Public Shared Function dbGetReaderAsync( _
    ByVal ConnectionName As String, _
    ByVal strSqlCmd As String, ByRef g_adoConn As SqlConnection, ByRef dbCmd As SqlCommand) As IAsyncResult
        g_adoConn = New SqlConnection(ConfigurationManager.ConnectionStrings(ConnectionName).ConnectionString)
        dbCmd = g_adoConn.CreateCommand()
        dbCmd.Connection = g_adoConn : dbCmd.CommandText = strSqlCmd
        g_adoConn.Open()
        Return dbCmd.BeginExecuteReader()
    End Function

    Public Shared Function dbExecuteNoQuery2(ByVal ConnectionStringName As String, ByVal strSqlCmd As String, Optional ByVal Parameters As SqlParameter() = Nothing) As Integer
        Dim g_adoConn As New SqlConnection(ConfigurationManager.ConnectionStrings(ConnectionStringName).ConnectionString)
        Dim dbCmd As SqlClient.SqlCommand = g_adoConn.CreateCommand()
        dbCmd.Connection = g_adoConn : dbCmd.CommandText = strSqlCmd
        Dim retInt As Integer = -1
        If Parameters IsNot Nothing AndAlso Parameters.Length > 0 Then
            dbCmd.Parameters.AddRange(Parameters)
        End If
        For i As Integer = 0 To 3
            Try
                g_adoConn.Open()
                Exit For
            Catch ex As SqlException
                If i = 3 Then Throw ex
                Threading.Thread.Sleep(100)
            End Try
        Next
        Try
            retInt = dbCmd.ExecuteNonQuery()
        Catch ex As Exception
            g_adoConn.Close() : Throw New Exception(ex.ToString + vbTab + "sql:" + strSqlCmd)
        End Try
        g_adoConn.Close() : Return retInt
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
    Public Shared Function dbExecuteNoQueryAsync( _
    ByVal ConnectionStringName As String, _
    ByVal strSqlCmd As String, ByRef g_adoConn As SqlConnection, ByRef dbCmd As SqlCommand) As IAsyncResult
        g_adoConn = New SqlConnection(ConfigurationManager.ConnectionStrings(ConnectionStringName).ConnectionString)
        dbCmd = g_adoConn.CreateCommand()
        dbCmd.Connection = g_adoConn : dbCmd.CommandText = strSqlCmd
        Dim ar As IAsyncResult = Nothing
        g_adoConn.Open()
        Return dbCmd.BeginExecuteNonQuery()
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
    Public Shared Function Reader2DataTable(ByVal _reader As System.Data.SqlClient.SqlDataReader) As DataTable
        Dim _table As System.Data.DataTable = _reader.GetSchemaTable()
        Dim _dt As New System.Data.DataTable()
        Dim _dc As System.Data.DataColumn
        Dim _row As System.Data.DataRow
        Dim _al As New System.Collections.ArrayList()
        For i As Integer = 0 To _table.Rows.Count - 1
            _dc = New System.Data.DataColumn()
            If Not _dt.Columns.Contains(_table.Rows(i)("ColumnName").ToString()) Then
                _dc.ColumnName = _table.Rows(i)("ColumnName").ToString()
                _dc.Unique = Convert.ToBoolean(_table.Rows(i)("IsUnique"))
                _dc.AllowDBNull = Convert.ToBoolean(_table.Rows(i)("AllowDBNull"))
                _dc.[ReadOnly] = Convert.ToBoolean(_table.Rows(i)("IsReadOnly"))
                _al.Add(_dc.ColumnName)
                _dt.Columns.Add(_dc)
            End If
        Next
        While _reader.Read()
            _row = _dt.NewRow()
            For i As Integer = 0 To _al.Count - 1
                _row(DirectCast(_al(i), String)) = _reader(DirectCast(_al(i), String))
            Next
            _dt.Rows.Add(_row)
        End While
        If _reader.IsClosed = False Then _reader.Close()
        Return _dt
    End Function
    Public Shared Function FindSqlAgentJob(ByVal SqlServerName As String, ByVal JobName As String) As Microsoft.SqlServer.Management.Smo.Agent.Job
        Dim srv As New Microsoft.SqlServer.Management.Smo.Server(SqlServerName)
        If srv.JobServer.Jobs.Contains(JobName) Then
            Return srv.JobServer.Jobs(JobName)
        Else
            Return Nothing
        End If
    End Function

    Public Shared Function DeleteSqlAgentJob(ByVal SqlServerName As String, ByVal JobName As String) As Boolean
        Dim job As Microsoft.SqlServer.Management.Smo.Agent.Job = FindSqlAgentJob(SqlServerName, JobName)
        If job IsNot Nothing Then
            If job.CurrentRunStatus = Microsoft.SqlServer.Management.Smo.Agent.JobExecutionStatus.Idle Then
                job.Drop() : Return True
            Else
                Return False
            End If
        End If
        Return False
    End Function

    Public Shared Function CreateSqlAgentJob( _
    ByVal SqlServerName As String, ByVal JobName As String, ByVal StepCommand As String, _
    ByVal SubSystem As Microsoft.SqlServer.Management.Smo.Agent.AgentSubSystem, _
    ByVal FrequencyType As Microsoft.SqlServer.Management.Smo.Agent.FrequencyTypes, _
    ByVal ActiveStartTimeOfDayTimeSpan As TimeSpan, ByRef ErrMessage As String) As Boolean
        If FindSqlAgentJob(SqlServerName, JobName) IsNot Nothing Then
            ErrMessage = "Job already exist!" : Return False
        End If
        Dim srv As New Microsoft.SqlServer.Management.Smo.Server(SqlServerName)
        Dim job1 As New Microsoft.SqlServer.Management.Smo.Agent.Job(srv.JobServer, JobName)
        Dim step1 As New Microsoft.SqlServer.Management.Smo.Agent.JobStep()
        step1.Name = "Step1"
        job1.JobSteps.Add(step1)
        step1.Command = StepCommand
        step1.SubSystem = SubSystem
        Dim sch1 As New Microsoft.SqlServer.Management.Smo.Agent.JobSchedule(job1, "schedule1")
        sch1.FrequencyTypes = FrequencyType
        sch1.ActiveStartDate = Now
        sch1.ActiveStartTimeOfDay = ActiveStartTimeOfDayTimeSpan
        job1.Create()
        step1.Create()
        sch1.Create()
        job1.ApplyToTargetServer(SqlServerName)
        Return True
    End Function
End Class

