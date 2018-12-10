    <%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/myMaster.master" CodeBehind="price_bylevel.aspx.vb" Inherits="EDOC.price_bylevel" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr valign="top">
            <td valign="bottom">
                <div class="euPageTitle">
                    Dx Price Inquiry</div>
            </td>
        </tr>
        <tr valign="top">
            <td align="center">
                Search By:
                <asp:DropDownList ID="drpFields" runat="server">
                    <asp:ListItem Value="PROD_LINE">Prod Ln</asp:ListItem>
                    <asp:ListItem Value="part_no">Part No</asp:ListItem>
                </asp:DropDownList>
                <asp:TextBox ID="txtStr" runat="server"></asp:TextBox>
                <asp:Button ID="btnSH" runat="server" Text="Search" OnClick="sh" />
            </td>
        </tr>
        <tr>
            <td>
                <font color="red"><b><%=titler%></b></font>&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:ImageButton runat="server" ID="BtnSave2Excel"
                        OnClick="Export2XLS" ImageUrl="~/images/excel.gif" ToolTip="Save To Excel" />
                <asp:GridView runat="server" ID="GridView1" DataSourceID="SqlDataSource1" OnRowDataBound="GridView1_RowDataBound"
                    AllowPaging="True" PageIndex="0" PageSize="30" Width="100%">
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MyLocal %>">
                </asp:SqlDataSource>
            </td>
        </tr>
    </table>
</asp:Content>
