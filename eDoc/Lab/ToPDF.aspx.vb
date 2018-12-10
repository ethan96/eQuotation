Imports System.IO

Public Class ToPDF
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ''Dim pdfConverter As Winnovative.WnvHtmlConvert.PdfConverter = New Winnovative.WnvHtmlConvert.PdfConverter()

        'Dim _htmlstr As String = ""


        'Dim strContents As String = "", FullPath As String = "D:\Advantech\MyAdvantech\eDoc\Lab\p1.htm"
        'Dim objReader As StreamReader
        'Try

        '    objReader = New StreamReader(FullPath)
        '    strContents = objReader.ReadToEnd()
        '    objReader.Close()
        '    'Return strContents
        'Catch Ex As Exception
        '    'ErrInfo = Ex.Message
        'End Try



        ''Dim myContentByte As Byte() = Util.DownloadQuotePDFbyStr(strContents)

        'Dim pdfConverter As Winnovative.WnvHtmlConvert.PdfConverter = New Winnovative.WnvHtmlConvert.PdfConverter()
        'pdfConverter.LicenseKey = "BC81JDUkNTYyJDAqNCQ3NSo1Nio9PT09"
        ''pdfConverter.PdfDocumentOptions.PdfPageSize = PdfPageSize.A4
        ''pdfConverter.PdfDocumentOptions.PdfCompressionLevel = PdfCompressionLevel.Normal
        ''pdfConverter.PdfDocumentOptions.PdfPageOrientation = PDFPageOrientation.Portrait
        ''pdfConverter.PdfDocumentOptions.ShowHeader = False
        ''pdfConverter.PdfDocumentOptions.ShowFooter = False
        'pdfConverter.PdfDocumentOptions.GenerateSelectablePdf = True
        ''Dim pdfBytes As Byte() = pdfConverter.GetPdfFromUrlBytes(urlToConvert)

        'pdfConverter.ge()

        'Document pdfDocument = pdfConverter.GetPdfDocumentObjectFromUrl(textBoxWebPageURL.Text.Trim());

        'Dim pdfBytes As Byte() = pdfConverter.GetPdfBytesFromHtmlString(Str)


        'Dim fname As String = "testpdb"
        'Response.Clear()
        'Response.AddHeader("Content-Type", "binary/octet-stream")
        'Response.AddHeader("Content-Disposition", "attachment; filename=" & HttpContext.Current.Server.UrlEncode(fname) & ".pdf;size = " & myContentByte.Length.ToString())
        'Response.Flush() : Response.BinaryWrite(myContentByte) : Response.Flush() : Response.End()
    End Sub

End Class