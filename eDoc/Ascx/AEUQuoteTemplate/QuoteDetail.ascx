<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="QuoteDetail.ascx.vb"
    Inherits="EDOC.QuoteDetail1" %>
<style>
    body
    {
        font-family: Calibri;
    }
    .QDtext1
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
    .QDtext2
    {
        color: #000000;
        font-family: Calibri;
        font-weight: bold;
        vertical-align: top;
        white-space: nowrap;
    }
    .QDtext3
    {
        color: #000000;
        font-family: Calibri;
        font-weight: bold;
        vertical-align: top;
    }
    .QDtext4
    {
        color: #004B9C;
        font-family: Calibri;
        font-weight: bold;
        vertical-align: top;
        white-space: nowrap;
    }
    .QDbr1
    {
        background-image: url(<%=Util.GetRuntimeSiteIP()%>/Images/AEUline3.gif);
        background-repeat: repeat-x;
        background-position: left bottom;
    }
    .QDtext8
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
<table width="720" align="center">
    <tr>
        <td colspan="2">
            <span class="QDtext8">Pricing </span>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:GridView runat="server" ID="gv1" AutoGenerateColumns="false" AllowPaging="false"
                HeaderStyle-CssClass="QDtext1 QDbr1" AllowSorting="true" Width="100%" EmptyDataText="No Order Line."
                DataKeyNames="lineNo" OnDataBound="gv1_DataBound" OnRowDataBound="gv1_RowDataBound"
                GridLines="None" CellPadding="4" >
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
                        Visible="true">
                        <HeaderTemplate>
                            <span class="PIHT">Line No.</span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# CType(Container.DataItem, IBUS.iCartLine).lineNo.Value%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="left">
                        <HeaderTemplate>
                            <span class="PIHT">Part Number:</span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# CType(Container.DataItem, IBUS.iCartLine).partNo.Value%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="left" ItemStyle-CssClass="QDtext4">
                        <HeaderTemplate>
                            <span class="PIHT">Virtual Part No</span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# CType(Container.DataItem, IBUS.iCartLine).VirtualPartNo.Value%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="left">
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
                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                        <HeaderTemplate>
                            <span class="PIHT">Quantity:</span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# CType(Container.DataItem, IBUS.iCartLine).Qty.Value%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" ItemStyle-Wrap="False">
                        <HeaderTemplate>
                            <span class="PIHT">List Price:</span><!--column index 10-->
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# FormatNumber(CType(Container.DataItem, IBUS.iCartLine).listPrice.Value, 2)%>&nbsp;<asp:Label runat="server" Text='<%# crs %>' ID="lbUnitPriceSign" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" ItemStyle-Wrap="False">
                        <HeaderTemplate>
                            <span class="PIHT">Unit Price:</span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# FormatNumber(CType(Container.DataItem, IBUS.iCartLine).newunitPrice.Value, 2)%>&nbsp;<asp:Label runat="server" Text='<%# crs %>' ID="lbUnitPriceSign" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" ItemStyle-Wrap="False">
                        <HeaderTemplate>
                            <span class="PIHT">Discount:</span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%--<%# FormatNumber(((CType(Container.DataItem, IBUS.iCartLine).listPrice.Value - CType(Container.DataItem, IBUS.iCartLine).unitPrice.Value) / CType(Container.DataItem, IBUS.iCartLine).listPrice.Value) * 100, 2)%>%--%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" ItemStyle-Wrap="False">
                        <HeaderTemplate>
                            <span class="PIHT">Sub Total:</span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# FormatNumber(CType(Container.DataItem, IBUS.iCartLine).newunitPrice.Value * CType(Container.DataItem, IBUS.iCartLine).Qty.Value, 2)%>&nbsp;<asp:Label runat="server" Text='<%# crs %>' ID="lbSubTotalSign" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <%--<RowStyle CssClass="QDbr1" />--%>
            </asp:GridView>
        </td>
    </tr>
    <tr runat="server" id="trsubtotal">
        <td align="right" width="580" class="QDtext4">
            Sub-Total：
        </td>
        <td align="right" class="QDtext3">
            <!--price and currency sign should indeed be shown on one line-->
            <asp:Label runat="server" ID="lbTotal" />&nbsp;<%= crs %>
        </td>
    </tr>
    <tr runat="server" id="trFreight">
        <td align="right"  class="QDtext4">
            Freight：
        </td>
        <td align="right" class="QDtext3">
            <!--price and currency sign should indeed be shown on one line-->
            <asp:Label runat="server" ID="lbFt" />&nbsp;<%= crs %>
        </td>
    </tr>
    <tr runat="server" id="trTax">
        <td align="right" runat="server"  class="QDtext4">
            <asp:Literal ID="LitTax" runat="server"></asp:Literal>% BTW：
        </td>
        <td align="right" class="QDtext3">
            <!--price and currency sign should indeed be shown on one line-->
            <asp:Label runat="server" ID="lbtax"/>&nbsp;<%= crs %>
        </td>
    </tr>
    <tr runat="server" id="trbr">
        <td height="2" colspan="2" class="QDbr1">
        </td>
    </tr>
    <tr runat="server" id="trtotal">
        <td align="right" class="QDtext4" width="580">
            Total：
        </td>
        <td align="right" class="QDtext3">
            <!--price and currency sign should indeed be shown on one line-->
            <asp:Label runat="server" ID="lbTotal2" />&nbsp;<%= crs %>
        </td>
    </tr>
    <tr runat="server" visible="false" id="trFreightDesc">
        <td colspan="2" style="text-align: left; color: red; font-size: 12px;">
            Our shipping cost is an estimation, this cost submit to modification by TNT contract and actual shipment packaging.  <br />
The real shipment cost will show on our official invoice day of shipment.
        </td>
    </tr>
</table>
