<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PipeLine.aspx.vb" Inherits="EDOC.PipeLine" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table>
        <tr>
            <td>
                Sales Person:
            </td>
            <td>
                <asp:TextBox ID="TextBox_SalesPerson" runat="server" />
            </td>
            <td>
                District:
            </td>
            <td>
                <asp:TextBox ID="TextBox_District" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                Company Name:
            </td>
            <td>
                <asp:TextBox ID="TextBox_AccountName" runat="server" />
            </td>
            <td>
                Company URL:
            </td>
            <td>
                <asp:TextBox ID="TextBox_AccoutURL" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                Industry:
            </td>
            <td>
                <asp:DropDownList ID="DropDownList_Industry" runat="server">
                    <asp:ListItem>Automotive</asp:ListItem>
                    <asp:ListItem>Government</asp:ListItem>
                    <asp:ListItem>Pharmaceutical</asp:ListItem>
                    <asp:ListItem>Manufacturing</asp:ListItem>
                    <asp:ListItem>Transportation</asp:ListItem>
                    <asp:ListItem>Medical</asp:ListItem>
                    <asp:ListItem>System Intergrator</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                Application:
            </td>
            <td>
                <asp:TextBox ID="TextBox_Application" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                Sales Stage:
            </td>
            <td>
                <asp:DropDownList ID="DropDownList_SalesStage" runat="server">
                    <asp:ListItem>0%</asp:ListItem>
                    <asp:ListItem Selected="False">25%</asp:ListItem>
                    <asp:ListItem>50%</asp:ListItem>
                    <asp:ListItem>75%</asp:ListItem>
                    <asp:ListItem>100%</asp:ListItem>
                </asp:DropDownList>

            </td>
            <td>
                Type of customer:
            </td>
            <td>
                <asp:DropDownList ID="DropDownList_CustomerType" runat="server">
                    <asp:ListItem>SI</asp:ListItem>
                    <asp:ListItem>End User</asp:ListItem>
                </asp:DropDownList>

            </td>
        </tr>
        <tr>
            <td>
                Ship Month:
            </td>
            <td>
                <asp:DropDownList ID="DropDownList_ShipMonth" runat="server">
                    <asp:ListItem>Jan</asp:ListItem>
                    <asp:ListItem>Feb</asp:ListItem>
                    <asp:ListItem>March</asp:ListItem>
                    <asp:ListItem>April</asp:ListItem>
                    <asp:ListItem>May</asp:ListItem>
                    <asp:ListItem>June</asp:ListItem>
                    <asp:ListItem>July</asp:ListItem>
                    <asp:ListItem>Aug</asp:ListItem>
                    <asp:ListItem>Sep</asp:ListItem>
                    <asp:ListItem>Oct</asp:ListItem>
                    <asp:ListItem>Nov</asp:ListItem>
                    <asp:ListItem>Dec</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td colspan="4">
                <asp:GridView Width="100%" ID="GridView1" runat="server" DataKeyNames="Line_No" AutoGenerateColumns="false">
                    <Columns>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:CheckBox ID="CheckBox1" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Line No." DataField="Line_No" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="Part No." DataField="PartNo" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="Qty" DataField="Qty" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="Unit Price" DataField="NewUnitPrice" ItemStyle-HorizontalAlign="Left" />
                        <asp:TemplateField HeaderText="Sub Total">
                            <ItemTemplate>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td colspan="4" align="center">
                <asp:Button ID="Button1" runat="server" Text="Send to Pipeline" />
                <asp:Button ID="Button2" runat="server" Text="Cancel" />
            </td>

        </tr>

    </table>

    </div>
    </form>
</body>
</html>
