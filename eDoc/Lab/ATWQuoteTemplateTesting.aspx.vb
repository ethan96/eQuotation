Imports Aspose.Words
Imports System.IO

Public Class ATWQuoteTemplateTesting
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Function getPageStr(ByVal _QID As String, ByVal isinternaluser As Boolean, ByVal _RBU As String) As String

        Dim pageHolder As New TBBasePage()
        pageHolder.IsVerifyRender = False
        Dim _filepath As String = String.Empty
        If _RBU.Equals("ATW", StringComparison.InvariantCultureIgnoreCase) Then
            _filepath = "~/Ascx/TWAOnlineQuoteTemplate.ascx"
        ElseIf _RBU.Equals("AKR", StringComparison.InvariantCultureIgnoreCase) Then
            _filepath = "~/Ascx/KRAOnlineQuoteTemplate.ascx"
        End If

        'Dim cw1 As UserControl = DirectCast(pageHolder.LoadControl("~/Ascx/TWAOnlineQuoteTemplate.ascx"), UserControl)
        Dim cw1 As UserControl = DirectCast(pageHolder.LoadControl(_filepath), UserControl)
        Dim viewControlType As Type = cw1.GetType
        Dim p_QuoteId As Reflection.PropertyInfo = viewControlType.GetProperty("QuoteId")
        p_QuoteId.SetValue(cw1, _QID, Nothing)
        Dim p_IsInternalUser As Reflection.PropertyInfo = viewControlType.GetProperty("IsInternalUser")
        p_IsInternalUser.SetValue(cw1, isinternaluser, Nothing)
        Dim _meth As Reflection.MethodInfo = viewControlType.GetMethod("LoadData")
        _meth.Invoke(cw1, Nothing)
        pageHolder.Controls.Add(cw1)
        Dim output As New IO.StringWriter()
        HttpContext.Current.Server.Execute(pageHolder, output, False)


        'editor1.Content = output.ToString()


        'Dim pageHolder As New TBBasePage()
        'pageHolder.IsVerifyRender = False
        'Dim cw1 As UserControl = DirectCast(pageHolder.LoadControl("~/Ascx/USAOnlineOrderTemplate.ascx"), UserControl)
        'Dim viewControlType As Type = cw1.GetType
        'Dim p_QuoteId As Reflection.PropertyInfo = viewControlType.GetProperty("OrderId")
        ''p_QuoteId.SetValue(cw1, "AUSO000062", Nothing)
        ''p_QuoteId.SetValue(cw1, "BT000511", Nothing)
        'p_QuoteId.SetValue(cw1, _QID, Nothing)

        'Dim p_IsInternalUser As Reflection.PropertyInfo = viewControlType.GetProperty("IsInternalUserMode")
        'p_IsInternalUser.SetValue(cw1, isinternaluser, Nothing)
        'Dim _meth As Reflection.MethodInfo = viewControlType.GetMethod("LoadData")
        '_meth.Invoke(cw1, Nothing)
        'pageHolder.Controls.Add(cw1)
        'Dim output As New IO.StringWriter()
        'HttpContext.Current.Server.Execute(pageHolder, output, False)
        'output()
        Return output.ToString()

    End Function

    Protected Sub RadioButtonList_PriviewMode_SelectedIndexChanged(sender As Object, e As System.EventArgs)
        Me.div1.InnerHtml = getPageStr(Me.TextBoxQID.Text, Me.RadioButtonList_PriviewMode.SelectedValue, Me.ListBoxRBU.SelectedValue)
        ' Me.UP_QuotationPreview.Update()
    End Sub

    Protected Sub Button1_Click(sender As Object, e As System.EventArgs) Handles ButtonQuery.Click
        Dim _html As String = getPageStr(Me.TextBoxQID.Text, Me.RadioButtonList_PriviewMode.SelectedValue, Me.ListBoxRBU.SelectedValue)
        Me.div1.InnerHtml = _html

        'Dim stream As MemoryStream = New MemoryStream()
        'Dim writer As StreamWriter = New StreamWriter(stream, Encoding.UTF8)
        'writer.Write(_html)
        'writer.Flush()
        'stream.Position = 0


        Dim license As New Aspose.Words.License()
        Dim strFPath As String = HttpContext.Current.Server.MapPath("~/Files/Aspose.Total.lic")
        license.SetLicense(strFPath)
        Dim aspDoc As New Aspose.Words.Document()
        Dim docBuilder As New Aspose.Words.DocumentBuilder(aspDoc)
        docBuilder.PageSetup.Orientation = Aspose.Words.Orientation.Landscape
        docBuilder.PageSetup.PaperSize = Aspose.Words.PaperSize.A4

        'Dim PDFBYTE() As Byte = Util.DownloadQuotePDFByHtmlString(_html, True)


        'Dim oStream As MemoryStream = New System.IO.MemoryStream(PDFBYTE)
        'Dim pdfConverter As New Aspose.Words.Document
        'pdfConverter.

        'Aspose.Words.Document()
        'Dim doc As Aspose.Words.Document = New Aspose.Words.Document(stream, "http://localhost:8300/Lab/", LoadFormat.Html, Nothing)

        ''doc.LicenseKey = pdfConverterLicenseKey
        'doc.Save("d:\temp\out.docx")
        _html = _html.Replace("&nbsp;", " ")

        Dim _StyleStart As Integer = 0
        Dim _StyleEnd As Integer = 0

        For i As Integer = 1 To 3
            _StyleStart = _html.IndexOf("<style")
            _StyleEnd = _html.IndexOf("</style>")
            _html = _html.Substring(0, _StyleStart) & _html.Substring(_StyleEnd + 8, _html.Length - (_StyleEnd + 8))

        Next




        docBuilder.InsertHtml(_html)
        aspDoc.Save("d:\temp\Bingo_" + Now.ToString("yyyyMMddHHmmss") + ".doc", Aspose.Words.SaveFormat.Doc)
        'Me.div1.InnerHtml = getPageStr(Me.TextBoxQID.Text, Me.RadioButtonList_PriviewMode.SelectedValue)
        'Me.UP_QuotationPreview.Update()
    End Sub

    Protected Sub ButtonSend_Click(sender As Object, e As System.EventArgs)

        Dim strEmails As String = Me.txtEmail.Text.Trim
        Dim emails As String() = strEmails.Split(";")
        Dim sendTo As String = Me.txtEmail.Text.Trim
        Dim strFromMail As String = "myadvantech@advantech.com", cc As String = ""
        Dim bcc As String = ""
        Dim subject As String = "ATW Quotation template testing(" & Me.TextBoxQID.Text & ")", mailbody As String = "", AttachmentName As String = ""
        AttachmentName = String.Format("{0}.pdf", Me.TextBoxQID.Text)
        Dim oStream As System.IO.Stream = Nothing



        'Dim PDFBYTE As Byte() = Util.GetPdfBytesFromHtmlString(getPageStr(Me.TextBoxSO.Text, Me.RadioButtonList_PriviewMode.SelectedValue))
        mailbody = getPageStr(Me.TextBoxQID.Text, Me.RadioButtonList_PriviewMode.SelectedValue, Me.ListBoxRBU.SelectedValue)
        'MailUtil.SendEmail(sendTo, strFromMail, subject, mailbody, True, cc, bcc)
        'oStream = New System.IO.MemoryStream(PDFBYTE)

        Util.SendEmail(strFromMail, sendTo, cc, bcc, subject, "", mailbody, "", oStream, AttachmentName)


    End Sub
End Class