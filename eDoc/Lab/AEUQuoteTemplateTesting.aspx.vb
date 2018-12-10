Public Class AEUQuoteTemplateTesting
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

  

    Private Function getPageStr(ByVal _QID As String, ByVal isinternaluser As Boolean) As String

        Dim pageHolder As New TBBasePage()
        pageHolder.IsVerifyRender = False
        Dim cw1 As UserControl = DirectCast(pageHolder.LoadControl("~/Ascx/JPAOnlineQuoteTemplateV2.ascx"), UserControl)
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

        Return output.ToString()

    End Function

    Protected Sub RadioButtonList_PriviewMode_SelectedIndexChanged(sender As Object, e As System.EventArgs)
        Me.div1.InnerHtml = Util.GetAllStringForAEU(Me.TextBoxQID.Text)
        ' Me.UP_QuotationPreview.Update()
    End Sub

    Protected Sub Button1_Click(sender As Object, e As System.EventArgs)
        Me.div1.InnerHtml = Util.GetAllStringForAEU(Me.TextBoxQID.Text)
        'Me.div1.InnerHtml = getPageStr(Me.TextBoxQID.Text, Me.RadioButtonList_PriviewMode.SelectedValue)
        'Me.UP_QuotationPreview.Update()
    End Sub

    Protected Sub ButtonSend_Click(sender As Object, e As System.EventArgs)

        'Dim strEmails As String = Me.txtEmail.Text.Trim
        'Dim emails As String() = strEmails.Split(";")
        'Dim sendTo As String = Me.txtEmail.Text.Trim
        'Dim strFromMail As String = "myadvantech@advantech.com", cc As String = ""
        'Dim bcc As String = ""
        'Dim subject As String = "Advantech Order(" & Me.TextBoxQID.Text & ")", mailbody As String = "", AttachmentName As String = ""
        ''AttachmentName = String.Format("{0}.pdf", Me.TextBoxSO.Text)
        'Dim oStream As System.IO.Stream = Nothing



        ''Dim PDFBYTE As Byte() = Util.GetPdfBytesFromHtmlString(getPageStr(Me.TextBoxSO.Text, Me.RadioButtonList_PriviewMode.SelectedValue))
        'mailbody = getPageStr(Me.TextBoxQID.Text, Me.RadioButtonList_PriviewMode.SelectedValue)
        ''MailUtil.SendEmail(sendTo, strFromMail, subject, mailbody, True, cc, bcc)
        ''oStream = New System.IO.MemoryStream(PDFBYTE)

        'Util.SendEmail(strFromMail, sendTo, cc, bcc, subject, "", mailbody, "", oStream, AttachmentName)


    End Sub


End Class