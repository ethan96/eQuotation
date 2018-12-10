<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ProdInvalidList.ascx.vb"
    Inherits="EDOC.ProdInvalidList" %>
<asp:GridView ID="gvUnAvailablePartNo" runat="server" AutoGenerateColumns="false">
    <Columns>
        <asp:TemplateField HeaderText="PartNo">
            <ItemTemplate>
                <%# CType(Container.DataItem, IBUS.iCartLine).partNo.Value%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Warning">
            <ItemTemplate>
               <b><%# CType(Container.DataItem, IBUS.iCartLine).partDesc.Value%></b> 
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
