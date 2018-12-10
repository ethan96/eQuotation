<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="PickAddressSiebel.ascx.vb" Inherits="EDOC.PickAddressSiebel" %>
 <asp:HiddenField ID="hRowId" runat="server" />
    
        <asp:HiddenField ID="hType" runat="server" />
<asp:GridView DataKeyNames="ad" ID="GridView1" AllowPaging="true"
    PageIndex="0" PageSize="10" runat="server" AutoGenerateColumns="false" OnPageIndexChanging="GridView1_PageIndexChanging">
    <Columns>
        <asp:TemplateField>
            <HeaderTemplate>
                <asp:Label runat="server" ID="lbPick" Text="Pick"></asp:Label>
            </HeaderTemplate>
            <ItemTemplate>
                <asp:LinkButton runat="server" ID="lbtnPick" Text="Pick" OnClick="lbtnPick_Click"></asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField HeaderText="Address" DataField="Ad" ItemStyle-HorizontalAlign="Left" />
    </Columns>
</asp:GridView>
