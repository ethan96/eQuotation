<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="PickSignature.ascx.vb" Inherits="EDOC.PickSignature" %>

<asp:GridView ID="GV1" runat="server" AutoGenerateColumns="False" EmptyDataText="No search results were found."
    Width="100%" AllowPaging="True" DataKeyNames="SID" AllowSorting="True" PageSize="20">
    <Columns>
        <asp:TemplateField ItemStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
            <HeaderTemplate>
                Pick
            </HeaderTemplate>
            <ItemTemplate>
                <asp:LinkButton runat="server" ID="lbtnPick" Text="Pick" OnClick="lbtnPick_Click" />
            </ItemTemplate>
        </asp:TemplateField>
<%--        <asp:BoundField ItemStyle-Width="200px" HeaderText="Active" DataField="Active" ItemStyle-HorizontalAlign="left"
            SortExpression="Active" />
        <asp:BoundField ItemStyle-Width="200px" HeaderText="File Name" DataField="FileName"
            ItemStyle-HorizontalAlign="left" SortExpression="FileName" />
--%>        <asp:TemplateField>
            <ItemTemplate>
                <asp:Image ID="Image1" runat="server" ImageUrl='<%# Eval("SID", "~\Ascx\DisplayImageHandler.ashx?Type=signature&ImageID={0}")%>' />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
