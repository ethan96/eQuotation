<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="USAOnlineQuoteTemplate.ascx.vb" Inherits="EDOC.USAOnlineQuoteTemplate" %>
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
                        <a title="Home" href="http://buy.advantech.com/Default.htm" style="text-decoration: none">
                            <font size="2" color="#767677"><b>Home</b></font></a> &nbsp; &nbsp; &nbsp; <a title="About Us"
                                href="http://buy.advantech.com/AboutUs/Default.htm" style="text-decoration: none">
                                <font size="2" color="#767677"><b>About Us</b></font></a>
                        &nbsp; &nbsp; &nbsp; <a title="Support" href="http://support.advantech.com.tw/" style="text-decoration: none">
                            <font size="2" color="#767677"><b>Support</b></font></a>
                        &nbsp; &nbsp; &nbsp; <a title="Contact Us" href="http://buy.advantech.com/ContactUs/Default.htm"
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
                                    Thank you for choosing Advantech products and services!
                                    <br />
                                    The quotation number
                                    <%= _QuoteNo%>
                                    has been created upon your request. Please contact your Advantech Sales Team at
                                    <asp:Literal runat="server" ID="litOfficeTel3"/>, should you have any questions regarding this quotation. </span>
                                <br />
                                <br />
                                <%-- <b>Comments: </b>
                                            <br />
                                            <asp:Label runat="server" ID="quoteNote"></asp:Label>--%>
                            </p>
                        </div>
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
                                    <asp:Label runat="server" ID="lblSalesPerson" Font-Bold="true" style="font-size: 14px;" />
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
                                            <asp:TemplateField  ItemStyle-HorizontalAlign="left" ItemStyle-Wrap="true" ItemStyle-Width="20%">
                                                <HeaderTemplate>
                                                    Part No
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lbPN" Text='<%#Bind("partNo")%>' style="vertical-align:middle"></asp:Label>&nbsp
                                                    <asp:Image runat="server" ID="imgRecyclingFee" ImageUrl="~/Images/Recycle.png" Visible="false" style="vertical-align:middle"/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
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
                                                    <asp:Label runat="server" Text='<%# getLocalTime("AUS",Eval("duedate")).ToString("MM/dd/yyyy")%>'
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
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:HiddenField runat="server" ID="hfRecyclingFee" Value='<%#Bind("RecyclingFee") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle BackColor="#FF6600" ForeColor="White" />
                                    </asp:GridView>
                                    <br />
                                    <table width="100%">
                                        <tr class="cartitem">
                                            <td id="tbSM" style="text-align: left;word-wrap:break-word;" rowspan="6" valign="top" width="58%" class="boder" runat="server" >
                                                Shipping Method:
                                                <asp:Label runat="server" ID="shipTerm" /><br />
                                                Payment Method:
                                                <asp:Label runat="server" ID="paymentTerm" /><br />
                                                PO#:
                                                <asp:Label runat="server" ID="lblPO1" /><br />
                                                Note:<br />
                                                <%= _lbExternalNote%>                                                
                                                <%--<asp:Table ID="Table_ExternalNote" CellPadding="0" BorderWidth="0" BorderColor="White" runat="server" HorizontalAlign="Left">
                                                    <asp:TableRow>
                                                        <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="100">Note:</asp:TableCell>
                                                        <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Height="50" Width="250" ID="Cell_ExternalNote" ><%= _lbExternalNote%></asp:TableCell>
                                                    </asp:TableRow>
                                                </asp:Table>--%>                                                
                                            </td>
                                            <td align="right">
                                                <asp:Label runat="server" ID="lbSubTotalTitle" Text="Sub Total:"/>
                                                <%--Sub Total:--%>
                                            </td>
                                            <td align="right">
                                                <asp:Label runat="server" ID="lbSubTotal1"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="cartitem">
                                            <td align="right">
                                                <font color="red">* </font>Shipping & Handling:
                                            </td>
                                            <td align="right">
                                                <asp:Label runat="server" ID="freight"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="cartitem" runat="server" id="RowRecyclingFee">
                                            <td align="right">
                                                <font color="red"></font>Recycling Fee:
                                            </td>
                                            <td align="right">
                                                <asp:Label runat="server" ID="RecyclingFee"></asp:Label>
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
                                            <td align="right">
                                                <asp:Label runat="server" ID="lbTotalTitle" Text="Total:"/>
                                                <%--Total:--%>
                                            </td>
                                            <td align="right" style="font-weight: bold; color: #FF0000;">
                                                <asp:Label runat="server" ID="lbTotal"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="cartitem">
                                            <td align="left" colspan="2" valign="top">
                                                <p>
                                                    <span lang="EN-US" style="font-size: 9.0pt; color: red; font-weight: bold;">*</span><span lang="EN-US"
                                                        style="font-size: 9.0pt; color: #333333; font-weight: bold;"> Indicates an Estimated Value<o:p></o:p></span><br />
                                                    <span lang="EN-US" style="font-size: 9.0pt; color: red; font-weight: bold;">*</span><span lang="EN-US" 
                                                        style="font-size: 9.0pt; color: #333333; font-weight: bold;"> All prices are in US dollars, FCA Milpitas, CA, USA<o:p></o:p></span>
                                                </p>

                                            </td>
                                        </tr>
                                    </table>
                                    <!-- CART DETAIL / -->
                                </td>
                            </tr>
                        </table>
                        <div style="color: #333; padding-left: 10px">
<%--                            <p>
                                <span lang="EN-US" style="font-size: 9.0pt; color: red">*</span><span lang="EN-US"
                                    style="font-size: 9.0pt; color: #333333"> Indicates an Estimated Value<o:p></o:p></span><br />
                                <span lang="EN-US" style="font-size: 9.0pt; color: red">*</span>
                                <span lang="EN-US" style="font-size: 9.0pt; color: #333333"> All prices are in US dollars, F.O.B. California, U.S.A.<o:p></o:p></span></p>--%>
                                <p>
                                     <span lang="EN-US" style="font-size: 9.0pt; color: #333333">  Advantech Rescheduling Policy：<br />
1) No rescheduling can be made within 4 weeks of the ship date.   <br />
2) Notification of push out request must be given within 4-8 weeks prior to the ship date,with a maximum push out of 4 weeks.     <br />
3) Notification within 8 weeks or more must be given if the order is to be pushed out beyond 4 weeks with a maximum 8 week push out date.  <br />
4) Anything beyond 8 weeks will be rejected.   <br />
5) Rescheduling of any order is limited to 1 time per PO.      </span>
                                </p>
                            <p>
                                <span lang="EN-US" style="font-size: 9.0pt; color: #333333">Any orders that ship to the following jurisdictions are charged sales tax: AZ, CA, 
                                    CO, CT, FL, GA, IA, IL, IN, KS, KY, MA, MD, MI, MN, NC, NJ, OH, PA, TN, TX, WA, WI and VA. If you are exempt from sales tax, select the Resale box during the checkout process. 
                                    Upon receiving the proper paperwork, Advantech will not include such taxes in the final invoices.<o:p></o:p></span></p>
                            <p>
                                <span lang="EN-US" style="font-size: 9.0pt; color: #333333">The export of any products or software purchased from Advantech must be made in 
                                    accordance with all relevant laws of the United States, including and without limitation, the US Export Administration Regulations. 
                                    This may require that you obtain a formal export license or make certain declarations to the United States Government regarding products to be exported, 
                                    their destination or their end-use.<o:p></o:p></span></p>
                            <p>
                                <span lang="EN-US" style="font-size: 9.0pt; color: #333333; font-weight:bold">Please refer questions regarding the provided quotation on 
                                    these terms and conditions to the indicated sales representative. <o:p></o:p></span></p>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <span style="color:#000000; font-weight:bold">Best regards,</span>
                        <br />
                        <a href="http://my.advantech.com" style="text-decoration: none; font-weight: bold;
                            color: #000000">Advantech Corp</a> 
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
            <a href="http://buy.advantech.com/resource/aus/terms_and_conditions_aus.pdf" style="text-decoration: none;
                font-size: 11px"><font color="#555555">Terms and Conditions</font>&nbsp;&nbsp;<font
                    color="#555555">|</font>&nbsp;&nbsp;</a><a href="http://buy.advantech.com/Info/ReturnPolicy.htm"
                        style="text-decoration: none; font-size: 11px"><font color="#555555">Return Policy</font></a>&nbsp;&nbsp;<font
                            color="#555555"><span style="font-size: 11px;">|</span></font>&nbsp;&nbsp;<a href="http://buy.advantech.com/Info/PrivacyPolicy.htm"
                                style="text-decoration: none; font-size: 11px"><font color="#555555">Privacy Policy</font></a>
        </td>
    </tr>
    <!-- Footer /-->
</table>