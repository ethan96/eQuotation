<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/myMaster.master"
    CodeBehind="USPriceOverWrite.aspx.vb" Inherits="EDOC.USPriceOverWrite1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField runat="server" ID="hQuoteId" />
    <asp:HiddenField runat="server" ID="hOrg" />
    <fieldset>
        <legend><span style="font-weight: bold; font-size: 12px; color: #666666">Search</span></legend>
        <table>
            <tr>
                <td>
                    <table>
                        <tr>
                            <td align="right">
                                Quote Id:
                            </td>
                            <td align="left">
                                <asp:TextBox runat="server" ID="txtQuoteId"></asp:TextBox>
                            </td>
                            <td>
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
                    </table>
                    <table style="text-align: center">
                        <tr>
                            <td>
                                <asp:Button runat="server" ID="btnSearch" Text=">>Search<<" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="lbMsg" ForeColor="Orange" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </fieldset>
    <hr />
    <asp:GridView runat="server" ID="gv1" AutoGenerateColumns="false" AllowPaging="false"
        DataKeyNames="LineNo" EmptyDataText="You can search by a specified quote No. to get a item list.">
        <Columns>
            <asp:TemplateField>
                <HeaderTemplate>
                    Quote No</HeaderTemplate>
                <ItemTemplate>
                    <%= Me.txtQuoteId.Text%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderTemplate>
                    Revision</HeaderTemplate>
                <ItemTemplate>
                    <%= Me.drpRevision.SelectedValue%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderTemplate>
                    Line No</HeaderTemplate>
                <ItemTemplate>
                    <asp:TextBox runat="server" ID="txtLineNo" Text=" <%# CType(Container.DataItem, IBUS.iCartLine).lineNo.Value%>"
                        ReadOnly="true"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderTemplate>
                    Part No</HeaderTemplate>
                <ItemTemplate>
                    <asp:TextBox runat="server" ID="txtPartNo" Text=" <%# CType(Container.DataItem, IBUS.iCartLine).PartNo.Value%>"
                        ReadOnly="true"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderTemplate>
                    List Price</HeaderTemplate>
                <ItemTemplate>
                    <asp:TextBox runat="server" ID="txtListPrice" Text=" <%# CType(Container.DataItem, IBUS.iCartLine).listPrice.Value%>"
                        ReadOnly="true"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderTemplate>
                    Unit Price</HeaderTemplate>
                <ItemTemplate>
                    <asp:TextBox runat="server" ID="txUnitPrice" Text=" <%# CType(Container.DataItem, IBUS.iCartLine).newUnitPrice.Value%>"
                        OnTextChanged="onPriceChanged"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <table style="text-align: center">
        <tr>
            <td>
                <asp:Button runat="server" ID="btnUpdate" Text=">> Update <<" />
            </td>
        </tr>
    </table>
</asp:Content>
