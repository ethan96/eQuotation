Public Class Quotation2SiebelAJP
    Inherits System.Web.UI.Page

    Public _QuoteID As String = String.Empty, _ServerPath As String = ""
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request("UID") IsNot Nothing Then
            Me._QuoteID = Request("UID")
        End If

        If Not IsPostBack AndAlso Request("UID") IsNot Nothing Then

            Dim QuoteTb As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(_QuoteID, COMM.Fixer.eDocType.EQ)
            'Frank 2012/07/30:If salesRowId of QuotationMaster is empty or no recodere in eqParnter then redirect to QuotationMaster.aspx
            '===Checking-start===
            If IsNothing(QuoteTb) Then
                Response.Redirect("~/home.aspx")
            End If
            If String.IsNullOrEmpty(QuoteTb.salesRowId) Then
                Response.Redirect("QuotationMaster.aspx?UID=" & _QuoteID)
            End If
            Dim aptEQPartner As New EQDSTableAdapters.EQPARTNERTableAdapter
            Dim QuotePartner As EQDS.EQPARTNERDataTable = aptEQPartner.GetPartnersByQuoteId(_QuoteID)
            If QuotePartner Is Nothing OrElse QuotePartner.Rows.Count = 0 Then
                Response.Redirect("QuotationMaster.aspx?UID=" & _QuoteID)
            End If
            '===Checking-end===

            Dim _QME As Quote_Master_Extension = MyQuoteX.GetMasterExtension(Me._QuoteID)
            If _QME IsNot Nothing Then
                If Not String.IsNullOrEmpty(_QME.JPCustomerOffice) Then
                    If Not Me.drpAJPOffice.Items.FindByValue(_QME.JPCustomerOffice) Is Nothing Then
                        Me.drpAJPOffice.Items.FindByValue(_QME.JPCustomerOffice).Selected = True
                    Else
                        Me.drpAJPOffice.Items.FindByValue("1").Selected = True
                    End If
                Else
                    Me.drpAJPOffice.Items.FindByValue("1").Selected = True
                End If
            End If

            Me.RadioButtonList_PriviewMode.SelectedValue = "false"
            Me.RadioButtonList_PriviewMode.Items.RemoveAt(0)

            ASCXQuote.QuoteId = Me._QuoteID
            ASCXQuote.LoadData()
        End If

    End Sub

    Protected Sub RadioButtonList_PriviewMode_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub

    Protected Sub drpAJPOffice_SelectedIndexChanged(sender As Object, e As EventArgs)

        'Log user selected value to DB and redirect to generate quote template again.
        Dim selected_value As String = Me.drpAJPOffice.SelectedValue
        If Not String.IsNullOrEmpty(selected_value) Then
            Dim _ME As Quote_Master_Extension = MyQuoteX.GetMasterExtension(_QuoteID)
            If _ME IsNot Nothing Then
                _ME.JPCustomerOffice = selected_value
                MyQuoteX.LogQuoteMasterExtension(_ME)
                ASCXQuote.QuoteId = Me._QuoteID
                ASCXQuote.LoadData()
                Me.UP_QuotationPreview.Update()
            End If
        End If
    End Sub

    Protected Sub ImageBtPdf_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim QM As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(Me._QuoteID, COMM.Fixer.eDocType.EQ)
        If QM IsNot Nothing Then
            Dim DT As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(_QuoteID, COMM.Fixer.eDocType.EQ)
            Dim myContentByte As Byte() = Util.DownloadQuotePDFByHtmlString(Business.getPageStrInternal(_QuoteID, DT.DocReg, True).ToString.Trim, True)
            Dim fname As String = Me._QuoteID.Replace("/", "_").ToString.Replace("\", "_").ToString.Replace(":", "_").ToString.Replace("?", "_").ToString.Replace("*", "_").ToString.Replace("""", "_").ToString.Replace("<", "_").ToString.Replace(">", "_").ToString.Replace("|", "_").ToString.Replace(" ", "_")
            If QM.CustomId IsNot Nothing AndAlso Not IsDBNull(QM.CustomId) AndAlso Not String.IsNullOrEmpty(QM.CustomId) Then
                fname = QM.CustomId.Replace("/", "_").ToString.Replace("\", "_").ToString.Replace(":", "_").ToString.Replace("?", "_").ToString.Replace("*", "_").ToString.Replace("""", "_").ToString.Replace("<", "_").ToString.Replace(">", "_").ToString.Replace("|", "_").ToString.Replace(" ", "_")
            End If
            Response.Clear()
            Response.AddHeader("Content-Type", "binary/octet-stream")
            Response.AddHeader("Content-Disposition", "attachment; filename=" & HttpContext.Current.Server.UrlEncode(fname) & ".pdf;size = " & myContentByte.Length.ToString())
            Response.Flush() : Response.BinaryWrite(myContentByte) : Response.Flush() : Response.End()
        End If
    End Sub

    Protected Sub btnConfirm_Click(sender As Object, e As EventArgs)
        ' Save ascx changes to database
        Me.ASCXQuote.QuoteId = _QuoteID
        Me.ASCXQuote.SavePageChanges()

        ' General quotation confirm process
        Dim QMDT As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(_QuoteID, COMM.Fixer.eDocType.EQ)
        Business.transQuote2Siebel(_QuoteID)
        SavePI2DB(_QuoteID, Business.getPageStrInternal(_QuoteID, QMDT.DocReg, False))
        Pivot.NewObjDocHeader.Update(_QuoteID, String.Format("quoteDate=getDate()"), COMM.Fixer.eDocType.EQ)

        ' Redirect
        Response.Redirect("~/quote/quoteByAccountOwner.aspx")
    End Sub

    Sub SavePI2DB(ByVal quoteid As String, ByVal str As String)

        Dim bt As New BigText("EQ", "BigText")
        bt.Delete(String.Format("quoteid='{0}'", quoteid))
        bt.Add(quoteid, str.Replace("'", "''"))
    End Sub

End Class