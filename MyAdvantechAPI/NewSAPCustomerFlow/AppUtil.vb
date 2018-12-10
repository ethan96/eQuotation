Imports Advantech.Myadvantech.DataAccess
Imports Advantech.Myadvantech.Business

Public Class AppUtil
    Public Shared Function getAppByID(ByVal AppID As String) As SA_APPLICATION
        Return MyAdminBusinessLogic.getApplicationByID(AppID)
    End Function
    Public Shared Function getAppByInstanceID(ByVal InstanceID As String) As SA_APPLICATION
        Return MyAdminBusinessLogic.getApplicationByWFInstanceID(InstanceID)
    End Function
    Public Shared Function updateApp(ByVal InstanceID As String, ByVal status As AccountWorkFlowStatus) As Boolean
        Dim currAPP As SA_APPLICATION = getAppByInstanceID(InstanceID)
        If currAPP IsNot Nothing Then
            currAPP.STATUS = Convert.ToInt32(status)
            currAPP.Update()
        End If
        Return True
    End Function
    Public Shared Function IsApprovedOrRejected(ByVal appid As String) As Boolean
        Dim currAPP As SA_APPLICATION = getAppByID(appid)
        If currAPP IsNot Nothing Then
            If currAPP.StatusX = AccountWorkFlowStatus.Reject Then
                Return False
            End If
        End If
        Return True
    End Function
    Public Shared Function IsHaveNextLeader(ByVal appid As String) As Boolean
        Dim _appid As Integer = Integer.Parse(appid)
        Dim pls As List(Of SA_Proposal) = MyAdminBusinessLogic.getProposal(_appid)
        For Each p As SA_Proposal In pls
            If p.Status = 0 Then Return True
        Next
        Return False
    End Function
    Public Shared Function IsCreateShipTo(ByVal AppID As String) As Boolean
        Dim currAPP As SA_APPLICATION = MyAdminBusinessLogic.getApplicationByID(AppID)
        If currAPP IsNot Nothing Then
            If currAPP.ShipToX IsNot Nothing Then
                Return True
            End If
        End If
        Return False
    End Function
    Public Shared Function IsCreateBillTo(ByVal AppID As String) As Boolean
        Dim currAPP As SA_APPLICATION = MyAdminBusinessLogic.getApplicationByID(AppID)
        If currAPP IsNot Nothing Then
            If currAPP.BillToX IsNot Nothing Then
                Return True
            End If
        End If
        Return False
    End Function
    Public Shared Function IsCreateSiebelAccount(ByVal AppID As String) As Boolean
        Dim currAPP As SA_APPLICATION = MyAdminBusinessLogic.getApplicationByID(AppID)
        If currAPP IsNot Nothing Then
            If currAPP.SholdToX IsNot Nothing AndAlso currAPP.SholdToX.IsExistSiebel = False Then
                Return True
            End If
        End If
        Return False
    End Function
    Public Shared Function UpdateApplicationStatus(ByVal AppID As String, ByVal Status As AccountWorkFlowStatus, Optional WFStanceID As String = "")
        Try
            Dim sql As String = String.Empty
            If Not String.IsNullOrEmpty(WFStanceID.Trim()) Then
                sql = String.Format("UPDATE  SA_APPLICATION SET [STATUS] ={1} ,[WFInstanceID]='{2}' WHERE ID ={0}", AppID, Convert.ToInt32(Status), WFStanceID)
            Else
                sql = String.Format("UPDATE  SA_APPLICATION SET [STATUS] ={1}  WHERE ID ={0}", AppID, Convert.ToInt32(Status))
            End If
            Common.dbExecuteNoQuery("MYADMIN", sql)
            Return True
        Catch ex As Exception
        End Try
        Return False
    End Function
    Public Shared Function UpdateProposalStatus(ByVal ID As String, ByVal Status As Integer)
        Try
            Dim sql As String = String.Empty

            sql = String.Format("update  SA_Proposal set status = {0} where id={1}", Status, ID)

            Common.dbExecuteNoQuery("MYADMIN", sql)
            Return True
        Catch ex As Exception
        End Try
        Return False
    End Function
    Public Shared Function IsTesting() As Boolean
        Return False
    End Function
    Public Shared Sub SendMail(ByVal fromuser As String, ByVal touser As String, ByVal cc As String, ByVal bcc As String, ByVal subject As String, ByVal body As String)
        Dim smtpClient1 As New Net.Mail.SmtpClient("172.20.0.76")
        'touser = "ming.zhao@advantech.com.cn"
        Dim NotifyMail As New Net.Mail.MailMessage()
        If String.IsNullOrEmpty(cc) Then
            cc = "myadvantech@advantech.com"
        End If
        If String.IsNullOrEmpty(bcc) Then bcc = "myadvantech@advantech.com"
        With NotifyMail
            NotifyMail.From = New Net.Mail.MailAddress(fromuser)
            Dim tolist As String() = touser.Split(New Char() {","}, System.StringSplitOptions.RemoveEmptyEntries)
            For Each user As String In tolist
                NotifyMail.To.Add(New Net.Mail.MailAddress(user))
            Next
            Dim cclist As String() = cc.Split(New Char() {","}, System.StringSplitOptions.RemoveEmptyEntries)
            For Each user As String In cclist
                NotifyMail.CC.Add(New Net.Mail.MailAddress(user))
            Next
            Dim bcclist As String() = bcc.Split(New Char() {","}, System.StringSplitOptions.RemoveEmptyEntries)
            For Each user As String In bcclist
                NotifyMail.Bcc.Add(New Net.Mail.MailAddress(user))
            Next
            .SubjectEncoding = Text.Encoding.UTF8
            .Subject = subject
            .Body = body.ToString()
            .BodyEncoding = Text.Encoding.UTF8
            .IsBodyHtml = True
        End With
        smtpClient1.Send(NotifyMail)
    End Sub
End Class
