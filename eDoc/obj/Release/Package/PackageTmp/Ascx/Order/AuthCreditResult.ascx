<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="AuthCreditResult.ascx.vb" Inherits="EDOC.AuthCreditResult" %>
<table width="95%" runat="server" id="trAuthInfo" visible="false" style="border-style:double" align="center">
    <tr>
        <td colspan="2"><strong>Verification Result：</strong></td>
    </tr>
    <tr>
        <td align="left">
            Result:
        </td>
        <td>
            <asp:Label runat="server" ID="Label1" Width="150px" />
        </td>
        <td align="left" runat="server" id="td1" visible="false">
            PN Reference:
        </td>
        <td runat="server" id="td2" visible="false">
            <asp:Label runat="server" ID="Label2" Width="150px" />
        </td>
    </tr>
    <tr>
        <td align="left">
            Response Message:
        </td>
        <td>
            <asp:Label runat="server" ID="Label3" />
        </td>
        <td align="left" runat="server" id="td3" visible="false">
            Authentication Code:
        </td>
        <td runat="server" id="td4" visible="false">
            <asp:Label runat="server" ID="Label4" Width="150px" />
        </td>
    </tr>
    <tr runat="server" id="tr2" visible="false">
        <td align="left">
            AVS ADDR:
        </td>
        <td>
            <asp:Label runat="server" ID="Label5" Width="150px" />
        </td>
        <td align="left">
            AVS ZIP:
        </td>
        <td>
            <asp:Label runat="server" ID="Label6" Width="150px" />
        </td>
    </tr>
    <tr runat="server" id="tr3" visible="false">
        <td align="left">
            IAVS:
        </td>
        <td>
            <asp:Label runat="server" ID="Label7" Width="150px" />
        </td>
        <td align="left">
            CVV2MATCH:
        </td>
        <td>
            <asp:Label runat="server" ID="Label8" Width="150px" />
        </td>
    </tr>
    <tr runat="server" id="tr4" visible="false">
        <td align="left">
            Duplicate:
        </td>
        <td colspan="3">
            <asp:Label runat="server" ID="Label9" Width="150px" />
        </td>
    </tr>
    <tr runat="server" id="trFraudRow" visible="false">
        <td align="left">
            PREFPSMSG(Fraud):
        </td>
        <td>
            <asp:Label runat="server" ID="Label10" Width="150px" />
        </td>
        <td align="left">
            POSTFPSMSG(Fraud):
        </td>
        <td>
            <asp:Label runat="server" ID="Label11" Width="150px" />
        </td>
    </tr>
    <tr>
        <td colspan="2"><asp:Label runat="server" ID="Label12" /></td>
    </tr>
</table>