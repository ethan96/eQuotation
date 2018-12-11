Imports System.Activities

Public NotInheritable Class SendMail
    Inherits NativeActivity
    
    'Define an activity input argument of type String
    Property Quoteid() As InArgument(Of String)
    Property IsCreateBookMark() As InArgument(Of Boolean )
    ' If your activity returns a value, derive from CodeActivity(Of TResult)
    ' and return the value from the Execute method.
    Protected Overloads Overrides Sub Execute(context As NativeActivityContext)
        'Obtain the runtime value of the Text input argument
        Dim _Quoteid As String = context.GetValue(Me.Quoteid)
        Advantech.Myadvantech.Business.QuoteBusinessLogic.SendGPmail(_Quoteid)
        If context.GetValue(Me.IsCreateBookMark) Then
            context.CreateBookmark("Waiting for CM's approval", New BookmarkCallback(AddressOf OnReadComplete))

        End If
   End Sub
    Sub OnReadComplete(context As NativeActivityContext, bookmark As Bookmark, state As Object)
        Dim curAppID As String = context.GetValue(Me.Quoteid)
        Dim currInStanceID As String = context.WorkflowInstanceId.ToString()
        'AppUtil.UpdateApplicationStatus(curAppID, AccountWorkFlowStatus.NotifyCM, currInStanceID)
        ''  Dim CMApprovalDetail1 = CType(state, CMApprovalDetail)
        'context.SetValue(Me.IsAEUCMApproved, True)
        'Dim _IsToLeader As Boolean = context.GetValue(Me.IsToLeader)
        'If _IsToLeader Then

        'End If
    End Sub
    Protected Overrides ReadOnly Property CanInduceIdle() As Boolean
        Get
            Return True
        End Get
    End Property
End Class
