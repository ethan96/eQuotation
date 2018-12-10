<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/myMaster.master" CodeBehind="BtosSearch.aspx.vb" Inherits="EDOC.BtosSearch" %>
<%@ Register Src="~/ascx/CheckUID.ascx" TagName="CheckUID" TagPrefix="myASCX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<myASCX:CheckUID runat="server" ID="ascxCheckUID" />
    <table>
        <tr>
            <td width="400">
              Catalog:  <asp:DropDownList runat="server" ID="ddlcatname">
                </asp:DropDownList>
            </td>
            <td>
                <label class="lbStyle">
                     BTOS Name(ex:IPC-*):</label>
                <asp:TextBox runat="server" ID="TBkey"></asp:TextBox>
                <asp:Button runat="server" Text="Search" ID="BTsearch" OnClick="BTsearch_Click" />
            </td>
        </tr>
        <tr valign="top">
            <td align="left" colspan="2">
                <asp:Panel runat="server" ID="plSBC">
                </asp:Panel>
                <asp:GridView runat="server" ID="AdxGrid1" AutoGenerateColumns="false" DataKeyNames="CATALOG_ID">
                    <Columns>
                        <asp:TemplateField ItemStyle-Width="2%" ItemStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                No.
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CATALOG_TYPE" HeaderText="Catalog Name" />
                        <asp:BoundField DataField="CATALOG_ID" HeaderText="BTOS Name" />
                        <asp:TemplateField HeaderText="QTY" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtQty" Text="1" Width="30px" />
                                <ajaxToolkit:FilteredTextBoxExtender runat="server" ID="ft3" TargetControlID="txtQty"
                                    FilterType="Numbers, Custom" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Assemble" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Button ID="btnConfig" runat="server" Text="Config" OnClick="btnConfig_Click" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
</asp:Content>
