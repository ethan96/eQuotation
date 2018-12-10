<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PI.aspx.vb" Inherits="EDOC.PI" %>
<%@ Register Src="~/Ascx/Order/soldtoshipto.ascx" TagName="soldtoshipto"
    TagPrefix="uc1" %>
<%@ Register Src="~/Ascx/Order/OrderInfo.ascx" TagName="OrderInfo" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        .mytable table
        {
            border-collapse: collapse;
        }
        
        .mytable tr td
        {
            background: #ffffff;
            border: #eeeeee 1px solid;
            padding: 2px;
            font-family: Arial;
            font-size:12px;
        }
          .mytable1 tr td
        {
            background: #ffffff;
            border: #eeeeee 0px solid;
            padding: 2px;
            font-family: Arial;
            font-size:12px;
        }
          .PIHT
        {
            color:#FFFFFF;
            font-size:14px;
            font-weight:bold;
            white-space:nowrap;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    .
    <div id="divHeader">
        <table>
            <tr>
                <td>
                    <img src="/images/header_advantech_logo.gif" alt="Advantech"/>
                </td>
            </tr>
        </table>
    </div>
    <div id="divCustInfo" class="mytable1">
        <br />
        <uc1:soldtoshipto runat="server" ID="soldtoshiptoUC" Visible="true" />
<%--        <table width="100%">
            <tr>
                <td style="background-color: #ededed; font-weight: bold">
                    Customer Information
                </td>
            </tr>
            <tr>
                <td style="text-align: center">
                    Customer Information
                </td>
            </tr>
            <tr>
                <td>
                    <table width="100%">
                        <tr>
                            <td>
                                Customer
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lbSoldName"></asp:Label>
                            </td>
                            <td>
                                Attention
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lbSoldAtt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td rowspan="2">
                                Address
                            </td>
                            <td rowspan="2">
                                <asp:Label runat="server" ID="lbSoldAddr"></asp:Label>
                            </td>
                            <td>
                                Tel No.
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lbSoldTel"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Fax No.
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lbSoldFax"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="text-align: center">
                    Shipping Information
                </td>
            </tr>
            <tr>
                <td>
                    <table width="100%">
                        <tr>
                            <td>
                                Customer
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lbShipName"></asp:Label>
                            </td>
                            <td>
                                Attention
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lbShipAtt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td rowspan="2">
                                Address
                            </td>
                            <td rowspan="2">
                                <asp:Label runat="server" ID="lbShipAddr"></asp:Label>
                            </td>
                            <td>
                                Tel No.
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lbShipTel"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Fax No.
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lbShipFax"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                </td>
            </tr>
        </table>--%>
    </div>
    <div id="divOrderInfo" class="mytable1">
        <br />
        <uc2:OrderInfo runat="server" ID="Orderinfo1" Visible="true" />
<%--        <table width="100%">
            <tr>
                <td style="background-color: #ededed; font-weight: bold">
                    Order Information
                </td>
            </tr>
            <tr>
                <td>
                    <table width="100%">
                        <tr>
                            <td>
                                PO No.
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lbPO"></asp:Label>
                            </td>
                            <td>
                                Advantech SO
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lbSO"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Order Date
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lbOrderDate"></asp:Label>
                            </td>
                            <td>
                                Payment Term
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lbPayTerm"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Required Date
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lbReqdate"></asp:Label>
                            </td>
                            <td>
                                Incoterm
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lbIncoterm"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Placed By
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lbPlacedBy"></asp:Label>
                            </td>
                            <td>
                                Incoterm Text
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lbIncotermText"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Freight
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lbFreight"></asp:Label>
                            </td>
                            <td>
                                Channel
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lbChannel"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Partial OK
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lbisPartial"></asp:Label>
                            </td>
                            <td>
                                Ship Condition
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lbShipCond"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Order Note (External Note)
                            </td>
                            <td colspan="3">
                                <asp:Label runat="server" ID="lbOrderNote"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Sales Note From Customer
                            </td>
                            <td colspan="3">
                                <asp:Label runat="server" ID="lbSalesNote"></asp:Label>
                            </td>
                        </tr>
                        <tr id="trEUOPN" runat="server">
                            <td>
                                EU OP Note
                            </td>
                            <td colspan="3">
                                <asp:Label runat="server" ID="lbOPNote"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Project Note
                            </td>
                            <td colspan="3">
                                <asp:Label runat="server" ID="lbPJNote"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>--%>
    </div>
    <div id="divDetailInfo" class="mytable">
        <br />
          <fieldset>
	 <legend><span style="font-weight:bold; font-size:12px; color:#666666 ">Purchased Products</span></legend>
        <table width="100%">
       
            <tr>
                <td>
                    <asp:GridView runat="server" ID="gv1" AutoGenerateColumns="false" AllowPaging="false"
                        AllowSorting="true" Width="100%" EmptyDataText="No Order Line." DataKeyNames="lineNo" OnDataBound="gv1_DataBound" OnRowDataBound="gv1_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <HeaderTemplate>
                                  <span class="PIHT">Seq.</span> 
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <HeaderTemplate>
                                   <span class="PIHT">Line No.</span> 
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# CType(Container.DataItem, IBUS.iCartLine).lineNo.Value%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="left">
                                <HeaderTemplate>
                                    <span class="PIHT">Product</span>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# CType(Container.DataItem, IBUS.iCartLine).partNo.Value%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="left">
                                <HeaderTemplate>
                                    <span class="PIHT">Customer P/N</span>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# Server.HtmlDecode(CType(Container.DataItem, IBUS.iCartLine).CustMaterial.Value)%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="left">
                                <HeaderTemplate>
                                    <span class="PIHT">Description</span> 
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# getDescForPN(CType(Container.DataItem, IBUS.iCartLine).partNo.Value, CType(Container.DataItem, IBUS.iCartLine).partDesc.Value)%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="left">
                                <HeaderTemplate>
                                    <span class="PIHT"><asp:Label runat="server" ID="lbHDueDate">Due Date</asp:Label></span>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# IIf(CDate(CType(Container.DataItem, IBUS.iCartLine).dueDate.Value).ToString(Pivot.CurrentProfile.DatePresentationFormat) = CDate("1900/01/01").ToString(Pivot.CurrentProfile.DatePresentationFormat), "TBD", IIf(True, CDate(CType(Container.DataItem, IBUS.iCartLine).dueDate.Value).ToString(Pivot.CurrentProfile.DatePresentationFormat), CDate(CType(Container.DataItem, IBUS.iCartLine).dueDate.Value).ToString(Pivot.CurrentProfile.DatePresentationFormat)))%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="left">
                                <HeaderTemplate>
                                    <span class="PIHT"><asp:Label runat="server" ID="lbHReqDate"> Required Date </asp:Label></span>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# IIf(True, CDate(CType(Container.DataItem, IBUS.iCartLine).reqDate.Value).ToString(Pivot.CurrentProfile.DatePresentationFormat), CDate(CType(Container.DataItem, IBUS.iCartLine).reqDate.Value).ToString(Pivot.CurrentProfile.DatePresentationFormat))%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <HeaderTemplate>
                                    <span class="PIHT">Sales Leads from Advantech (DMF)</span>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# CType(Container.DataItem, IBUS.iCartLine).DMFFlag.Value%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <HeaderTemplate>
                                    <span class="PIHT">Extended Warranty Months</span>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lbew" Text='<%# CType(Container.DataItem, IBUS.iCartLine).EWFlag.Value%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <HeaderTemplate>
                                    <span class="PIHT">Qty.</span>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# CType(Container.DataItem, IBUS.iCartLine).Qty.Value%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <HeaderTemplate>
                                    <span class="PIHT">Price</span>
                                </HeaderTemplate>
                                <ItemTemplate>
                                     <asp:Label runat="server" Text='<%# crs %>' ID="lbUnitPriceSign"></asp:Label> <%# FormatNumber(CType(Container.DataItem, IBUS.iCartLine).newunitPrice.Value, 2)%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <HeaderTemplate>
                                    <span class="PIHT">Sub Total</span>
                                </HeaderTemplate>
                                <ItemTemplate>
                                     <asp:Label runat="server" Text='<%# crs %>' ID="lbSubTotalSign"></asp:Label> <%# FormatNumber(CType(Container.DataItem, IBUS.iCartLine).newunitPrice.Value * CType(Container.DataItem, IBUS.iCartLine).Qty.Value, 2)%>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr><td align="right" runat="server" id="trFreight" visible="false"> Freight：<%= crs %><asp:Label runat="server" ID="lbFt"></asp:Label></td></tr>
            <tr><td align="right" runat="server" id="trTax" visible="false"> Tax Rate：<asp:Label runat="server" ID="lbtax"></asp:Label></td></tr>
            <tr><td align="right"> Total：<%= crs %><asp:Label runat="server" ID="lbTotal"></asp:Label></td></tr>
        </table>
        </fieldset>
    </div>
    <div id="divFooter">
        <br />
    </div>
    .
    </form>
</body>
</html>