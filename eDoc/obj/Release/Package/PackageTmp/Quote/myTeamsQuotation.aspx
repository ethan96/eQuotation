<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/myMaster.master" CodeBehind="myTeamsQuotation.aspx.vb" Inherits="EDOC.myTeamsQuotation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table>
        <tr>
            <td>
                <asp:Label runat="server" ID="Label3" Text="Quote No"></asp:Label>
                :
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtQuoteId"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label runat="server" ID="lbRoleName" Text="Quote Desc"></asp:Label>
                :
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtCustomId"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label runat="server" ID="lbRoleValue" Text="<%$ Resources:myRs,AccountName %>"></asp:Label>
                :
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtAccountName"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label runat="server" ID="Label1" Text="<%$ Resources:myRs,CompanyID %>"></asp:Label>
                :
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtCompanyID"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label runat="server" ID="Label2" Text="<%$ Resources:myRs,Status %>"></asp:Label>
                :
            </td>
            <td>
                <asp:DropDownList ID="drpStatus" runat="server">
                    <asp:ListItem Value="">All</asp:ListItem>
                    <asp:ListItem Value="0">Draft</asp:ListItem>
                    <asp:ListItem Value="1">Finish</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label runat="server" ID="Label4" Text="State"></asp:Label>
                :
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtState"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label runat="server" ID="Label5" Text="Province"></asp:Label>
                :
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtProvince"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table>
        <tr>
            <td align="center">
                <asp:Button runat="server" ID="btnSH" OnClick="btnSH_Click" Text="Search" />
            </td>
        </tr>
    </table>
    <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString="<%$ connectionStrings: EQ %>"
        SelectCommand=""></asp:SqlDataSource>
    <asp:GridView DataKeyNames="quoteId" ID="GridView1" DataSourceID="SqlDataSource1"
        AllowSorting="true" AllowPaging="true" PageIndex="0" PageSize="30" runat="server"
        AutoGenerateColumns="false" OnPageIndexChanging="GridView1_PageIndexChanging"
        Width="100%" OnRowDataBound="GridView1_RowDataBound" OnSorting="GridView1_Sorting">
        <Columns>
<%--            <asp:BoundField HeaderText="Quote ID" DataField="quoteId" ItemStyle-HorizontalAlign="Left"
                SortExpression="quoteId" />
--%>            <asp:BoundField HeaderText="Quote No" DataField="quoteNo" ItemStyle-HorizontalAlign="Left"
                SortExpression="quoteNo" />
            <asp:BoundField HeaderText="Quote Desc" DataField="customId" ItemStyle-HorizontalAlign="Left"
                SortExpression="customId" />
            <asp:BoundField HeaderText="ERP ID" DataField="quoteToErpId" ItemStyle-HorizontalAlign="Left"
                SortExpression="quoteToErpId" />
            <asp:BoundField HeaderText="Account Name" DataField="quoteToName" ItemStyle-HorizontalAlign="Left"
                SortExpression="quoteToName" />
            <asp:TemplateField>
                <HeaderTemplate>
                    <asp:Label runat="server" ID="lbUpdateErpId" Text="GP Flow"></asp:Label>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" ID="lbGpFlow"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderTemplate>
                    <asp:Label runat="server" ID="lbHdEdit" Text="Process"></asp:Label>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" ID="lbProcess"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="Quote Date" DataField="quoteDate" DataFormatString="{0:d}"
                ItemStyle-HorizontalAlign="Left" SortExpression="quoteDate" />
            <asp:BoundField HeaderText="Status" DataField="DOCSTATUS" ItemStyle-HorizontalAlign="Left"
                SortExpression="DOCSTATUS" />
            <asp:BoundField HeaderText="Creator" DataField="createdBy" ItemStyle-HorizontalAlign="Left"
                SortExpression="DOCSTATUS" />
            <asp:TemplateField>
                <HeaderTemplate>
                    <asp:Label runat="server" ID="lbHdEdit" Text="<%$ Resources:myRs,Detail %>"></asp:Label>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:ImageButton ImageUrl="~/Images/search.gif" runat="server" ID="ibtnDetail" OnClick="ibtnDetail_Click" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>
