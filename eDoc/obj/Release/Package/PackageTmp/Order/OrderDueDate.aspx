<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/myMaster.master"
    CodeBehind="OrderDueDate.aspx.vb" Inherits="EDOC.OrderDueDate" %>

<%@ Register Src="~/ASCX/Order/PickCalendar.ascx" TagName="Calendar" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <center>
        <div style="width: 1000px">
            <asp:Literal runat="server" ID="litorderinfo"></asp:Literal>
            <div id="divDetailInfo" class="mytable">
                <div class="bk5">
                </div>
                <fieldset>
                    <legend><span style="font-weight: bold; font-size: 12px; color: #666666">Purchased Products</span></legend>
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:UpdatePanel runat="server" ID="UpReqDate" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView runat="server" ID="gv1" AutoGenerateColumns="false" AllowPaging="false"
                                            AllowSorting="true" Width="100%" EmptyDataText="No Order Line." DataKeyNames="lineNo"
                                            OnDataBound="gv1_DataBound" OnRowDataBound="gv1_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>
                                                        Seq.
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex + 1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>
                                                        Line No.
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <%# CType(Container.DataItem, IBUS.iCartLine).lineNo.Value%>
                                                        <asp:HiddenField ID="HFlineNo" runat="server" Value='<%# CType(Container.DataItem, IBUS.iCartLine).lineNo.Value%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Customer PN.">
                                                    <ItemTemplate>
                                                        <asp:TextBox runat="server" ID="txtCustPN" Text='<%#Server.HtmlDecode(CType(Container.DataItem, IBUS.iCartLine).CustMaterial.Value) %>'
                                                            Width="80px" OnTextChanged="txtCustPN_TextChanged"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="left">
                                                    <HeaderTemplate>
                                                        Product
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <%# CType(Container.DataItem, IBUS.iCartLine).partNo.Value%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="left">
                                                    <HeaderTemplate>
                                                        Description
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <%# getDescForPN(CType(Container.DataItem, IBUS.iCartLine).partNo.Value, CType(Container.DataItem, IBUS.iCartLine).partDesc.Value)%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="left">
                                                    <HeaderTemplate>
                                                        <asp:Label runat="server" ID="lbHDueDate">Due Date</asp:Label>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <%# IIf(CDate(CType(Container.DataItem, IBUS.iCartLine).dueDate.Value).ToString(Pivot.CurrentProfile.DatePresentationFormat) = CDate("1900/01/01").ToString(Pivot.CurrentProfile.DatePresentationFormat), "TBD", CDate(CType(Container.DataItem, IBUS.iCartLine).dueDate.Value).ToString(Pivot.CurrentProfile.DatePresentationFormat))%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="left">
                                                    <HeaderTemplate>
                                                        <asp:Label runat="server" ID="lbHReqDate"> Required Date </asp:Label>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="txtreqdate" name="txtreqdate" runat="server" Text='<%# CDate(CType(Container.DataItem, IBUS.iCartLine).reqDate.Value).ToString(Pivot.CurrentProfile.DatePresentationFormat)%>'
                                                                        Width="65px" Style="text-align: right"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:ImageButton runat="server" ID="ibtnReqDate" OnClick="ibtnReqDate_Click" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>
                                                        Sales Leads from Advantech (DMF)
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <%# CType(Container.DataItem, IBUS.iCartLine).DMFFlag.Value%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>
                                                        Extended Warranty Months
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lbew" Text='<%# CType(Container.DataItem, IBUS.iCartLine).EWFlag.Value %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>
                                                        Qty.
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <%# CType(Container.DataItem, IBUS.iCartLine).Qty.Value%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>
                                                        Price
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%# crs%>' ID="lbUnitPriceSign"></asp:Label>
                                                        <%# FormatNumber(CType(Container.DataItem, IBUS.iCartLine).newunitPrice.Value, 2)%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>
                                                        Sub Total
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%# crs%>' ID="lbSubTotalSign"></asp:Label>
                                                        <%# FormatNumber(CType(Container.DataItem, IBUS.iCartLine).newunitPrice.Value * CType(Container.DataItem, IBUS.iCartLine).Qty.Value, 2)%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" runat="server" id="trFreight" visible="false">
                                Freight：<%= crs%><asp:Label runat="server" ID="lbFt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                Total：<%= crs%><asp:Label runat="server" ID="lbTotal"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <table width="100%">
                    <tr>
                        <td align="center">
                            <asp:Button runat="server" Text=" >> Update << " ID="btnUpdate" OnClick="btnUpdate_Click" />
                        </td>
                    </tr>
                </table>
                <br />
                <table width="100%">
                    <tr>
                        <td align="center">
                            <asp:Button runat="server" Text="Next" ID="btnConfirm" OnClick="btnConfirm_Click"
                                Width="120px" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label runat="server" ID="lbMsg" Font-Bold="true" ForeColor="Tomato" />
                        </td>
                    </tr>
                </table>
            </div>
            <asp:Panel ID="PLPickCalendar" runat="server" Style="display: none" CssClass="modalPopup">
                <div style="text-align: right;">
                    <asp:ImageButton ID="CancelCalendar" runat="server" ImageUrl="~/Images/del.gif" />
                </div>
                <div>
                    <asp:UpdatePanel runat="server" ID="UPASCXCAL" UpdateMode="Conditional">
                        <ContentTemplate>
                            <uc1:Calendar ID="ascxCalendar" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </asp:Panel>
            <asp:LinkButton ID="lbDummyCal" runat="server"></asp:LinkButton>
            <ajaxToolkit:ModalPopupExtender ID="MPPickCalendar" runat="server" TargetControlID="lbDummyCal"
                PopupControlID="PLPickCalendar" BackgroundCssClass="modalBackground" CancelControlID="CancelCalendar"
                DropShadow="true" />
        </div>
    </center>
</asp:Content>
