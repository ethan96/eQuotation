<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/myMaster.master"
    CodeBehind="OrderPreview.aspx.vb" Inherits="EDOC.OrderPreview" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css" id="Sty" runat="server">
        .mytable table
        {
            width: 100%;
            border-collapse: collapse;
        }
        
        .mytable tr td
        {
            background: #ffffff;
          
            padding: 2px;
            font-family: Arial;
            font-size: 12px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<center>
<div style="width:1000px">
    <table width="100%">
        <tr>
            <td align="left">
                <asp:Label runat="server" ID="lbThanks"></asp:Label>
                <br />
                <%= getMassage()%>
            </td>
            <td align="right">
                <table style="width:auto">
                    <tr>
                        <td>
                            <a href="#" onclick="DoPrint()">Print</a>
                        </td>
                        <td>
                            |
                        </td>
                        <td>
                            <asp:HyperLink runat="server" ID="hlHome" Text="Home" NavigateUrl="~/home.aspx"></asp:HyperLink>
                        </td>
                        <td>
                            |
                        </td>
                        <td>
                            <asp:HyperLink runat="server" ID="hlNew" Text="New Quote" NavigateUrl="~/Quote/QuotationMaster.aspx"></asp:HyperLink>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <asp:Label runat="server" ID="lb_Cust" CssClass="mytable"></asp:Label>
    <asp:Label runat="server" ID="lb_Order" CssClass="mytable"></asp:Label>
    <asp:Label runat="server" ID="lb_Detail" CssClass="mytable"></asp:Label>
   
    <div id="warndiv" style="font-size: 12px; color: #FF0000">
    </div>
    <table width="100%">
        <tr>
            <td align="center">
                <asp:Button runat="server" ID="btnOrder" Text=" >> Confirm Order << " Visible="false"
                    OnClick="btnOrder_Click" />
            </td>
        </tr>
    </table>
    </div>
    <script type="text/javascript">


        function DoPrint() {
            var obj0 = document.getElementById('<%=Me.Sty.ClientID%>');
            var obj1 = document.getElementById('<%=Me.lb_Cust.ClientID%>');
            var obj2 = document.getElementById('<%=Me.lb_Order.ClientID%>');
            var obj3 = document.getElementById('<%=Me.lb_Detail.ClientID%>');

            var text0 = obj0.outerHTML;
            var text1 = obj1.innerHTML;
            var text2 = obj2.innerHTML;
            var text3 = obj3.innerHTML;
            document.open();
            document.write("");
            document.write(text0 + text1 + text2 + text3);
            document.close();
            print();
            window.location.href = window.location.href;
        }

    </script>
    </center>
</asp:Content>
