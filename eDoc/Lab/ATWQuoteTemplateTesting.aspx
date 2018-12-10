<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ATWQuoteTemplateTesting.aspx.vb" Inherits="EDOC.ATWQuoteTemplateTesting" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="sm1" />
    <h1>ATW,AKR Quotation Template Testing Page</h1> 
<%--    <asp:UpdatePanel ID="UP_QuotationPreview" runat="server" UpdateMode="Conditional"
        Visible="true">
        <ContentTemplate>--%>
            <table width="600px" id="view_type_option" runat="server" border="1">
                <tr>
                    <td width="200" align="right">
                        <asp:Label ID="Label4" runat="server" Text="RBU"/>
                    </td>
                    <td width="300">
                        <asp:ListBox ID="ListBoxRBU" runat="server">
                            <asp:ListItem Value="ATW">ATW</asp:ListItem>
                            <asp:ListItem Value="AKR" Selected="True">AKR</asp:ListItem>
                        </asp:ListBox>
                    </td>
                    <td width="100">
                    </td>
                </tr>
                <tr>
                    <td width="200" align="right">
                        <asp:Label ID="Label2" runat="server" Text="Quote ID:"></asp:Label>
                    </td>
                    <td width="300">
                        <%-- atw quoteid 82ac1a36224149b--%>
                        <asp:TextBox ID="TextBoxQID" runat="server">ed903e08d255440</asp:TextBox>
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
    </form></body>
</html>