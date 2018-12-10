<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="PickShipToBillToSAP.ascx.vb" Inherits="EDOC.PickShipToBillToSAP" %>
<asp:Panel DefaultButton="btnSH" runat="server" ID="pldd">
    <asp:HiddenField ID="hType" runat="server" />
    <asp:HiddenField ID="hPERPID" runat="server" />
    <asp:HiddenField ID="hORG" runat="server" />
    <asp:HiddenField runat="server" ID="hdDistChannel" />
    <asp:HiddenField runat="server" ID="hdDivision" />
    <asp:HiddenField runat="server" ID="hdSalesGroup" />
    <asp:HiddenField runat="server" ID="hdSalesOffice" />
    <asp:HiddenField runat="server" ID="hIsAll" />
    <h3>
        Pick Ship-To/Bill-To</h3>
    <table width="600px">
        <tr>
            <th align="left" style="color: #333333">
                Name:
            </th>
            <td>
                <asp:TextBox runat="server" ID="txtAccountName" Width="120" />
            </td>
            <th align="left" style="color: #333333">
                ERP ID:
            </th>
            <td>
                <asp:TextBox runat="server" ID="txtERPID" Width="100" />
            </td>
            <th align="left" style="color: #333333">
                Address:
            </th>
            <td>
                <asp:TextBox runat="server" ID="txtAddress" Width="150" />
            </td>
            <td colspan="6">
                <asp:Button runat="server" ID="btnSH" OnClick="btnSH_Click" Text="Search" />
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:Panel runat="server" ID="PanelResult" ScrollBars="Auto">
    <asp:GridView DataKeyNames="COMPANY_ID,COMPANY_NAME,ADDRESS" ID="GridView1" AllowPaging="true"
        PageIndex="0" PageSize="10" runat="server" AutoGenerateColumns="false" OnPageIndexChanging="GridView1_PageIndexChanging"
         OnRowDataBound="GridView1_RowDataBound">
        <Columns>
            <asp:TemplateField>
                <HeaderTemplate>
                    <asp:Label runat="server" ID="lbPick" Text="Pick" />
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:HiddenField runat="server" ID="hdRowSGroup" Value='<%#Eval("SalesGroup") %>' />
                    <asp:HiddenField runat="server" ID="hdRowSOffice" Value='<%#Eval("SalesOffice") %>' />
                    <asp:LinkButton runat="server" ID="lbtnPick" Text="Pick" OnClick="lbtnPick_Click" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="ERP ID" DataField="COMPANY_ID" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="Account Name" DataField="COMPANY_NAME" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="Address" DataField="ADDRESS" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="Type" DataField="PARTNER_FUNCTION" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Division" DataField="division" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Sales Group" DataField="SalesGroup" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Sales Office" DataField="SalesOffice" ItemStyle-HorizontalAlign="Center" />
        </Columns>
    </asp:GridView>
</asp:Panel>