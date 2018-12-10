<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CNAOnlineQuoteTemplate.ascx.vb" Inherits="EDOC.CNAOnlineQuoteTemplate" %>
<style type="text/css">

    .stylenormal
    {
        color: black;
        font-size: 13.0pt;
        text-decoration: none;
        font-family: Microsoft JhengHei;
        text-align: left;
        
    }

    .stylenormaleng
    {
        color: black;
        font-size: 10.0pt;
        text-decoration: none;
        font-family: Arial;
        text-align: left;
    }

    .QDtext3
    {
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
            width: 189px;
        }
    </style>
    <table width="900" border="0" cellspacing="0" cellpadding="0" align="center">
        <tr>
            <td align="left" valign="top" width="210">
                <img src="<%=Util.GetRuntimeSiteIP() %>/Images/aeuheard.gif" alt="研華科技" width="200"  />
            </td>
            <td align="center" valign="top" width="480">
                <font style="font-size: 30px; font-weight: bold; letter-spacing: 1px; font-family: Microsoft JhengHei;">
                北京研华兴业电子科技有限公司</font>
                <p>
                    <font style="font-size: 24px; font-family: Arial;">Advantech(China) Co.,Ltd.</font>
                </p>
            </td>
            <td width="10">
            </td>
            <td width="200" align="left" valign="top" class="style1">
                <font style="font-size: 11px; text-align: left">
                    北京市海淀区上地信息产业基地<br />
                    六街七号研华大厦<br />
                    邮编 : 100085<br />
                    电话 : 86-10-6298-4346<br />
                </font>
            </td>
        </tr>
        <tr>
            <td>&nbsp;<br /></td>
        </tr>
        <tr>
            <td align="center" valign="top" colspan="4">
                <font style="font-size: 30px; font-weight: bold; font-family: Microsoft JhengHei; text-decoration: underline;">
                报&nbsp;&nbsp; 价&nbsp;&nbsp; 单</font>
            </td>
        </tr>
    </table>
</div>
<br />
<table width="900" border="0" cellspacing="3" cellpadding="0" align="center" 
    class="stylenormal">
    <tr>
        <td align="left" width="90px">
            购买单位：
        </td>
        <td align="left" width="540px">
            <asp:Literal runat="server" ID="litAccountName" />
        </td>
        <td align="left" width="90px">
            报价日期：
        </td>
        <td align="right" width="180px">
            <asp:Literal runat="server" ID="LitQuoteDate" />
        </td>
    </tr>
    <tr>
        <td align="left">
            发货地址：
        </td>
        <td align="left">
            <asp:Literal runat="server" ID="litAccountContactAddress" />
        </td>
        <td align="left">
            报价单号：
        </td>
        <td align="right">
            <asp:Literal runat="server" ID="litQuoteNo" />
        </td>
    </tr>
    <tr>
        <td align="left">
            联&nbsp; 系&nbsp; 人：
        </td>
        <td align="left">
            <asp:Literal runat="server" ID="litAccountContactPerson" />
        </td>
        <td align="left">
            REV#：
        </td>
        <td align="right">
            <asp:Literal runat="server" ID="LitQuoteVersion" />
        </td>
    </tr>
    <tr runat="server" id="expireddate" visible="false">
        <td align="left">
            統一編號：
        </td>
        <td align="left">
            <asp:Literal runat="server" ID="LitAccountERPID" />
        </td>
        <td align="left">
            有效日期：
        </td>
        <td align="right">
            <asp:Literal runat="server" ID="LitExpiredDate" />
        </td>
    </tr>
    <tr>
        <td align="left">
            电&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 话：
        </td>
        <td align="left">
            <asp:Literal runat="server" ID="LitAccountContactTEL" />
        </td>
        <td align="left">
            负责业务：
        </td>
        <td align="right">
            <asp:Literal runat="server" ID="LitSalesRepresentative" />
        </td>
    </tr>
    <tr>
        <td align="left">
            传&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 真：
        </td>
        <td align="left">
            <asp:Literal runat="server" ID="LitAccountContactFAX" />
        </td>
        <td align="left">
            业务电话：
        </td>
        <td align="right">
            <asp:Literal runat="server" ID="LitSalesRepresentativeTEL" />
        </td>
    </tr>
    <tr>
        <td align="left">
        </td>
        <td align="left">
        </td>
        <td align="left">
            业务传真：
        </td>
        <td align="right">
            <asp:Literal runat="server" ID="LitSalesRepresentativeFAX" />
        </td>
    </tr>
</table>
<table width="900" border="0" cellspacing="3" cellpadding="0" align="center" 
    class="stylenormal">
    <tr>
        <td align="left" width="100">
            1.保固期间：
        </td>
        <td align="left">
            <asp:Literal runat="server" ID="LitExtendWarranty" Text="2 Years" />
        </td>
    </tr>
    <tr>
        <td align="left" valign="top">
            2.付款条件：
        </td>
        <td align="left">
            公司名称：北京研华兴业电子科技有限公司<br />
            帐    号：11001045300056024025<br />
            开 户 行：中国建设银行北京上地支行<br />
            电     话：010-62984346<br />
            传     真：010-62984346-6335	<br />		
        </td>
    </tr>
</table>
<%--<p style="page-break-before:always" />--%>
<table width="900" border="0" align="center">
    <tr>
        <td>
            <asp:GridView DataKeyNames="line_no" ID="gv1" runat="server" AllowPaging="false"
                EmptyDataText="no Item." AutoGenerateColumns="false" OnRowDataBound="gv1_RowDataBound"
                Font-Size="12pt" Width="100%" >
                <Columns>
                    <asp:BoundField DataField="DisplayLineNo" HeaderText="序号" ItemStyle-HorizontalAlign="center" ItemStyle-Width="70">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="VirtualPartNo" HeaderText="名称" ItemStyle-Width="150px" ItemStyle-HorizontalAlign="left"
                        ItemStyle-Wrap="false" >
                        <ItemStyle HorizontalAlign="Left" CssClass="stylenormal"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="VirtualPartNo" HeaderText="名称" Visible="false" ItemStyle-HorizontalAlign="left">
                        <ItemStyle HorizontalAlign="Left" CssClass="stylenormal"></ItemStyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="描述" ItemStyle-Width="310px">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lbdescription"  CssClass="stylenormal" Text='<%#Bind("description") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="80">
                        <HeaderTemplate>
                            数量
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%#Bind("DisplayQty")%>' ID="lbGVQty" />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="120">
                        <HeaderTemplate>
                            单价
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
                            总价
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
                <RowStyle Font-Names="Arial"/>
            </asp:GridView>
            <font class="stylenormal" style="font-weight: bold; color: #FF0000;">本价格含税、含运费（非空运），请勿向第三方透露。</font>
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
                                <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="20">註:</asp:TableCell>
                                <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Height="50" ID="Cell_ExternalNote"><%= _lbExternalNote%></asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </td>
                    <td align="right">
                        <strong>合计</strong>:
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
                        <strong>税 额(<asp:Label runat="server" ID="lbTaxRate"></asp:Label>)</strong>:
                    </td>
                    <td align="right">
                        <asp:Label runat="server" ID="tax"></asp:Label>
                    </td>
                </tr>
                <tr class="cartitem">
                    <td align="right">
                        <strong>含税合计</strong>:
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
            <table width="900">
                <tr>
                    <td width="730" class="stylenormal">
                    </td>
                    <td width="270" align="center" valign="top">
                        <font class="stylenormal" style="font-weight: bold;">
                        北京研华兴业电子科技有限公司<br />
                        <asp:Label runat="server" ID="lbSalesEngineerName" />
                        </font>
                        <%--電話 : <asp:Label runat="server" Visible="false" ID="lbSalesEngineerTEL" /><br />--%>
                    </td>
                </tr>
                <tr>
                    <td class="stylenormal">
                       发票要求（勾选）：1.增值发票   2.普通发票
                    </td>
                    <td class="stylenormal">
                        普通发票信息：
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
                        </td>
                        <td width="40%" class="stylenotice">
                            <font style="font-size: 12px; font-weight: bold; font-family: Arial;"><strong>WebSites
                            Information</strong>
                            <br />
                            1. Home Page :<a href="http://www.advantech.com.cn/">http://www.advantech.com.cn/</a><br />
                        </font>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
<br />

