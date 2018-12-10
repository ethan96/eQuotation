Public Class UpdateQuotePrintFormat
    Inherits System.Web.UI.Page

    Private _QuoteID As String = String.Empty, _QuoreNo As String = String.Empty

    Dim _IsUsaUser As Boolean = False, _IsHQDCUser As Boolean = False, _IsTWUser As Boolean = False, _IsJPUser As Boolean = False

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        _QuoteID = Request("QuoteId")

        _IsUsaUser = Role.IsUsaUser
        _IsHQDCUser = Role.IsHQDCSales
        _IsTWUser = Role.IsTWAonlineSales
        _IsJPUser = Role.IsJPAonlineSales

        'If Not Page.IsPostBack AndAlso String.IsNullOrEmpty(Me._QuoteID) = False AndAlso Me._QuoteID.StartsWith("AUSQ", StringComparison.CurrentCultureIgnoreCase) Then
        If Not Page.IsPostBack _
            AndAlso String.IsNullOrEmpty(Me._QuoteID) = False _
            AndAlso (_IsUsaUser OrElse _IsHQDCUser OrElse _IsTWUser OrElse _IsJPUser) Then

            If _IsTWUser Then
                'btnUpdateFormatOption.Attributes.Add("onclick", "return ATWDownload();")
                Me.btnUpdateFormatOption.Text = "Download"
            Else
                btnUpdateFormatOption.Attributes.Add("onclick", "return DisableButton();")
            End If

            Dim dtQM As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(_QuoteID, COMM.Fixer.eDocType.EQ)
            If Not IsNothing(dtQM) Then
                hdQuoteId.Value = dtQM.Key
                lbQuoteId.Text = dtQM.quoteNo

                Me.QuotationViewOptionUC1.QuoteID = _QuoteID

                tbFormatSetting.Visible = True
            End If
            'End If
        End If
    End Sub

    Protected Sub btnUpdateFormatOption_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdateFormatOption.Click
        'Frank 2012/09/18:User control will auto update the print out format value to quotationMaster.
        'Dim apt As New EQDSTableAdapters.QuotationMasterTableAdapter
        'apt.UpdatePrintFormat(rblFormatOptions.SelectedValue, hdQuoteId.Value)



        generatePDF(hdQuoteId.Value)

        'Dim id As String = hdQuoteId.Value

        'Dim dt As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(id, COMM.Fixer.eDocType.EQ)
        'Dim _QuoteNo As String = dt.quoteNo

        'If Not IsNothing(dt) Then
        '    Dim myContentByte As Byte() = Nothing
        '    Dim strUSAOnlineQuoteTemplateHtml As String = Business.getPageStrInternal(id, dt.DocReg, False)
        '    myContentByte = Util.DownloadQuoteWordByHtmlString(strUSAOnlineQuoteTemplateHtml, False)

        '    Dim fname As String = _QuoteNo.Replace("/", "_").ToString.Replace("\", "_").ToString.Replace(":", "_").ToString.Replace("?", "_").ToString.Replace("*", "_").ToString.Replace("""", "_").ToString.Replace("<", "_").ToString.Replace(">", "_").ToString.Replace("|", "_").ToString.Replace(" ", "_")
        '    Response.Clear()
        '    Response.AddHeader("Content-Type", "binary/octet-stream")
        '    Response.AddHeader("Content-Disposition", "attachment; filename=" & HttpContext.Current.Server.UrlEncode(fname) & ".doc;size = " & myContentByte.Length.ToString())
        '    Response.Flush() : Response.BinaryWrite(myContentByte) : Response.Flush() : Response.End()
        'End If

    End Sub


    Protected Sub generatePDF(ByVal id As String)

        'Frank 2015/10/06 Real-time PDF generation

        'Dim O As New BigText("eq", "BigText")
        'Dim dtC As New DataTable
        'dtC = O.GetDT(String.Format("quoteId='{0}'", id), "")

        Dim dt As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(id, COMM.Fixer.eDocType.EQ)

        'If dtC.Rows.Count > 0 AndAlso Not Role.IsUsaUser Then
        '    Dim co As String = dtC.Rows(0).Item("content")
        '    Dim myContentByte As Byte() = Util.DownloadQuotePDFbyStr(co)
        '    If Not IsNothing(dt) Then
        '        'Dim fname As String = dt.CustomId.ToString.Replace("/", "_").ToString.Replace("\", "_").ToString.Replace(":", "_").ToString.Replace("?", "_").ToString.Replace("*", "_").ToString.Replace("""", "_").ToString.Replace("<", "_").ToString.Replace(">", "_").ToString.Replace("|", "_").ToString.Replace(" ", "_")
        '        Dim fname As String = dt.quoteNo.Replace("/", "_").ToString.Replace("\", "_").ToString.Replace(":", "_").ToString.Replace("?", "_").ToString.Replace("*", "_").ToString.Replace("""", "_").ToString.Replace("<", "_").ToString.Replace(">", "_").ToString.Replace("|", "_").ToString.Replace(" ", "_")
        '        Response.Clear()
        '        Response.ClearHeaders()
        '        'Response.AddHeader("Content-Type", "binary/octet-stream")
        '        Response.ContentType = "application/pdf;name=" + fname
        '        Response.AddHeader("Content-Disposition", "inline; filename=" & fname & ".pdf;size = " & myContentByte.Length.ToString())
        '        ' Response.Flush() : Response.BinaryWrite(myContentByte) : Response.Flush() 'Response.End()
        '        Response.BinaryWrite(myContentByte) : Response.Flush() : Response.Close() : Response.End()
        '    End If
        'Else

        Dim _filetype As String = "application/pdf"
        Dim _fileextension As String = "pdf"
        Dim _downloadMethod As String = "inline"

        If Not IsNothing(dt) Then
            Dim myContentByte As Byte() = Nothing

            If _IsUsaUser OrElse _IsHQDCUser Then

                Dim strUSAOnlineQuoteTemplateHtml As String = Business.getPageStrInternal(id, dt.DocReg, False)

                If Role.IsAonlineUsa() Then
                    If QuotationViewOptionUC1.NCNR = True Then
                        Dim strNCNRAgreementHtml As String = Business.getNCNRStr()
                        Dim PageBreak As String = Replace("<p style='page-break-after:always;'></p>", "'", Chr(34))
                        strUSAOnlineQuoteTemplateHtml = strUSAOnlineQuoteTemplateHtml + PageBreak + strNCNRAgreementHtml
                    End If
                End If

                'Sabrina Lin 20160919
                'Hi Frank, 
                'Can you help to add “Quotation” in the eQuotation ?
                'Some customer will need Performa Invoice to remit payment instead Quotation. 
                'In SAP, there is an option to change Quotation title to Performa Invoice, T-code is ZRSD03TW, can you help to add this option in “Print out to PDF” function?
                If _IsHQDCUser Then
                    If QuotationViewOptionUC1.IsChangeTitle Then
                        strUSAOnlineQuoteTemplateHtml = strUSAOnlineQuoteTemplateHtml.Replace("<span id=""ctl00_lbtitle"">Quotation</span>", "<span id=""ctl00_lbtitle"">Proforma Invoice</span>")
                    End If
                End If

                myContentByte = Util.DownloadQuotePDFByHtmlString(strUSAOnlineQuoteTemplateHtml, False)

            ElseIf _IsTWUser Then
                Dim strUSAOnlineQuoteTemplateHtml As String = Business.getPageStrInternal(id, dt.DocReg, False)
                _downloadMethod = "attachment"
                Select Case dt.PRINTOUT_FORMAT

                    Case COMM.Fixer.USPrintOutFormat.ATW_Quote_PDF_Englisn, COMM.Fixer.USPrintOutFormat.ATW_Quote_PDF_TraditionalChinese
                        myContentByte = Util.DownloadQuotePDFByHtmlString(strUSAOnlineQuoteTemplateHtml, False)

                    Case COMM.Fixer.USPrintOutFormat.ATW_Quote_Word_Englisn, COMM.Fixer.USPrintOutFormat.ATW_Quote_Word_TraditionalChinese
                        myContentByte = Util.DownloadQuoteWordByHtmlString(strUSAOnlineQuoteTemplateHtml, False)
                        _filetype = "binary/octet-stream"
                        _fileextension = "doc"
                        _downloadMethod = "attachment"

                        'Dim dt As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(id, COMM.Fixer.eDocType.EQ)
                        'Dim _QuoteNo As String = dt.quoteNo

                        'If Not IsNothing(dt) Then
                        '    Dim myContentByte As Byte() = Nothing
                        '    Dim strUSAOnlineQuoteTemplateHtml As String = Business.getPageStrInternal(id, dt.DocReg, False)
                        '    myContentByte = Util.DownloadQuoteWordByHtmlString(strUSAOnlineQuoteTemplateHtml, False)

                        '    Dim fname As String = _QuoteNo.Replace("/", "_").ToString.Replace("\", "_").ToString.Replace(":", "_").ToString.Replace("?", "_").ToString.Replace("*", "_").ToString.Replace("""", "_").ToString.Replace("<", "_").ToString.Replace(">", "_").ToString.Replace("|", "_").ToString.Replace(" ", "_")
                        '    Response.Clear()
                        '    Response.AddHeader("Content-Type", "binary/octet-stream")
                        '    Response.AddHeader("Content-Disposition", "attachment; filename=" & HttpContext.Current.Server.UrlEncode(fname) & ".doc;size = " & myContentByte.Length.ToString())
                        '    Response.Flush() : Response.BinaryWrite(myContentByte) : Response.Flush() : Response.End()
                        'End If

                End Select

                ''Me.limg.InnerHtml = ""

            ElseIf _IsJPUser Then
                Dim strJPAOnlineQuoteTemplateHtml As String = Business.getPageStrInternal(id, dt.DocReg, False)
                myContentByte = Util.DownloadQuotePDFByHtmlString(strJPAOnlineQuoteTemplateHtml, False)
            Else
                Dim URL As String = String.Format("http://{0}{1}{2}/quote/{3}?UID={4}&ROLE=1", _
                                    Request.ServerVariables("SERVER_NAME"), _
                                    IIf(Request.ServerVariables("SERVER_PORT") = "80", "", ":" + Request.ServerVariables("SERVER_PORT")), _
                                    HttpRuntime.AppDomainAppVirtualPath, _
                                    Business.getPiPage(id, dt.DocReg), id)
                myContentByte = Util.DownloadQuotePDF(URL)

            End If

            'If _IsTWUser Then
            '    'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CleanLimg", "CleanLimg();", True)
            '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hwa", "alert('Hello World');", True)
            'End If



            'Dim fname As String = id.Replace("/", "_").ToString.Replace("\", "_").ToString.Replace(":", "_").ToString.Replace("?", "_").ToString.Replace("*", "_").ToString.Replace("""", "_").ToString.Replace("<", "_").ToString.Replace(">", "_").ToString.Replace("|", "_").ToString.Replace(" ", "_")
            Dim fname As String = dt.quoteNo.Replace("/", "_").ToString.Replace("\", "_").ToString.Replace(":", "_").ToString.Replace("?", "_").ToString.Replace("*", "_").ToString.Replace("""", "_").ToString.Replace("<", "_").ToString.Replace(">", "_").ToString.Replace("|", "_").ToString.Replace(" ", "_")
            Response.Clear()
            Response.ClearHeaders()
            'Response.AddHeader("Content-Type", "binary/octet-stream")
            Response.ContentType = _filetype + ";name=" + fname
            Response.AddHeader("Content-Disposition", _downloadMethod + "; filename=" & fname & "." + _fileextension + ";size = " & myContentByte.Length.ToString())
            'Response.Flush() : Response.BinaryWrite(myContentByte) : Response.Flush() : Response.End()
            Response.BinaryWrite(myContentByte) : Response.Flush()
            'Response.Close()


            Response.End()



        End If
        'End If
    End Sub

End Class