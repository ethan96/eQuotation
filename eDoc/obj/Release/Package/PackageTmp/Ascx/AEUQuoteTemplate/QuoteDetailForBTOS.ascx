<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="QuoteDetailForBTOS.ascx.vb"
    Inherits="EDOC.QuoteDetailForBTOS" %>
<style>
    body
    {
        font-family: Calibri;
    }
    .text1
    {
        color: #004B9C;
        font-family: Calibri;
        vertical-align: top;
        font-weight: normal;
        font-size: 14px;
        white-space: nowrap;
        border-bottom-width: 1px;
        border-bottom-style: solid;
        border-bottom-color: #004B9C;
    }
    .text2
    {
        color: #000000;
        font-family: Calibri;
        font-weight: bold;
        vertical-align: top;
        white-space: nowrap;
    }
    .text3
    {
        color: #000000;
        font-family: Calibri;
        font-weight: bold;
        vertical-align: top;
    }
    .text4
    {
        color: #004B9C;
        font-family: Calibri;
        font-weight: bold;
        vertical-align: top;
        white-space: nowrap;
    }
    .btosbr1
    {
      /* background-image: url(<%=Util.GetRuntimeSiteIP()%>/Images/AEUline2.gif);
        background-repeat: repeat-x;
        background-position: left bottom;*/
    }
    .text5
    {
        color: #004B9C;
        font-family: Calibri;
        vertical-align: top;
        font-weight: normal;
        font-size: 14px;
        white-space: nowrap;
        text-align: right;
    }
    .text6
    {
        color: #000000;
        font-family: Calibri;
        font-weight: bold;
        vertical-align: top;
        white-space: nowrap;
        text-align: right;
    }
    .text7
    {
        color: #004B9C;
        font-family: Calibri;
        font-weight: bold;
        font-size: 20px;
        vertical-align: top;
        white-space: nowrap;
    }
    .text8
    {
        color: #004B9C;
        font-family: Calibri;
        font-weight: bold;
        font-size: 28px;
        vertical-align: top;
        white-space: nowrap;
        line-height: 28px;
    }
</style>
<table width="675" align="center" cellpadding="0" cellspacing="0">
<tr><td colspan="2"><span class="text8">Pricing </span></td></tr>
    <tr>
        <td colspan="2">
            <table width="675" align="center">
                <tr>
                    <td class="text4">
                    </td>
                    <td class="text5">
                        Quantity:
                    </td>
                    <td class="text5">
                        Unit Price:
                    </td>
                    <td class="text5">
                        Price:
                    </td>
                </tr>
                <tr>
                    <td class="text7">
                        <asp:Literal ID="LitBtosPartNO" runat="server"></asp:Literal>
                    </td>
                    <td class="text6">
                        <asp:Literal ID="LitQTY" runat="server"></asp:Literal>
                    </td>
                    <td class="text6">
                        <asp:Literal ID="LitUnitPrice" runat="server"></asp:Literal>
                        <%= crs %>
                    </td>
                    <td class="text6">
                        <asp:Literal ID="LitPrice" runat="server"></asp:Literal>
                        <%= crs %>
                    </td>
                </tr>
                <tr>
                    <td height="10" colspan="4">
                        <img src="<%=Util.GetRuntimeSiteIP()%>/Images/AEUline.gif" />
                    </td>
                </tr>
            </table>
            <asp:GridView runat="server" ID="gv1" AutoGenerateColumns="false" ShowHeader="false"
                AllowPaging="false" HeaderStyle-CssClass="text1" AllowSorting="true" Width="100%"
                EmptyDataText="No Order Line." DataKeyNames="lineNo" OnDataBound="gv1_DataBound"
                OnRowDataBound="gv1_RowDataBound" GridLines="None" CellPadding="4">
                <Columns>
                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                        Visible="false">
                        <HeaderTemplate>
                            <span class="PIHT">Seq.</span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                        Visible="false">
                        <HeaderTemplate>
                            <span class="PIHT">Line No.</span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# CType(Container.DataItem, IBUS.iCartLine).lineNo.Value%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="left"
                        ItemStyle-CssClass="text4">
                        <HeaderTemplate>
                            <span class="PIHT">Part Number:</span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# CType(Container.DataItem, IBUS.iCartLine).partNo.Value%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="left" ItemStyle-CssClass="text4">
                        <HeaderTemplate>
                            <span class="PIHT">Virtual Part No</span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# Server.HtmlDecode(CType(Container.DataItem, IBUS.iCartLine).VirtualPartNo.Value)%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="left"
                        ItemStyle-CssClass="text3">
                        <HeaderTemplate>
                            <span class="PIHT">Description:</span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# getDescForPN(CType(Container.DataItem, IBUS.iCartLine).partNo.Value, CType(Container.DataItem, IBUS.iCartLine).partDesc.Value)%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="left"
                        Visible="false">
                        <HeaderTemplate>
                            <span class="PIHT">
                                <asp:Label runat="server" ID="lbHDueDate">Due Date</asp:Label></span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# IIf(CDate(CType(Container.DataItem, IBUS.iCartLine).dueDate.Value).ToString(Pivot.CurrentProfile.DatePresentationFormat) = CDate("1900/01/01").ToString(Pivot.CurrentProfile.DatePresentationFormat), "TBD", IIf(True, CDate(CType(Container.DataItem, IBUS.iCartLine).dueDate.Value).ToString(Pivot.CurrentProfile.DatePresentationFormat), CDate(CType(Container.DataItem, IBUS.iCartLine).dueDate.Value).ToString(Pivot.CurrentProfile.DatePresentationFormat)))%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="left"
                        Visible="false">
                        <HeaderTemplate>
                            <span class="PIHT">
                                <asp:Label runat="server" ID="lbHReqDate"> Required Date </asp:Label></span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# IIf(True, CDate(CType(Container.DataItem, IBUS.iCartLine).reqDate.Value).ToString(Pivot.CurrentProfile.DatePresentationFormat), CDate(CType(Container.DataItem, IBUS.iCartLine).reqDate.Value).ToString(Pivot.CurrentProfile.DatePresentationFormat))%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                        Visible="false">
                        <HeaderTemplate>
                            <span class="PIHT">Sales Leads from Advantech (DMF)</span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# CType(Container.DataItem, IBUS.iCartLine).DMFFlag.Value%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                        Visible="false">
                        <HeaderTemplate>
                            <span class="PIHT">Extended Warranty Months</span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lbew" Text='<%# CType(Container.DataItem, IBUS.iCartLine).EWFlag.Value%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                        ItemStyle-CssClass="text3" Visible="false">
                        <HeaderTemplate>
                            <span class="PIHT">Quantity:</span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# CType(Container.DataItem, IBUS.iCartLine).Qty.Value%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                        ItemStyle-CssClass="text2" Visible="false">
                        <HeaderTemplate>
                            <span class="PIHT">Unit Price:</span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# FormatNumber(CType(Container.DataItem, IBUS.iCartLine).newunitPrice.Value, 2)%>
                            <asp:Label runat="server" Text='<%# crs %>' ID="lbUnitPriceSign"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                        ItemStyle-CssClass="text2" Visible="false">
                        <HeaderTemplate>
                            <span class="PIHT">Price:</span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# FormatNumber(CType(Container.DataItem, IBUS.iCartLine).newunitPrice.Value * CType(Container.DataItem, IBUS.iCartLine).Qty.Value, 2)%>
                            <asp:Label runat="server" Text='<%# crs %>' ID="lbSubTotalSign"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <RowStyle CssClass="btosbr1" />
            </asp:GridView>
        </td>
    </tr>
      <tr runat="server" id="trline1" >
        <td height="2" colspan="2" style="padding-top:8px;">
            <img src="<%=Util.GetRuntimeSiteIP()%>/Images/AEUline.gif" width="100%" />
        </td>
    </tr>
    <tr runat="server" visible="false">
        <td align="right" width="580" class="text4">
            Sub-Total：
        </td>
        <td align="right" class="text3">
            <asp:Label runat="server" ID="lbTotal"></asp:Label>
            <%= crs %>
        </td>
    </tr>
    <tr>
        <td height="5" colspan="2">
        </td>
    </tr>
    <tr runat="server" id="trFreight">
        <td align="right"  class="text4" width="580">
            Freight：
        </td>
        <td align="right" class="text3">
            <asp:Label runat="server" ID="lbFt"></asp:Label>
            <%= crs %>
        </td>
    </tr>
    <tr  runat="server" id="trTax">
        <td align="right" class="text4" width="580">
            <asp:Literal ID="LitTax" runat="server"></asp:Literal>% BTW：
        </td>
        <td align="right" class="text3">
            <asp:Label runat="server" ID="lbtax"></asp:Label>
            <%= crs %>
        </td>
    </tr>
    <tr>
        <td height="20" colspan="2">
            <img src="<%=Util.GetRuntimeSiteIP()%>/Images/AEUline.gif" width="100%" />
        </td>
    </tr>
    <tr>
        <td align="right" class="text4" width="580">
            Total：
        </td>
        <td align="right" class="text3">
            <asp:Label runat="server" ID="lbTotal2"></asp:Label>
            <%= crs %>
        </td>
    </tr>
</table>
