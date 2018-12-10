<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="piACN.aspx.vb" Inherits="EDOC.piACN" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">.dummy{}body{font-family: Arial,宋体;font-size: 12px;margin: 0px;}table{width: 100%;}td{border: solid 1px #EEEEEE;}p{line-height: 20px;}</style>
</head>
<body>
    <form id="form1" runat="server">
    .
    <div runat="server" id="divLogo">
        <table>
            <tr>
                <td>
                    <asp:Image ImageUrl="http://eq.advantech.com/Images/LogoPi.jpg" runat="server" ID="imgLogo" />
                </td>
                <td align="right">
                   <%-- <asp:Label ID="lbAddressInfo" runat="server"></asp:Label>--%>
                    <p style="language:zh-CN;margin-top:0pt;margin-bottom:0pt;margin-right:0in;
text-indent:0in;text-align:left;direction:rtl;unicode-bidi:embed">
                        <span style="font-size:13.0pt;font-family:宋体;mso-ascii-font-family:宋体;mso-fareast-font-family:
宋体;mso-bidi-font-family:+mn-cs;color:black;language:zh-CN;font-weight:bold;
font-style:normal">研华（中国）分公司</span></p>
                    <p style="language:zh-CN;margin-top:0pt;margin-bottom:0pt;margin-right:0in;
text-indent:0in;text-align:left;direction:rtl;unicode-bidi:embed">
                        <span style="font-size:10.5pt;font-family:&quot;Times New Roman&quot;;mso-ascii-font-family:
&quot;Times New Roman&quot;;mso-fareast-font-family:宋体;mso-bidi-font-family:&quot;Times New Roman&quot;;
color:black;language:zh-CN;font-weight:normal;font-style:normal"><span style="mso-spacerun:yes">&nbsp;
                        </span></span>
                        <span style="font-size:9.0pt;
font-family:宋体;mso-ascii-font-family:宋体;mso-fareast-font-family:宋体;mso-bidi-font-family:
+mn-cs;color:black;language:zh-CN;font-weight:normal;font-style:normal">地址：上海市江场三路</span><span style="font-size:9.0pt;font-family:&quot;Times New Roman&quot;;mso-ascii-font-family:
&quot;Times New Roman&quot;;mso-fareast-font-family:宋体;mso-bidi-font-family:&quot;Times New Roman&quot;;
color:black;language:en-US;font-weight:normal;font-style:normal">136</span><span style="font-size:9.0pt;font-family:宋体;mso-ascii-font-family:宋体;mso-fareast-font-family:
宋体;mso-bidi-font-family:+mn-cs;color:black;language:zh-CN;font-weight:normal;
font-style:normal">号</span><span style="font-size:9.0pt;font-family:&quot;Times New Roman&quot;;
mso-ascii-font-family:&quot;Times New Roman&quot;;mso-fareast-font-family:宋体;mso-bidi-font-family:
&quot;Times New Roman&quot;;color:black;language:zh-CN;font-weight:normal;font-style:
normal"> </span>
                    </p>
                    <p style="language:zh-CN;margin-top:0pt;margin-bottom:0pt;margin-right:0in;
text-indent:0in;text-align:left;direction:rtl;unicode-bidi:embed">
                        <span style="font-size:9.0pt;font-family:&quot;Times New Roman&quot;;mso-ascii-font-family:
&quot;Times New Roman&quot;;mso-fareast-font-family:宋体;mso-bidi-font-family:&quot;Times New Roman&quot;;
color:black;language:zh-CN;font-weight:normal;font-style:normal"><span style="mso-spacerun:yes">&nbsp;
                        </span></span>
                        <span style="font-size:9.0pt;
font-family:宋体;mso-ascii-font-family:宋体;mso-fareast-font-family:宋体;mso-bidi-font-family:
+mn-cs;color:black;language:zh-CN;font-weight:normal;font-style:normal">电话：</span><span style="font-size:9.0pt;font-family:&quot;Times New Roman&quot;;mso-ascii-font-family:
&quot;Times New Roman&quot;;mso-fareast-font-family:宋体;mso-bidi-font-family:&quot;Times New Roman&quot;;
color:black;language:en-US;font-weight:normal;font-style:normal">021-36321616<span 
                            style="mso-spacerun:yes">&nbsp;&nbsp; </span></span>
                        <span style="font-size:9.0pt;
font-family:宋体;mso-ascii-font-family:宋体;mso-fareast-font-family:宋体;mso-bidi-font-family:
+mn-cs;color:black;language:zh-CN;font-weight:normal;font-style:normal">传真：</span><span style="font-size:9.0pt;font-family:&quot;Times New Roman&quot;;mso-ascii-font-family:
&quot;Times New Roman&quot;;mso-fareast-font-family:宋体;mso-bidi-font-family:&quot;Times New Roman&quot;;
color:black;language:en-US;font-weight:normal;font-style:normal">021-36321516*3322</span><span style="font-size:10.5pt;font-family:&quot;Times New Roman&quot;;mso-ascii-font-family:
&quot;Times New Roman&quot;;mso-fareast-font-family:宋体;mso-bidi-font-family:&quot;Times New Roman&quot;;
color:black;language:en-US;font-weight:normal;font-style:normal"> </span>
                    </p>
                </td>
            </tr>
        </table>
    </div>
    <div runat="server" id="divMaster">
        <table>
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label19" Text="Quote Description"></asp:Label>
                    :
                </td>
                <td>
                    <asp:Label runat="server" ID="lbCustomId"></asp:Label>
                </td>
            </tr>
              <tr>
                <td>
                    <asp:Label runat="server" ID="Label23" Text="Quote Id"></asp:Label>
                    :
                </td>
                <td>
                    <asp:Label runat="server" ID="lbQuoteID"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label20" Text="<%$ Resources:myRs,CreatedBy %>"></asp:Label>
                    :
                </td>
                <td>
                    <asp:Label runat="server" ID="lbCreatedBy"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label21" Text="<%$ Resources:myRs,QuoteDate %>"></asp:Label>
                    :
                </td>
                <td>
                    <asp:Label runat="server" ID="lbQuoteDate"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label22" Text="<%$ Resources:myRs,PreparedBy %>"></asp:Label>
                    :
                </td>
                <td>
                    <asp:Label runat="server" ID="lbPreparedBy"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label1" Text="<%$ Resources:myRs,QuoteTo %>"></asp:Label>
                    :
                </td>
                <td>
                    <asp:Label runat="server" ID="lbQuoteToName"></asp:Label>  <asp:Label runat="server" ID="lbQuoteToId"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label2" Text="<%$ Resources:myRs,AccountInfo %>"></asp:Label>
                    :
                </td>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="Label13" Text="<%$ Resources:myRs,Office %>"></asp:Label>
                                :
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lbOffice"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="Label14" Text="<%$ Resources:myRs,Currency %>"></asp:Label>
                                :
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lbCurrency"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="Label15" Text="<%$ Resources:myRs,SalesEmail %>"></asp:Label>
                                :
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lbSalesEmail"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="Label16" Text="<%$ Resources:myRs,DirectPhone %>"></asp:Label>
                                :
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lbDirectPhone"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="Label17" Text="<%$ Resources:myRs,Attention %>"></asp:Label>
                                :
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lbAttention"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="Label18" Text="<%$ Resources:myRs,BankAccount %>"></asp:Label>
                                :
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lbBankAccount"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label3" Text="<%$ Resources:myRs,DeliveryDate %>"></asp:Label>
                    :
                </td>
                <td>
                    <asp:Label runat="server" ID="lbDeliveryDate"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label4" Text="<%$ Resources:myRs,ExpiredDate %>"></asp:Label>
                    :
                </td>
                <td>
                    <asp:Label runat="server" ID="lbExpiredDate"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label5" Text="<%$ Resources:myRs,ShippingTerms %>"></asp:Label>
                    :
                </td>
                <td>
                    <asp:Label runat="server" ID="lbShippingTerms"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label6" Text="<%$ Resources:myRs,PaymentTerms %>"></asp:Label>
                    :
                </td>
                <td>
                    <asp:Label runat="server" ID="lbPaymentTerms"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label7" Text="<%$ Resources:myRs,Freight %>"></asp:Label>
                    :
                </td>
                <td>
                    <asp:Label runat="server" ID="lbFreight"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label8" Text="<%$ Resources:myRs,Insurance %>"></asp:Label>
                    :
                </td>
                <td>
                    <asp:Label runat="server" ID="lbInsurance"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label9" Text="<%$ Resources:myRs,SpecialCharge %>"></asp:Label>
                    :
                </td>
                <td>
                    <asp:Label runat="server" ID="lbSpecialCharge"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label10" Text="<%$ Resources:myRs,Tax %>"></asp:Label>
                    :
                </td>
                <td>
                    <asp:Label runat="server" ID="lbTax"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label11" Text="<%$ Resources:myRs,QuoteNotes %>"></asp:Label>
                    :
                </td>
                <td>
                    <asp:Label runat="server" ID="lbQuoteNotes"></asp:Label>
                </td>
            </tr>
            <tr runat="server" id="trRelatedInfo">
                <td>
                    <asp:Label runat="server" ID="Label12" Text="<%$ Resources:myRs,RelatedInformation %>"></asp:Label>
                    :
                </td>
                <td>
                    <asp:Label runat="server" ID="lbRelatedInformation"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <div runat="server" id="divDetail">
        <asp:GridView DataKeyNames="line_no" ID="gv1" runat="server" AllowPaging="false"
            EmptyDataText="no Item." AutoGenerateColumns="false" OnRowDataBound="gv1_RowDataBound">
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
                                             <asp:Label runat="server" Text='<%# iif(CDate(Eval("duedate")).toString("yyyy/MM/dd")="1900/01/01","TBD",CDate(Eval("duedate")).toString("yyyy/MM/dd")) %>'
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
                    <asp:Label runat="server" ID="lbSPRNO"  Text='<%#Eval("sprno") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            </Columns>
        </asp:GridView>
            <p style="text-align:right"><b>Total:</b><%= currencySign%>
            <asp:Label runat="server" ID="lbtotal" Text="0.00"></asp:Label>
            <%--<span runat="server" id="spMargin">&nbsp; &nbsp; &nbsp;<b>Total Margin:</b><asp:Label runat="server" ID="lbTotalMargin" Text="0.00"></asp:Label>%</span></p>
    <p style="text-align:right"><span runat="server" id="spMargin1">Total margin calculated without AGS & PTD items.</span></p>--%>
    </div>
    .
    </form>
</body>
</html>

