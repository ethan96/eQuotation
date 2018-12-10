<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ICG_Spec.aspx.vb" Inherits="EDOC.ICG_Spec" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Table ID="Table1" runat="server" Height="252px">
            <asp:TableRow>
                <asp:TableCell BackColor="#99FFCC">
                    Product Category
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                    <asp:CheckBox ID="CheckBox1" runat="server" />Media Converter<br />
                    <asp:CheckBox ID="CheckBox2" runat="server" />Layer 2 - Unmanaged
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell BackColor="#99FFCC">
                    Installation
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                    <asp:CheckBox ID="CheckBox3" runat="server" />DIN-Rail/Wall mounting<br />
                    <asp:CheckBox ID="CheckBox4" runat="server" />Rack mounting
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell BackColor="#99FFCC">
                    Port Configuration
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                    <table>
                        <tr>
                            <td>
                                Media Type
                            </td>
                            <td>Port Qty</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:DropDownList ID="DropDownList1" runat="server">
                                    <asp:ListItem>Copper</asp:ListItem>
                                    <asp:ListItem>Fiber</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="DropDownList2" runat="server">
                                    <asp:ListItem>5</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Port Speed
                            </td>
                            <td>Port Interface</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:DropDownList ID="DropDownList3" runat="server">
                                    <asp:ListItem>10/100Mbps</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="DropDownList4" runat="server">
                                    <asp:ListItem>RJ45</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell BackColor="#99FFCC">
                    Certification
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                    <asp:CheckBox ID="CheckBox5" runat="server" />CE/FCC<br />
                    <asp:CheckBox ID="CheckBox6" runat="server" />UL60950<br />
                    <asp:CheckBox ID="CheckBox7" runat="server" />UL508<br />
                    <asp:CheckBox ID="CheckBox8" runat="server" />EN 50155<br />
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell BackColor="#99FFCC">
                    Power
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                    <asp:CheckBox ID="CheckBox9" runat="server" />12-48VDC<br />
                    <asp:CheckBox ID="CheckBox11" runat="server" />24-48VDC<br />
                    <asp:CheckBox ID="CheckBox10" runat="server" />48VDC<br />
                    <asp:CheckBox ID="CheckBox12" runat="server" />110-220VAC<br />
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell BackColor="#99FFCC">
                    Operating Temperature
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                    <asp:CheckBox ID="CheckBox13" runat="server" />-10°C to 60°C<br />
                    <asp:CheckBox ID="CheckBox14" runat="server" />-40°C to 75°C<br />
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </div>
    </form>
</body>
</html>
