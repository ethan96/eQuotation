<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/myMaster.master" CodeBehind="Agent.aspx.vb" Inherits="EDOC.Agent" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr valign="top">
            <td>
            </td>
        </tr>
        <tr valign="top">
            <td>
                <table width="100%" id="Table2">
                    <tr valign="top">
                        <td height="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="2">
                            <div class="euPageTitle">
                                Agent Setting</div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="right">
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="2" height="2" align="center">
                            <table width="500px" cellpadding="2" cellspacing="0" runat="server" border="0" id="RegTable">
                                <tr>
                                    <th align="left" style="color:#333333;width: 20%; border-bottom: SOLID 1PX #EEEEEE;">
                                        Primary Owner :
                                    </th>
                                    <td align="left" style="width: 70%; border-bottom: SOLID 1PX #EEEEEE; border-left: SOLID 1PX #EEEEEE">
                                        &nbsp;<asp:TextBox runat="server" ID="txt_Primary" Width="250px" />
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" style="Color:#333333;width: 20%; border-bottom: SOLID 1PX #EEEEEE;">
                                        Agent
                                    </th>
                                    <td align="left" style="width: 70%; border-bottom: SOLID 1PX #EEEEEE; border-left: SOLID 1PX #EEEEEE">
                                        &nbsp;<asp:TextBox runat="server" ID="txt_Agent" Width="250px" />
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" style="Color:#333333;width: 20%; border-bottom: SOLID 1PX #EEEEEE;">
                                        Sequence
                                    </th>
                                    <td align="left" style="width: 70%; border-bottom: SOLID 1PX #EEEEEE; border-left: SOLID 1PX #EEEEEE">
                                        &nbsp;<asp:TextBox runat="server" ID="txt_Seq" Width="25px" />
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" style="Color:#333333;width: 20%; border-bottom: SOLID 1PX #EEEEEE;">
                                        From Date :
                                    </th>
                                    <td align="left" style="width: 70%; border-bottom: SOLID 1PX #EEEEEE; border-left: SOLID 1PX #EEEEEE">
                                        &nbsp;<asp:TextBox runat="server" ID="txt_From" Width="100px" />
                                        <ajaxToolkit:CalendarExtender TargetControlID="txt_From" runat="server" Format="yyyyMMdd"
                                            ID="calDate" />
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" style="Color:#333333;width: 20%">
                                        To Date :
                                    </th>
                                    <td align="left" style="width: 70%; border-left: SOLID 1PX #EEEEEE">
                                        &nbsp;<asp:TextBox runat="server" ID="txt_To" Width="100px" />
                                        <ajaxToolkit:CalendarExtender TargetControlID="txt_To" runat="server" Format="yyyyMMdd"
                                            ID="CalendarExtender1" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="left">
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center">
                                        <asp:Button runat="server" ID="btnSubmit" Text=" Add " OnClick="btnSubmit_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="border-bottom: dashed 1px #6699CC">
                            &nbsp;
                        </td>
                    </tr>
                    <tr valign="top">
                        <td align="left">
                            <table>
                                <tr>
                                    <td>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    </td>
                                    <td>
                                        Search By:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpFields" runat="server">
                                            <asp:ListItem Value="email">Primary Owner</asp:ListItem>
                                            <asp:ListItem Value="aemail">Agent</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtStr" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnSH" runat="server" Text="Search" OnClick="btnSH_Click" />
                                    </td>
                                </tr>
                            </table>
                            <table cellpadding="1" width="100%">
                                <tr>
                                    <td style="background-color: #666666">
                                        <asp:GridView runat="server" ID="GridView1" DataSourceID="SqlDataSource1" AllowPaging="True"
                                            PageIndex="0" PageSize="30" Width="100%" AutoGenerateColumns="false" DataKeyNames="ID">
                                            <Columns>
                                                <asp:BoundField DataField="email" HeaderText="Primary Owner" />
                                                <asp:BoundField DataField="aemail" HeaderText="Agent" />
                                                <asp:BoundField DataField="seq" HeaderText="Seq." />
                                                <asp:BoundField DataField="fdate" HeaderText="From Date" />
                                                <asp:BoundField DataField="tdate" HeaderText="To Date" />
                                                <%--<asp:TemplateField HeaderText="Status">
                                                                <ItemTemplate>
                                                                    <asp:DropDownList runat="server" SelectedValue='<%#Bind("active") %>' ID="active"
                                                                        Enabled="false">
                                                                        <asp:ListItem Value="1">Active</asp:ListItem>
                                                                        <asp:ListItem Value="0">Non-Active</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:DropDownList runat="server" SelectedValue='<%#Bind("active") %>' ID="active">
                                                                        <asp:ListItem Value="1">Active</asp:ListItem>
                                                                        <asp:ListItem Value="0">Non-Active</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </EditItemTemplate>
                                                            </asp:TemplateField>--%>
                                                <asp:CommandField ShowDeleteButton="true" />
                                                <asp:CommandField ShowEditButton="true" />
                                            </Columns>
                                        </asp:GridView>
                                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:EQ %>"
                                            UpdateCommand="update [agent] set [email]=@email,[aemail]=@aemail,[seq]=@seq,[fdate]=@fdate,[tdate]=@tdate
                                                                 where ID=@ID" DeleteCommand="DELETE FROM [agent] WHERE ID=@ID">
                                            <UpdateParameters>
                                                <asp:Parameter Type="string" Name="email" />
                                                <asp:Parameter Type="string" Name="aemail" />
                                                <asp:Parameter Type="string" Name="seq" />
                                                <asp:Parameter Type="string" Name="fdate" />
                                                <asp:Parameter Type="string" Name="tdate" />
                                            </UpdateParameters>
                                        </asp:SqlDataSource>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td height="2">
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr valign="top">
            <td height="2">
                &nbsp;
            </td>
        </tr>
        <tr valign="top">
            <td>
            </td>
        </tr>
    </table>
</asp:Content>
