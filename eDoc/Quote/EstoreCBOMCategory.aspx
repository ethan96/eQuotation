<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/myMaster.master" CodeBehind="EstoreCBOMCategory.aspx.vb" Inherits="EDOC.EstoreCBOMCategory" %>
<%@ Register Src="~/ascx/CheckUID.ascx" TagName="CheckUID" TagPrefix="myASCX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<myASCX:CheckUID runat="server" ID="ascxCheckUID" />
    <table>
        <tr valign="top">
            <td align="left">
                <asp:GridView runat="server" ID="AdxGrid1" DataSourceID="SqlDataSource1" AutoGenerateColumns="false"
                    DataKeyNames="CategoryName" Width="400px">
                    <Columns>
                        <asp:TemplateField ItemStyle-Width="2%" ItemStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                No.
                            </HeaderTemplate>
                            <ItemTemplate>
                                <font size="2"><%# Container.DataItemIndex + 1 %></font>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--        <asp:HyperLinkField DataTextField="CategoryName" HeaderText="Category Name" DataNavigateUrlFields="CategoryName"
                            DataNavigateUrlFormatString="EstoreCBOMList.aspx?CategoryName={0}&UID=<%=Request("UID") %> />--%>
                        <asp:TemplateField HeaderText="Category Name">
                            <ItemTemplate>
                                <font size="2">
                                <a href="EstoreCBOMList.aspx?CategoryName=<%# Server.UrlEncode(Eval("CategoryName "))%>&UID=<%=Request("UID") %>">
                                    <%# Eval("CategoryName ")%>
                                </a></font>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:B2B %>">
                </asp:SqlDataSource>
            </td>
        </tr>
        <tr>
            <td align="left">
            </td>
        </tr>
    </table>
</asp:Content>
