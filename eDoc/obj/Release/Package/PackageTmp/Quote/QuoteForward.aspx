<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="QuoteForward.aspx.vb" Inherits="EDOC.QuoteForward" %>
<%@ Register assembly="EDOC" namespace="EDOC" tagprefix="cc1" %>
<%@ Register Namespace="eBizAEUControls" TagPrefix="uc1" %>
<%@ Register Src="~/ascx/SendeQuotationUI.ascx" TagName="SendeQuotationUI" TagPrefix="myASCX" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Quote Forward</title>
    <link href="../Css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <style type="text/css"> 
    .disable 
    { 
        border-style:none; 
        border-width: thin; 
        background-color:Transparent; 
        color: #000000; 
        font-size:16px;
        cursor:wait; 
    } 
    </style> 
</head>
<body>


    <script type="text/javascript" language="javascript">
        function DisableButton() {
            document.getElementById('<%=Me.btnSend.ClientId %>').className = "disable";
            document.getElementById('<%=Me.btnSend.ClientId %>').value = 'waiting...';
            document.getElementById('<%=Me.btnSend.ClientId %>').onclick = Function("return false;");
            document.body.style.cursor = "wait"
            return true;
        } 
    </script> 

    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="margin-top: 10px;">
    
                <myASCX:SendeQuotationUI ID="forwardEquotationUI" runat="server" />
                <table width="600">
                    <tr style="height: 25px" align="center">
                        <td colspan="2">
                            <asp:Label runat="server" ID="lbForwardMsg" Font-Bold="true" ForeColor="Tomato" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <asp:Button ID="btnSend" runat="server" Text="Send" OnClick="btnSend_Click" />
                          
                          <%--  <asp:UpdateProgress runat="server" ID="ups1" AssociatedUpdatePanelID="UPFW">
                          
                                <ProgressTemplate>
                                    <img src="../Images/loading.gif" />
                                </ProgressTemplate>
                            </asp:UpdateProgress>--%>
                        </td>
                    </tr>
                    <tr runat="server" id="trSignature" visible="false">
                        <td colspan="2">
                            <b>Signature : <asp:Button ID="ButtonSaveSignature" runat="server" Text="Save Signature" /></b>
                            <cc1:notoolbareditor2 id="txtSignature" runat="server" height="100" width="100%"
                                previewmode="true" previewoninit="true" showwaitmessage="true" showquickformat="true"
                                submit="false" noscript="True" autofocus="false" />

                        </td>
                    </tr>
                </table>
<%--                <table width="600">
                    <tr>
                        <td align="right" width="200">
                            <b>Quote ID :</b>
                        </td>
                        <td>
                            <asp:TextBox ID="txtQuoteIdFW" runat="server" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <b>Recipient Email:</b>
                        </td>
                        <td>
                            <asp:TextBox ID="txtEmail" runat="server" Width="350"></asp:TextBox><br />
                            For multi-Email address please separates with ";"
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <b>Subject:</b>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtFwdSubject" Width="400px" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <b>Type :</b>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rbtnFW" runat="server" RepeatColumns="2">
                                <asp:ListItem Value="HTM">Html</asp:ListItem>
                                <asp:ListItem Value="PDF" Selected="True">PDF</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <b>Email Greeting :</b>
                            <uc1:notoolbareditor2 id="txtemailgreeting" runat="server" height="250" width="100%"
                                previewmode="true" previewoninit="true" showwaitmessage="true" showquickformat="true"
                                submit="false" noscript="True" />
                        </td>
                    </tr>
                    <tr style="height: 25px" align="center">
                        <td colspan="2">
                            <asp:Label runat="server" ID="lbForwardMsg" Font-Bold="true" ForeColor="Tomato" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <asp:Button ID="btnSend" runat="server" Text="Send" OnClick="btnSend_Click" />
                            <asp:UpdateProgress runat="server" ID="ups1" AssociatedUpdatePanelID="UPFW">
                                <ProgressTemplate>
                                    <img src="../Images/loading.gif" />
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </td>
                    </tr>
                </table>--%>
           
    </div>
    </form>
</body>
</html>
