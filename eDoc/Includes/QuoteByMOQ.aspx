<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="QuoteByMOQ.aspx.vb" Inherits="EDOC.QuoteByMOQ" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 100px;
        }
        .auto-style2 {
            width: 600px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="700">
            <tr>
                <td valign="baseline" class="auto-style1">Scales:</td>
                <td valign="baseline" align="left" class="auto-style2">
                    1 / <asp:TextBox ID="TxtScale1" runat="server" Width="40px"/>
                      / <asp:TextBox ID="TxtScale2" runat="server" Width="40px"/>
                      / <asp:TextBox ID="TxtScale3" runat="server" Width="40px"/>
                      / <asp:TextBox ID="TxtScale4" runat="server" Width="40px"/>
                    <asp:Button ID="Button1" runat="server" Text="Apply Scales" />
                </td>
            </tr>
                        <tr>
                <td colspan="2" width="700px" align="left"></td></tr>
            <tr>
                <td colspan="2" width="700px">
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"></asp:GridView>
                </td>
            </tr>
            <tr>
                <td colspan="7" align="left" width="600px" valign="baseline">
                    <asp:Label ID="Label1" runat="server" Text="Replace quote's qty/unit price by breakdown qty: "></asp:Label>                    
                    <asp:TextBox ID="TxtApplyQuoteQty" runat="server" Width="40px"/>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="TxtApplyQuoteQty" runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="\d+"></asp:RegularExpressionValidator>
                </td>
            <tr>
                <td colspan="2" width="700px" align="center">
                    <asp:Button ID="Button2" runat="server" Text="Save" /><BR/>
                &nbsp;<asp:Label ID="LBMsg" runat="server" Text="" ForeColor="#ff3300"></asp:Label>
                </td>
            </tr>
            </tr>
        </table>

    </div>
    </form>
</body>
</html>
