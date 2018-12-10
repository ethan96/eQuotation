Imports Advantech.Myadvantech.DataAccess
Imports Advantech.Myadvantech.Business

Public Class Approval
    Inherits System.Web.UI.Page

    Private _UID As String
    Public Property UID As String
        Get
            If Request("UID") IsNot Nothing AndAlso Not String.IsNullOrEmpty(Request("UID")) Then
                _UID = Request("UID")
            End If
            Return _UID
        End Get
        Set(ByVal value As String)
            _UID = value
        End Set
    End Property
    Private ReadOnly Property currQuoteApproval As Advantech.Myadvantech.DataAccess.QuoteApproval
        Get
            Dim QA As Advantech.Myadvantech.DataAccess.QuoteApproval = eQuotationContext.Current.QuoteApproval.Find(UID)
            Return QA
        End Get
    End Property
    Private ReadOnly Property currQuotationMaster As QuotationMaster
        Get
            Dim QM As QuotationMaster = eQuotationContext.Current.QuotationMaster.Find(currQuoteApproval.QuoteID)
            Return QM
        End Get
    End Property
    Private _IsKRAonlineUser As Boolean = False, _IsHQDCUser As Boolean = False
    Private _IsANAAonlineIAG As Boolean = False, _IsANAAonlineiSystem As Boolean = False

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
       
        If currQuoteApproval Is Nothing Then
            Response.End()
        End If
        'If Not String.Equals(Pivot.CurrentProfile.UserId.ToString(), currQuoteApproval.Approver.ToString(), StringComparison.InvariantCultureIgnoreCase) Then
        '    btnProcess.Attributes.Add("disabled", "disabled")
        '    btnReject.Attributes.Add("disabled", "disabled")
        'End If
        If Role.IsKRAonlineSales() Then _IsKRAonlineUser = True
        If Role.IsHQDCSales Then Me._IsHQDCUser = True
        If Role.IsAonlineUsaIag() Then _IsANAAonlineIAG = True
        If Role.IsAonlineUsaISystem Then Me._IsANAAonlineiSystem = True

        If String.Equals(Pivot.CurrentProfile.UserId.ToString.Trim, "frank.chung@advantech.com.tw", StringComparison.InvariantCultureIgnoreCase) AndAlso String.Equals(currQuotationMaster.siebelRBU, "HQDC", StringComparison.InvariantCultureIgnoreCase) Then
            _IsHQDCUser = True
        End If

        Dim MailUtil As New PROF.MailGroup()
        Dim IsGPApprover As Boolean = False

        If _IsKRAonlineUser OrElse _IsHQDCUser OrElse _IsANAAonlineIAG OrElse _IsANAAonlineiSystem Then
            Dim pars As String = "AKR"
            If _IsHQDCUser Then pars = "INTERCON"
            If _IsANAAonlineIAG Then pars = "ANA-IIoT-IAG-AOnline"
            If _IsANAAonlineiSystem Then pars = "ANA-IIoT-ISG-AOnline"

            Dim dt As DataTable = eQuotationDAL.getGP_Parameter(pars)
            For Each dr As DataRow In dt.Rows
                If Not String.IsNullOrEmpty(dr("Approver").ToString()) Then
                    If String.Equals(Pivot.CurrentProfile.UserId.ToString.Trim, dr("Approver").ToString.Trim, StringComparison.InvariantCultureIgnoreCase) AndAlso String.Equals(Pivot.CurrentProfile.UserId.ToString(), currQuoteApproval.Approver.ToString(), StringComparison.InvariantCultureIgnoreCase) Then
                        IsGPApprover = True
                    End If
                End If
            Next
        End If

        If Not (MailUtil.IsInMailGroupEQ(Pivot.CurrentProfile.UserId.ToString(), New String() {"gpapproval.aac", "MyAdvantech"})) AndAlso Not IsGPApprover Then
            btnProcess.Attributes.Add("disabled", "disabled")
            btnReject.Attributes.Add("disabled", "disabled")
        End If

        Dim QAlist As List(Of Advantech.Myadvantech.DataAccess.QuoteApproval) = eQuotationContext.Current.QuoteApproval.Where(Function(p) p.QuoteID = currQuotationMaster.quoteId AndAlso p.LevelNum = currQuoteApproval.LevelNum AndAlso p.Status <> 0).ToList()

        If QAlist.Count > 0 Then
            btnProcess.Attributes.Add("disabled", "disabled")
            btnReject.Attributes.Add("disabled", "disabled")
        End If

        Dim QM As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(currQuoteApproval.QuoteID, COMM.Fixer.eDocType.EQ)
        Me.divContent.InnerHtml = Business.getPageStrInternal(currQuoteApproval.QuoteID, QM.DocReg)
        If _IsKRAonlineUser Then
            LitDetail.Text = QuoteBusinessLogic.getGPmailBodyForAKRApprover(currQuoteApproval.QuoteID)
        ElseIf _IsHQDCUser Then
            LitDetail.Text = QuoteBusinessLogic.getGPmailBodyForInterconApprover(currQuoteApproval.QuoteID)
        Else
            Dim GPInfo1 As SAPDAL.SAPDAL.GPInfo = SAPDAL.SAPDAL.CalcANAQuoteGP(currQuoteApproval.QuoteID)
            If GPInfo1.LineItems IsNot Nothing Then
                Me.gvGPQuoteLine.DataSource = GPInfo1.LineItems : Me.gvGPQuoteLine.DataBind()
                Dim gpinfoList As New List(Of SAPDAL.SAPDAL.GPInfo)
                gpinfoList.Add(GPInfo1)
                gvAACGPTotalInfo.DataSource = gpinfoList : gvAACGPTotalInfo.DataBind()
            End If
        End If

        litTxt.Text = QuoteBusinessLogic.getApprovalTxt(currQuotationMaster.quoteId)
    End Sub

    Private Sub btnProcess_Click(sender As Object, e As EventArgs) Handles btnProcess.Click
        currQuoteApproval.ApprovedReason = ""
        currQuoteApproval.ApprovedDate = Now
        currQuoteApproval.Status = QuoteApprovalStatus.Approved
        currQuoteApproval.ApprovedBy = Pivot.CurrentProfile.UserId.ToString()
        currQuoteApproval.Update()
   
        GPcontrolAPI.GPcontrolBiz.CallFlow(currQuotationMaster.quoteId, "")
        Util.JSAlertRedirect(Me.Page, String.Format("Quote {0} has been approved", currQuotationMaster.quoteNo), Request.Url.ToString())

    End Sub

    Private Sub btnReject_Click(sender As Object, e As EventArgs) Handles btnReject.Click
        currQuoteApproval.RejectReason = Request("rtxt")
        currQuoteApproval.RejectedDate = Now
        currQuoteApproval.Status = QuoteApprovalStatus.Rejected
        currQuoteApproval.RejectedBy = Pivot.CurrentProfile.UserId.ToString()
        currQuoteApproval.Update()
        GPcontrolAPI.GPcontrolBiz.CallFlow(currQuotationMaster.quoteId, "")
        Util.JSAlertRedirect(Me.Page, String.Format("Quote {0} has been rejected", currQuotationMaster.quoteNo), Request.Url.ToString())
    End Sub
End Class