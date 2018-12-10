Imports System.Activities
Imports Advantech.Myadvantech.DataAccess

Public NotInheritable Class NotifyRequestor
    Inherits NativeActivity

    Public Property IsNotifyReject As InArgument(Of Boolean)
    Public Property IsAEUCMApproved As InArgument(Of Boolean)
    Property myApplicationId() As InArgument(Of String)

    Protected Overrides Sub Execute(context As NativeActivityContext)
        Dim curAppID As String = context.GetValue(Me.myApplicationId)

        Dim _IsNotifyReject As Boolean = context.GetValue(Me.IsNotifyReject)
        Dim _IsAEUCMApproved As Boolean = context.GetValue(Me.IsAEUCMApproved)
        Dim cueeApp As SA_APPLICATION = AppUtil.getAppByID(curAppID)
        If cueeApp Is Nothing Then Exit Sub
        If _IsNotifyReject Then
            Dim Subject = String.Format("Your application new SAP account has been rejected by  {0}. AplicationNO: {1}", cueeApp.REJECTED_BY, cueeApp.AplicationNO)
            Dim msgBody As New System.Text.StringBuilder
            With msgBody
                .AppendFormat("Dear {0},<br/>", Common.GetNameVonEmail(cueeApp.REQUEST_BY))
                .AppendFormat("The reason of denial is "" {1} "", Please <a href=""{0}"">click</a> to modify the detail. Thanks.",
                              "http://172.20.1.30:4002/" + String.Format("/admin/ATW/CreateSAPAccount.aspx?ID={0}", _
                                                                                             cueeApp.ID), cueeApp.COMMENT)
            End With
            AppUtil.SendMail("myadvantech@advantech.com", cueeApp.REQUEST_BY, cueeApp.REJECTED_BY, "", Subject, msgBody.ToString())
            Dim currInStanceID As String = context.WorkflowInstanceId.ToString()
            AppUtil.UpdateApplicationStatus(curAppID, AccountWorkFlowStatus.Reject, currInStanceID)
        End If
        If _IsAEUCMApproved Then
            Dim Subject = String.Format("Your application new SAP account has been approved by {0}. Company Name: {1}({2})", Common.GetNameVonEmail(cueeApp.APPROVED_BY), cueeApp.SholdToX.CompanyID, cueeApp.AplicationNO)
            Dim msgBody As New System.Text.StringBuilder
            With msgBody
                .AppendFormat("Dear {0},<br/>", Common.GetNameVonEmail(cueeApp.REQUEST_BY))
                .AppendFormat("New ERP ID is ""{0}"", Company Name : {1}. Thanks.",
                          cueeApp.SholdToX.CompanyID, cueeApp.SholdToX.SA_KNA1.FirstOrDefault().Kunnr)
            End With
            AppUtil.SendMail("myadvantech@advantech.com", cueeApp.REQUEST_BY, cueeApp.APPROVED_BY, "", Subject, msgBody.ToString())
            Dim currInStanceID As String = context.WorkflowInstanceId.ToString()
            AppUtil.UpdateApplicationStatus(curAppID, AccountWorkFlowStatus.Approved, currInStanceID)
        End If
    End Sub

    Sub OnReadComplete(context As NativeActivityContext, bookmark As Bookmark, state As Object)
        Dim CMApprovalDetail1 = CType(state, CMApprovalDetail)
        If Not CMApprovalDetail1.IsApproved Then
        End If
        context.SetValue(Me.IsAEUCMApproved, CMApprovalDetail1.IsApproved)
    End Sub

    Protected Overrides ReadOnly Property CanInduceIdle() As Boolean
        Get
            Return True
        End Get
    End Property

End Class
