<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="TWAOnlineQuoteTemplate.ascx.vb"
    Inherits="EDOC.TWAOnlineQuoteTemplate" %>
<style type="text/css">
    .stylenormal {
        color: black;
        font-size: 13.0pt;
        text-decoration: none;
        font-family: Microsoft JhengHei;
        text-align: left;
    }

    .stylenormaleng {
        color: black;
        font-size: 10.0pt;
        text-decoration: none;
        font-family: Arial;
        text-align: left;
    }

    .QDtext3 {
        color: #000000;
        font-size: 12.0pt;
        font-family: Microsoft JhengHei;
        font-weight: bold;
        vertical-align: top;
    }
</style>

<div id="divHeader">
    <style type="text/css">
        .style1 {
            color: black;
            font-size: 9.0pt;
            text-decoration: none;
            font-family: Microsoft JhengHei;
            text-align: left;
        }

        .style2 {
            width: 231px;
        }

        .style3 {
            color: black;
            font-size: 10.0pt;
            text-decoration: none;
            font-family: Microsoft JhengHei;
            text-align: left;
            width: 20%;
        }
    </style>
    <table width="900" border="0" cellspacing="0" cellpadding="0" align="center">
        <tr>
            <td align="left" valign="top" class="style3" width="20%">
                <img src="<%=Util.GetRuntimeSiteIP() %>/Images/Advantech logo.jpg" alt="研華科技" />
                <div class="style1">ISO-9001/ISO-14000 認證廠商</div>
            </td>
            <td runat="server" id="tdAdvantechHeader1" align="center" valign="top" class="style2" width="50%">
                <font style="font-size: 30px; font-weight: bold; letter-spacing: 2px; font-family: Microsoft JhengHei;">
                    <asp:Label ID="Label_AdvantechName" runat="server" Text="研華股份有限公司" />
                </font>
                <p>
                    <font style="font-size: 24px; font-family: Arial;">Advantech Co., Ltd. </font>
                </p>
                <div style="height:5px">

                </div>
                <font style="font-size: 30px; font-weight: bold; font-family: Microsoft JhengHei; text-decoration: underline;">
                    <asp:Label ID="lbQuotationTitle1" runat="server" Text="報  價  單" />
                </font>
            </td>
            <td runat="server" id="tdAdvantechHeader2" align="center" valign="top" class="style2" width="50%" visible="false">
                <font style="font-size: 30px; font-weight: bold; letter-spacing: 2px; font-family: Microsoft JhengHei;">
                    <asp:Label ID="Label1" runat="server" Text="研華智聯" />
                    <br />
                    <asp:Label ID="Label2" runat="server" Text="股份有限公司" />
                </font>
                <p>
                    <font style="font-size: 18px; font-family: Arial;">Advantech Service-IoT Co., Ltd.</font>
                    <br />
                    <font style="font-size: 18px; font-family: Arial;">Taiwan Branch</font>
                </p>
                <div style="height:5px">

                </div>
                <font style="font-size: 30px; font-weight: bold; font-family: Microsoft JhengHei; text-decoration: underline;">
                    <asp:Label ID="lbQuotationTitle2" runat="server" Text="報  價  單" />
                </font>
            </td>
            <td runat="server" id="tdAdvantechInfo1" width="30%" align="right" valign="top" class="style1">
                <font style="font-size: 11px; text-align: left">
                    <asp:Label ID="Label_AdvantechHQAddress" runat="server" Text="總公司 台北市內湖區瑞光路26巷20弄1號" />
                    <%--總公司&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 台北市內湖區瑞光路26巷20弄1號--%><br />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    TEL : (02)2792-7818 FAX: (02)<asp:Label ID="Label_HQOfficeTel" runat="server" Text="2794-7302" /><%--2794-7302--%><br />
                    <asp:Label ID="Label_AdvantechTaichungAddress" runat="server" Text="台中分公司 台中市西區忠明南路499號3F" />
                    <%--台中分公司&nbsp;台中市西屯區台灣大道二段633號6樓之5--%><br />
                    &nbsp;
                    TEL : (04)2372-5058 FAX : (04)2372-6028<br />
                    <asp:Label ID="Label_AdvantechKaohsiungAddress" runat="server" Text="高雄分公司 高雄市三民區九如一路502號21樓A1" />
                    <%--高雄分公司&nbsp; 高雄市三民區九如一路502號21樓A1--%><br />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    TEL : (07)392-3600 FAX : (07)380-0217<br />
                </font>
            </td>
            <td runat="server" id="tdAdvantechInfo2" visible="false" width="30%" align="left" valign="top" class="style1">
                <div id="td2ChineseVersion" runat="server" visible="false">
                    <font style="font-size: 11px; text-align: left">
                        英屬開曼群島商&nbsp;&nbsp;研華智聯(股)公司&nbsp;&nbsp;台灣分公司<br />
                        Advantech Service-IoT  Co., Ltd. Taiwan Branch<br />
                        台北市內湖區瑞光路 26 巷 20 弄 1 號<br />
                        No. 1, Alley 20, Lane 26, Rueiguang Road, Neihu District<br />
                        11491, Taiwan, R.O.C.&nbsp;&nbsp;Tel: 886-2-2792-7818<br />
                    </font>
                </div>
                <div id="td2EnglishVersion" runat="server" visible="false">
                    <font style="font-size: 11px; text-align: left">
                        Advantech Service-IoT  Co., Ltd. Taiwan Branch<br />
                        No. 1, Alley 20, Lane 26, Rueiguang Road, Neihu District<br />
                        11491, Taiwan, R.O.C.&nbsp;&nbsp;Tel: 886-2-2792-7818<br />
                    </font>
                </div>
            </td>
        </tr>
        <%--<tr>
            <td align="center" valign="top" colspan="4" style="margin-top:20px">
                <font style="font-size: 30px; font-weight: bold; font-family: Microsoft JhengHei; text-decoration: underline;">
                    <asp:Label ID="LabelQuotationTitle" runat="server" Text="報  價  單" />
                </font>
            </td>
        </tr>--%>
    </table>
</div>
<br />
<table width="900" border="0" cellspacing="3" cellpadding="0" align="center"
    class="stylenormal">
    <tr>
        <td align="left" width="<%=_columnnamewith%>px" nowrap>
            <asp:Label ID="LableColumnName_AccountName" runat="server" Text="客戶名稱" />：
        </td>
        <td align="left" width="<%=_columnvalueith%>px">
            <asp:Literal runat="server" ID="litAccountName" />
        </td>
        <td align="left" width="150px">
            <asp:Label ID="LableColumnName_QuoteDate" runat="server" Text="報價日期" />：
        </td>
        <td align="right" width="180px">
            <asp:Literal runat="server" ID="LitQuoteDate" />
        </td>
    </tr>
    <tr>
        <td align="left">
            <asp:Label ID="LableColumnName_AccountAddress" runat="server" Text="聯絡地址" />：
        </td>
        <td align="left">
            <asp:Literal runat="server" ID="litAccountContactAddress" />
        </td>
        <td align="left">
            <asp:Label ID="LableColumnName_QuoteNumber" runat="server" Text="報價單號" />：
        </td>
        <td align="right">
            <asp:Literal runat="server" ID="litQuoteNo" />
        </td>
    </tr>
    <tr>
        <td align="left">
            <asp:Label ID="LableColumnName_AccountContactPerson" runat="server" Text="聯 絡 人" />：
        </td>
        <td align="left">
            <asp:Literal runat="server" ID="litAccountContactPerson" />
        </td>
        <td align="left">
            <asp:Label ID="LableColumnName_QuoteRevNumber" runat="server" Text="REV#" />：
        </td>
        <td align="right">
            <asp:Literal runat="server" ID="LitQuoteVersion" />
        </td>
    </tr>
    <tr>
        <td align="left">
            <asp:Label ID="LableColumnName_AccountCompanyID" runat="server" Text="統一編號" />：
        </td>
        <td align="left">
            <asp:Literal runat="server" ID="LitAccountERPID" />
        </td>
        <td align="left">
            <asp:Label ID="LableColumnName_QuoteExpiredDate" runat="server" Text="有效日期" />：
        </td>
        <td align="right">
            <asp:Literal runat="server" ID="LitExpiredDate" />
        </td>
    </tr>
    <tr>
        <td align="left">
            <asp:Label ID="LableColumnName_AccountTEL" runat="server" Text="電       話" />：
        </td>
        <td align="left">
            <asp:Literal runat="server" ID="LitAccountContactTEL" />
        </td>
        <td align="left">
            <asp:Label ID="LableColumnName_SalesRepresentative" runat="server" Text="負責業務" />：
        </td>
        <td align="right">
            <asp:Literal runat="server" ID="LitSalesRepresentative" />
        </td>
    </tr>
    <tr>
        <td align="left">
            <asp:Label ID="LableColumnName_AccountFAX" runat="server" Text="傳       真" />：
        </td>
        <td align="left">
            <asp:Literal runat="server" ID="LitAccountContactFAX" />
        </td>
        <td align="left">
            <asp:Label ID="LableColumnName_SalesRepresentativeTEL" runat="server" Text="業務電話" />：
        </td>
        <td align="right">
            <asp:Literal runat="server" ID="LitSalesRepresentativeTEL" />
        </td>
    </tr>
    <tr>
        <td align="left"></td>
        <td align="left"></td>
        <td align="left">
            <asp:Label ID="LableColumnName_SalesRepresentativeFAX" runat="server" Text="業務傳真" />：
        </td>
        <td align="right">
            <asp:Literal runat="server" ID="LitSalesRepresentativeFAX" />
        </td>
    </tr>
    <tr>
        <td align="left"></td>
        <td align="left"></td>
        <td align="left">
            <asp:Label ID="LableColumnName_SalesRepresentativeEmail" runat="server" Text="業務Email" />：
        </td>
        <td align="right">
            <asp:Literal runat="server" ID="LitSalesRepresentativeEmail" />
        </td>
    </tr>

</table>
<table width="900" border="0" cellspacing="3" cellpadding="0" align="center"
    class="stylenormal" runat="server" id="TablePayMentArea">
    <tr>
        <td align="left" width="100px">
            <asp:Label ID="LabelTitle_ShippingDate" runat="server" Text="1.交貨日期" />：
        </td>
        <td align="left">
            <asp:Literal runat="server" ID="LitReqDate" Text="下單前請與業務再次確認" />
        </td>
    </tr>
    <tr>
        <td align="left">
            <asp:Label ID="LabelTitle_ShippingCondition" runat="server" Text="2.交貨方式" />：
        </td>
        <td align="left">
            <asp:Literal runat="server" ID="ltShipment" Text="送至貴處" />
        </td>
    </tr>
    <tr>
        <td align="left">
            <asp:Label ID="LabelTitle_Warranty" runat="server" Text="3.保固期間" />：
        </td>
        <td align="left">
            <asp:Literal runat="server" ID="LitExtendWarranty" Text="2 Years" />
        </td>
    </tr>
    <tr>
        <td align="left" valign="top">
            <asp:Label ID="LabelTitle_PaymentTerm" runat="server" Text="4.付款條件" />：
        </td>
        <td align="left">
            <%--<asp:Literal runat="server" ID="LitPaymentTerm" Text="當月結30天，若T/T請電匯至花旗(台灣)商業銀行營業部(銀行代碼：021-0018)，帳號：5028523019" />--%>
            <asp:Literal runat="server" ID="LitPaymentTerm" Text="" />
            <asp:Label ID="LabelTitle_PaymentTermText1" runat="server" Text="，若T/T請電匯至花旗(台灣)商業銀行營業部" />
            <br />
            <asp:Label ID="LabelTitle_PaymentTermText2" runat="server" Text="(銀行代碼：021-0018)，帳號：" />
            <asp:Literal runat="server" ID="lt_BackAccount" Text="5028523019" />
        </td>
    </tr>
</table>
<%--<p style="page-break-before:always" />--%>
<table width="900" border="0" align="center">
    <tr>
        <td>
            <asp:GridView DataKeyNames="line_no" ID="gv1" runat="server" AllowPaging="false"
                EmptyDataText="no Item." AutoGenerateColumns="false" OnRowDataBound="gv1_RowDataBound"
                Font-Size="12pt" Width="100%">
                <Columns>
                    <asp:BoundField DataField="DisplayLineNo" HeaderText="序號" ItemStyle-HorizontalAlign="center" ItemStyle-Width="70">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="VirtualPartNo" HeaderText="產品編號" ItemStyle-Width="150px" ItemStyle-HorizontalAlign="left"
                        ItemStyle-Wrap="false">
                        <ItemStyle HorizontalAlign="Left" CssClass="stylenormal"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="VirtualPartNo" HeaderText="虛擬產品編號" Visible="false" ItemStyle-HorizontalAlign="left">
                        <ItemStyle HorizontalAlign="Left" CssClass="stylenormal"></ItemStyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="品名規格說明" ItemStyle-Width="310px">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lbdescription" CssClass="stylenormal" Text='<%#Bind("description") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="80">
                        <HeaderTemplate>
                            數量
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%#Bind("DisplayQty")%>' ID="lbGVQty" />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="120">
                        <HeaderTemplate>
                            單價
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lbUnitPriceSign" Text="" />
                            <asp:Label runat="server" ID="lbUnitPrice" Text='<%#Bind("DisplayUnitPrice")%>' />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="120">
                        <HeaderTemplate>
                            金額
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lbSubTotalSign" Text="" />
                            <asp:Label runat="server" ID="lbSubTotal" Text="" />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle BackColor="White" ForeColor="Black" Font-Bold="false" Font-Names="微軟正黑體" />
                <RowStyle Font-Names="Arial" />
            </asp:GridView>
        </td>
    </tr>
    <tr>
        <td>
            <table width="100%" class="stylenormal">
                <tr class="cartitem">
                    <td id="tbSM" style="text-align: left" rowspan="4" valign="top" width="60%" class="boder"
                        runat="server">
                        <asp:Table ID="Table_ExternalNote" CellPadding="0" BorderWidth="0" BorderColor="White"
                            runat="server" HorizontalAlign="Left">
                            <asp:TableRow>
                                <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="20">
                                    <asp:Label ID="Label_Note" runat="server" Text="註" />:</asp:TableCell>
                                <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Height="50" ID="Cell_ExternalNote"><%= _lbExternalNote%></asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </td>
                    <td align="right">
                        <strong>
                            <asp:Label ID="Label_QuoteAmoutWithoutTax" runat="server" Text="報價淨額" />:</strong>
                    </td>
                    <td align="right">
                        <asp:Label runat="server" ID="lbSubTotal1"></asp:Label>
                    </td>
                </tr>
                <%--                <tr class="cartitem">
                    <td align="right">
                        <font color="red">* </font>Freight:
                    </td>
                    <td align="right">
                        <asp:Label runat="server" ID="freight"></asp:Label>
                    </td>
                </tr>
                --%>
                <tr class="cartitem">
                    <td align="right">
                        <strong>
                            <asp:Label ID="Label_Tax" runat="server" Text="稅 額" />(<asp:Label runat="server" ID="lbTaxRate"></asp:Label>):</strong>
                    </td>
                    <td align="right">
                        <asp:Label runat="server" ID="tax"></asp:Label>
                    </td>
                </tr>
                <tr class="cartitem">
                    <td align="right">
                        <strong>
                            <asp:Label ID="Label_QuoteAmoutWithTax" runat="server" Text="總 金 額" />:</strong>
                    </td>
                    <td align="right" style="font-weight: bold; color: #FF0000;">
                        <asp:Label runat="server" ID="lbTotal"></asp:Label>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
            <p />
        </td>
    </tr>
    <tr>
        <td>
            <table width="100%">
                <tr>
                    <td width="50%" class="stylenormal">
                        <div runat="server" id="DIV_question1">
                            若同意視為正式訂單，請於回傳時加蓋<b style="color: red">公司大小章或發票章</b><br />
                            並請確認上述聯絡地址是否為送貨地址<br />
                            〔 〕是<br />
                            〔 〕否，送貨地址:<br />
                            <br />
                            <br />
                            <br />
                            <br />
                            期望交貨日期:
                        </div>
                    </td>
                    <td width="10%" valign="top" class="stylenormal"></td>
                    <td width="40%" valign="top" class="stylenormal">
                        <asp:Label ID="LabelTitle_SaleEngineer" runat="server" Text="業務工程師/專員" />:
                        <asp:Label runat="server" ID="lbSalesEngineerName" /><br />
                        <asp:Label ID="LabelTitle_SaleEngineerTEL" runat="server" Text="電話" />:
                        <asp:Label runat="server" ID="lbSalesEngineerTEL" /><br />
                        <asp:Label ID="LabelTitle_ePlatformTollFreeNumber" runat="server" Text=" e化電腦平台服務熱線" />: 0800-777-111<br />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
            <p />
        </td>
    </tr>
</table>
<div id="divFooter">
    <style type="text/css">
        .stylenotice {
            color: black;
            font-size: 12.0pt;
            text-decoration: none;
            font-family: Microsoft JhengHei;
            text-align: left;
        }
    </style>
    <table width="900" border="0" cellspacing="0" cellpadding="0" align="center">
        <tr>
            <td>
                <table width="100%">
                    <tr>
                        <td width="60%" class="stylenotice">
                            <div runat="server" id="TermsAndConditions">
                                買受方同意遇有不依約定償還價款或完<br />
                                成特定條件者, 出賣人得依動產擔保交<br />
                                易法第28條規定取回買受方占有標的物<br />
                            </div>
                        </td>
                        <td width="40%" class="stylenotice" valign="top">
                            <font style="font-size: 12px; font-weight: bold; font-family: Arial;">
                                <strong>WebSites Information</strong>
                            <br />
                            1. Home Page :<a href="http://www.advantech.com.tw">http://www.advantech.com.tw</a><br />
                            2. e.RMA :<a href="http://rma.advantech.com.tw">http://rma.advantech.com.tw</a><br />
                        </font>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
<br />


