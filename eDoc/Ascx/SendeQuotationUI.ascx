<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="SendeQuotationUI.ascx.vb" Inherits="EDOC.SendeQuotationUI" %>

<%@ Register Src="~/ascx/QuotationViewOption.ascx" TagName="QuotationViewOptionUC" TagPrefix="myASCX" %>
<%@ Register assembly="EDOC" namespace="EDOC" tagprefix="cc1" %>
<table width="600">
    <tr>
        <td align="right" width="200">
            <b>Quote No :</b>
        </td>
        <td>
            <asp:TextBox ID="txtQuoteIdFW" runat="server" ReadOnly="true"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td align="right">
            <b>Recipient Email :</b>
        </td>
        <td>
            <asp:TextBox ID="txtEmail" runat="server" Width="350"></asp:TextBox><br />
            For multi-Email address please separates with ";"
        </td>
    </tr>
    <tr>
        <td align="right">
            <b>Subject :</b>
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
    <tr runat="server" id="trAttachedFiles" visible="false">
        <td align="right">
            <b>Attached Files :</b>
        </td>
        <td>
            <table>
                <tr>
                    <td>
                        <input type="file" runat="server" id="upload_your_file1" />
                    </td>
                    <td>
                        <input type="file" runat="server" id="upload_your_file2" /></td>
                </tr>
                <tr>
                    <td>
                        <input type="file" runat="server" id="upload_your_file3" />
                    </td>
                    <td>
                        <input type="file" runat="server" id="upload_your_file4" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr runat="server" id="trDisplayFormat">
        <td align="right">
            <b>Display Format :</b>
        </td>
        <td>
           <myASCX:QuotationViewOptionUC ID="QuotationViewOptionUC1" runat="server" />
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <b>Email Greeting : <asp:Button ID="ButtonSaveSignature" runat="server" Text="Save Signature" Visible="false" /></b>
         <%--   <uc1:NoToolBarEditor2 ID="txtemailgreeting" runat="server" Height="200" Width="100%"
                previewmode="true" previewoninit="true" showwaitmessage="true" showquickformat="true"
                submit="false" NoScript="True" />--%>
         
            <cc1:NoToolBarEditor2 ID="txtemailgreeting" runat="server" Height="100" Width="100%"
                previewmode="true" previewoninit="true" showwaitmessage="true" showquickformat="true"
                submit="false" NoScript="True"  AutoFocus="false"/>
         
        </td>
    </tr>
</table>