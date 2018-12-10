<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="PartialDeliver.ascx.vb" Inherits="EDOC.PartialDeliver" %>
<table width="100%" cellpadding="0" cellspacing="0" id="pdtb" runat="server" >
    <tr>
        <td class="h5" style="width: 25%; white-space:nowrap">
            Partial OK?:
        </td>
        <td>
            <asp:RadioButtonList runat="server" ID="rbtnIsPartial" RepeatDirection="Horizontal">
                <asp:ListItem Value="1" Selected="True">Y</asp:ListItem>
                <asp:ListItem Value="0">N</asp:ListItem>
            </asp:RadioButtonList>
        </td>
    </tr>
</table>