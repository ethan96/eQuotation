<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="PickQuoteCopy.ascx.vb" Inherits="EDOC.PickQuoteCopy" %>
<asp:Panel DefaultButton="btnSH" runat="server" ID="pldd">
    <br />
    QuoteId/Desc:<asp:TextBox runat="server" ID="txtName"></asp:TextBox>
    <asp:Button runat="server" ID="btnSH" OnClick="btnSH_Click" Text="Search" />
</asp:Panel>
<asp:GridView DataKeyNames="quoteNo" ID="GridView1" AllowPaging="true" PageIndex="0"
    PageSize="10" runat="server" AutoGenerateColumns="false" OnPageIndexChanging="GridView1_PageIndexChanging" OnRowDataBound="GridView1_RowDataBound">
    <Columns>
        <asp:TemplateField>
            <HeaderTemplate>
                <asp:Label runat="server" ID="lbPick" Text="Pick"></asp:Label>
            </HeaderTemplate>
            <ItemTemplate>
                <asp:LinkButton runat="server" ID="lbtnPick" Text="Pick" OnClick="lbtnPick_Click"></asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>
        <%--<asp:BoundField HeaderText="Quote ID" DataField="quoteId" ItemStyle-HorizontalAlign="Center" />--%>
        <asp:BoundField HeaderText="Quote No" DataField="quoteNo" ItemStyle-HorizontalAlign="Center" />
        <%--   <asp:BoundField HeaderText="Description" DataField="CustomId" ItemStyle-HorizontalAlign="Left" />--%>
        <asp:BoundField HeaderText="ERP ID" DataField="quoteToErpId" ItemStyle-HorizontalAlign="Center"
            SortExpression="quoteToErpId" />
        <asp:BoundField HeaderText="Quote Date" DataField="quoteDate" DataFormatString="{0:yyyy/MM/dd}"
            HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
        <asp:BoundField HeaderText="Status" DataField="DOCSTATUS" ItemStyle-HorizontalAlign="Center" />
        <asp:TemplateField HeaderText="Created By" ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
                <asp:Label runat="server" ID="lbCreatedby" Text="" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField ItemStyle-HorizontalAlign="Center" Visible="false">
            <HeaderTemplate>
                <asp:Label runat="server" ID="lbQuote" Text="Action" />
            </HeaderTemplate>
            <ItemTemplate>
                <asp:Button runat="server" ID="btnQuote" Text="Copy to a new Quote" OnClick="btnQuote_Click" />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
