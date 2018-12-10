<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/myMaster.master" CodeBehind="MyAccountQuotation.aspx.vb" Inherits="EDOC.MyAccountQuotation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <table width="100%">
        <tr>
            <td>
                <table></table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel runat="server" ID="up1" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView runat="server" ID="gv1" Width="100%" AutoGenerateColumns="false" AllowSorting="true" 
                            AllowPaging="true" PageSize="50" PagerSettings-Position="TopAndBottom" DataSourceID="src1" OnPageIndexChanging="gv1_PageIndexChanging" OnSorting="gv1_Sorting">
                            <Columns>
                                <%--<asp:BoundField HeaderText="quoteId" DataField="quoteId" SortExpression="quoteId" />--%>
                                <asp:BoundField HeaderText="quoteNo" DataField="quoteNo" SortExpression="quoteNo" />
                                <asp:BoundField HeaderText="quoteToName" DataField="quoteToName" SortExpression="quoteToName" />
                                <asp:BoundField HeaderText="createdBy" DataField="createdBy" SortExpression="createdBy" />
                                <asp:BoundField HeaderText="createdDate" DataField="createdDate" SortExpression="createdDate" />
                            </Columns>
                        </asp:GridView>
                        <asp:SqlDataSource runat="server" ID="src1" ConnectionString="<%$ConnectionStrings:EQ %>" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>
