<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="PickProduct.ascx.vb" Inherits="EDOC.PickProduct" %>
<asp:Panel DefaultButton="btnSH" runat="server" ID="pldd">
    Part No:<asp:TextBox runat="server" ID="txtName"></asp:TextBox>
    <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" TargetControlID="txtName"
                                ServicePath="~/Services/AutoComplete.asmx" ServiceMethod="GetPartNo" MinimumPrefixLength="2">
                            </ajaxToolkit:AutoCompleteExtender>
    Desc:<asp:TextBox runat="server" ID="txtDesc"></asp:TextBox>
    Org:<asp:DropDownList runat="server" ID="drpOrg">
        <asp:ListItem Text="Australia" Value="AU01" />
        <asp:ListItem Text="Brazil" Value="BR01" />
        <asp:ListItem Text="China" Value="CN01" />
        <asp:ListItem Text="Japan" Value="JP01" />
        <asp:ListItem Text="Korea" Value="KR01" />
        <asp:ListItem Text="Malaysia" Value="MY01" />
        <asp:ListItem Text="Singapore" Value="SG01" />
        <asp:ListItem Text="Thailand" Value="TL01" />
        <asp:ListItem Text="Taiwan" Value="TW01" />
        <asp:ListItem Text="US" Value="US01" />
        <asp:ListItem Text="EU" Value="EU10" />
    </asp:DropDownList>
    <%--<asp:TextBox runat="server" ID="txtOrg" ReadOnly="true"></asp:TextBox>--%>
    <asp:Button runat="server" ID="btnSH" OnClick="btnSH_Click" Text="Search" />
</asp:Panel>
<asp:GridView DataKeyNames="Part_no" ID="GridView1" AllowPaging="true" PageIndex="0"
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
        <asp:BoundField HeaderText="Part No" DataField="part_no" ItemStyle-HorizontalAlign="Left" />
        <asp:BoundField HeaderText="Model No" DataField="model_no" ItemStyle-HorizontalAlign="Left" />
        <asp:BoundField HeaderText="Description" DataField="product_desc" ItemStyle-HorizontalAlign="Left" />
        <asp:BoundField HeaderText="Status" DataField="product_status" ItemStyle-HorizontalAlign="Left" />
    </Columns>
</asp:GridView>
