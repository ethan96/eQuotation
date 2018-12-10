Imports System.Activities
Imports Advantech.Myadvantech.DataAccess
Imports Advantech.Myadvantech.Business

Public NotInheritable Class NotifyAndWaitCM
    Inherits NativeActivity

    'Define an activity input argument of type String
    Property TicketId() As InArgument(Of String)
    Property CustomerId() As InArgument(Of String)
    Property RequestedBy() As InArgument(Of String)
    Public Property IsAEUCMApproved As OutArgument(Of Boolean)
    Property InStanceID() As InArgument(Of String)
    Property myApplicationId() As InArgument(Of String)
    Property IsToLeader() As InArgument(Of Boolean)
    Protected Overloads Overrides Sub Execute(context As NativeActivityContext)
        Dim currInStanceID As String = context.WorkflowInstanceId.ToString()
        Dim curAppID As String = context.GetValue(Me.myApplicationId)
        Dim _IsToLeader As Boolean = context.GetValue(Me.IsToLeader)
        Dim currApp As SA_APPLICATION = AppUtil.getAppByID(curAppID)
        If currApp Is Nothing Then Exit Sub
        Dim Subject = String.Format("Request to create new SAP customer by {0}, ApplicationNO is  {1}", currApp.REQUEST_BY, currApp.AplicationNO)
        Dim msgBody As New System.Text.StringBuilder
        
        If _IsToLeader Then
            Dim _appid As Integer = Integer.Parse(curAppID)
            Dim pl As SA_Proposal = MyAdminBusinessLogic.getProposal(_appid).OrderBy(Function(p) p.CreateBy).FirstOrDefault(Function(p) p.Status = 0)
            If pl IsNot Nothing Then
                With msgBody
                    .AppendFormat("Dear {0},<br/>", Common.GetNameVonEmail(pl.MailTo))
                    .AppendFormat("{0} requested to create new SAP account. " + _
                                  "Please click <a href='http://172.20.1.30:4002/admin/ATW/CreateSAPAccount.aspx?ID={1}'>[{2}]</a> to see detail.", _
                                  Common.GetNameVonEmail(currApp.REQUEST_BY), currApp.ID, currApp.AplicationNO)
                End With
                msgBody.AppendFormat("<br/>{0}: {1}", Common.GetNameVonEmail(pl.CreateBy), pl.Comment)
                AppUtil.SendMail("myadvantech@advantech.com", pl.MailTo, "", "", Subject, msgBody.ToString())
                context.CreateBookmark(String.Format("Waiting for {0}'s approval", pl.MailTo), New BookmarkCallback(AddressOf OnReadComplete))
                AppUtil.UpdateProposalStatus(pl.ID, 1)
            End If
        Else
            With msgBody
                .Append("Dear Polar,<br/>")
                .AppendFormat("{0} requested to create new SAP account. " + _
                              "Please click <a href='http://172.20.1.30:4002/admin/ATW/CreateSAPAccount.aspx?ID={1}'>[{2}]</a> to see detail.", _
                              Common.GetNameVonEmail(currApp.REQUEST_BY), currApp.ID, currApp.AplicationNO)
            End With
            'Polar.Yu@advantech.com.tw
            AppUtil.SendMail("myadvantech@advantech.com", "Polar.Yu@advantech.com.tw,Vanage.Lin@advantech.com.tw", "", "myadvantech@advantech.com", Subject, msgBody.ToString())
            context.CreateBookmark("Waiting for CM's approval", New BookmarkCallback(AddressOf OnReadComplete))
            AppUtil.UpdateApplicationStatus(curAppID, AccountWorkFlowStatus.NotifyCM, currInStanceID)
        End If
    End Sub

    Sub OnReadComplete(context As NativeActivityContext, bookmark As Bookmark, state As Object)
        Dim curAppID As String = context.GetValue(Me.myApplicationId)
        Dim currInStanceID As String = context.WorkflowInstanceId.ToString()
        AppUtil.UpdateApplicationStatus(curAppID, AccountWorkFlowStatus.NotifyCM, currInStanceID)
        '  Dim CMApprovalDetail1 = CType(state, CMApprovalDetail)
        context.SetValue(Me.IsAEUCMApproved, True)
        Dim _IsToLeader As Boolean = context.GetValue(Me.IsToLeader)
        If _IsToLeader Then

        End If
    End Sub

    Protected Overrides ReadOnly Property CanInduceIdle() As Boolean
        Get
            Return True
        End Get
    End Property

End Class
