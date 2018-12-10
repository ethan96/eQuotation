<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="USAOnlineShipBillTo.ascx.vb" Inherits="EDOC.USAOnlineShipBillTo" %>
<asp:Panel runat="server" ID="panelUSshipbillto" DefaultButton="btnSearch">
    <table width="550px">
        <tr>
            <td align="right">
                <asp:LinkButton runat="server" ID="lnkCloseBtn" Text="Close" OnClick="lnkCloseBtn_Click" />
            </td>
        </tr>
        <tr>
            <td>
                <table>
                    <tr>
                        <td align="left">
                            Ship-To ID:
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtShipID" />
                        </td>
                        <td align="left">
                            Ship-To Name:
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtShipName" />
                        </td>
                        <td>
                            <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:GridView runat="server" ID="gvBillShipTo" AutoGenerateColumns="false" AllowPaging="true"
                    PageIndex="0" PageSize="8" Width="100%" DataKeyNames="company_id,STREET,CITY,STATE,ZIP_CODE,COUNTRY,Attention,TEL_NO,STR_SUPPL3,COMPANY_NAME"
                    OnPageIndexChanging="GridView1_PageIndexChanging">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                ID
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="lbtnPick" OnClick="lbtnPick_Click" Text='<%# Eval("company_id")%>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Name" DataField="COMPANY_NAME" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="Address" DataField="ADDRESS" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="Attention" DataField="Attention" />
                        <asp:BoundField HeaderText="Type" DataField="PARTNER_FUNCTION" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="Division" DataField="division" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="Sales Group" DataField="SalesGroup" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="Sales Office" DataField="SalesOffice" ItemStyle-HorizontalAlign="Center" />
                    </Columns>
                </asp:GridView>
                <asp:Label runat="server" ID="test" Text="Label" Visible="false" />
            </td>
        </tr>
    </table>
</asp:Panel>