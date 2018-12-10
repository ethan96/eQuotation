<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UCCreditInfo.ascx.vb" Inherits="EDOC.UCCreditInfo" %>

<div>
    <style type="text/css">
        .style1 {
            border-color: black;
            border-width: 1px;
            border-style: Solid;
            width: 70%;
            border-collapse: collapse;
        }

        .style2 {
            border-color: black;
            border-width: 1px;
            border-style: Solid;
            width: 15%;
            border-collapse: collapse;
            background-color: #F85C12;
            color: white;
            font-size: 14px;
        }

        .style3 {
            border-color: black;
            border-width: 1px;
            border-style: Solid;
            width: 35%;
            border-collapse: collapse;
            font-size: 14px;
        }
    </style>

    <div>
        <table class="style1" style="margin:auto">
            <tr>
                <th align="left" colspan="4" style="color: black; font-size: 14px;">Customer's Credit Info</th>
            </tr>
            <tr>
                <td class="style2">ORG :
                </td>
                <td class="style3">
                    <asp:Label ID="lbORG" runat="server" Text="" />
                </td>
                <td class="style2">ERP ID :
                </td>
                <td class="style3">
                    <asp:Label ID="lbERPID" runat="server" Text="" />
                </td>
            </tr>
            <tr>
                <td class="style2">Credit Control Area :
                </td>
                <td class="style3">
                    <asp:Label ID="lbCreditControlArea" runat="server" Text="" />
                </td>
                <td class="style2">Currency :
                </td>
                <td class="style3">
                    <asp:Label ID="lbCurrency" runat="server" Text="" />
                </td>
            </tr>

            <tr>
                <td class="style2">Credit Limit :
                </td>
                <td class="style3">
                    <asp:Label ID="lbCreditLimit" runat="server" Text="" />
                </td>
                <td class="style2">Credit Exposure :
                </td>
                <td class="style3">
                    <asp:Label ID="lbCreditExposure" runat="server" Text="" />
                </td>
            </tr>
            <tr>
                <td class="style2">Credit limit used :
                </td>
                <td class="style3">
                    <asp:Label ID="lbPercentage" runat="server" Text="" />
                </td>
                <td class="style2">Receivables :
                </td>
                <td class="style3">
                    <asp:Label ID="lbReceivables" runat="server" Text="" />
                </td>
            </tr>
            <tr>
                <td class="style2">Special Liability :
                </td>
                <td class="style3">
                    <asp:Label ID="lbSpecialLiability" runat="server" Text="" />
                </td>
                <td class="style2">Sales Value :
                </td>
                <td class="style3">
                    <asp:Label ID="lbSalesValue" runat="server" Text="" />
                </td>
            </tr>
            <tr>
                <td class="style2">Risk Category :
                </td>
                <td class="style3">
                    <asp:Label ID="lbRiskCategory" runat="server" Text="" />
                </td>
                <td class="style2">Blocked :
                </td>
                <td class="style3">
                    <asp:Label ID="lbBlocked" runat="server" Text="" />
                </td>
            </tr>
        </table>
    </div>
</div>
