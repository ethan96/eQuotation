Public Class CreateQuotationByRef
    Inherits System.Web.UI.Page

    Public _IsEUUser As Boolean = False
    Dim myQD As New quotationDetail("EQ", "quotationDetail")

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me._IsEUUser = Role.IsEUSales()

    End Sub

    'Function copy(ByVal oldNo As String, ByVal newNo As String) As Boolean
    '    If Not Business.isValidQuote(oldNo, COMM.Fixer.eDocType.EQ) Then
    '        Util.showMessage("Invalid Ref Id!")
    '        Return False
    '    End If

    '    Pivot.NewObjDocHeader.CopyPasteHeaderLine(oldNo, newNo, Pivot.CurrentProfile, COMM.Fixer.eDocType.EQ)
    '    myQD.Copy(oldNo, newNo)
    '    Return True
    'End Function

    Protected Sub btnConfirm_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim newNo As String = ""
        'Dim newNo As String = Business.GetNoByPrefix(Pivot.CurrentProfile)
        'copy(Util.ReplaceSQLChar(Me.txtRefId.Text.Trim), newNo)
        'Dim em As String = ""
        'If Business.CopyQuotation(Me.txtRefId.Text.Trim, newNo, em, COMM.Fixer.eDocType.EQ) = 1 Then
        '    Response.Redirect(String.Format("~/quote/QuotationMaster.aspx?UID={0}", newNo))
        'Else
        '    Util.showMessage(em)
        'End If
        'Dim newNo As String = Business.GetNoByPrefix(Pivot.CurrentProfile)
        'copy(Util.ReplaceSQLChar(Me.txtRefId.Text.Trim), newNo)
        Dim OldQuoteID As String = String.Empty, ErrorStr As String = String.Empty
        Dim _QuoteMaster As Quote_Master = MyQuoteX.GetQuoteMasterByQuoteNo(Me.txtRefId.Text.Trim)
        If _QuoteMaster IsNot Nothing Then
            Dim NewQuoteID As String = String.Empty, retbool As Boolean = False

            'Ryan 20161228 EU copy will use its own function CopyAEUQuotationV2
            If _IsEUUser Then
                retbool = Business.CopyAEUQuotationV2(_QuoteMaster.quoteId, NewQuoteID, ErrorStr, COMM.Fixer.eDocType.EQ, Convert.ToInt32(CopyPurpose.SelectedValue))
            Else
                retbool = Business.CopyQuotation(_QuoteMaster.quoteId, NewQuoteID, ErrorStr, COMM.Fixer.eDocType.EQ)
            End If

            If retbool Then
                Response.Redirect(String.Format("~/quote/QuotationMaster.aspx?UID={0}", NewQuoteID))
            Else
                Util.showMessage(ErrorStr)
            End If
        Else
            ErrorStr = "QuoteNo is invalid."
            Util.showMessage(ErrorStr)
        End If

    End Sub

    Protected Sub ibtnPickQuoteCopy_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Me.ascxPickQuoteCopy.ShowData("")
        Me.UPPickQuoteCopy.Update()
        Me.MPPickQuoteCopy.Show()
    End Sub
    Public Sub PickQuoteCopyEnd(ByVal str As Object)
        Dim quoteId As String = str(0).ToString
        Me.txtRefId.Text = quoteId
        Me.UPForm.Update()
        Me.MPPickQuoteCopy.Hide()
    End Sub

End Class