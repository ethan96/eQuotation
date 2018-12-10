<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/myMaster.master"
    CodeBehind="USPriceOW.aspx.vb" Inherits="EDOC.USPriceOW" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField runat="server" ID="hQuoteId" />
    <asp:HiddenField runat="server" ID="hQNO" />
    <asp:HiddenField runat="server" ID="hRID" />
    <asp:HiddenField runat="server" ID="hOrg" />
    <br />Unit Price Overwriting <br /><br />
    <fieldset>
        <legend><span style="font-weight: bold; font-size: 12px; color: #666666">Search</span></legend>
        <table>
            <tr>
                <td>
                    <table>
                        <tr>
                            <td align="left" width="70px">
                                Quote No:
                            </td>
                            <td align="left" width="100px">
                                <asp:TextBox runat="server" ID="txtQuoteId"></asp:TextBox>
                            </td>
                            <td align="left" width="100px">
                                <asp:Button runat="server" ID="btnSearch" Text=">>Search<<" />
                            </td>
                            <td>
                                Click <asp:HyperLink ID="HyperLink1" NavigateUrl="USPriceOW_Log.aspx" runat="server">here</asp:HyperLink> to trace price overwriting log
                            </td>
                            <td>
                                <asp:Panel ID="PLRevsion" runat="server" Style="display: none" CssClass="modalPopup">
                                    <div style="text-align: right;">
                                        <asp:ImageButton ID="CancelButtonRevsion" runat="server" ImageUrl="~/Images/del.gif" />
                                    </div>
                                    <div>
                                        <asp:UpdatePanel ID="UPPickRevsion" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            Revision Pick:
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList runat="server" ID="drpRevision" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Button runat="server" ID="btnConfirm" Text="Confirm" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </asp:Panel>
                                <asp:LinkButton ID="lbDummyRevsion" runat="server" />
                                <ajaxToolkit:ModalPopupExtender ID="MPPickRevsion" runat="server" TargetControlID="lbDummyRevsion"
                                    PopupControlID="PLRevsion" BackgroundCssClass="modalBackground" CancelControlID="CancelButtonRevsion"
                                    DropShadow="false" />
                            </td>
                        </tr>
                        <tr><td colspan="3" align="left"><asp:Label runat="server" ID="lbMsg" ForeColor="Orange" /></td></tr>
                    </table>
                </td>
            </tr>
        </table>
    </fieldset>
    <hr />
    <asp:GridView runat="server" ID="gv1" AutoGenerateColumns="false" AllowPaging="false"
        EmptyDataText="You can search by a specified quote No. to get a item list.">
        <Columns>
            <asp:TemplateField ItemStyle-Width="100">
                <HeaderTemplate>
                    Quote No</HeaderTemplate>
                <ItemTemplate>
                    <%= Me.txtQuoteId.Text%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-Width="100">
                <HeaderTemplate>
                    Revision</HeaderTemplate>
                <ItemTemplate>
                    <%= Me.drpRevision.SelectedValue%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-Width="100">
                <HeaderTemplate>
                    Line No</HeaderTemplate>
                <ItemTemplate>
                    <asp:TextBox runat="server" ID="txtLineNo" BackColor="#eeeeee" Text=" <%# CType(Container.DataItem, IBUS.iCartLine).lineNo.Value%>"
                        ReadOnly="true" Width="50px" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderStyle-Width="250" ItemStyle-Width="300">
                <HeaderTemplate>
                    Part No</HeaderTemplate>
                <ItemTemplate>
                    <asp:TextBox runat="server" ID="txtPartNo" BackColor="#eeeeee" Text=" <%# CType(Container.DataItem, IBUS.iCartLine).PartNo.Value%>"
                        ReadOnly="true" Width="250px" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderStyle-Width="100" ItemStyle-Width="150">
                <HeaderTemplate>
                    List Price</HeaderTemplate>
                <ItemTemplate>
                    <asp:TextBox runat="server" ID="txtListPrice" BackColor="#eeeeee" Text=" <%# CType(Container.DataItem, IBUS.iCartLine).listPrice.Value%>"
                        ReadOnly="true" Width="100px" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Unit Price">
                <ItemTemplate>
                    <asp:TextBox runat="server" ID="txtUnitPrice" Text=" <%# CType(Container.DataItem, IBUS.iCartLine).newUnitPrice.Value%>"
                        OnTextChanged="txtUnitPrice_TextChanged" />
                    <asp:Label runat="server" ID="lbLineUpdateMsg" ForeColor="Orange" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <table style="text-align: center">
        <tr>
            <td width="870px" align="right">
                <asp:Button runat="server" ID="btnUpdate" Text=">> Update <<" />
            </td>
            <td align="left">
            </td>
        </tr>
    </table>
</asp:Content>
