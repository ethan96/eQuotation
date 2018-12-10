<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="piAIN.aspx.vb" Inherits="EDOC.piAIN" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        table.contact
        {
            font: bold 13px/normal Arial,Helvetica,sans-serif;
            border-collapse: collapse;
            border-color: #ffffff;
            background: #FFFFFF;
            color: #333;
            width: 100%;
        }
        .contact th
        {
            width: 33%;
            border-collapse: collapse;
            background: #EFF4FB;
        }
        .contact td
        {
            font: 12px/normal Arial,Helvetica,sans-serif;
            border-collapse: collapse;
            background: #FFFFFF;
            color: #333;
        }
        
        table.estoretable
        {
            font: 13px/normal Arial,Helvetica,sans-serif;
            border-collapse: collapse;
            color: #333;
            width: 100%;
        }
        
        .estoretable th
        {
            padding: 3px;
            border-style: solid;
            border-width: 1px;
            border-color: #E5E5E5;
            text-align: center;
            background: #EFF4FB;
        }
        
        .estoretable td
        {
            padding: 3px;
            border-style: solid;
            border-width: 1px;
            border-color: #E5E5E5;
        }
        
        table.estoretable2
        {
            font: 13px/normal Arial,Helvetica,sans-serif;
            border-collapse: collapse;
            color: #333;
            width: 100%;
        }
        .estoretable2 td
        {
            padding: 3px;
            text-align: right;
        }
        
        .boder
        {
            border: 1px solid #E5E5E5;
        }
        
        .estoretable caption
        {
            padding: 0 0 .5em 0;
            text-align: left;
            text-transform: uppercase;
            color: #333;
            background: transparent;
        }
        .cartitem p
        {
            margin: 0;
            padding: 5px 0;
        }
        .cartitem label
        {
            font-weight: bold;
        }
        .style2
        {
            width: 345px;
        }
        .style4
        {
            width: 201px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    .
    <div>
        <div runat="server" id="divLogo">
        </div>
        <div runat="server" id="divMaster">
            <table width="720" border="0" cellspacing="0" cellpadding="0" align="center" style="font-family: Arial, Helvetica, sans-serif">
                <tr>
                    <td align="left">
                        <img src="/images/logoUS.gif" alt="Advantech eStore" />
                        <br />
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="720" border="0" cellpadding="0" cellspacing="0" height="30">
                            <tr>
                                <td bgcolor="#E5E5E5" style="background-color: #E5E5E5" height="30">
                                    <a title="Home" href="http://buy.advantech.com/Default.htm" style="text-decoration: none">
                                        <font size="2" color="#767677"><b>Home</b></font></a> &nbsp; &nbsp; &nbsp; <a title="About Us"
                                            href="http://buy.advantech.com/AboutUs/Default.htm" style="text-decoration: none">
                                            <font size="2" color="#767677"><font size="2" color="#767677"><b>About Us</b></font></a>
                                    &nbsp; &nbsp; &nbsp; <a title="Support" href="http://support.advantech.com.tw/" style="text-decoration: none">
                                        <font size="2" color="#767677"><font size="2" color="#767677"><b>Support</b></font></a>
                                    &nbsp; &nbsp; &nbsp; <a title="Contact Us" href="http://buy.advantech.com/ContactUs/Default.htm"
                                        style="text-decoration: none"><font size="2" color="#767677"><font size="2" color="#767677">
                                            <b>Contact Us</b></font></a>
                                </td>
                            </tr>
                            <tr>
                                <td height="18" bgcolor="#708AAC" style="background-color: #708AAC; padding: 5px 10px">
                                    <div style="color: #FFFFFF; font-size: 12px">
                                        Advantech Corporation | eStore Division | Melbourne 1/3 Southpark Close, Keysborough
                                        VIC 3173
                                        <br />
                                        Tel: 1300-308-531 | Fax: 61-3-9797-0199 | <a href="mailto:buy@advantech.net.au">buy@advantech.net.au</a>&nbsp;&nbsp;&nbsp;</div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="100%" border="0" cellspacing="0" cellpadding="6" style="border: 1px solid #CCCCCC;
                            background-color: #FFFFE6" bgcolor="#FFFFE6" align="center">
                            <tr>
                                <td style="font-size: 13px;">
                                    <div>
                                        <b>Dear Customer</b></div>
                                    <div style="padding-left: 10px">
                                        <p>
                                            Thank you for allowing the Advantech eStore to provide you with a quotation.
                                        </p>
                                        <p>
                                            The shipping and handling charges will be determined by shipment characteristics,
                                            weight, and applicable service charges of the actual shipment. You are welcome to
                                            contact our account managers at +886-2-2218-4567 Monday through Friday. When contacting
                                            us please make sure to have the Quotation Number ready.
                                            <br />
                                            You can also <a href="http://eq.advantech.com/quote/MyQuotationRecord.aspx">check this
                                                quote online</a>.
                                            <br />
                                            <br />
                                            <b>Comments: </b>
                                            <br />
                                            <asp:Label runat="server" ID="quoteNote"></asp:Label>
                                        </p>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td bgcolor="#FFFFFF">
                                    <table width="100%" border="0" cellspacing="2" cellpadding="0">
                                        <tr>
                                            <td align="left" width="40%">
                                                <span style="font-size: 24px; color: #003D7C"><b>Quotation</b></span>
                                            </td>
                                            <td align="left" width="60%">
                                                <span style="font-size: 14px; color: #003D7C">Quotation Number: <b>
                                                    <asp:Label runat="server" ID="QuoteID"></asp:Label></b></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="font-size: 14px; padding-left: 10px">
                                              <%--  <div>
                                                    [/PromotionCode]</div>--%>
                                                <div>
                                                  <%--  Request Date:
                                                    <asp:Label runat="server" ID="quoteDate"></asp:Label>
                                                    <span style="color: Red;">Valid Through:
                                                        <asp:Label runat="server" ID="expiredDate"></asp:Label></span>--%>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" background="http://buy.advantech.com/App_Themes/AUS/line.gif" height="1">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <!-- Contact Info -->
                                                <table width="100%" class="contact">
                                                    <tr>
                                                        <th style="color:#333333">
                                                            Sold to
                                                        </th>
                                                        <th style="color:#333333">
                                                            Bill to
                                                        </th>
                                                        <th style="color:#333333">
                                                            Ship to
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Attention:
                                                            <asp:Label runat="server" ID="attentionEmail"></asp:Label>
                                                        </td>
                                                        <td>
                                                            Attention:
                                                            <asp:Label runat="server" ID="lbAttention3"></asp:Label>
                                                        </td>
                                                        <td>
                                                            Attention:
                                                            <asp:Label runat="server" ID="lbAttention4"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Company:
                                                            <asp:Label runat="server" ID="quoteToErpId"></asp:Label>
                                                        </td>
                                                        <td>
                                                            Company:
                                                            <asp:Label runat="server" ID="lbCompany1"></asp:Label>
                                                        </td>
                                                        <td>
                                                            Company:
                                                            <asp:Label runat="server" ID="lbCompany2"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Address:
                                                            <asp:Label runat="server" ID="lbAddress1"></asp:Label>
                                                        </td>
                                                        <td>
                                                            Address:
                                                            <asp:Label runat="server" ID="lbAddress2"></asp:Label>
                                                        </td>
                                                        <td>
                                                            Address:
                                                            <asp:Label runat="server" ID="lbAddress3"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Tel:
                                                            <asp:Label runat="server" ID="lbTel1"></asp:Label>
                                                        </td>
                                                        <td>
                                                            Tel:
                                                            <asp:Label runat="server" ID="lbTel2"></asp:Label>
                                                        </td>
                                                        <td>
                                                            Tel:
                                                            <asp:Label runat="server" ID="lbTel3"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Mobile:
                                                            <asp:Label runat="server" ID="lbMobile1"></asp:Label>
                                                        </td>
                                                        <td>
                                                            Mobile:
                                                            <asp:Label runat="server" ID="lbMobile2"></asp:Label>
                                                        </td>
                                                        <td>
                                                            Mobile:
                                                            <asp:Label runat="server" ID="lbMobile3"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                </table>
                                                <!-- Contact Info/ -->
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" height="5">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <span style="font-size: 14px; color: #003D7C"><b>Quotation Detail:</b></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:GridView DataKeyNames="line_no" ID="gv1" runat="server" AllowPaging="false"
                                                    EmptyDataText="no Item." AutoGenerateColumns="false" OnRowDataBound="gv1_RowDataBound" Font-Size="Smaller" >
                                                    <Columns>
                                                        <asp:BoundField DataField="line_no" HeaderText="No." ItemStyle-HorizontalAlign="center" />
                                                        <asp:TemplateField HeaderText="Category">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lbCategory" Text='<%#Bind("category") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="partNo" HeaderText="Part No" ItemStyle-HorizontalAlign="left" />
                                                        <asp:TemplateField HeaderText="Description">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lbdescription" Text='<%#Bind("description") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Extended Warranty">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lbew" Text='<%#Bind("ewflag") %>'></asp:Label>
                                                                months
                                                                <asp:Label runat="server" Text='<%#currencySign %>' ID="lbEWSign"></asp:Label>
                                                                <asp:Label runat="server" ID="gv_lbEW"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="rohs" HeaderText="Rohs" ItemStyle-HorizontalAlign="center" />
                                                        <asp:BoundField DataField="classABC" HeaderText="Class" ItemStyle-HorizontalAlign="center" />
                                                        <asp:TemplateField ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="Center">
                                                            <HeaderTemplate>
                                                                List Price
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%#currencySign %>' ID="lbListPriceSign"></asp:Label>
                                                                <asp:Label runat="server" Text='<%#FormatNumber(Eval("listprice"),2) %>' ID="lbListPrice"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="Center">
                                                            <HeaderTemplate>
                                                                Quotation Price
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%#currencySign %>' ID="lbUnitPriceSign"></asp:Label>
                                                                <asp:Label runat="server" Text='<%#FormatNumber(Eval("newunitprice"),2) %>' ID="lbUnitPrice"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="" HeaderText="Disc." ItemStyle-HorizontalAlign="right" />
                                                        <asp:TemplateField ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="Center">
                                                            <HeaderTemplate>
                                                                Qty.
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%#Bind("qty") %>' ID="lbGVQty"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="Center">
                                                            <HeaderTemplate>
                                                                Req. Date
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%#CDate(Eval("reqdate")).ToShortDateString()%>'
                                                                    ID="lbreqdate"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="Center">
                                                            <HeaderTemplate>
                                                                Due Date
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%#CDate(Eval("duedate")).ToShortDateString()%>'
                                                                    ID="lbDueDate"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="Center">
                                                            <HeaderTemplate>
                                                                Sub Total
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%#currencySign %>' ID="lbSubTotalSign"></asp:Label>
                                                                <asp:Label runat="server" Text="" ID="lbSubTotal"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="ITP(FOB AESC)">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%#currencySign %>' ID="lbItpSign"></asp:Label>
                                                                <asp:Label runat="server" Text='<%#Eval("newITP") %>' ID="lbItp"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="Center">
                                                            <HeaderTemplate>
                                                                Margin
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lbMargin"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center">
                                                            <HeaderTemplate>
                                                                SPR No
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lbSPRNO" Text='<%#Eval("sprno") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                                <br />
                                                <table width="100%" class="estoretable2">
                                                    <tr class="cartitem">
                                                        <td align="left" rowspan="5" valign="top" width="45%" class="boder">
                                                            <asp:Label runat="server" ID="shipTerm"></asp:Label>
                                                        <%--    [/Courier_account] [/PROMOTIONCODE] [/RESELLERID]--%>
                                                        </td>
                                                        <td>
                                                            Sub Total:
                                                        </td>
                                                        <td>
                                                            <asp:Label runat="server" ID="lbSubTotal"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="cartitem">
                                                        <td>
                                                            <font color="red">* </font>Freight:
                                                        </td>
                                                        <td>
                                                            <asp:Label runat="server" ID="freight"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="cartitem">
                                                        <td>
                                                            <font color="red">* </font>Tax(<asp:Label runat="server" ID="lbTaxRate"></asp:Label>):
                                                        </td>
                                                        <td>
                                                            <asp:Label runat="server" ID="tax"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr id="CartDiscount" runat="server" class="cartitem">
                                                        <td>
                                                            <font color="red">* </font>Discount:
                                                        </td>
                                                        <td>
                                                            <asp:Label runat="server" ID="lbDiscount"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="cartitem">
                                                        <td>
                                                            Total:
                                                        </td>
                                                        <td style="font-weight: bold; color: #FF0000;">
                                                            <asp:Label runat="server" ID="lbTotal"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <!-- CART DETAIL / -->
                                            </td>
                                        </tr>
                                    </table>
                                    <div style="color: #333; padding-left: 10px">
                                        <p style="font-size: 12px;">
                                            <span style="color: red">*</span> This quotation is valid for Australia only.</p>
                                        <p style="font-size: 12px;">
                                            <span style="color: red">*</span> <span style="font-size: 12px;">Pricing, specifications,
                                                availability and terms of offers may change without notice, and are not transferable.</span>
                                            Taxes, fees, shipping, handling and any applicable restocking charges vary and are
                                            not subject to discount.
                                        </p>
                                        <p style="font-size: 12px;">
                                            <span style="color: red">*</span> Any orders that shipped within Australia are charged
                                            GST (Goods & Services Tax).<span style="font-size: 12px;"> If you are exempt from sales
                                                tax, select the Resale box during the checkout process. Upon receiving the proper
                                                paperwork, Advantech will not include such taxes in the final invoices.</span></p>
                                        <p style="font-size: 12px;">
                                            <span style="color: red">*</span> The estimated ship days represents the approximate
                                            time it takes to process your order and assembling your system based on approved
                                            credit card or net term purchase. The estimated shipment date is not intended to
                                            provide you with an actual ship date. <span style="font-size: 12px;">Your estimated
                                                ship date may vary based upon the payment method you choose among other factors
                                                pertaining to the nature of the order.</span></p>
                                        <p style="font-size: 12px;">
                                            <span style="color: red">*</span> The export of any products or software purchased
                                            from Advantech Australia must be made in accordance with all relevant laws of Australia,
                                            including and without limitation, the Australian Export Regulations. This may require
                                            that you obtain a formal export license or make certain declarations to the Australian
                                            Government regarding products to be exported, their destination or their end-use.
                                        </p>
                                        <p style="font-size: 12px;">
                                            <span style="color: red">*</span> <strong>Quotations are valid for 10 days.</strong>.</p>
                                        <p style="font-size: 13px;">
                                            <span style="color: red">*</span><a href="http://buy.advantech.net.au/Resources/AAU/Advantech_Australia_eStore_Terms_Conditions.pdf">Terms
                                                & Condition</a></p>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <p style="font-size: 13px; padding-left: 10px">
                                        Again,<span style="font-size: 13px; padding-left: 10px"> thank you for your visiting
                                            the <a href="http://buy.advantech.com/">Advantech eStore</a>.</span> A sales
                                        representative will contact you regarding your quotation shortly. If you have any
                                        questions regarding your configuration, or would like to speak to a representative
                                        immediately, you may call 1300 308 531 from 8.30 a.m. to 5 p.m. Monday through Friday,
                                        or e-mail us at <a href="mailto:buy@advantech.net.au">buy@advantech.net.au.</a></p>
                                    <p style="font-size: 13px;">
                                        Best regards,
                                        <br />
                                        <a href="http://[/STOREURL]" style="text-decoration: none; font-weight: bold; color: #000000">
                                            Advantech eStore</a>
                                    </p>
                                </td>
                            </tr>
                        </table>
                        <div>
                        </div>
                    </td>
                </tr>
                <!-- Footer -->
                <tr>
                    <td height="15" bgcolor="#708AAC" style="background-color: #708AAC; text-align: center;
                        color: #FFFFFF; font-size: 12px" align="center">
                        <a href="http://eq.advantech.com">Advantech eQuotation</a> / +886-2-2218-4567
                    </td>
                </tr>
                <tr>
                    <td height="30" style="text-align: center; border: 1px solid #CCCCCC; background-color: #FFFFE6"
                        bgcolor="#FFFFE6" align="center" valign="middle">
                        <a href="http://buy.advantech.com/resources/aus/terms_and_conditions_aus.pdf" style="text-decoration: none;
                            font-size: 11px"><font color="#555555">Terms and Conditions</font>&nbsp;&nbsp;<font
                                color="#555555">|</font>&nbsp;&nbsp;</a><a href="http://buy.advantech.com/Info/ReturnPolicy.htm"
                                    style="text-decoration: none; font-size: 11px"><font color="#555555">Return Policy</font></a>&nbsp;&nbsp;<font
                                        color="#555555"><span style="font-size: 11px;">|</span></font>&nbsp;&nbsp;<a href="http://buy.advantech.com/Info/PrivacyPolicy.htm"
                                            style="text-decoration: none; font-size: 11px"><font color="#555555">Privacy Policy</font></a>
                    </td>
                </tr>
                <!-- Footer /-->
            </table>
        </div>
        <div runat="server" id="divDetail">
        </div>
        <br />
        <br />
    </div>
    .
    </form>
</body>
</html>
