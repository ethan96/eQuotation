Imports System.Activities
Imports System.Diagnostics
Imports System.Linq
Imports System.Threading
Imports System.Data.SqlClient
Imports System.Configuration
Imports Advantech.Myadvantech.DataAccess

Public Class Common

    Public Shared Sub CallFlow(ByVal AppID As String)
        Dim cueeApp As SA_APPLICATION = AppUtil.getAppByID(AppID)
        If cueeApp Is Nothing Then Exit Sub
        If cueeApp.WFInstanceID Is Nothing OrElse String.IsNullOrEmpty(cueeApp.WFInstanceID.Trim()) Then
            StartFlow(AppID)
        Else
            Dim dt As DataTable = dbGetDataTable(String.Empty, String.Format("select top 1  Id,BlockingBookmarks  from   [System.Activities.DurableInstancing].InstancesTable where id='{0}'", cueeApp.WFInstanceID))
            If dt.Rows.Count = 1 Then
                Dim bookmark As String = dt.Rows(0)("BlockingBookmarks").ToString()
                Dim subbookmark As String = bookmark.Substring(1, bookmark.IndexOf(":") - 1)
                ResumeGPFlow(cueeApp.WFInstanceID, subbookmark, cueeApp.ID.ToString())
            End If
        End If
    End Sub
    Shared Sub StartFlow(ByVal AppID As String)
        Dim Inputs As New Dictionary(Of String, Object)
        With Inputs
            .Add("currApplicationId", AppID)
        End With
        Dim wfApp = GetWorkflowApplication(Inputs)
        wfApp.Run()
    End Sub
    Shared Sub ResumeGPFlow(ByVal InstanceID As String, ByVal BookmarkName As String, ByVal appid As String)
        Dim Inputs As New Dictionary(Of String, Object)
        With Inputs
            .Add("currApplicationId", appid)
        End With
        Dim wfApp = GetWorkflowApplication(Nothing)
        wfApp.Load(New Guid(InstanceID))
        wfApp.ResumeBookmark(BookmarkName, Nothing)
        wfApp.Run()
        'Dim bb = wfApp.GetBookmarks.FirstOrDefault()
    End Sub
    Shared Function GetWorkflowApplication(ByVal input As Dictionary(Of String, Object)) As WorkflowApplication
        Const connectionString = "Data Source=aclecampaign\MATEST;Initial Catalog=WF45GettingStartedTutorial;Persist Security Info=True;User ID=b2bsa;Password=@dvantech!"
        Dim store As New DurableInstancing.SqlWorkflowInstanceStore(connectionString)
        Dim AccountFlowIndentity As New WorkflowIdentity With {.Name = "CreateSAPAccountFlow", .Version = New Version(1, 0, 0, 1)}
        WorkflowApplication.CreateDefaultInstanceOwner(store, Nothing, WorkflowIdentityFilter.Any)
        Dim wfApp As WorkflowApplication = Nothing
        If input Is Nothing Then
            wfApp = New WorkflowApplication(New NewSAPCustomerFlow.Activity1, AccountFlowIndentity)
        Else
            wfApp = New WorkflowApplication(New NewSAPCustomerFlow.Activity1, input, AccountFlowIndentity)
        End If

        'IC 增加Tracking代碼
        Dim WorkflowQuery As New Tracking.WorkflowInstanceQuery
        With WorkflowQuery
            .States.Add("*")
        End With
        Dim ActivityQuery As New Tracking.ActivityStateQuery
        With ActivityQuery
            .ActivityName = "*"
            .States.Add("*")
            .Variables.Add("*")
            .Arguments.Add("*")
        End With
        Dim FaultQuery As New Tracking.FaultPropagationQuery
        With FaultQuery
            .FaultHandlerActivityName = "*"
            .FaultSourceActivityName = "*"
        End With
        Dim CustomQuery As New Tracking.CustomTrackingQuery
        With CustomQuery
            .ActivityName = "*"
            .Name = "*"
        End With
        Dim TrackingProfile As New Tracking.TrackingProfile()
        With TrackingProfile
            .Queries.Add(WorkflowQuery)
            .Queries.Add(ActivityQuery)
            .Queries.Add(FaultQuery)
            .Queries.Add(CustomQuery)
        End With
        Dim trackingParticipant As New NewSAPCustomerFlow.MyTracking()
        trackingParticipant.TrackingProfile = TrackingProfile
        wfApp.Extensions.Add(trackingParticipant)
        wfApp.InstanceStore = store
        wfApp.Completed = _
    Sub(e As WorkflowApplicationCompletedEventArgs)
        If e.CompletionState = ActivityInstanceState.Faulted Then
            UpdateStatus(String.Format("Workflow Terminated. Exception: {0}" & vbCrLf & "{1}", _
                e.TerminationException.GetType().FullName, _
                e.TerminationException.Message))
        ElseIf e.CompletionState = ActivityInstanceState.Canceled Then
            UpdateStatus("Workflow Canceled.")
        Else
            'Dim LDR_Email As String = Convert.ToString(e.Outputs("LDR_Email"))
            'Dim FinalSBU As String = Convert.ToString(e.Outputs("FinalSBU"))
            Console.WriteLine("FinalSBU:{0},LDR:{1}", "", "")
        End If
        'GameOver()  
        Dim aa As String = ""
    End Sub
        '流产的；夭折的
        wfApp.Aborted = _
            Sub(e As WorkflowApplicationAbortedEventArgs)
                UpdateStatus(String.Format("Workflow Aborted. Exception: {0}" & vbCrLf & "{1}", _
                    e.Reason.GetType().FullName, _
                    e.Reason.Message))
            End Sub

        wfApp.OnUnhandledException = _
            Function(e As WorkflowApplicationUnhandledExceptionEventArgs)
                UpdateStatus(String.Format("Unhandled Exception: {0}" & vbCrLf & "{1}", _
                    e.UnhandledException.GetType().FullName, _
                    e.UnhandledException.Message))
                'GameOver()
                Return UnhandledExceptionAction.Terminate
            End Function
        '停顿的
        wfApp.PersistableIdle = _
            Function(e As WorkflowApplicationIdleEventArgs)

                'Send the current WriteLine outputs to the status window.
                Dim writers = e.GetInstanceExtensions(Of IO.StringWriter)()
                For Each writer In writers
                    UpdateStatus(writer.ToString())
                Next
                Return PersistableIdleAction.Unload
            End Function

        Return wfApp
    End Function
    Public Shared Sub UpdateStatus(msg As String)
        Console.WriteLine("UpdateStatus msg:" + msg)
    End Sub
    Public Shared Function dbGetDataTable(ByVal ConnectionName As String, ByVal strSqlCmd As String) As DataTable
        Const connectionString = "Data Source=aclecampaign\MATEST;Initial Catalog=WF45GettingStartedTutorial;Persist Security Info=True;User ID=b2bsa;Password=@dvantech!"
        Dim g_adoConn As New SqlConnection(connectionString)
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
    Public Shared Function dbExecuteNoQuery(ByVal ConnectionStringName As String, ByVal strSqlCmd As String) As Integer
        Const connectionString = "Data Source=aclmyadmin;Initial Catalog=MyAdmin;Persist Security Info=True;User ID=b2bsa;Password=@dvantech!;Application Name=MyAdvantech;Failover Partner=aclmyadmin;async=true;Connect Timeout=300;pooling='true'"
        Dim g_adoConn As New SqlConnection(connectionString)

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
        Return 1
    End Function
    Public Shared Function GetNameVonEmail(ByVal email As String) As String
        If email.Contains("@") Then
            Dim strNamePart As String = Split(email, "@")(0)
            '字首改大寫，其他改小寫 ex:tc.chen ->Tc.Chen
            Dim strNameArry() As String = Split(strNamePart, ".")
            For i As Integer = 0 To strNameArry.Length - 1
                If strNameArry(i).Length > 1 Then
                    strNameArry(i) = strNameArry(i).Substring(0, 1).ToUpper() + strNameArry(i).Substring(1).ToLower()
                Else
                    strNameArry(i) = strNameArry(i).ToUpper()
                End If
            Next
            strNamePart = String.Join(".", strNameArry)
            Return strNamePart
        Else
            Return email
        End If
    End Function
End Class
