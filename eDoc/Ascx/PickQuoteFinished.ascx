<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="PickQuoteFinished.ascx.vb" Inherits="EDOC.PickQuoteFinished" %>
<asp:Panel DefaultButton="btnSH" runat="server" ID="pldd">
    quoteId/Desc:<asp:TextBox runat="server" ID="txtName"></asp:TextBox>
    <asp:Button runat="server" ID="btnSH" OnClick="btnSH_Click" Text="Search" />
</asp:Panel>
<asp:GridView DataKeyNames="quoteId" ID="GridView1" AllowPaging="true"
    PageIndex="0" PageSize="10" runat="server" AutoGenerateColumns="false" OnPageIndexChanging="GridView1_PageIndexChanging"
     OnRowDataBound="GridView1_RowDataBound">
    <Columns>
        <asp:TemplateField>
            <HeaderTemplate>
                <asp:Label runat="server" ID="lbPick" Text="Pick"></asp:Label>
            </HeaderTemplate>
            <ItemTemplate>
                <asp:LinkButton runat="server" ID="lbtnPick" Text="Pick" OnClick="lbtnPick_Click"></asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>
        <%--<asp:BoundField HeaderText="Quote ID" DataField="quoteId" ItemStyle-HorizontalAlign="Left" />--%>
        <asp:BoundField HeaderText="Quote No" DataField="quoteNo" ItemStyle-HorizontalAlign="Left" />
        <asp:BoundField HeaderText="Description" DataField="CustomId" ItemStyle-HorizontalAlign="Left" />
        <asp:BoundField HeaderText="Quote Date" DataField="quoteDate" ItemStyle-HorizontalAlign="Left" />
        <asp:BoundField HeaderText="Status" DataField="DOCSTATUS" ItemStyle-HorizontalAlign="Left" />
    </Columns>
</asp:GridView>