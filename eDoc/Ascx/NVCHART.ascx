<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="NVCHART.ascx.vb" Inherits="EDOC.NVCHART" %>
<table style="width: auto">
    <tr>
        <td>
            <asp:HyperLink NavigateUrl="~/quote/chartover.aspx" runat="server" ID="hlChart" Text="Overview"></asp:HyperLink>
        </td>
        <td>
            |
        </td>
        <td>
            <asp:HyperLink NavigateUrl="~/quote/chart.aspx" runat="server" ID="HyperLink1" Text="Detail"></asp:HyperLink>
        </td>
        <td>
            |
        </td>
        <td>
            <asp:HyperLink NavigateUrl="~/quote/chartproduct.aspx" runat="server" ID="HyperLink2"
                Text="Product"></asp:HyperLink>
        </td>
    </tr>
</table>