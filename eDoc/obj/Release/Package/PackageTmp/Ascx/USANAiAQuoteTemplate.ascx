<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="USANAiAQuoteTemplate.ascx.vb" Inherits="EDOC.USANAiAQuoteTemplate" %>
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


.verticaltext {
    transform: rotate(90deg);
    transform-origin: right, top;
    -ms-transform: rotate(90deg);
    -ms-transform-origin:right, top;
    -webkit-transform: rotate(90deg);
    -webkit-transform-origin:right, top;
    color:black;
    font-size:24px;
    position:absolute;
    top:250px;
    left:-80px;
  
}
</style>
<div class="verticaltext">Send all POs to <a href="mailto:orders.aac@advantech.com">orders.aac@advantech.com</a></div>
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
                                    Thank you for your interest in Advantech’s products and services. Below please find the quotation you requested. Please contact your Advantech Sales Team at 1-800-800-6889 should you have any questions regarding this quotation.                                    
                                </span>
                                <br /><br />
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
                                    <span style="font-size: 14px; color: #003D7C"><b>Quotation Detail:</b></span>&nbsp;&nbsp;&nbsp;<asp:Label runat="server" ID="lblSalesPerson" Font-Bold="true" />&nbsp;&nbsp;&nbsp;<asp:Label runat="server" ID="lblPO" Font-Bold="true" />
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
                                        </Columns>
                                        <HeaderStyle BackColor="#FF6600" ForeColor="White" />
                                    </asp:GridView>
                                    <br />
                                    <table width="100%">
                                        <tr class="cartitem">
                                            <td id="tbSM" style="text-align: left" rowspan="5" valign="top" width="60%" class="boder" runat="server">
                                                Shipping Method:
                                                <asp:Label runat="server" ID="shipTerm"></asp:Label><br />
                                                Payment Method:
                                                <asp:Label runat="server" ID="paymentTerm"></asp:Label><br /><br />
                                                <asp:Table ID="Table_ExternalNote" CellPadding="0" BorderWidth="0" BorderColor="White" runat="server" HorizontalAlign="Left">
                                                    <asp:TableRow>
                                                        <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="100">Note:</asp:TableCell>
                                                        <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Height="50" ID="Cell_ExternalNote"><%= _lbExternalNote%></asp:TableCell>
                                                    </asp:TableRow>
                                                </asp:Table>
                                            </td>
                                            <td align="right">
                                                <asp:Label runat="server" ID="lbLPSubTotalTitle" Text="List Price Sub Total:" Visible="false" />
                                                
                                            </td>
                                            <td align="right">
                                                <asp:Label runat="server" ID="lbLPSubTotal"  Visible="false" />
                                            </td>
                                        </tr>
                                        <tr class="cartitem">
                                            <td align="right">
                                                Extended Price Sub Total:
                                            </td>
                                            <td align="right">
                                                <asp:Label runat="server" ID="lbUPSubTotal" />
                                            </td>
                                        </tr>
                                        <tr class="cartitem">
                                            <td align="right">
                                                <font color="red">* </font>Shipping & Handling:
                                            </td>
                                            <td align="right">
                                                <asp:Label runat="server" ID="freight" />
                                            </td>
                                        </tr>
                                        <tr class="cartitem">
                                            <td align="right">
                                                <font color="red">* </font>Tax(<asp:Label runat="server" ID="lbTaxRate"></asp:Label>):
                                            </td>
                                            <td align="right">
                                                <asp:Label runat="server" ID="tax" />
                                            </td>
                                        </tr>
                                        <tr class="cartitem">
                                            <td align="right">
                                                Total:
                                            </td>
                                            <td align="right" style="font-weight: bold; color: #FF0000;">
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
                                <span lang="EN-US" style="font-size: 9.0pt; color: #333333"> All prices are in US dollars, F.O.B. California, U.S.A.<o:p></o:p></span></p>
                                <p>
                                    <span lang="EN-US" style="font-size: 9.0pt; color: #333333">Advantech Shipping Policy：<br />
                                        Shipments of any product purchases are subject to Advantech’s schedule. Advantech will try to meet delivery dates quoted or acknowledged.  However, Advantech is not liable for its failure to meet such dates.
                                    </span>
                                </p>
                            <p>
                                <span lang="EN-US" style="font-size: 9.0pt; color: #333333">Advantech ships via UPS, FedEx, and other major carriers upon request.  Please inspect all packages immediately upon receipt. Any damage or loss in transit should be noted on any delivery receipt and must be reported to Advantech promptly. Customers should file claims on carriers for any loss immediately.
                                    
                                </span>
                            </p>
                            <p>
                                <span lang="EN-US" style="font-size: 9.0pt; color: #333333">Quotation subject to Advantech’s standard terms and conditions.</span></p>
                            <p>
                                <span lang="EN-US" style="font-size: 9.0pt; color: #333333;">Please direct any questions regarding this quotation to your Advantech sales representative.</span></p>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <span style="color: #000000; font-weight: bold">Best regards,
                        <br />
                            Advantech Corporation
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
