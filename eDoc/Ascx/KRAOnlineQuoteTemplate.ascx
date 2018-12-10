<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="KRAOnlineQuoteTemplate.ascx.vb" Inherits="EDOC.KRAOnlineQuoteTemplate" %>
<style type="text/css">
    .stylenormal {
        color: black;
        font-size: 12.0pt;
        text-decoration: none;
        font-family: Malgun Gothic;
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

    .grid {
        border: solid 3px black;
        border-bottom: 2px solid black;
    }

        .grid td {
            border-collapse: collapse;
            border-top: 0px solid;
            border-right: 0px solid;
        }

        .grid th {
            border-collapse: collapse;
            border-top: 0px solid;
            border-right: 0px solid;
        }

        .grid td.first {
            border-collapse: collapse;
            border-top: 0px solid;
            border-right: 0px solid;
            border-left: 0px solid;
        }

        .grid th.first {
            border-collapse: collapse;
            border-top: 0px solid;
            border-right: 0px solid;
            border-left: 0px solid;
        }
</style>

<div id="divHeader">
    <style type="text/css">
        .style1 {
            color: black;
            font-size: 12.0pt;
            text-decoration: none;
            font-family: Malgun Gothic;
        }

        .style2 {
            width: 231px;
        }

        .style3 {
            color: black;
            font-size: 10.0pt;
            text-decoration: none;
            font-family: Malgun Gothic;
        }
    </style>
    <table width="900" border="0" cellspacing="0" cellpadding="0" align="center">
        <tr><td colspan="4">&nbsp;<br />&nbsp;<br /><!--keep more space--></td></tr>
        <tr>
            <td align="left" valign="top" class="style3" width="150">
            </td>
            <td align="center" valign="bottom" class="style2" width="480">
                <font></font>
                <font style="font-size: 40px; letter-spacing: 25px; font-family: Malgun Gothic;">
                <strong>견 적 서</strong></font>
            </td>
            <td width="20">
            </td>
            <td width="260" align="right" valign="top" class="style1">
                <a href="http://www.advantech.co.kr/">
                    <img src="<%=Util.GetRuntimeSiteIP() %>/Images/2010-Logo-with-Slogan.jpg" alt="Advantech" width="274" />
                </a>
            </td>
        </tr>
    <tr>
        <td align="center" valign="top" colspan="4">
            <hr style=" border:0; height:1px; background-color:black;	color:black">
        </td>
    </tr>
    </table>
</div>
<br />
<table width="900" align="center" >
    <tr>
        <td width="900">


<table width="900" border="0" cellspacing="3" cellpadding="0" align="center" 
    class="stylenormal">
    <tr>
        <td align="left" width="500px">
            <font style="font-size: 20px; font-family: Malgun Gothic;">
                <strong>
                    <asp:Literal runat="server" ID="litAccountName" />
                </strong>
            </font> 귀중
            <hr style=" border:0; height:1px; background-color:black;	color:black" />
        </td>
        <td width="50px" />
        <td align="left" width="350px" class="stylenormal">
            DATE:&nbsp;<asp:Literal runat="server" ID="LitQuoteDate" /><br/>
            OUR REF No.&nbsp;<asp:Literal runat="server" ID="litQuoteNo" />
            REV#:&nbsp;<asp:Literal runat="server" ID="LitQuoteVersion" />
        </td>
    </tr>
    <tr>
        <td align="left" class="stylenormal">
            <table border="0">
                <tr>
                    <td>ATTN:</td>
                    <td>
                        <asp:Literal runat="server" ID="litAccountContactPerson" />
                    </td>
                </tr>
                <tr>
                    <td>TEL:</td>
                    <td>
                        <asp:Literal runat="server" ID="LitAccountContactTEL" />
                    </td>
                </tr>
                <tr>
                    <td>E-mail:</td>
                    <td>
                        <asp:Literal runat="server" ID="LitAccountContactEmail" />
                    </td>
                </tr>
                <tr>
                    <td height="8px"></td>
                    <td></td>
                </tr>
                <tr>
                    <td>FROM:</td>
                    <td>
                        <asp:Literal runat="server" ID="LitSalesRepresentative" />
                        /&nbsp;<asp:Literal runat="server" ID="LitSalesRepresentativeTEL" />
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:Literal runat="server" ID="LitSalesRepresentativeEmail" />
                        <asp:Literal runat="server" ID="LitSalesRepresentativeCellPhone" />
                    </td>
                </tr>
            </table>

        </td>
        <td width="100px" />
        <td align="left" class="stylenormal">
            <font style="font-size: 30px; font-family: Malgun Gothic;">
                <strong>
                어드밴텍케이알(주)
                </strong>
            </font>
            <br />
            서울특별시 강서구 등촌동 684-1<br />
            에이스 테크노 타워 1202호<br />
            <table>
                <tr>
                    <td valign="top">전화: 02-3663-9494<br />
                        팩스: 02-3663-4955<br />
                        대표이사<strong> 허 츈 성</strong>
                    </td>
                    <td>
                        <img src="<%=Util.GetRuntimeSiteIP() %>/Images/AKRStamp.png" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<table width="900" border="0" align="center">
    <tr><td class="stylenormal">아래와 같이 견적합니다.</td></tr>
</table>

<asp:GridView DataKeyNames="line_no" ID="gv1" runat="server" AllowPaging="false" 
    EmptyDataText="no Item." AutoGenerateColumns="false" OnRowDataBound="gv1_RowDataBound"
    Font-Size="12pt" Width="100%" HorizontalAlign="Center" style="left:0px; top:0px" CssClass="grid" >
    <Columns>
        <asp:BoundField DataField="line_no" HeaderText="No." ItemStyle-HorizontalAlign="center" ItemStyle-Width="70" HeaderStyle-CssClass="first" ItemStyle-CssClass="first">
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
        </asp:BoundField>
        <asp:BoundField DataField="PartNo" HeaderText="ITEM" Visible="false" ItemStyle-Width="150px" ItemStyle-HorizontalAlign="left"
            ItemStyle-Wrap="false">
            <ItemStyle HorizontalAlign="Left" CssClass="stylenormal"></ItemStyle>
        </asp:BoundField>
        <asp:BoundField DataField="VirtualPartNo" HeaderText="ITEM" ItemStyle-HorizontalAlign="left">
            <ItemStyle HorizontalAlign="Left" CssClass="stylenormal"></ItemStyle>
        </asp:BoundField>
        <asp:TemplateField HeaderText="DESCRIPTION" ItemStyle-Width="310px"  ItemStyle-VerticalAlign="Middle">
            <ItemTemplate>
                <asp:Label runat="server" ID="lbdescription" Class="stylenormal" Text='<%#Bind("description") %>' />
            </ItemTemplate>
            <ItemStyle VerticalAlign="Middle" Width="310px"></ItemStyle>
        </asp:TemplateField>
        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="60">
            <HeaderTemplate>
                EA
            </HeaderTemplate>
            <ItemTemplate>
                <asp:Label runat="server" Text='<%#Bind("Qty")%>' ID="lbGVQty" />
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
        </asp:TemplateField>
        <asp:TemplateField ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="130">
            <HeaderTemplate>
                PRICE
            </HeaderTemplate>
            <ItemTemplate>
                <asp:Label runat="server" ID="lbUnitPriceSign" Text="" />
                <asp:Label runat="server" ID="lbUnitPrice" Text='<%#Bind("NewUnitPrice")%>' />
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            <ItemStyle HorizontalAlign="Right"></ItemStyle>
        </asp:TemplateField>
        <asp:TemplateField ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="130">
            <HeaderTemplate>
                TOTAL
            </HeaderTemplate>
            <ItemTemplate>
                <asp:Label runat="server" ID="lbSubTotalSign" Text="" />
                <asp:Label runat="server" ID="lbSubTotal" Text="" />
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            <ItemStyle HorizontalAlign="Right"></ItemStyle>
        </asp:TemplateField>
    </Columns>
<%--    <HeaderStyle BackColor="White" BorderStyle="Solid" BorderColor="Black" Font-Names="Malgun Gothic" BorderWidth="10px" />
    <RowStyle Font-Names="Malgun Gothic" BorderStyle="Solid" BorderColor="Black" BorderWidth="2px" />--%>
<%--    <HeaderStyle Font-Names="Malgun Gothic" CssClass="first"  />
    <RowStyle Font-Names="Malgun Gothic" CssClass="first"" />--%>

</asp:GridView>
<table width="900" border="1" class="stylenormal"  align="center" style="border:3px black solid;padding:0px;border-collapse:collapse; border-top:0px solid">
    <tr class="cartitem" runat="server" id="trListPriceTotal">
        <td align="center" Class="stylenormal" width="765px">
            <strong>List Price</strong>
        </td>
        <td align="right" width="135x">
            <asp:Label runat="server" ID="lbListPriceTotal"/>
        </td>
    </tr>
    <tr class="cartitem" runat="server" id="trNegoPriceTotal">
        <%--<td id="tbSM" style="text-align: center" rowspan="3" valign="top" width="563px" class="boder"
            runat="server">
            <asp:Table ID="Table_ExternalNote" CellPadding="0" BorderWidth="0" BorderColor="White"
                runat="server" HorizontalAlign="Left">
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="20" CssClass="stylenormal">Note:</asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Height="50" CssClass="stylenormal" ID="Cell_ExternalNote"><%= _lbExternalNote%></asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </td>--%>
        <td align="center" Class="stylenormal" width="765px">
            <strong>Nego</strong>
        </td>
        <td align="right" width="135x">
            <asp:Label runat="server" ID="lbSubTotal1"/>
        </td>
    </tr>
    <tr class="cartitem">
        <td align="center"  Class="stylenormal">
            <strong>V.A.T(<asp:Label runat="server" ID="lbTaxRate"></asp:Label>)</strong>
        </td>
        <td align="right">
            <asp:Label runat="server" ID="tax"/>
        </td>
    </tr>
    <tr class="cartitem">
        <td align="center"  Class="stylenormal">
            <strong>Total</strong>
        </td>
        <td align="right"">
            <asp:Label runat="server" ID="lbTotal"/>
        </td>
    </tr>
    <tr class="cartitem">
        <td colspan="2" style="height:40px" class="stylenormal">            
           <asp:Literal ID ="ltNotes" runat="server"></asp:Literal>
        </td>
    </tr>
</table>
<table width="900" border="1" cellspacing="3" cellpadding="0" align="center" style="border:3px black solid;padding:0px;border-collapse:collapse; border-top:0px solid">
    <tr>
        <td align="left" width="72" class="stylenormal">특기사항
        </td>
        <td align="left" class="stylenormal">

            * 견적유효기간 (Validity Term) : 견적일로부터
            <asp:Literal runat="server" ID="LitExpiredDate" />
            일<br />
            * 유지보수 : 납품일로부터 무상 A/S
            <asp:Literal runat="server" ID="LitExtendWarranty" Text="2" />
            년<br />
            * 제품 납기 (Delivery Date) : 최대 4주 ( 부품수입 일정에 따라 약간의 변동 있을수 있음)<br />
            * 결제 조건 (Payment Conditions) :
            <asp:Literal runat="server" ID="LitPaymentTerm" Text="" />
            ( 계좌번호:기업은행 311-045123-04-012 어드밴텍케이알(주) )<br />
            * 당사의 모든제품은 구매자의 주문에 의해 수입되므로 반품및 교환이 불가능하며, 배상책임한도는 납품가액을 상회하지않음을 원칙으로 합니다.
        </td>
    </tr>
</table>

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
<%--                <table width="100%">
                    <tr>
                        <td width="60%" class="stylenotice">買受方同意遇有不依約定償還價款或完<br />
                            成特定條件者, 出賣人得依動產擔保交<br />
                            易法第28條規定取回買受方占有標的物<br />
                        </td>
                        <td width="40%" class="stylenotice">
                            <font style="font-size: 12px; font-weight: bold; font-family: Arial;"><strong>WebSites
                            Information</strong>
                            <br />
                            1. Home Page :<a href="http://www.advantech.com.tw">http://www.advantech.com.tw</a><br />
                            2. e.RMA :<a href="http://rma.advantech.com.tw">http://rma.advantech.com.tw</a><br />
                        </font>
                        </td>
                    </tr>
                </table>--%>
            </td>
        </tr>
    </table>
</div>
<br />
