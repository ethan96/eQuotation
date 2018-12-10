<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="InterconQuoteTemplate.ascx.vb" Inherits="EDOC.InterconQuoteTemplate" %>
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
</style>
<table width="720" border="0" cellspacing="0" cellpadding="0" align="center" style="font-family: Arial, Helvetica, sans-serif">
    <tr>
        <td align="left">
            <img src='<%=Util.GetRuntimeSiteIP() %>/Images/Advantech logo.jpg' alt="Advantech eStore" />
        </td>
    </tr>
    <tr>
        <td>
            <table width="720" border="0" cellpadding="0" cellspacing="0" height="30">
                <tr>
                    <td bgcolor="#E5E5E5" style="background-color: #E5E5E5" height="30">
                        <a title="Home" href="http://www.advantech.com/industrial-automation" style="text-decoration: none">
                            <font size="2" color="#767677"><b>Home</b></font></a>&nbsp; &nbsp; &nbsp; <a title="About Us"
                                href="http://www.advantech.com/about/Default.aspx?flag=MissionAndFocus" style="text-decoration: none">
                                <font size="2" color="#767677"><b>About Us</b></font></a>
                        &nbsp; &nbsp; &nbsp; <a title="Support" href="http://support.advantech.com/support/new_default.aspx" style="text-decoration: none">
                            <font size="2" color="#767677"><b>Support</b></font></a>
                        &nbsp; &nbsp; &nbsp; <a title="Contact Us" href="http://www.advantech.com/contact/default.aspx?page=contact_detail&ID=66"
                            style="text-decoration: none"><font size="2" color="#767677"><b>Contact Us</b></font></a>
                    </td>
                </tr>
                <tr>
                    <td height="18" bgcolor="#708AAC" style="background-color: #708AAC; padding: 5px 10px">
                        <div style="color: #FFFFFF; font-size: 12px" id="officeInformation_USA" runat="server">
                            Advantech Corporation | <asp:Literal runat="server" ID="litOfficeAddr" />
                            <br />
                            <asp:Literal runat="server" ID="litOfficeTelTime1" />
                        </div>
                        <div  style="color: #FFFFFF; font-size: 12px" id="officeInformation_Mexico" runat="server" visible="false">
                            <b>Advantech Electronics</b>
                             <br />
                            Ave. Baja California # 245, Int. 704 
                              <br />
                            Col. Hipódromo Condesa Del. Cuauhtémoc 06100, México D.F., México
                            <br />
                            Toll Free: 1-800-467-2415&nbsp;&nbsp;&nbsp;&nbsp;Phone: +52-55-6275-2777
                        </div>
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
                                <span lang="EN-US" style="font-size: 10.0pt; font-family: Arial,sans-serif">
                                    Thank you for your interest in Advantech’s products and services. Below please find the quotation you requested. Please contact your Advantech Sales Team at <asp:Label ID="LbSalesEmailatTop" runat="server" Text="" /> should you have any questions regarding this quotation.                                    
                                </span>
                                <br />
                            </p>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="font-size: 20px;" align="center">
                        <b><asp:Label ID="lbtitle" runat="server" Text="Quotation"/></b>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF">
                        <table width="100%" border="0" cellspacing="2" cellpadding="0">
                            <tr>
                                <td align="center" width="28%"><b>Quote No: <asp:Label runat="server" ID="LabelQuoteID" /></b></td>
                                <td align="center" width="12%"><b>Version: <asp:Label runat="server" ID="LabelQuoteRevisionNumber" /></b></td>
                                <td align="center" width="30%"><b>Quote Date: <asp:Label runat="server" ID="quoteDate" /></b></td>
                                <td align="center" width="30%"><b>Expiration Date: <asp:Label runat="server" ID="expiredDate" /></b></td>
                            </tr>
<%--                            <tr>
                                
                                <td align="center" width="33%"></td>
                                <td align="center" width="33%"></td>
                            </tr>
--%>                            
                            <tr>
                                <td colspan="4" background="http://buy.advantech.com/App_Themes/AUS/line.gif" height="1">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <!-- Contact Info -->
                                    <table width="100%">
                                        <tr valign="top">
                                            <td style="width: 33%">
                                                <asp:Table ID="Table_SoldTo" width="100%" class="contact" runat="server" border="1" cellspacing="0" cellpadding="3" style="border: 1px solid #CCCCCC">
                                                    <asp:TableHeaderRow><asp:TableHeaderCell ColumnSpan="2" style="color: #333333;">Sold to &nbsp;<asp:Label runat="server" ID="lblSoldtoERPID" /></asp:TableHeaderCell></asp:TableHeaderRow>
                                                    <asp:TableRow>
                                                        <asp:TableHeaderCell style="color: #333333;" HorizontalAlign="Right">Company:</asp:TableHeaderCell>
                                                        <asp:TableCell ID="Cell_SoldTo_Company" Width="100%"><%=_lbSoldtoCompany%></asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableHeaderCell style="color: #333333;" HorizontalAlign="Right">Address:</asp:TableHeaderCell>
                                                        <asp:TableCell ID="Cell_SoldTo_Address"><%=_lbSoldtoAddr%></asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableHeaderCell style="color: #333333;" HorizontalAlign="Right">Tel:</asp:TableHeaderCell>
                                                        <asp:TableCell ID="Cell_SoldTo_Tel"><%=_lbSoldtoTel%></asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableHeaderCell style="color: #333333;" HorizontalAlign="Right">Attention:</asp:TableHeaderCell>
                                                        <asp:TableCell ID="Cell_SoldTo_Attention"><%=_lbSoldtoAttention%></asp:TableCell>
                                                    </asp:TableRow>
                                                </asp:Table>
                                            </td>
                                            <td style="width: 33%">
                                                <asp:Table ID="Table_ShipTo" width="100%" class="contact" runat="server" border="1" cellspacing="0" cellpadding="3" style="border: 1px solid #CCCCCC">
                                                    <asp:TableHeaderRow><asp:TableHeaderCell ColumnSpan="2" style="color: #333333;">Ship to &nbsp;<asp:Label runat="server" ID="lblShipToERPID" /></asp:TableHeaderCell></asp:TableHeaderRow>
                                                    <asp:TableRow>
                                                        <asp:TableHeaderCell style="color: #333333;" HorizontalAlign="Right">Company:</asp:TableHeaderCell>
                                                        <asp:TableCell ID="Cell_ShipTo_Company" Width="100%"><%=_lbShiptoCompany%></asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableHeaderCell style="color: #333333;" HorizontalAlign="Right">Address:</asp:TableHeaderCell>
                                                        <asp:TableCell ID="Cell_ShipTo_Address"><%=_lbShiptoAddr%></asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableHeaderCell style="color: #333333;" HorizontalAlign="Right">Address2:</asp:TableHeaderCell>
                                                        <asp:TableCell ID="TableCell2"><%=_lbShiptoAddr2%></asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableHeaderCell style="color: #333333;" HorizontalAlign="Right">Tel:</asp:TableHeaderCell>
                                                        <asp:TableCell ID="Cell_ShipTo_Tel"><%=_lbShiptoTel%></asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableHeaderCell style="color: #333333;" HorizontalAlign="Right">Attention:</asp:TableHeaderCell>
                                                        <asp:TableCell ID="Cell_ShipTo_Attention"><%=_lbShiptoAttention%></asp:TableCell>
                                                    </asp:TableRow>
                                                </asp:Table>
                                            </td>
                                            <td style="width: 33%" runat="server" id="tdBill">
                                                <asp:Table ID="Table_BillTo" width="100%" class="contact" runat="server" border="1" cellspacing="0" cellpadding="3" style="border: 1px solid #CCCCCC">
                                                    <asp:TableHeaderRow><asp:TableHeaderCell ColumnSpan="2" style="color: #333333;">Bill to &nbsp;<asp:Label runat="server" ID="lblBillToERPID" /></asp:TableHeaderCell></asp:TableHeaderRow>
                                                    <asp:TableRow>
                                                        <asp:TableHeaderCell style="color: #333333;" HorizontalAlign="Right">Company:</asp:TableHeaderCell>
                                                        <asp:TableCell ID="Cell_BillTo_Company" Width="100%"><%=_lbBilltoCompany%></asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableHeaderCell style="color: #333333;" HorizontalAlign="Right">Address:</asp:TableHeaderCell>
                                                        <asp:TableCell ID="Cell_BillTo_Address"><%=_lbBilltoAddr%></asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableHeaderCell style="color: #333333;" HorizontalAlign="Right">Tel:</asp:TableHeaderCell>
                                                        <asp:TableCell ID="Cell_BillTo_Tel"><%=_lbBilltoTel%></asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableHeaderCell style="color: #333333;" HorizontalAlign="Right">Attention:</asp:TableHeaderCell>
                                                        <asp:TableCell ID="Cell_BillTo_Attention"><%=_lbBilltoAttention%></asp:TableCell>
                                                    </asp:TableRow>
                                                </asp:Table>
                                            </td>
                                        </tr>
                                    </table>
                                    <!-- Contact Info/ -->
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" height="5">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <span style="font-size: 14px; color: #003D7C"><b>Quotation Detail:</b></span>&nbsp;&nbsp;&nbsp;<asp:Label runat="server" ID="lblSalesPerson" Font-Bold="true" />&nbsp;&nbsp;&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:GridView DataKeyNames="line_no" ID="gv1" runat="server" AllowPaging="false"
                                        EmptyDataText="no Item." AutoGenerateColumns="false" 
                                        OnRowDataBound="gv1_RowDataBound" Font-Size="10pt" Width="100%">
                                        <AlternatingRowStyle BackColor="#EBEBEB" />
                                        <Columns>
                                            <asp:BoundField DataField="line_no" HeaderText="No." ItemStyle-HorizontalAlign="center">
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="partNo" HeaderText="Part No" ItemStyle-HorizontalAlign="left" ItemStyle-Wrap="false">
                                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="VirtualPartNo" HeaderText="Virtual Part No" ItemStyle-HorizontalAlign="left">
                                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Description">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lbdescription" Text='<%#Bind("description") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    Available Date
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%# Eval("duedate")%>'
                                                        ID="lbDueDate"></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="inventory" HeaderText="Available Qty." ItemStyle-HorizontalAlign="Right">
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    Order Qty.
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%#Bind("qty") %>' ID="lbGVQty"></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    List Price
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%#_currencySign %>' ID="lbListPriceSign"></asp:Label>
                                                    <asp:Label runat="server" Text='<%#FormatNumber(Eval("listprice"),2) %>' ID="lbListPrice"></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Right" Wrap="false"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    Unit Price
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%#_currencySign %>' ID="lbUnitPriceSign"></asp:Label>
                                                    <asp:Label runat="server" Text='<%#FormatNumber(Eval("newunitprice"),2) %>' ID="lbUnitPrice"></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Right" Wrap="false"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    Disc.
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='' ID="lbDisc"></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    Extended Price
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%#_currencySign %>' ID="lbSubTotalSign"></asp:Label>
                                                    <asp:Label runat="server" Text="" ID="lbSubTotal"></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Right" Wrap="false"></ItemStyle>
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle BackColor="#FF6600" ForeColor="White" />
                                    </asp:GridView>
                                    <br />
                                    <table width="100%">
                                        <tr class="cartitem">
                                            <td id="tbSM" style="text-align: left" rowspan="4" valign="top" width="60%" class="boder" runat="server">
                                                PO#:
                                                <asp:Label runat="server" ID="lblPO" Font-Bold="true" /><br />
                                                <%--Shipping Method:
                                                <asp:Label runat="server" ID="shipTerm"></asp:Label><br />--%>
                                                Payment Method:
                                                <asp:Label runat="server" ID="paymentTerm"></asp:Label><br />
                                                Bank Account:<br />
                                                CITIBANK TAIWAN LIMITED, TAIPEI BRANCH<br />
                                                SWIFT Address: CITITWTX<br />
                                                <asp:Label runat="server" ID="Account"></asp:Label><br />
                                                <asp:Label runat="server" ID="lbBeneficiary" Text="Beneficiary: Advantech Co., LTD"></asp:Label><br />
                                                <asp:Table ID="Table_ExternalNote" CellPadding="0" BorderWidth="0" BorderColor="White" runat="server" HorizontalAlign="Left">
                                                    <asp:TableRow>
                                                        <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="100">Note:</asp:TableCell>
                                                        <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Height="50" ID="Cell_ExternalNote"><%= _lbExternalNote%></asp:TableCell>
                                                    </asp:TableRow>
                                                </asp:Table>
                                            </td>
                                            <td align="right" >
                                                Sub Total:
                                            </td>
                                            <td align="right"  >
                                                <asp:Label runat="server" ID="lbSubTotal1" />
                                            </td>
                                        </tr>
                                        <tr class="cartitem">
                                            <td align="right">
                                                <font color="red">* </font>Freight:
                                            </td>
                                            <td align="right">
                                                <asp:Label runat="server" ID="freight"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="cartitem">
                                            <td align="right">
                                                <font color="red">* </font>Tax(<asp:Label runat="server" ID="lbTaxRate"></asp:Label>):
                                            </td>
                                            <td align="right">
                                                <asp:Label runat="server" ID="tax"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="cartitem">
                                            <td align="right" valign="top">
                                                Total:
                                            </td>
                                            <td align="right" valign="top" style="font-weight: bold; color: #FF0000;">
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
                                    style="font-size: 9.0pt; color: #333333"> Indicates an Estimated Value<o:p></o:p></span><br />
                                <span lang="EN-US" style="font-size: 9.0pt; color: red">*</span>
                                <span lang="EN-US" style="font-size: 9.0pt; color: #333333"> All prices are in US dollars, <asp:Label runat="server" ID="shipTerm" /><o:p></o:p></span></p>
                                <p>
                                    <span lang="EN-US" style="font-size: 9.0pt; color: #333333">Advantech Shipping Policy：<br />

                                        You shall strictly abide by all applicable export/import control laws and regulations
                                        with regard to Strategic High-Tech Commodities, and shall not use the goods supplied by
                                        Advantech for the purpose of manufacturing or developing missiles, nuclear, biological
                                        weapons or other military purposes. Unless otherwise permitted by applicable laws or
                                        regulations, you shall not ship or re-export the goods supplied by Advantech to another
                                        country or area without obtaining the required license or approval from governmental
                                        authorities.
                                    </span>
                                </p>
                            <p>
                                <span lang="EN-US" style="font-size: 9.0pt; color: #333333">
                                    <font style="font-family: Microsoft JhengHei">
                                    台端應嚴格遵守關於戰略性高科技貨品之相關進出口法令，絕不會將研華之產品用於生產或發展飛彈、核子、生化武器或
                                    其他軍事用途。除非法令明文允許，否則台端未獲政府主管機關許可之前，不得將研華之產品運送或再出口至任何國家或
                                    地區。</font><br />
                                    One shipment<br />
                                    Partial shipment allowed<br />
                                    Receipted and confirmed by
                                    
                                </span>
                            </p>
<%--                            <p>
                                <span lang="EN-US" style="font-size: 9.0pt; color: #333333">Quotation subject to Advantech’s standard terms and conditions.</span></p>
                            <p>
                                <span lang="EN-US" style="font-size: 9.0pt; color: #333333;">Please direct any questions regarding this quotation to your Advantech sales representative.</span></p>--%>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <span style="color: #000000; font-weight: bold">Best regards,
                        <br />
                            Advantech Co., Ltd.
                        </span>
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
            <asp:Literal runat="server" ID="litOfficeTelTime2" />
        </td>
    </tr>
    <tr>
        <td height="30" style="text-align: center; border: 1px solid #CCCCCC; background-color: #FFFFE6"
            bgcolor="#FFFFE6" align="center" valign="middle">
            <a href="http://my.advantech.com/files/Terms_USA.aspx" style="text-decoration: none;
                font-size: 11px"><font color="#555555">Terms and Conditions</font>&nbsp;&nbsp;<font
                    color="#555555">|</font>&nbsp;&nbsp;</a><a href="http://erma.advantech.com.tw/usa_index.asp"
                        style="text-decoration: none; font-size: 11px"><font color="#555555">Return Policy</font></a>&nbsp;&nbsp;<font
                            color="#555555"><span style="font-size: 11px;">|</span></font>&nbsp;&nbsp;<a href="http://www.advantech.com/about/privacy.aspx"
                                style="text-decoration: none; font-size: 11px"><font color="#555555">Privacy Policy</font></a>
        </td>
    </tr>
    <!-- Footer /-->
</table>
