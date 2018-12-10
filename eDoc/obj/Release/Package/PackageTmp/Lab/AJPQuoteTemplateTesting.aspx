<%@ Page Language="VB" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">

    
    Protected Sub Page_Load(sender As Object, e As System.EventArgs)
        If Not Page.IsPostBack Then
            ' Me.div1.InnerHtml = getPageStr(Me.TextBoxQID.Text, Me.RadioButtonList_PriviewMode.SelectedValue)
        End If
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
        Me.div1.InnerHtml = getPageStr(Me.TextBoxQID.Text, Me.RadioButtonList_PriviewMode.SelectedValue)
       ' Me.UP_QuotationPreview.Update()
    End Sub

    Protected Sub Button1_Click(sender As Object, e As System.EventArgs)
        Me.div1.InnerHtml = getPageStr(Me.TextBoxQID.Text, Me.RadioButtonList_PriviewMode.SelectedValue)
        'Me.UP_QuotationPreview.Update()
    End Sub

    Protected Sub ButtonSend_Click(sender As Object, e As System.EventArgs)
        
        Dim strEmails As String = Me.txtEmail.Text.Trim
        Dim emails As String() = strEmails.Split(";")
        Dim sendTo As String = Me.txtEmail.Text.Trim
        Dim strFromMail As String = "myadvantech@advantech.com", cc As String = ""
        Dim bcc As String = ""
        Dim subject As String = "Advantech Order(" & Me.TextBoxQID.Text & ")", mailbody As String = "", AttachmentName As String = ""
        'AttachmentName = String.Format("{0}.pdf", Me.TextBoxSO.Text)
        Dim oStream As System.IO.Stream = Nothing
        
        
        
        'Dim PDFBYTE As Byte() = Util.GetPdfBytesFromHtmlString(getPageStr(Me.TextBoxSO.Text, Me.RadioButtonList_PriviewMode.SelectedValue))
        mailbody = getPageStr(Me.TextBoxQID.Text, Me.RadioButtonList_PriviewMode.SelectedValue)
        'MailUtil.SendEmail(sendTo, strFromMail, subject, mailbody, True, cc, bcc)
        'oStream = New System.IO.MemoryStream(PDFBYTE)

        Util.SendEmail(strFromMail, sendTo, cc, bcc, subject, "", mailbody, "", oStream, AttachmentName)
        
        
    End Sub
    
</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="sm1" />
    <h1>AJP Quotation Template Testing Page</h1> 
<%--    <asp:UpdatePanel ID="UP_QuotationPreview" runat="server" UpdateMode="Conditional"
        Visible="true">
        <ContentTemplate>--%>
            <table width="600px" id="view_type_option" runat="server" border="1">
                <tr>
                    <td width="200" align="right">
                        <asp:Label ID="Label2" runat="server" Text="Quote ID:"></asp:Label>
                    </td>
                    <td width="300">
                        <asp:TextBox ID="TextBoxQID" runat="server">AJPQ000233</asp:TextBox>
                    </td>
                    <td width="100">
                        <asp:Button ID="ButtonQuery" runat="server" Text="Query" OnClick="Button1_Click" />
                    </td>
                </tr>
                <tr>
                    <td width="200" align="right">
                        <asp:Label ID="Label3" runat="server" Text="Send PI to:"></asp:Label>
                    </td>
                    <td width="300">
                        <asp:TextBox ID="txtEmail" runat="server" Width="264px">frank.chung@advantech.com.tw</asp:TextBox>
                    </td>
                    <td width="100">
                        <asp:Button ID="ButtonSend" runat="server" Text="Send" OnClick="ButtonSend_Click" />
                    </td>
                </tr>
                <tr>
                    <td width="200" align="right">
                        <asp:Label ID="Label1" runat="server" Text="View Type:"></asp:Label>
                    </td>
                    <td width="300">
                        <asp:RadioButtonList ID="RadioButtonList_PriviewMode" runat="server" RepeatDirection="Horizontal"
                            OnSelectedIndexChanged="RadioButtonList_PriviewMode_SelectedIndexChanged" AutoPostBack="True"
                            Width="300px">
                            <asp:ListItem Value="true" Selected="True">Internal User</asp:ListItem>
                            <asp:ListItem Value="false">External User</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td width="100">
                      <%--  <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="UP_QuotationPreview" runat="server">
                            <ProgressTemplate>
                                <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/LoadingRed.gif" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>--%>
                    </td>
                </tr>
            </table>
            <br />
            <br />
            <div id="div1" runat="server" style="width: 1000px">
            </div>
       <%--    </ContentTemplate>
     <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ButtonQuery" />
            <asp:AsyncPostBackTrigger ControlID="ButtonSend" />
            <asp:AsyncPostBackTrigger ControlID="RadioButtonList_PriviewMode" />
        </Triggers>
    </asp:UpdatePanel>--%>
    </form>
</body>
</html>