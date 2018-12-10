Imports Microsoft.VisualBasic
Imports Sgml
Imports System.Xml
Imports System.IO
Imports Winnovative


'Nada removed unused functions 
Public Class Util
    'for old version, the key is BC81JDUkNTYyJDAqNCQ3NSo1Nio9PT09
    'if you want to use demo version, then keep pdfConverterLicenseKey in nothing, it can't be string.empty or ""
    Private Shared pdfConverterLicenseKey As String = "fvDg8eDx4+jx6P/h8eLg/+Dj/+jo6Og="

    Public Shared Sub Write2Fie(ByVal value As String)
        COMM.Util.Write2Fie(value)
    End Sub


    Shared Function GetClientIP() As String
        Return COMM.Util.GetClientIP()
    End Function


    Shared Function DownloadQuotePDF(ByVal URL As String) As Byte()
        Dim urlToConvert As String = URL
        Dim pdfConverter As New PdfConverter()
        'pdfConverter.LicenseKey = "BC81JDUkNTYyJDAqNCQ3NSo1Nio9PT09"
        pdfConverter.LicenseKey = pdfConverterLicenseKey
        'pdfConverter.PdfDocumentOptions.GenerateSelectablePdf = True
        Dim pdfBytes As Byte() = pdfConverter.GetPdfBytesFromUrl(urlToConvert)
        Return pdfBytes
    End Function
    Shared Function GetAllStringForAEU(ByVal QuoteID As String) As String
        Return GetTemplateStringForAEU("QuoteMaster", QuoteID) + GetTemplateStringForAEU("QuoteDetail", QuoteID) +
      GetTemplateStringForAEU("QuoteNotes", QuoteID) + GetTemplateStringForAEU("Conditions", QuoteID)
    End Function
    Shared Function GetTemplateStringForAEU(ByVal UserControlName As String, ByVal QuoteID As String) As String
        Dim pageHolder As New TBBasePage()
        pageHolder.IsVerifyRender = False
        Dim ControlURL As String = String.Format("~/Ascx/AEUQuoteTemplate/{0}.ascx", UserControlName)
        If String.Equals(UserControlName, "QuoteDetail", StringComparison.CurrentCultureIgnoreCase) Then
            Dim MasterRef As IBUS.iDocHeaderLine = Nothing, myOrderDetail As IBUS.iCart(Of IBUS.iCartLine) = Nothing
            MasterRef = Pivot.NewObjDocHeader.GetByDocID(QuoteID, COMM.Fixer.eDocType.EQ)
            myOrderDetail = Pivot.FactCart.getCartByAppArea(COMM.Fixer.eCartAppArea.EQ, QuoteID, MasterRef.org)
            Dim dtDetail As IBUS.iCartList = myOrderDetail.GetListAll(COMM.Fixer.eDocType.EQ)
            'Dim Btosline As IBUS.iCartLine = dtDetail.Where(Function(p) p.itemType.Value = COMM.Fixer.eItemType.Parent).FirstOrDefault()
            'If Btosline IsNot Nothing Then
            '    ControlURL = String.Format("~/Ascx/AEUQuoteTemplate/{0}.ascx", "QuoteDetailForBTOS")
            'End If
        End If
        Dim cw1 As UserControl = DirectCast(pageHolder.LoadControl(ControlURL), UserControl)
        Dim viewControlType As Type = cw1.GetType
        Dim UID As Reflection.PropertyInfo = viewControlType.GetProperty("UID")
        UID.SetValue(cw1, QuoteID, Nothing)
        pageHolder.Controls.Add(cw1)
        Dim output As New IO.StringWriter()
        HttpContext.Current.Server.Execute(pageHolder, output, False)
        Return output.ToString.Trim
    End Function
    Shared Function DownloadQuotePDFforAEU(ByVal Quoteid As String) As Byte()
        Dim pdfConverter As New PdfConverter()
        pdfConverter.LicenseKey = pdfConverterLicenseKey
        pdfConverter.PdfDocumentOptions.ShowHeader = True
        pdfConverter.PdfDocumentOptions.ShowFooter = True
        pdfConverter.PdfHeaderOptions.HeaderHeight = 60
        pdfConverter.PdfFooterOptions.FooterHeight = 60
        Dim logoImg As String = "AEUheard.gif"
        Dim _footerhtml As String = String.Format("<img src=""{0}/Images/AEUbottom.gif"">", Util.GetRuntimeSiteIP())

        'Frank 20170829 get quote's partner VE code
        Dim _SalesCode As String = Advantech.Myadvantech.Business.QuoteBusinessLogic.GetSalesCodeByQuoteID(Quoteid)
        ' _SalesCode = "37100004" B+B Paul O'Shaughness
        Dim _isbbsales As Boolean = Advantech.Myadvantech.Business.UserRoleBusinessLogic.IsBBIrelandBySalesID(_SalesCode)

        If _isbbsales Then
            logoImg = "logo_bb.png"
            _footerhtml = "Westlink Commercial Park, Oranmore, Co. Galway"
            _footerhtml &= "<br>Ireland<br>"
            _footerhtml &= "Phone: +353 91 792444; Fax: +353 91 792445<br>"
            _footerhtml &= "Email: <a href=""mailto:eSales@advantech-bb.com"">eSales@advantech-bb.com</a>"
        End If

        'Dim headerHtml As HtmlToPdfElement = New HtmlToPdfElement(String.Format("<img src=""{0}/Images/AEUheard.gif"">", Util.GetRuntimeSiteIP()), Nothing)
        Dim headerHtml As HtmlToPdfElement = New HtmlToPdfElement(String.Format("<img src=""{0}/Images/" & logoImg & """>", Util.GetRuntimeSiteIP()), Nothing)
        headerHtml.FitHeight = True
        pdfConverter.PdfHeaderOptions.AddElement(headerHtml)
        Dim footerTextElement As TextElement = New TextElement(0, 30, "Page &p; of &P;  ", New Drawing.Font(New Drawing.FontFamily("Arial"), 14, System.Drawing.FontStyle.Bold))
        footerTextElement.TextAlign = HorizontalTextAlign.Right
        pdfConverter.PdfFooterOptions.AddElement(footerTextElement)
        'Dim footerHtml As HtmlToPdfElement = New HtmlToPdfElement(String.Format("<img src=""{0}/Images/AEUbottom.gif"">", Util.GetRuntimeSiteIP()), Nothing)
        Dim footerHtml As HtmlToPdfElement = New HtmlToPdfElement(_footerhtml, Nothing)
        footerHtml.FitHeight = True
        pdfConverter.PdfFooterOptions.AddElement(footerHtml)
        Dim document1 As Document = pdfConverter.GetPdfDocumentObjectFromHtmlString(GetTemplateStringForAEU("QuoteMaster", Quoteid))
        Dim conversionSummary As ConversionSummary = pdfConverter.ConversionSummary
        Dim lastPage As PdfPage = document1.Pages(conversionSummary.LastPageIndex)
        Dim newPage As PdfPage = document1.Pages.AddNewPage()
        Dim addResult As AddElementResult = Nothing
        Dim htmlToPdfURL2 As HtmlToPdfElement = New HtmlToPdfElement(GetTemplateStringForAEU("QuoteDetail", Quoteid), Nothing)
        addResult = newPage.AddElement(htmlToPdfURL2)
        newPage = document1.Pages.AddNewPage()

        'Frank 2013/10/30 If both the quote note and special terms and conditions are null value, then do not load QuoteNotes page
        Dim _QM As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(Quoteid, COMM.Fixer.eDocType.EQ)
        Dim QuoteExtensionline As IBUS.iDOCHeaderExtensionLine = Pivot.NewObjDocHeaderExtension.GetQuoteExtension(Quoteid)
        Dim _IsNullQuoteNote As Boolean = False, _IsNullSpecialTandC As Boolean = False
        If IsNothing(_QM) OrElse String.IsNullOrEmpty(_QM.quoteNote) Then _IsNullQuoteNote = True
        If IsNothing(QuoteExtensionline) OrElse String.IsNullOrEmpty(QuoteExtensionline.SpecialTandC) Then _IsNullSpecialTandC = True
        If Not _IsNullQuoteNote OrElse Not _IsNullSpecialTandC Then
            Dim htmlToPdfURL3 As HtmlToPdfElement = New HtmlToPdfElement(GetTemplateStringForAEU("QuoteNotes", Quoteid), Nothing)
            addResult = newPage.AddElement(htmlToPdfURL3)
            newPage = document1.Pages.AddNewPage()
        End If

        Dim htmlToPdfURL4 As HtmlToPdfElement = New HtmlToPdfElement(GetTemplateStringForAEU("Conditions", Quoteid), Nothing)
        addResult = newPage.AddElement(htmlToPdfURL4)
        document1.AutoCloseAppendedDocs = True
        Dim pdfBytes As Byte() = Nothing
        Try
            pdfBytes = document1.Save()
        Finally
            document1.Close()
        End Try
        Return pdfBytes
    End Function
    Shared Function DownloadQuotePDFByHtmlString(ByVal HtmlString As String, ByVal ShowPageNumber As Boolean) As Byte()
        Dim selectablePDF As Boolean = True
        Dim pdfConverter As New PdfConverter()
        'pdfConverter.LicenseKey = "BC81JDUkNTYyJDAqNCQ3NSo1Nio9PT09"
        pdfConverter.LicenseKey = pdfConverterLicenseKey
        pdfConverter.PdfDocumentOptions.EmbedFonts = False

        If ShowPageNumber Then
            pdfConverter.PdfDocumentOptions.ShowFooter = True
            'pdfConverter.PdfFooterOptions.FooterText = ""
            'pdfConverter.PdfFooterOptions.ShowPageNumber = True
        End If

        Dim footBlock As String = "", headerBlock As String = ""
        Dim doc As New HtmlAgilityPack.HtmlDocument
        Dim HtmlReader As TextReader = New StringReader(HtmlString.Replace("&nbsp;", " "))
        doc.Load(HtmlReader)

        If Role.IsTWAonlineSales() OrElse Role.IsCNAonlineSales() Then
            pdfConverter.PdfDocumentOptions.EmbedFonts = True

            'Header control
            pdfConverter.PdfDocumentOptions.ShowHeader = True
            pdfConverter.PdfHeaderOptions.HeaderHeight = 130
            headerBlock = doc.GetElementbyId("divHeader").InnerHtml
            Dim headerHtml As HtmlToPdfElement = New HtmlToPdfElement(0, 0, 600, headerBlock, "")
            headerHtml.FitWidth = True
            headerHtml.EmbedFonts = True
            pdfConverter.PdfHeaderOptions.AddElement(headerHtml)

            'footer control
            pdfConverter.PdfDocumentOptions.ShowFooter = True
            pdfConverter.PdfFooterOptions.FooterHeight = 100
            footBlock = doc.GetElementbyId("divFooter").InnerHtml
            Dim footerHtml As HtmlToPdfElement = New HtmlToPdfElement(0, 0, 600, footBlock, "")
            footerHtml.FitWidth = True
            footerHtml.EmbedFonts = True
            pdfConverter.PdfFooterOptions.AddElement(footerHtml)


            'pdfConverter.PdfFooterOptions.PageNumberingStartIndex = 1


            HtmlString = HtmlString.Replace("id=""divHeader""", "id=""divHeader"" style=""display:none;""")
            HtmlString = HtmlString.Replace("id=""divFooter""", "id=""divFooter"" style=""display:none;""")
        End If

        If Role.IsJPAonlineSales() Then
            pdfConverter.PdfDocumentOptions.EmbedFonts = True
            'pdfConverter.SvgFontsEnabled = True


            headerBlock = doc.GetElementbyId("divHeader").InnerHtml
            footBlock = doc.GetElementbyId("divFooter").InnerHtml
            'Dim MyDOC As New System.Xml.XmlDocument
            'MyDOC.LoadXml(HtmlString.Replace("&nbsp;", " "))
            'Util.getXmlBlockByID("div", "divHeader", MyDOC, headerBlock)
            'Util.getXmlBlockByID("div", "divFooter", MyDOC, footBlock)
            pdfConverter.PdfDocumentOptions.ShowHeader = True
            pdfConverter.PdfHeaderOptions.HeaderHeight = 80
            'pdfConverter.PdfHeaderOptions.HtmlToPdfArea = New Winnovative.WnvHtmlConvert.HtmlToPdfArea(headerBlock, Nothing)

            Dim headerHtml As HtmlToPdfElement = New HtmlToPdfElement(headerBlock, Nothing)
            headerHtml.FitHeight = True : headerHtml.EmbedFonts = True
            pdfConverter.PdfHeaderOptions.AddElement(headerHtml)



            pdfConverter.PdfDocumentOptions.ShowFooter = True
            pdfConverter.PdfFooterOptions.FooterHeight = 130
            Dim Footer As HtmlToPdfElement = New HtmlToPdfElement(footBlock, Nothing)
            Footer.EmbedFonts = True
            pdfConverter.PdfFooterOptions.AddElement(Footer)
            ''
            Dim footerTextElement As TextElement = New TextElement(445, 170, "Page &p; / &P;  ", New System.Drawing.Font(New System.Drawing.FontFamily("Arial"), 14, System.Drawing.FontStyle.Bold))
            footerTextElement.TextAlign = HorizontalTextAlign.Right
            pdfConverter.PdfFooterOptions.AddElement(footerTextElement)
            ''
            'pdfConverter.PdfFooterOptions.ShowPageNumber = True
            HtmlString = HtmlString.Replace("id=""divHeader""", "id=""divHeader"" style=""display:none;""")
            HtmlString = HtmlString.Replace("id=""divFooter""", "id=""divFooter"" style=""display:none;""")
        End If
        Dim pdfBytes As Byte() = pdfConverter.GetPdfBytesFromHtmlString(HtmlString)
        Return pdfBytes

    End Function


    Shared Function DownloadQuoteWordByHtmlString(ByVal HtmlString As String, ByVal ShowPageNumber As Boolean) As Byte()

        Dim license As New Aspose.Words.License()
        Dim strFPath As String = HttpContext.Current.Server.MapPath("~/Files/Aspose.Total.lic")
        license.SetLicense(strFPath)
        Dim aspDoc As New Aspose.Words.Document()
        Dim docBuilder As New Aspose.Words.DocumentBuilder(aspDoc)
        'docBuilder.PageSetup.Orientation = Aspose.Words.Orientation.Landscape
        docBuilder.PageSetup.RightMargin = 20
        docBuilder.PageSetup.LeftMargin = 20
        docBuilder.PageSetup.Orientation = Aspose.Words.Orientation.Portrait
        docBuilder.PageSetup.PaperSize = Aspose.Words.PaperSize.A4

        HtmlString = HtmlString.Replace("&nbsp;", " ").Replace("width=" & Chr(34) & "900" & Chr(34) & "", "width=" & Chr(34) & "750" & Chr(34) & "")

        Dim _StyleStart As Integer = 0
        Dim _StyleEnd As Integer = 0

        For i As Integer = 1 To 3
            _StyleStart = HtmlString.IndexOf("<style")
            _StyleEnd = HtmlString.IndexOf("</style>")
            HtmlString = HtmlString.Substring(0, _StyleStart) & HtmlString.Substring(_StyleEnd + 8, HtmlString.Length - (_StyleEnd + 8))
        Next

        docBuilder.InsertHtml(HtmlString)
        Dim outStream As New MemoryStream()
        aspDoc.Save(outStream, Aspose.Words.SaveFormat.Doc)

        Dim pdfBytes As Byte() = outStream.ToArray
        Return pdfBytes

    End Function

    Shared Function DownloadQuotePDFbyStr(ByVal str As String) As Byte()
        'Dim urlToConvert As String = URL
        Dim selectablePDF As Boolean = True
        Dim pdfConverter As New PdfConverter()
        'pdfConverter.LicenseKey = "BC81JDUkNTYyJDAqNCQ3NSo1Nio9PT09"
        pdfConverter.LicenseKey = pdfConverterLicenseKey
        'pdfConverter.PdfDocumentOptions.PdfPageSize = PdfPageSize.A4
        'pdfConverter.PdfDocumentOptions.PdfCompressionLevel = PdfCompressionLevel.Normal
        'pdfConverter.PdfDocumentOptions.PdfPageOrientation = PDFPageOrientation.Portrait
        'pdfConverter.PdfDocumentOptions.ShowHeader = False
        'pdfConverter.PdfDocumentOptions.ShowFooter = False
        ' pdfConverter.PdfDocumentOptions.GenerateSelectablePdf = selectablePDF
        'Dim pdfBytes As Byte() = pdfConverter.GetPdfFromUrlBytes(urlToConvert)
        Dim pdfBytes As Byte() = pdfConverter.GetPdfBytesFromHtmlString(str)
        Return pdfBytes
        'Dim Response As System.Web.HttpResponse = System.Web.HttpContext.Current.Response
        'Response.Clear()
        'Response.AddHeader("Content-Type", "binary/octet-stream")
        'Response.AddHeader("Content-Disposition", "attachment; filename=quote.pdf;size = " & pdfBytes.Length.ToString())
        'Response.Flush()
        'Response.BinaryWrite(pdfBytes)
        'Response.Flush() : Response.End()

        '、、、、、、、、、、、、、、、、、、a
        'Dim client As New Net.WebClient, ms As IO.MemoryStream = Nothing, doc As New HtmlAgilityPack.HtmlDocument
        'Try
        '    ms = New IO.MemoryStream(client.DownloadData(URL))
        'Catch ex As Exception

        'End Try
        'If ms.Length < 200000 Then
        '    doc.Load(ms)
        'End If
        'If ms IsNot Nothing AndAlso ms.Length > 0 Then
        '    Dim tUrl As String = URL, tHTML As String = ""
        '    tHTML = doc.DocumentNode.OuterHtml
        '    Dim p As New Winnovative.WnvHtmlConvert.PdfConverter()
        '    p.LicenseKey = "BC81JDUkNTYyJDAqNCQ3NSo1Nio9PT09"
        '    p.f()
        '    Dim bs() As Byte = p.GetPdfBytesFromHtmlString(tHTML)
        '    HttpContext.Current.Response.Clear()
        '    With HttpContext.Current.Response
        '        .ContentType = "application/pdf"
        '        .AddHeader("Content-Disposition", String.Format("attachment; filename={0};", "quote.pdf"))
        '    End With
        '    HttpContext.Current.Response.BinaryWrite(bs)
        '    HttpContext.Current.Response.End()
        'End If

    End Function

    Shared Function HtmlStrToXML(ByVal MyString As String, ByRef XMLDOC As System.Xml.XmlDocument, ByVal elementName As String) As String
        Return COMM.Util.HtmlStrToXML(MyString, XMLDOC, elementName)
    End Function
    Shared Function HtmlToXML(ByVal URL As String, ByRef XMLDOC As System.Xml.XmlDocument) As String
        Return COMM.Util.HtmlToXML(URL, XMLDOC)
    End Function
    Shared Function getXmlBlockByID(ByVal TYPE As String, ByVal ID As String,
                                   ByVal XMLDOC As System.Xml.XmlDocument, ByRef retXMLBlock As String) As String
        Return COMM.Util.getXmlBlockByID(TYPE, ID, XMLDOC, retXMLBlock)
    End Function
    Public Overloads Shared Function showMessage(ByVal str As String, Optional ByVal DestUrl As String = "") As Integer
        Return COMM.Util.showMessage(str, DestUrl)
    End Function
    'Public Overloads Shared Function showMessage(ByVal page As Page, ByVal str As String) As Integer
    '    If Not IsNothing(page.Master) Then
    '        CType(page.Master.FindControl("lbErroMessage"), Label).Text = str
    '        CType(page.Master.FindControl("UPError"), UpdatePanel).Update()
    '    End If
    '    Return 1
    'End Function
    Public Shared Function InitializeCulture(ByVal CultureName As String) As Integer
        System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture(CultureName)
        System.Threading.Thread.CurrentThread.CurrentUICulture = New System.Globalization.CultureInfo(CultureName)
        Return 1
    End Function

    Public Shared Function ReplaceSQLChar(ByVal str As String) As String
        str = str.Replace("'", "''")
        'str = str.Replace("<", "[")
        'str = str.Replace(">", "]")
        Return str
    End Function

    Public Shared Sub SetASPOSELicenseCell()
        'Try
        Dim license As Aspose.Cells.License = New Aspose.Cells.License()
        Dim strFPath As String = HttpContext.Current.Server.MapPath("~/Files/Aspose.Total.lic")
        'If IO.File.Exists(strFPath) = False Then strFPath = "C:\MyAdvantech\Files\Aspose.Total.lic"
        license.SetLicense(strFPath)
        'Catch ex As Exception

        'End Try
    End Sub
    Public Shared Sub SetASPOSELicensePdf()
        'Try
        Dim license As Aspose.Pdf.License = New Aspose.Pdf.License()
        Dim strFPath As String = HttpContext.Current.Server.MapPath("~/Files/Aspose.Total.lic")
        'If IO.File.Exists(strFPath) = False Then strFPath = "C:\MyAdvantech\Files\Aspose.Total.lic"
        license.SetLicense(strFPath)
        'Catch ex As Exception

        'End Try
    End Sub
    Public Shared Function ExcelFile2DataTable(ByVal fs As System.IO.Stream, ByVal startRow As Integer, ByVal startColumn As Integer) As DataTable
        SetASPOSELicenseCell()
        Dim dt As New DataTable

        Dim wb As New Aspose.Cells.Workbook
        wb.Open(fs)

        For i As Integer = startColumn To wb.Worksheets(0).Cells.Columns.Count - 1
            If wb.Worksheets(0).Cells(0, i).Value IsNot Nothing AndAlso wb.Worksheets(0).Cells(0, i).Value.ToString <> "" Then
                dt.Columns.Add(wb.Worksheets(0).Cells(0, i).Value)
            Else
                Exit For
            End If
        Next
        For i As Integer = startRow To wb.Worksheets(0).Cells.Rows.Count - 1
            Dim r As DataRow = dt.NewRow
            For j As Integer = 0 To dt.Columns.Count - 1
                r.Item(j) = wb.Worksheets(0).Cells(i, j).Value
            Next
            dt.Rows.Add(r)
        Next

        Return dt
    End Function
    Public Shared Sub DataTable2ExcelDownload(ByVal dt As DataTable, ByVal FileName As String)
        SetASPOSELicenseCell()
        Dim wb As New Aspose.Cells.Workbook
        wb.Worksheets.Add(Aspose.Cells.SheetType.Worksheet)
        For i As Integer = 0 To dt.Columns.Count - 1
            wb.Worksheets(0).Cells(0, i).PutValue(dt.Columns(i).ColumnName)
        Next
        For i As Integer = 0 To dt.Rows.Count - 1
            For j As Integer = 0 To dt.Columns.Count - 1
                wb.Worksheets(0).Cells(i + 1, j).PutValue(dt.Rows(i).Item(j))
            Next
        Next
        With HttpContext.Current.Response
            If FileName.StartsWith("EQ") = False Then FileName = "EQ_" + FileName + ".xls"
            .Clear()
            .ContentType = "application/vnd.ms-excel"
            .AddHeader("Content-Disposition", String.Format("attachment; filename={0};", FileName))
            .BinaryWrite(wb.SaveToStream().ToArray)
        End With
    End Sub
    Public Shared Sub List2ExcelDownload(Of T)(ByVal _list As List(Of T), ByVal FileName As String)
        If _list Is Nothing OrElse _list.Count = 0 Then
            Exit Sub
        End If
        SetASPOSELicenseCell()
        Dim wb As New Aspose.Cells.Workbook
        wb.Worksheets.Add(Aspose.Cells.SheetType.Worksheet)
        Dim _itemProperties() As Reflection.PropertyInfo = _list.Item(0).GetType().GetProperties()
        For i As Integer = 0 To _itemProperties.Length - 1
            wb.Worksheets(0).Cells(0, i).PutValue(_itemProperties(i).Name)
        Next
        Dim _value = Nothing
        For i As Integer = 0 To _list.Count - 1            ' _itemProperties = _list(i).GetType().GetProperties()
            For j As Integer = 0 To _itemProperties.Length - 1
                _value = _itemProperties(j).GetValue(_list(i), Nothing)
                If Date.TryParse(_value, Now) Then
                    _value = Date.Parse(_value).ToString("MM/dd/yyyy")
                End If
                If _value IsNot Nothing Then
                    wb.Worksheets(0).Cells(i + 1, j).PutValue(_value)
                End If
            Next
        Next
        With HttpContext.Current.Response
            If FileName.StartsWith("EQ") = False Then FileName = "EQ_" + FileName + ".xls"
            .Clear()
            .ContentType = "application/vnd.ms-excel"
            .AddHeader("Content-Disposition", String.Format("attachment; filename={0};", FileName))
            .BinaryWrite(wb.SaveToStream().ToArray)
        End With
    End Sub
    Public Shared Function FormatMoney(ByVal money As Double, ByVal currency As String) As String
        Select Case UCase(Trim(currency))
            Case "EUR"
                Return "&euro;" + money.ToString()
            Case "USD", "US"
                Return "$" + money.ToString()
            Case "YEN", "JPY"
                Return "&yen;" + money.ToString()
            Case "NTD", "TWD"
                Return "NT " + money.ToString()
            Case "RMB"
                Return "RMB " + money.ToString()
            Case "GBP"
                Return "&pound;" + money.ToString()
            Case "AUD"
                Return "AUD" + money.ToString()
            Case "MYR"
                Return "RM" + money.ToString()
            Case Else
                Return currency + " " + money.ToString()
        End Select
    End Function

    Public Shared Function FormatCurrency(ByVal currency As String) As String
        Select Case UCase(Trim(currency))
            Case "EUR"
                Return "€"
            Case "USD", "US"
                Return "$"
            Case "YEN", "JPY"
                Return "¥"
            Case "NTD", "TWD"
                Return "NT "
            Case "RMB"
                Return "RMB "
            Case "GBP"
                Return "£"
            Case "AUD"
                Return "AUD"
            Case "MYR"
                Return "RM"
            Case Else
                Return currency
        End Select
    End Function

    Public Shared Sub Html2PdfDownload(ByVal URL As String, ByVal FileName As String)
        SetASPOSELicensePdf()
        Dim opdf As New Aspose.Pdf.Pdf()

        '// create an instance of PDF class
        'Pdf pdf = new Aspose.Pdf.Pdf();
        Dim webClient As New System.Net.WebClient()
        Dim myDataBuffer As Byte() = webClient.DownloadData(URL)
        Dim myMS As System.IO.MemoryStream = New System.IO.MemoryStream(myDataBuffer)
        opdf.HtmlInfo.ImgUrl = URL
        opdf.BindHTML(myMS)

        Dim oStream As New MemoryStream
        opdf.Save(oStream)
        With HttpContext.Current.Response
            If FileName.StartsWith("EQ") = False Then FileName = "EQ_" + FileName
            .Clear()
            .ContentType = "application/pdf"
            .AddHeader("Content-Disposition", String.Format("attachment; filename={0};", FileName))
            .BinaryWrite(oStream.ToArray)
        End With
        oStream.Close()
    End Sub

    Public Shared Function showDT(ByVal ODT As DataTable) As String
        Return COMM.Util.showDT(ODT)
    End Function
    Public Shared Function getDTHtml(ByVal ODT As DataTable) As String
        Return COMM.Util.getDTHtml(ODT)
    End Function
    Public Shared Function DateFormat(ByVal DATESTR As String, ByVal FF As String, ByVal TF As String, ByVal FSP As String, ByVal TSP As String) As String
        Return COMM.Util.DateFormat(DATESTR, FF, TF, FSP, TSP)
    End Function
    Public Shared Function isEmail(ByVal str As String) As Boolean
        Return COMM.Util.isEmail(str)
    End Function

    Public Shared Sub EmbedChildNodeImageSrc(ByRef sn As System.Xml.XmlNode, ByRef ImageCounter As Integer, ByRef LinkSrcArray As System.Net.Mail.LinkedResource())

        Dim ssn As System.Xml.XmlNode
        Try
            For Each ssn In sn.ChildNodes

                If LCase(ssn.Name) = "img" Then

                    If File.Exists(HttpContext.Current.Server.MapPath(ssn.Attributes("src").Value)) Then

                        Dim ImgLinkSrc1 As System.Net.Mail.LinkedResource = Nothing
                        'Try
                        ImgLinkSrc1 = New System.Net.Mail.LinkedResource(HttpContext.Current.Server.MapPath(ssn.Attributes("src").Value))
                        'Catch ex As Exception
                        '    HttpContext.Current.Response.Write(ex.Message)
                        'End Try

                        ImgLinkSrc1.ContentId = "Img" & ImageCounter
                        ImgLinkSrc1.ContentType.Name = "Img" & ImageCounter
                        ssn.Attributes("src").Value = "cid:Img" & ImageCounter
                        ImageCounter += 1
                        ReDim Preserve LinkSrcArray(ImageCounter - 1)
                        LinkSrcArray(ImageCounter - 1) = ImgLinkSrc1

                    End If

                End If

                If ssn.ChildNodes.Count > 0 Then
                    EmbedChildNodeImageSrc(ssn, ImageCounter, LinkSrcArray)
                End If
            Next
        Catch ex As Exception
            Exit Sub
        End Try

    End Sub
    Public Shared Sub SendEmail(ByVal FROM_Email As String, ByVal TO_Email As String,
                                        ByVal CC_Email As String, ByVal BCC_Email As String,
                                        ByVal Subject_Email As String, ByVal AttachFile As String,
                                        ByVal MailBody As String, ByVal str_type As String, Optional ByVal atts As System.IO.Stream = Nothing, Optional ByVal attsName As String = "")
        COMM.Util.SendEmail(FROM_Email, TO_Email, CC_Email, BCC_Email, Subject_Email, AttachFile, MailBody, str_type, atts, attsName)
    End Sub

    Public Shared Function getCurrSign(ByVal currency As String) As String
        Return COMM.Util.getCurrSign(currency)
    End Function
    Public Shared Function GetLocalTime(ByVal org As String, ByVal ServerTime As DateTime) As DateTime
        'For interface, but have  logic errors.
        ' Return Pivot.NewObjDoc.GetLocalTime(org)
        If DateTime.TryParse(ServerTime, DateTime.Now) = False Then Return DateTime.Now
        'Frank 2012/09/14:Do not transfer the 9999-12-31, it means the inventory=0
        If ServerTime.ToString("yyyy-MM-dd").Equals("9999-12-31") Then Return ServerTime
        org = org.ToUpper.Trim
        Dim utcTime As DateTime = DateTime.Now.ToUniversalTime()
        Dim Org2Timezone As Dictionary(Of String, String) = CType(HttpContext.Current.Cache("Org2Timezone"), Dictionary(Of String, String))
        If Org2Timezone Is Nothing Then
            Org2Timezone = New Dictionary(Of String, String)
            HttpContext.Current.Cache.Add("Org2Timezone", Org2Timezone, Nothing, DateTime.Now.AddHours(8), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Default, Nothing)
        End If
        If Not Org2Timezone.ContainsKey(org) Then
            Dim timezone As Object = tbOPBase.dbExecuteScalar("EQ", String.Format("select top 1 isnull(timezonename,'') as timezonename from TIMEZONE where org like '%{0}'", org))
            If timezone IsNot Nothing AndAlso Not String.IsNullOrEmpty(timezone) Then
                Org2Timezone.Add(org, timezone.ToString)
            End If
        End If
        If Not String.IsNullOrEmpty(Org2Timezone.Item(org)) Then
            Dim TZ_Local As TimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(Org2Timezone.Item(org))
            Dim TZI_Tw As TimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Taipei Standard Time")
            Dim TimeDifference As TimeSpan = TZ_Local.GetUtcOffset(utcTime) - TZI_Tw.GetUtcOffset(utcTime)
            Return ServerTime.Add(TimeDifference)
        End If
        Return DateTime.Now
    End Function
    'Public Shared Function GetTimeSpan(ByVal org As String) As TimeSpan

    '    Dim utcTime As DateTime = DateTime.Now.ToUniversalTime()
    '    Dim timezone As Object = tbOPBase.dbExecuteScalar("EQ", String.Format("select top 1 isnull(timezonename,'') as timezonename from TIMEZONE where org like '%{0}'", org))
    '    If timezone IsNot Nothing AndAlso Not String.IsNullOrEmpty(timezone) Then
    '        Dim TZI As TimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timezone)
    '        Return TZI.GetUtcOffset(utcTime)
    '    End If

    '    Return Nothing
    'End Function

    'Public Shared Function TransferToLocalTime(ByVal _ts As TimeSpan, ByVal OriDate As DateTime) As DateTime
    '    OriDate = OriDate.Add(_ts)
    '    Return OriDate
    'End Function


    Public Shared Sub InsertMyErrLog(ByRef strEx As String, Optional ByVal UserId As String = "")
        'Try


        'Catch ex As Exception
        '    Exit Sub
        'End Try

        Dim iUrl As String = Left(HttpContext.Current.Request.ServerVariables("URL").Replace("'", "''"), 500)
        Dim iQString As String = ""
        If HttpContext.Current.Request.QueryString.HasKeys Then
            For i As Integer = 0 To HttpContext.Current.Request.QueryString.Count - 1
                iQString += HttpContext.Current.Request.QueryString.Keys(i) & "=" &
                         HttpContext.Current.Request.QueryString.Item(i) & "&"
            Next
            iQString.Replace("'", "&aps").Replace("'", "''")
        End If
        tbOPBase.dbExecuteNoQuery("MY", String.Format("INSERT INTO MY_ERR_LOG (ROW_ID, USERID, URL, QSTRING, EXMSG, APPID) VALUES (N'{0}', N'{1}', N'{2}', N'{3}', N'{4}', 'EQ')",
                                                           Left(System.Guid.NewGuid().ToString().Replace("-", ""), 10), UserId, Left(iUrl, 500), Left(iQString, 1000), Left(Replace(strEx, "'", "''"), 2000)))
        'Catch ex As Exception

        'End Try
    End Sub



    Shared Sub ClearCookie_Login(ByVal K As String)
        Dim DMCOOK As HttpCookie = New HttpCookie(K)
        DMCOOK.Expires = DateTime.Now.AddDays(-1)
        HttpContext.Current.Response.Cookies.Add(DMCOOK)
    End Sub

    Shared Sub CreateCookie_Login(ByVal K As String, ByVal V As String)
        If Not IsValidCookie_Login(K) Then
            Dim DMCOOK As HttpCookie = New HttpCookie(K)
            DMCOOK.Expires = DateTime.Now.AddDays(7)
            DMCOOK.Value = V
            HttpContext.Current.Response.Cookies.Add(DMCOOK)
        End If
    End Sub

    Shared Function IsValidCookie_Login(ByVal K As String) As Boolean
        Dim ADEQCOOK As HttpCookie = HttpContext.Current.Request.Cookies(K)
        If IsNothing(ADEQCOOK) Then
            Return False
        Else
            Return True
        End If
    End Function

    Shared Sub ShowCookie_Login(ByVal K As String)
        Dim ADEQCOOK As HttpCookie = HttpContext.Current.Request.Cookies(K)
        If Not IsNothing(ADEQCOOK) Then
            HttpContext.Current.Response.Write(ADEQCOOK.Value)
        End If
    End Sub

    Public Shared Function GetRuntimeSiteUrl() As String
        'Frank 2016/03/28 This function have some problem on eQ production site.
        'Please call function GetRuntimeSiteIP next to me
        With HttpContext.Current
            Return String.Format("http://{0}{1}{2}",
                                 .Request.ServerVariables("SERVER_NAME"),
                                 IIf(.Request.ServerVariables("SERVER_PORT") = "80", "", ":" + .Request.ServerVariables("SERVER_PORT")),
                                 IIf(HttpRuntime.AppDomainAppVirtualPath = "/", "", HttpRuntime.AppDomainAppVirtualPath))
        End With

    End Function
    Public Shared Function GetRuntimeSiteIP() As String
        With HttpContext.Current
            Dim domain As String = .Request.ServerVariables("SERVER_NAME")
            Dim port As String = .Request.ServerVariables("SERVER_PORT")
            Dim currIP As String = domain
            If String.Equals(domain, "eq.advantech.com", StringComparison.InvariantCultureIgnoreCase) Then
                Dim IPs As System.Net.IPAddress() = System.Net.Dns.GetHostAddresses("eq.advantech.com")
                If IPs.Length >= 1 Then
                    currIP = IPs.FirstOrDefault().ToString().Trim()
                End If
                If Not COMM.Util.IsTesting Then
                    If port = "80" Then port = "5001"
                End If
            End If
            Return String.Format("http://{0}{1}{2}",
                                 currIP,
                                 IIf(port = "80", "", ":" + port),
                                 IIf(HttpRuntime.AppDomainAppVirtualPath = "/", "", HttpRuntime.AppDomainAppVirtualPath))
        End With
    End Function
    Public Shared Sub AjaxShowLoading(ByVal UpdatePanel1 As UpdatePanel, ByVal title As String, ByVal msg As String, Optional ByVal Num As Int32 = 0)
        ScriptManager.RegisterStartupScript(UpdatePanel1, HttpContext.Current.GetType(), "show", "ShowMasterErr('" & title & "','" & msg & "', " & Num.ToString() & ");", True)
    End Sub
    Public Shared Sub AjaxShowMsg(ByVal UpdatePanel1 As UpdatePanel, ByVal msg As String, ByVal divid As String)
        ScriptManager.RegisterStartupScript(UpdatePanel1, HttpContext.Current.GetType(), "show", "ShowMsg('" & divid & "','" & msg & "');", True)
    End Sub

    'Public Shared Function GetSalesRepresentative(ByVal salesEmail As String) As String

    '    If String.IsNullOrEmpty(salesEmail) Then Return ""

    '    Dim _SalesPerson As String = String.Empty
    '    Dim firstname As String = "", lastname As String = ""
    '    Util.GetInternalNamebyADAndSiebel(salesEmail, lastname, firstname)
    '    If lastname = "" AndAlso firstname = "" Then
    '        Dim email_name As String = salesEmail.ToString.Split("@")(0)
    '        If email_name.Contains(".") Then
    '            For Each name As String In email_name.Split(".")
    '                _SalesPerson += name.Substring(0, 1).ToUpper() + name.Substring(1, name.Length - 1).ToLower + " "
    '            Next
    '        Else
    '            _SalesPerson += email_name.Substring(0, 1).ToUpper() + email_name.Substring(1, email_name.Length - 1).ToLower
    '        End If
    '    Else
    '        _SalesPerson += firstname + " " + lastname
    '    End If

    '    Return _SalesPerson

    'End Function

    'Public Shared Sub GetInternalNamebyADAndSiebel(ByVal email As String, ByRef last_name As String, ByRef first_name As String)
    '    Dim sql As String = String.Format("select isnull(b.firstname,'') as firstname, isnull(b.lastname,'') as lastname from ADVANTECH_ADDRESSBOOK b inner join ADVANTECH_ADDRESSBOOK_ALIAS a on a.ID=b.ID where a.Email ='{0}' or b.PrimarySmtpAddress ='{0}' ", email)
    '    Dim dt As DataTable = tbOPBase.dbGetDataTable("MY", sql)
    '    If dt.Rows.Count > 0 Then
    '        first_name = dt.Rows(0).Item("firstname").ToString : last_name = dt.Rows(0).Item("lastname").ToString
    '    End If
    '    If String.IsNullOrEmpty(first_name) AndAlso String.IsNullOrEmpty(last_name) Then
    '        sql = String.Format("select isnull(firstname,'') as firstname, isnull(lastname,'') as lastname from SIEBEL_CONTACT where EMAIL_ADDRESS='{0}' ", email)
    '        dt = tbOPBase.dbGetDataTable("MY", sql)
    '        If dt.Rows.Count > 0 Then
    '            first_name = dt.Rows(0).Item("firstname").ToString : last_name = dt.Rows(0).Item("lastname").ToString
    '        End If
    '    End If
    'End Sub

    Public Shared Function GetNameVonEmail(ByVal email As String) As String
        If email.Contains("@") Then
            Dim strNamePart As String = Split(email, "@")(0)
            '字首改大寫，其他改小寫 ex:tc.chen ->Tc.Chen
            Dim strNameArry() As String = Split(strNamePart, ".")
            For i As Integer = 0 To strNameArry.Length - 1
                If strNameArry(i).Length > 1 Then
                    strNameArry(i) = strNameArry(i).Substring(0, 1).ToUpper() + strNameArry(i).Substring(1).ToLower()
                Else
                    strNameArry(i) = strNameArry(i).ToUpper()
                End If
            Next
            strNamePart = String.Join(".", strNameArry)
            Return strNamePart
        Else
            Return email
        End If
    End Function
    Public Shared Function GetRBUforQuote(ByVal SiebelRowID As String, ByVal ErpID As String) As String
        Dim RBU As Object = tbOPBase.dbExecuteScalar("MY", String.Format("select top 1  isnull(RBU,'') as RBU  from  SIEBEL_ACCOUNT where ROW_ID ='{0}'", SiebelRowID))
        If RBU IsNot Nothing AndAlso Not String.IsNullOrEmpty(RBU.ToString.Trim) Then
            Return RBU
        End If
        RBU = tbOPBase.dbExecuteScalar("MY", String.Format("select top 1 isnull(SALESOFFICENAME,'') as RBU  from SAP_DIMCOMPANY where COMPANY_ID='{0}'", ErpID))
        If RBU IsNot Nothing AndAlso Not String.IsNullOrEmpty(RBU.ToString.Trim) Then
            Return RBU
        End If
        Return String.Empty
    End Function
    Public Shared Function getStoreIdByCountrycode(ByVal _CountryCode As Object) As String
        If _CountryCode IsNot Nothing AndAlso Not String.IsNullOrEmpty(_CountryCode.ToString.Trim) Then
            _CountryCode = _CountryCode.ToString.Trim
            Dim _StoreID As Object = tbOPBase.dbExecuteScalar("Estore", String.Format("select  top 1 isnull(StoreID,'')as StoreID   from country  where Shorts='{0}'", _CountryCode))
            If _StoreID IsNot Nothing Then
                Return _StoreID.ToString.Trim
            End If
        End If
        Return String.Empty
    End Function
    Public Shared Function getNewShipRate(ByVal Quoteid As String) As DataTable
        '  Dim QuoteID As String = "2c26430b9888459"
        Dim QM As Quote_Master = MyQuoteX.GetQuoteMaster(Quoteid)
        If QM Is Nothing Then Return Nothing
        Dim QItems As List(Of QuoteItem) = MyQuoteX.GetQuoteList(Quoteid)
        If QItems Is Nothing OrElse QItems.Count = 0 Then Return Nothing
        Dim myHash As New Hashtable()
        If Role.IsEUSales() Then
            myHash("StoreId") = "AEU"
        Else
            myHash("StoreId") = "AUS"
        End If
        Dim sb As New StringBuilder
        sb.AppendFormat(" select isnull(b.COUNTRY,a.COUNTRY)as country,isnull(b.ZIP_CODE,a.ZIPCODE)as zip_code,a.TYPE,a.STATE ")
        sb.AppendFormat(" from EQPARTNER a left join MyAdvantechGlobal.dbo.SAP_DIMCOMPANY b ")
        sb.AppendFormat(" on a.ERPID = b.COMPANY_ID where a.TYPE in ('SOLDTO','S','B') and a.QUOTEID = '{0}' ", Quoteid)
        Dim dt As DataTable = tbOPBase.dbGetDataTable("EQ", sb.ToString())
        If dt.Rows.Count = 0 Then Return Nothing
        Dim drShold As DataRow() = dt.Select(String.Format(" TYPE='{0}' ", "SOLDTO"))
        If drShold.Length > 0 Then
            '设置 SholdTo相关信息
            Dim R As DataRow = drShold(0)
            Dim _StoreID As String = Util.getStoreIdByCountrycode(R.Item("country"))
            If Not String.IsNullOrEmpty(_StoreID) Then
                myHash("StoreId") = _StoreID
            End If
            '设置 ShipTo相关信息
            Dim drS As DataRow() = dt.Select(String.Format(" TYPE='{0}' ", "S"))
            If drS.Length > 0 Then
                R = drS(0)
            End If
            myHash("shiptoCountrycode") = R.Item("country")
            myHash("shiptoZipcode") = R.Item("zip_code")
            'myHash("shiptoStateCode") = ""
            myHash("shiptoStateCode") = R.Item("STATE")
            '设置BillTo 相关信息
            R = drShold(0)
            Dim drB As DataRow() = dt.Select(String.Format(" TYPE='{0}' ", "B"))
            If drB.Length > 0 Then
                R = drB(0)
            End If
            myHash("billtoCountrycode") = R.Item("country")
            myHash("billtoZipcode") = R.Item("zip_code")
            'myHash("billtoStateCode") = ""
            myHash("billtoStateCode") = R.Item("STATE")
        End If
        '绑定料
        Dim items As New DataTable()
        items.Columns.Add("lineno", GetType(Integer))
        items.Columns.Add("type", GetType(Integer))
        items.Columns.Add("partno", GetType(String))
        items.Columns.Add("Qty", GetType(Integer))
        items.Columns.Add("HigherLevel", GetType(Integer))
        Dim Qlist As List(Of QuoteItem) = MyQuoteX.GetQuoteList(Quoteid)
        For Each i As QuoteItem In Qlist
            If i.partNo.StartsWith("ags-ew", StringComparison.CurrentCultureIgnoreCase) OrElse i.partNo.StartsWith("ags-ctos-", StringComparison.CurrentCultureIgnoreCase) Then
                Continue For
            End If
            Dim dr As DataRow = items.NewRow()
            dr("lineno") = i.line_No
            dr("Qty") = i.qty
            dr("HigherLevel") = i.HigherLevel
            dr("type") = 0
            If i.ItemTypeX = QuoteItemType.BtosParent Then
                dr("type") = -1
            ElseIf i.ItemTypeX = QuoteItemType.BtosPart Then
                dr("type") = 1
            End If
            dr("partno") = i.partNo
            items.Rows.Add(dr)
        Next
        items.AcceptChanges()
        myHash("items") = items
        Dim shiplist As Object = Nothing
        'eStoreServices.eStoreTool.GeteStoreShippingRate(myHash, shiplist)
        Try
            eStoreServices.eStoreTool.GetShippingRate(myHash)
        Catch ex As Exception
            Util.InsertMyErrLog(ex.ToString, GetCurrentUserID())
            Return Nothing
        End Try
        If myHash("ShippingRates") IsNot Nothing Then
            Return CType(myHash("ShippingRates"), DataTable)
        End If
        Return Nothing
    End Function
    Public Shared Function GetCurrentUserID() As String
        Dim user As String = "Anonymous"
        If Not IsNothing(HttpContext.Current) AndAlso (Not IsNothing(HttpContext.Current.User)) AndAlso HttpContext.Current.User.Identity.IsAuthenticated Then
            user = HttpContext.Current.User.Identity.Name
        End If
        Return user
    End Function
    Public Shared Sub JSAlertRedirect(ByVal Page As Page, ByVal msg As String, ByVal url As String)
        Dim jscript As String =
        "<script type='text/javascript'>" + vbCrLf +
        "alert('" + Replace(msg, "'", "''") + "');" + vbCrLf +
        "location.href = '" + url + "';" + vbCrLf +
        "</script>"
        Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "JSAlertRedirect", jscript)
    End Sub
End Class

Public Class TBBasePage
    Inherits System.Web.UI.Page
    Private FIsVerifyRender As Boolean = True
    Public Property IsVerifyRender() As Boolean
        Get
            Return FIsVerifyRender
        End Get
        Set(ByVal value As Boolean)
            FIsVerifyRender = value
        End Set
    End Property
    Public Overrides Sub VerifyRenderingInServerForm(ByVal Control As System.Web.UI.Control)
        If Me.IsVerifyRender Then
            MyBase.VerifyRenderingInServerForm(Control)
        End If
    End Sub
    Public Overrides Property EnableEventValidation() As Boolean
        Get
            If Me.IsVerifyRender Then
                Return MyBase.EnableEventValidation
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            MyBase.EnableEventValidation = value
        End Set
    End Property
End Class
