Imports Advantech.Myadvantech.Business

Public Class QuoteForward
    Inherits System.Web.UI.Page

    Private _QuoteID As String = String.Empty
    Private _IsJPAonline As Boolean = False, _IsTWAonline As Boolean = False, _IsKRAonline As Boolean = False

    Protected Sub btnSend_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        lbForwardMsg.Text = ""
        Dim _forwardmsg As String = String.Empty, _IsSuccessForward As Boolean = True
        Dim _IsShowPageNum As Boolean = False
        If _IsJPAonline Then _IsShowPageNum = True
        Dim _EmailContent As String = Me.forwardEquotationUI.EmailGreeting
        If Not String.IsNullOrEmpty(Me.txtSignature.Content) Then
            _EmailContent &= "<br/>"
            _EmailContent &= Me.txtSignature.Content
        End If

        _IsSuccessForward = Business.ForwardeQuotation(Me._QuoteID, Me.forwardEquotationUI.ContentType _
                                                       , Me.forwardEquotationUI.RecipientEmail _
                                                       , Pivot.CurrentProfile.UserId _
                                                       , Pivot.CurrentProfile.UserId _
                                                       , "" _
                                                       , Me.forwardEquotationUI.Subject _
                                                       , _EmailContent _
                                                       , Me.forwardEquotationUI.AttachedFiles _
                                                       , _IsShowPageNum, _forwardmsg)

        lbForwardMsg.Text = _forwardmsg

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsNothing(Pivot.CurrentProfile) Then
            Response.End()
        End If
        _QuoteID = Request("QuoteID")
        Me._IsTWAonline = Role.IsTWAonlineSales()
        Me._IsJPAonline = Role.IsJPAonlineSales()
        Me._IsKRAonline = Role.IsKRAonlineSales()
        If Not IsPostBack Then
            btnSend.Attributes.Add("onclick", "return DisableButton();")
            Me.forwardEquotationUI.QuoteID = _QuoteID
            Dim QuoteDt As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(_QuoteID, COMM.Fixer.eDocType.EQ)
            Me.forwardEquotationUI.Subject = "Advantech eQuotation " & QuoteDt.quoteNo

            'Frank 2015/02/11
            '用Email發報價單給客戶的功能中，recipient要預設帶出contact的email
            If _IsTWAonline OrElse _IsKRAonline Then
                'If Util.isEmail(QuoteDt.attentionEmail) Then
                Me.forwardEquotationUI.RecipientEmail = QuoteDt.attentionEmail
                'End If

                'Me.trSignature.Visible = True
                'Dim _DefaultSignature As String = QuoteBusinessLogic.GetDefaultSignature(Pivot.CurrentProfile.UserId)
                'Me.txtSignature.Content = _DefaultSignature

            End If

        End If
    End Sub

    Protected Sub ButtonSaveSignature_Click(sender As Object, e As EventArgs) Handles ButtonSaveSignature.Click
        QuoteBusinessLogic.SaveDefaultSignature(Pivot.CurrentProfile.UserId, Me.txtSignature.Content)
    End Sub

End Class