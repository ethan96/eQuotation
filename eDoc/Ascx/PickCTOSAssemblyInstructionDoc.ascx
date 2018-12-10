<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="PickCTOSAssemblyInstructionDoc.ascx.vb" Inherits="EDOC.PickCTOSAssemblyInstructionDoc" %>
<asp:HiddenField runat= "server" ID="h_rowid" />
<asp:HiddenField runat= "server" ID="h_system_name" />
<table>
    <tr>
        <td>
            <asp:Panel runat="server" ID="PanelSearch" DefaultButton="btnSearch">
                <table>
                    <tr>
                        <th align="left" style="color: Black">
                            ERPID:
                        </th>
                        <td>
                            <asp:TextBox runat="server" ID="txtERPID" />
                        </td>
                        <th align="left" style="color: Black">
                            File Name:
                        </th>
                        <td>
                            <asp:TextBox runat="server" ID="txtDocTxt" />
                        </td>
                        <td>
                            <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </td>
    </tr>
    <tr>
        <td>
            <asp:GridView ID="GridView1" DataKeyNames="FILEP" AllowPaging="true" PageIndex="0" EmptyDataText="No search results were found."
    PageSize="15" runat="server" AutoGenerateColumns="false" OnPageIndexChanging="GridView1_PageIndexChanging" Width="100%">
    <Columns>
        <asp:TemplateField>
            <HeaderTemplate>
                <asp:Label runat="server" ID="lbPick" Text="Pick" />
            </HeaderTemplate>
            <ItemTemplate>
                <asp:LinkButton runat="server" ID="lbtnPick" Text="Pick" OnClick="lbtnPick_Click" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField HeaderText="Name" DataField="DOKNR" ItemStyle-HorizontalAlign="Left" />
        <asp:TemplateField>
            <HeaderTemplate>
                File
            </HeaderTemplate>
            <ItemTemplate>
                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%#Bind("FILEP")%>' Target="_blank">
                    <asp:Label ID="Label1" runat="server" Text='<%#Bind("DKTXT")%>' />
                </asp:HyperLink>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField HeaderText="File Version" DataField="DOKVR" ItemStyle-HorizontalAlign="Left" />
        <asp:BoundField HeaderText="File Type" DataField="DAPPL" ItemStyle-HorizontalAlign="Left" />
    </Columns>
</asp:GridView>
        </td>
    </tr>
</table>