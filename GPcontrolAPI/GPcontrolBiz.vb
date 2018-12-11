Imports System.Web
Imports System.Activities
Imports System.Diagnostics
Imports System.Linq
Imports System.Threading
Imports System.Data.SqlClient
Imports System.Configuration
Imports Advantech.Myadvantech.Business
Imports Advantech.Myadvantech.DataAccess
Imports System.Data.Entity

Public Class GPcontrolBiz


    Public Shared Sub CallFlow(ByVal Quoteid As String, ByVal url As String)
        Dim cueeApp As QuoteApproval = eQuotationContext.Current.QuoteApproval.FirstOrDefault(Function(p) p.QuoteID = Quoteid AndAlso p.WorkFlowID <> "")
        'If cueeApp Is Nothing Then Exit Sub
        If cueeApp Is Nothing OrElse String.IsNullOrEmpty(cueeApp.WorkFlowID.Trim()) Then
            StartFlow(Quoteid, url)
        Else
            Dim dt As DataTable = dbGetDataTable(String.Empty, String.Format("select top 1  Id,BlockingBookmarks  from   [System.Activities.DurableInstancing].InstancesTable where id='{0}'", cueeApp.WorkFlowID))
            If dt.Rows.Count = 1 Then
                Dim bookmark As String = dt.Rows(0)("BlockingBookmarks").ToString()
                Dim subbookmark As String = bookmark.Substring(1, bookmark.IndexOf(":") - 1)
                ResumeGPFlow(cueeApp.WorkFlowID, subbookmark, cueeApp.QuoteID)
            End If
        End If
    End Sub
    Shared Sub StartFlow(ByVal AppID As String, ByVal url As String)
        Dim Inputs As New Dictionary(Of String, Object)
        With Inputs
            .Add("QuoteID", AppID)
            .Add("Url", url)
        End With
        Dim wfApp = GetWorkflowApplication(Inputs)
        wfApp.Run()
    End Sub
    Shared Sub ResumeGPFlow(ByVal InstanceID As String, ByVal BookmarkName As String, ByVal appid As String)
        Dim Inputs As New Dictionary(Of String, Object)
        With Inputs
            .Add("QuoteID", appid)
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
            wfApp = New WorkflowApplication(New GPcontrolAPI.GPcontrol, AccountFlowIndentity)
        Else
            wfApp = New WorkflowApplication(New GPcontrolAPI.GPcontrol, input, AccountFlowIndentity)
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
        Dim trackingParticipant As New GPcontrolAPI.MyTracking()
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
    Public Shared ReadOnly Property Current As GPcontrolBiz
        Get
            Return New GPcontrolBiz()
        End Get
    End Property

    Public Sub TEST()

        Dim AA As QuoteApproval = eQuotationContext.Current.QuoteApproval.Find("453CA3EF-3753-4574-8332-C1C1B479FD0E")
        AA.GPBrief = "567"
        AA.Update()
        Dim B = 56
    End Sub
    Public Shared Function IsApprovedByAll(ByVal Quoteid As String) As Boolean
        Return QuoteBusinessLogic.IsApprovedByAll(Quoteid)
    End Function
    Public Shared Function IsRejected(ByVal Quoteid As String) As Boolean
        Return QuoteBusinessLogic.IsRejected(Quoteid)
    End Function
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
End Class

