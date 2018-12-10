<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="QuoteMaster.ascx.vb"
    Inherits="EDOC.QuoteMaster" %>
<style type="text/css">
    body
    {
        font-family: Calibri;
    }
    .QMtext1
    {
        color: #004B9C;
        font-family: Calibri;
        vertical-align: top;
    }
    .QMtext2
    {
        color: #000000;
        font-family: Calibri;
        font-weight: bold;
        vertical-align: top;
    }
    .QMtext3
    {
        font-size: 18px;
        color: #004B9C;
        font-family: Calibri;
        vertical-align: top;
    }
    .QMtext4
    {
        font-size: 24px;
        color: #004A84;
        font-family: Calibri;
        vertical-align: top;
        font-weight: bold;
    }
</style>
<table style="width: 675px;" align="center" border="0" cellpadding="0" cellspacing="0">
    <tr>
<%--        <td align="center" style="background-position: left top; width:675px; height:146px;
            background-image: url('<%=Util.GetRuntimeSiteIP()%>/Images/heard.jpg'); background-repeat: no-repeat;">--%>
                <td align="center" style="background-position: left top; width:675px; height:146px;
            background-image: url('<%=Util.GetRuntimeSiteIP()%>/Images/<%=logoImg%>'); background-repeat: no-repeat;">
        <%--    <table style="width:100%;height:110px;" align="left" border="0" cellpadding="0" cellspacing="0" >
                <tr>
                    <td height="20px" width="20px">
                    </td>
                    <td>
                    </td>
                </tr>
                 <tr>
                    <td>
                    </td>
                    <td class="QMtext4" height="30px">
                       Advantech Quotation
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td class="QMtext3">
                       For:  <asp:Literal ID="litsoldtocompany" runat="server" />
                    </td>
                </tr>
            </table>--%>
        </td>
    </tr>
    <tr>
        <td height="20">
        </td>
    </tr>
    <tr>
        <td>
            <table style="width: 675px;" align="center">
                <tr>
                    <td class="QMtext1" width="40%">
                        Quotation Number:
                    </td>
                    <td class="QMtext1" width="35%">
                        Customer Number:
                    </td>
                    <td class="QMtext1" width="25%">
                        Expired Date:
                    </td>
                </tr>
                <tr>
                    <td class="QMtext2">
                        <asp:Literal ID="LitQuoteNo" runat="server" />
                    </td>
                    <td class="QMtext2">
                        <asp:Literal ID="LitCustomerNo" runat="server" />
                    </td>
                    <td class="QMtext2">
                        <asp:Literal ID="LitExpiredDate" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="QMtext1">
                        Created Date:
                    </td>
                    <td class="QMtext1">
                        Customer Name:
                    </td>
                    <td class="QMtext1">
                        Payment Terms:
                    </td>
                </tr>
                <tr>
                    <td class="QMtext2">
                        <asp:Literal ID="LitCreatedDate" runat="server" />
                    </td>
                    <td class="QMtext2">
                        <asp:Literal ID="LitCustomerName" runat="server" />
                    </td>
                    <td class="QMtext2">
                        <asp:Literal ID="LitPaymentTerms" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="QMtext1">
                        PO Number:
                    </td>
                    <td class="QMtext1">
                        Customer ERP ID:
                    </td>
                    <td class="QMtext1">
                        Shipping Method:
                    </td>
                </tr>
                <tr>
                    <td class="QMtext2">
                        <asp:Literal ID="LitPONumber" runat="server" />
                    </td>
                    <td class="QMtext2">
                        <asp:Literal ID="LitERPID" runat="server" />
                    </td>
                    <td class="QMtext2">
                        <asp:Literal ID="LitShippingMethod" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="QMtext1">
                        Account Manager:
                    </td>
                    <td class="QMtext1">
                        Customer Address:
                    </td>
                    <td class="QMtext1" valign="top">
                    </td>
                </tr>
                <tr>
                    <td class="QMtext2"> 
                        <asp:Literal ID="LitAccountManager" runat="server" />
                        <asp:Literal ID="LitAccountManagerTel" runat="server" />
                    </td>
                    <td class="QMtext2">
                        <asp:Literal ID="litsoldtoaddress" runat="server" />
                    </td>
                    <td class="QMtext2" valign="top">
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td height="20" align="center">
            <img src="<%=Util.GetRuntimeSiteIP()%>/Images/AEUline.gif" />
        </td>
    </tr>
    <tr>
        <td>
            <table width="675" align="center">
                <tr>
                    <td width="50%" class="QMtext1">
                        Billing Address:
                    </td>
                    <td class="QMtext1">
                        Shipping Address:
                    </td>
                </tr>
                <tr>
                    <td width="50%" class="QMtext2">
                        <asp:Literal ID="litBilltocompany" runat="server" /><br />
                        <asp:Literal ID="litBilltoaddress" runat="server" />
                    </td>
                    <td class="QMtext2">
                        <asp:Literal ID="litshiptocompany" runat="server" /><br />
                        <asp:Literal ID="litshiptoaddress" runat="server" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td height="20" align="center">
             <img src="<%=Util.GetRuntimeSiteIP()%>/Images/AEUline.gif" />
        </td>
    </tr>
    <tr>
        <td height="20">
        </td>
    </tr>
    <tr>
        <td style="width: 675px;">
        <table style="width: 675px;">
        <tr><td><%--word-break:break-all;--%>
            <div style="word-wrap:break-word;width:672px;margin:0 auto; overflow: hidden;">
            <asp:Literal ID="LitGreeting" runat="server" /></div>
            <br />
            <br />
            <br />
           <%-- Yours sincerely,--%>
            <asp:Literal ID="LitSales" runat="server" Visible="false" />
            <asp:Image ID="ImgSignature" runat="server"   />
        </td></tr>
        </table>
          
        </td>
    </tr>
</table>
