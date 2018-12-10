<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="PickCalendar.ascx.vb" Inherits="EDOC.PickCalendar" %>
<asp:HiddenField ID="hDesC" runat="server" />
<asp:HiddenField ID="hCompany" runat="server" />
        <table cellpadding="0" cellspacing="0" style="width:auto">
            <tr>
                <td align="left" valign="top" colspan="4" style="text-align:center">
                    <asp:TextBox runat="server" ID="txtType" Text="" Visible="false"></asp:TextBox>
                    <asp:TextBox runat="server" ID="txtElement" Text="" Visible="false"></asp:TextBox>
                    <asp:TextBox runat="server" ID="txtFormat" Text="dd/MM/yyyy" Visible="false"></asp:TextBox>
                    <asp:TextBox runat="server" ID="txtSalesOrg" Text="EU10" Visible="false"></asp:TextBox>
                    <asp:TextBox runat="server" ID="txtCustomerId" Text="Default" Visible="false"></asp:TextBox>
                    <asp:TextBox runat="server" ID="txtPlant" Text="Default" Visible="false"></asp:TextBox>
                    <asp:Calendar ID="Calendar1" runat="server" BackColor="White" ForeColor="Black"
                        Font-Names="Verdana" Font-Size="8pt" 
                        
                        PrevMonthText="<font color='#ffffff'><b><&nbsp;Prev</b></font>" NextMonthText="<font color='#ffffff'><b>Next&nbsp;></b></font>"
                        SelectedDayStyle-BorderStyle="Solid" 
                        SelectedDayStyle-BorderColor="#999999" SelectedDayStyle-BorderWidth="1px"
                        SelectedDayStyle-BackColor="#ffffff" SelectedDayStyle-ForeColor="red" SelectedDayStyle-Font-Bold="true"
                        OnDayRender="Calendar1_DayRender" 
                        OnVisibleMonthChanged="Calendar1_VisibleMonthChanged" 
                        OnPreRender="Calendar1_PreRender" BorderColor="#999999" CellPadding="4" 
                        DayNameFormat="Shortest">
                        <DayHeaderStyle Font-Bold="True" Font-Size="7pt" BackColor="#CCCCCC" />
                        <NextPrevStyle 
                            VerticalAlign="Bottom" />
                        <OtherMonthDayStyle ForeColor="Gray" />
<SelectedDayStyle BackColor="#666666" BorderColor="#999999" BorderWidth="1px" BorderStyle="Solid" 
                            ForeColor="White" Font-Bold="True"></SelectedDayStyle>

                        <SelectorStyle BackColor="#CCCCCC" />

<TitleStyle BackColor="#0066FF" CssClass="NoUndl" Height="20px" Font-Bold="True" Font-Size="8pt" ForeColor="White" BorderColor="Black"></TitleStyle>

                        <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
                        <WeekendDayStyle BackColor="#FFFFFF" />
                    </asp:Calendar>
                </td>
            </tr>
            <tr>
                <td>
                    <b>&nbsp;Month:</b><input type="hidden" name="Flag" id="Flag" value="NO" />
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddMonth"
                        AutoPostBack="true">
                        <asp:ListItem Value="1" Text="January"></asp:ListItem>
                        <asp:ListItem Value="2" Text="February"></asp:ListItem>
                        <asp:ListItem Value="3" Text="March"></asp:ListItem>
                        <asp:ListItem Value="4" Text="April"></asp:ListItem>
                        <asp:ListItem Value="5" Text="May"></asp:ListItem>
                        <asp:ListItem Value="6" Text="June"></asp:ListItem>
                        <asp:ListItem Value="7" Text="July"></asp:ListItem>
                        <asp:ListItem Value="8" Text="August"></asp:ListItem>
                        <asp:ListItem Value="9" Text="September"></asp:ListItem>
                        <asp:ListItem Value="10" Text="October"></asp:ListItem>
                        <asp:ListItem Value="11" Text="November"></asp:ListItem>
                        <asp:ListItem Value="12" Text="December"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <b>Year</b>
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddYear"
                        AutoPostBack="true">
                        <asp:ListItem Value="2012" Text="2012" Selected="True" />
                        <asp:ListItem Value="2013" />
                        <asp:ListItem Value="2014" />
                        <asp:ListItem Value="2015" />
                    </asp:DropDownList>
                </td>
            </tr>
            <%--<tr>
                <td colspan="4">
                    <font color="red"><b>Tip: Day in red is today.</b></font>
                </td>
            </tr>--%>
        </table>
