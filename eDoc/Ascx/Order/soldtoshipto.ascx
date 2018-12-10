<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="soldtoshipto.ascx.vb" Inherits="EDOC.soldtoshipto" %>
<div id="divCustInfo" class="mytable">
    <div class="bk5">
    </div>
     <fieldset>
	 <legend><span style="font-weight:bold; font-size:12px; color:#666666">Customer Information</span></legend>
    <table width="100%">
        <tr>
            <td class="h5" width="15%">
                Sold to:
            </td>
            <td width="35%">
                <asp:Literal runat="server" ID="litsoldto"></asp:Literal>
            </td>
            <td class="h5" width="15%">
                Ship to:
            </td>
            <td width="35%">
                <asp:Literal runat="server" ID="litshipto"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td class="h5">
                Company:
            </td>
            <td>
                <asp:Literal runat="server" ID="litsoldtocompany"></asp:Literal>
            </td>
            <td class="h5">
                Company:
            </td>
            <td>
                <asp:Literal runat="server" ID="litshiptocompany"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td class="h5">
                Address:
            </td>
            <td>
                <asp:Literal runat="server" ID="litsoldtoaddress"></asp:Literal>
            </td>
            <td class="h5">
                Address:
            </td>
            <td>
                <asp:Literal runat="server" ID="litshiptoaddress"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td class="h5">
                Tel:
            </td>
            <td>
                <asp:Literal runat="server" ID="litsoldtotel"></asp:Literal>
            </td>
            <td class="h5">
                Address2:
            </td>
            <td>
                <asp:Literal runat="server" ID="litshiptoaddress2"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td class="h5">
            </td>
            <td>
            </td>
            <td class="h5">
                Tel:
            </td>
            <td>
                <asp:Literal runat="server" ID="litshiptotel"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Literal runat="server" ID="litsoldtoattention" Visible="false"></asp:Literal>
            </td>
            <td class="h5">
                Attention:
            </td>
            <td>
                <asp:Literal runat="server" ID="litshiptoattention"></asp:Literal>
            </td>
        </tr>
    </table></fieldset>
</div>