<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/myMaster.master" CodeBehind="atpAndPrice.aspx.vb" Inherits="EDOC.atpAndPrice" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:UpdatePanel runat="server" ID="upContent" UpdateMode="Conditional" ChildrenAsTriggers="false">
        <ContentTemplate>
            
            <asp:GridView runat="server" ID="gv1" AutoGenerateColumns="false" AllowPaging="false"
                AllowSorting="true" Width="100%" EmptyDataText="No search results were found."
                EmptyDataRowStyle-Font-Size="Larger" EmptyDataRowStyle-Font-Bold="true" OnRowDataBound="gv1_RowDataBound">
                <Columns>
                    <asp:TemplateField ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                        <HeaderTemplate>
                            No.
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:BoundField HeaderText="Part No." DataField="part_no" SortExpression="part_no" />
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                        <HeaderTemplate>
                            Qty.
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:GridView runat="server" ID="gv2" AutoGenerateColumns="false" AllowPaging="false"
                                Width="100%" EmptyDataText="N/A" EmptyDataRowStyle-Font-Size="Larger" EmptyDataRowStyle-Font-Bold="true">
                                <Columns>
                                    <asp:BoundField HeaderText="Available Date" DataField="DATE" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField HeaderText="Qty." DataField="QTY" ItemStyle-HorizontalAlign="Center" />
                                </Columns>
                            </asp:GridView>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="List Price" DataField="listPrice" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField HeaderText="Unit Price" DataField="UnitPrice" ItemStyle-HorizontalAlign="Right" />
                   
                </Columns>
                <FooterStyle BackColor="#A4B5BD" ForeColor="White" Font-Bold="True" />
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Justify" />
                <HeaderStyle BackColor="#A4B5BD" Font-Bold="True" ForeColor="White" />
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                <PagerSettings PageButtonCount="10" Position="TopAndBottom" />
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
