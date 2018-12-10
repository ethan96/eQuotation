<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ProductReport.aspx.vb" Inherits="EDOC.ProductReport1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Product Report</title>
    <style>
        #tbreport td {
            padding-left: 9px;
            padding-right: 5px;
            padding-top: 10px;
        }

        body {
            padding: 10px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <table align="center" width="96%" id="tbreport">
            <tr><td height="5"></td></tr>
            <tr>
                <td align="left">PartNo:
                     <strong>
                         <asp:Literal ID="LitPartNo" runat="server"></asp:Literal>
                         <asp:ImageButton ID="ibtnExcel" ImageUrl="~/Images/excel.gif" runat="server" OnClick="ibtnExcel_Click"/>
                     </strong>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:GridView ID="GridView1" Width="100%" runat="server" EmptyDataText="NO DATA."></asp:GridView>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
