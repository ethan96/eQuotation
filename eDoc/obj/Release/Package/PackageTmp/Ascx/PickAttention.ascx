<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="PickAttention.ascx.vb" Inherits="EDOC.PickAttention" %>
<asp:Panel DefaultButton="btnSH" runat="server" ID="pldd" Width="800px">
    Row ID:<asp:TextBox runat="server" ID="txtID" ReadOnly="true" BackColor="#ebebe4"></asp:TextBox>
    Email/Name:<asp:TextBox runat="server" ID="txtName"></asp:TextBox>
    <asp:Button runat="server" ID="btnSH" OnClick="btnSH_Click" Text="Search" />
</asp:Panel>
<asp:GridView DataKeyNames="ROW_ID,EMAIL_ADDRESS" ID="GridView1" AllowPaging="true" PageIndex="0"
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
        <asp:BoundField HeaderText="First Name" DataField="FirstName" ItemStyle-HorizontalAlign="Left" />
        <asp:BoundField HeaderText="Mid Name" DataField="MiddleName" ItemStyle-HorizontalAlign="Left" />
        <asp:BoundField HeaderText="Last Name" DataField="LastName" ItemStyle-HorizontalAlign="Left" />
        <asp:BoundField HeaderText="Email" DataField="EMAIL_ADDRESS" ItemStyle-HorizontalAlign="Left" />
    </Columns>
</asp:GridView>