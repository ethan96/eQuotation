Imports Advantech.Myadvantech.DataAccess

Public Class MobileApprove
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim MID As String = Request("MID")
        If Not String.IsNullOrEmpty(MID) Then
            Dim QA As Advantech.Myadvantech.DataAccess.QuoteApproval = Nothing
            If MID.StartsWith("MY") Then
                QA = eQuotationContext.Current.QuoteApproval.SingleOrDefault(Function(P) P.MobileYes = MID)
                If QA IsNot Nothing Then
                    QA.ApprovedReason = "MobileApprove:Approved"
                    QA.ApprovedBy = QA.Approver
                    QA.ApprovedDate = Now
                    QA.Status = QuoteApprovalStatus.Approved
                    QA.Update()
                End If
                GPcontrolAPI.GPcontrolBiz.CallFlow(QA.QuoteID, "")
                Response.Write("This quote has been approved.")
                Response.End()
            End If

            If MID.StartsWith("MN") Then
                QA = eQuotationContext.Current.QuoteApproval.SingleOrDefault(Function(P) P.MobileNo = MID)
                If QA IsNot Nothing Then
                    QA.RejectReason = "MobileApprove:rejected"
                    QA.RejectedBy = QA.Approver
                    QA.RejectedDate = Now
                    QA.Status = QuoteApprovalStatus.Rejected
                    QA.Update()
                    GPcontrolAPI.GPcontrolBiz.CallFlow(QA.QuoteID, "")
                End If
                Response.Write("This quote has been rejected.")
                Response.End()
            End If

            End If
    End Sub

End Class