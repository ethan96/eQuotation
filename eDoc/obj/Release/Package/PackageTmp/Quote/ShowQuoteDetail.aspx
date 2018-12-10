<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/myMaster.master" CodeBehind="ShowQuoteDetail.aspx.vb" Inherits="EDOC.ShowQuoteDetail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <div>
        <div runat="server" id="divLogo">
        </div>
        <div runat="server" id="divMaster">
            <table width="720" border="0" cellspacing="0" cellpadding="0" align="center" style="font-family: Arial, Helvetica, sans-serif">
                <tr>
                    <td align="left">
                        <img src="http://buy.advantech.com/App_Themes/AUS/logo.gif" alt="Advantech eStore" />
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
                                        Advantech Corporation | eStore Division | 380 Fairview way, Milpitas, 95035 CA,
                                        US
                                        <br />
                                        1-888-576-9668 8 am- 8 pm (EST) Mon-Fri</div>
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
                                            <span lang="EN-US" style="font-size: 10.0pt; font-family: Arial,sans-serif; mso-fareast-font-family: 宋体;
                                                mso-ansi-language: EN-US; mso-fareast-language: ZH-CN; mso-bidi-language: AR-SA">
                                                Thank you for ordering with the Advantech eStore!
                                                <br />
                                                You may check your order status anytime by simply <a href="http://buy.advantech.com/Cart/orderdetail.aspx?orderid=OUS027212&amp;storeid=AUS">
                                                    clicking here</a>. (Please allow 24 hours for any updates to your status to
                                                take effect). If contacting Advantech regarding any questions about your order,
                                                we recommend that you keep this information handy for easy reference during your
                                                call.</span><br />
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
                                                <table width="100%">
                                                    <tr>
                                                        <td style="width: 33%">
                                                            <table width="100%" class="contact">
                                                                <th style="color: #333333;">
                                                                    Sold to
                                                                </th>
                                                                <tr>
                                                                    <td>
                                                                        Attention:
                                                                        <asp:Label runat="server" ID="attentionEmail"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        Company:
                                                                        <asp:Label runat="server" ID="quoteToErpId"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        Address:
                                                                        <asp:Label runat="server" ID="lbAddress1"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        Tel:
                                                                        <asp:Label runat="server" ID="lbTel1"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        Mobile:
                                                                        <asp:Label runat="server" ID="lbMobile1"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td style="width: 33%">
                                                            <table width="100%" class="contact">
                                                                <th style="color: #333333">
                                                                    Ship to
                                                                </th>
                                                                <tr>
                                                                    <td>
                                                                        Attention:
                                                                        <asp:Label runat="server" ID="lbAttention3"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        Company:
                                                                        <asp:Label runat="server" ID="lbCompany1"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        Address:
                                                                        <asp:Label runat="server" ID="lbAddress2"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        Tel:
                                                                        <asp:Label runat="server" ID="lbTel2"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        Mobile:
                                                                        <asp:Label runat="server" ID="lbMobile2"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td style="width: 33%" runat="server" id="tdBill">
                                                            <table width="100%" class="contact">
                                                                <th style="color: #333333">
                                                                    Bill to
                                                                </th>
                                                                <tr>
                                                                    <td>
                                                                        Attention:
                                                                        <asp:Label runat="server" ID="lbAttention4"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        Company:
                                                                        <asp:Label runat="server" ID="lbCompany2"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        Address:
                                                                        <asp:Label runat="server" ID="lbAddress3"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        Tel:
                                                                        <asp:Label runat="server" ID="lbTel3"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        Mobile:
                                                                        <asp:Label runat="server" ID="lbMobile3"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
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
                                                    EmptyDataText="no Item." AutoGenerateColumns="false" Font-Size="Smaller">
                                                    <Columns>
                                                        <asp:BoundField DataField="line_no" HeaderText="No." ItemStyle-HorizontalAlign="center" />
                                                        <asp:TemplateField HeaderText="Category">
                                                            <ItemTemplate>
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
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="rohs" HeaderText="Rohs" ItemStyle-HorizontalAlign="center" />
                                                        <asp:BoundField DataField="classABC" HeaderText="Class" ItemStyle-HorizontalAlign="center" />
                                                        <asp:TemplateField ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="Center">
                                                            <HeaderTemplate>
                                                                List Price
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="Center">
                                                            <HeaderTemplate>
                                                                Quotation Price
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="" HeaderText="Disc." ItemStyle-HorizontalAlign="right" />
                                                        <asp:TemplateField ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="Center">
                                                            <HeaderTemplate>
                                                                Qty.
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="Center">
                                                            <HeaderTemplate>
                                                                Req. Date
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="Center">
                                                            <HeaderTemplate>
                                                                Due Date
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="Center">
                                                            <HeaderTemplate>
                                                                Sub Total
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="ITP(FOB AESC)">
                                                            <ItemTemplate>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="Center">
                                                            <HeaderTemplate>
                                                                Margin
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center">
                                                            <HeaderTemplate>
                                                                SPR No
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
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
                                                    <tr id="CartDiscount" class="cartitem">
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
                                        <p>
                                            <span lang="EN-US" style="font-size: 9.0pt; color: red">*</span><span lang="EN-US"
                                                style="font-size: 9.0pt; color: #333333"> Indicates an Estimated Value<o:p></o:p></span></p>
                                        <p>
                                            <span lang="EN-US" style="font-size: 9.0pt; color: #333333">All prices are in US dollars,
                                                F.O.B. California, U.S.A.<o:p></o:p></span></p>
                                        <p>
                                            <span lang="EN-US" style="font-size: 9.0pt; color: #333333">Pricing, specifications,
                                                availability and terms of offers may change without notice, and are not transferable.
                                                Taxes, fees, shipping, handling and any applicable restocking charges extra vary
                                                and are not subject to discount.<o:p></o:p></span></p>
                                        <p>
                                            <span lang="EN-US" style="font-size: 9.0pt; color: #333333">Any orders that ship to
                                                the following jurisdictions are charged sales tax: AZ, CA, CO, CT, FL, GA, IL, IN,
                                                KY, MD, MA, NC, NJ, OH, TN, TX, WA, and WI. If you are exempt from sales tax, select
                                                the Resale box during the checkout process. Upon receiving the proper paperwork,
                                                Advantech will not include such taxes in the final invoices.<o:p></o:p></span></p>
                                        <p>
                                            <span lang="EN-US" style="font-size: 9.0pt; color: #333333">The estimated ship days
                                                represents the approximate time it takes to process your order and assemble your
                                                system based on approved credit card or net term purchase. The estimated shipment
                                                date is not intended to provide you with an actual ship date. Your estimated ship
                                                date may vary based upon the payment method you choose among other factors pertaining
                                                to the nature of the order.<o:p></o:p></span></p>
                                        <span lang="EN-US" style="font-size: 9.0pt; font-family: Times New Roman,serif; mso-fareast-font-family: 宋体;
                                            color: #333333; mso-ansi-language: EN-US; mso-fareast-language: ZH-CN; mso-bidi-language: AR-SA">
                                            The export of any products or software purchased from Advantech must be made in
                                            accordance with all relevant laws of the United States, including and without limitation,
                                            the US Export Administration Regulations. This may require that you obtain a formal
                                            export license or make certain declarations to the United States Government regarding
                                            products to be exported, their destination or their end-use.</span></div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <p>
                                        <span lang="EN-US" style="font-size: 10.0pt">Again, thank you for shopping at the <a
                                            href="http://buy.advantech.com/">Advantech eStore</a>. We will process your order
                                            as soon as possible. Call 1-888-576-9668 8 am- 8 pm (EST) Mon-Fri if you have any
                                            question about this order.<o:p></o:p></span></p>
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
                        <a href="http://eq.advantech.com">buy.advantech.com</a> / 1-888-576-9668 8 am- 8
                        pm (EST) Mon-Fri
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
</asp:Content>
