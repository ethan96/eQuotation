<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="pickBTO.ascx.vb" Inherits="EDOC.pickBTO" %>
<asp:Panel DefaultButton="btnSH" runat="server" ID="pldd">
    Part No:<asp:TextBox runat="server" ID="txtName"></asp:TextBox>

    <%--<asp:TextBox runat="server" ID="txtOrg" ReadOnly="true"></asp:TextBox>--%>
     <asp:HiddenField ID="HORG" runat="server" />
     <asp:HiddenField ID="HRBU" runat="server" />

    <asp:Button runat="server" ID="btnSH" OnClick="btnSH_Click" Text="Search" />
</asp:Panel>
<asp:GridView DataKeyNames="CATALOG_ID" ID="GridView1" AllowPaging="true" PageIndex="0"
    PageSize="10" runat="server" AutoGenerateColumns="false" OnPageIndexChanging="GridView1_PageIndexChanging">
    <Columns>
        <asp:TemplateField>
            <HeaderTemplate>
                <asp:Label runat="server" ID="lbPick" Text="Pick"></asp:Label>
            </HeaderTemplate>
            <ItemTemplate>
                <asp:LinkButton runat="server" ID="lbtnPick" Text="Pick" OnClick="lbtnPick_Click"></asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField HeaderText="Part No" DataField="CATALOG_ID" ItemStyle-HorizontalAlign="Left" />
        <asp:BoundField HeaderText="Catalog Type" DataField="CATALOG_TYPE" ItemStyle-HorizontalAlign="Left" />
    </Columns>
</asp:GridView>