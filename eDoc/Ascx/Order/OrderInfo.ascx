<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="OrderInfo.ascx.vb" Inherits="EDOC.OrderInfo1" %>
<div id="divOrderInfo" class="mytable">
    <div class="bk5">
    </div>
     <fieldset>
	 <legend><span style="font-weight:bold; font-size:12px; color:#666666">Order Information</span></legend>
    <table width="100%">
        <tr>
            <td class="h5" width="15%">
                PO No:
            </td>
            <td width="35%">
                <asp:Label runat="server" ID="lbPO"/>
            </td>
            <td class="h5"  width="15%">
                <asp:Literal runat="server" ID="litSO">Advantech Quote No:</asp:Literal>
            </td>
            <td width="35%">
                <asp:Label runat="server" ID="lbSO"/>
            </td>
        </tr>
        <tr>
            <td>
                Order Date:
            </td>
            <td>
                <asp:Label runat="server" ID="lbOrderDate"/>
            </td>
            <td>
                Payment Term:
            </td>
            <td>
                <asp:Label runat="server" ID="lbPayTerm"/>
            </td>
        </tr>
        <tr>
            <td>
                Placed By:
            </td>
            <td>
                <asp:Label runat="server" ID="lbPlacedBy"/>
            </td>
            <td>
                Incoterm:
            </td>
            <td>
                <asp:Label runat="server" ID="lbIncoterm"/>
            </td>
        </tr>
        <tr>
            <td>
                Freight:
            </td>
            <td>
                <asp:Label runat="server" ID="lbFreight"/>
            </td>

            <td>
                <asp:Label runat="server" ID="lbSalesRepTitle">Sales Representative:</asp:Label>
            </td>
            <td>
                <asp:Label runat="server" ID="lbSalesRep"/>
            </td>
        </tr>
        <tr id="trPartial" runat="server" visible="true">
            <td>
                Partial OK:
            </td>
            <td colspan="3">
                <asp:Label runat="server" ID="lbisPartial"/>
            </td>
        </tr>
        <tr>
            <td>
                Order Note:<br/>(External Note)
            </td>
            <td>
                <asp:Label runat="server" ID="lbOrderNote"/>
            </td>
            <td>
                eQuote#:
            </td>
            <td>
                <asp:Label runat="server" ID="lbQuoteNo"/>
            </td>
<%--            <td>
                <asp:Label runat="server" ID="LabelChannelTitle" Visible="false">Channel:</asp:Label>
            </td>
            <td>
                <asp:Label runat="server" ID="lbChannel" Visible="false"></asp:Label>
            </td>
--%>        </tr>
<%--        <tr id="trPartial" runat="server" visible="false">
            <td>
                Partial OK:
            </td>
            <td>
                <asp:Label runat="server" ID="lbisPartial"></asp:Label>
            </td>
            <td>
                Incoterm Text:
            </td>
            <td>
                <asp:Label runat="server" ID="lbIncotermText"></asp:Label>
            </td>
        </tr>
--%>        <tr id="trReqdate" runat="server" visible="false">
            <td>
                Required Date:
            </td>
            <td>
                <asp:Label runat="server" ID="lbReqdate"/>
            </td>
            <td>
                <asp:Label runat="server" ID="LabelShipCondition" Visible="false">Ship Condition:</asp:Label>
            </td>
            <td>
                <asp:Label runat="server" ID="lbShipCond" Visible="false"/>
            </td>
        </tr>
        <tr id="trSalesNote" runat="server" visible="false">
            <td>
                Sales Note From Customer:
            </td>
            <td colspan="3">
                <asp:Label runat="server" ID="lbSalesNote"/>
            </td>
        </tr>
        <tr id="trEUOPN" runat="server">
            <td>
                EU OP Note:
            </td>
            <td colspan="3">
                <asp:Label runat="server" ID="lbOPNote"/>
            </td>
        </tr>
    </table></fieldset>
    <%--    <table width="100%">
        <tr>
            <td style="background-color: #ededed; font-weight: bold" colspan="4">
                Order Information
            </td>
        </tr>
        <tr>
            <td>
                PO No.
            </td>
            <td>
                <asp:Label runat="server" ID="lbPO"></asp:Label>
            </td>
            <td>
                <asp:Literal runat="server" ID="litSO"> Advantech Quote No.</asp:Literal>
            </td>
            <td>
                <asp:Label runat="server" ID="lbSO"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                Order Date
            </td>
            <td>
                <asp:Label runat="server" ID="lbOrderDate"></asp:Label>
            </td>
            <td>
                Payment Term
            </td>
            <td>
                <asp:Label runat="server" ID="lbPayTerm"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                Required Date
            </td>
            <td>
                <asp:Label runat="server" ID="lbReqdate"></asp:Label>
            </td>
            <td>
                Incoterm
            </td>
            <td>
                <asp:Label runat="server" ID="lbIncoterm"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                Placed By
            </td>
            <td>
                <asp:Label runat="server" ID="lbPlacedBy"></asp:Label>
            </td>
            <td>
                Incoterm Text
            </td>
            <td>
                <asp:Label runat="server" ID="lbIncotermText"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                Freight
            </td>
            <td>
                <asp:Label runat="server" ID="lbFreight"></asp:Label>
            </td>
            <td>
                <asp:Label runat="server" ID="LabelChannelTitle">Channel</asp:Label>
            </td>
            <td>
                <asp:Label runat="server" ID="lbChannel"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                Partial OK
            </td>
            <td>
                <asp:Label runat="server" ID="lbisPartial"></asp:Label>
            </td>
            <td>
                Ship Condition
            </td>
            <td>
                <asp:Label runat="server" ID="lbShipCond"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                Order Note (External Note)
            </td>
            <td colspan="3">
                <asp:Label runat="server" ID="lbOrderNote"></asp:Label>
            </td>
        </tr>
        <tr id="trSalesNote" runat="server">
            <td>
                Sales Note From Customer
            </td>
            <td colspan="3">
                <asp:Label runat="server" ID="lbSalesNote"></asp:Label>
            </td>
        </tr>
        <tr id="trEUOPN" runat="server">
            <td>
                EU OP Note
            </td>
            <td colspan="3">
                <asp:Label runat="server" ID="lbOPNote"></asp:Label>
            </td>
        </tr>
    </table>--%>
</div>
