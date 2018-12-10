<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UpdateQuotePrintFormat.aspx.vb" Inherits="EDOC.UpdateQuotePrintFormat" %>
<%@ Register Src="~/ascx/QuotationViewOption.ascx" TagName="QuotationViewOptionUC"
    TagPrefix="myASCX" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css"> 
     .disable 
    { 
        border-style:none; 
        border-width: thin; 
        background-color:Transparent; 
        color: #000000; 
        font-size:16px;
        cursor:wait; 
    } 
    </style> 
</head>

<body>

    <script type="text/javascript" language="javascript">
        function winSizer() {
            windowWidth = window.screen.availWidth;
            windowHeight = window.screen.availHeight;
            window.moveTo(0, 0);
            window.resizeTo(windowWidth, windowHeight);
        }

        function DisableButton() {
            document.getElementById('<%=Me.btnUpdateFormatOption.ClientId %>').className = "disable";
            document.getElementById('<%=Me.btnUpdateFormatOption.ClientId %>').value = 'waiting...';
            document.getElementById('<%=Me.btnUpdateFormatOption.ClientId %>').onclick = Function("return false;");
            document.body.style.cursor = "wait"
            IniLimg();
            winSizer();
            return true;
        }


        function ATWDownload() {
            IniLimg();
            return true;
        }

        function CleanLimg() {
            var imgID = new Image();
            imgID.src = "";
            imgID.onload = function () {
                document.getElementById("limg").innerHTML = "";
            }
        }


        function IniLimg() {
            var imgID = new Image();
            imgID.src = "/Images/LoadingRed.gif";
            imgID.onload = function () {
                document.getElementById("limg").innerHTML = "<img src=" + imgID.src + " />";
            }
        }
    </script> 
    <form id="form1" runat="server">
    <asp:HiddenField runat="server" ID="hdQuoteId" />
    <asp:ScriptManager runat="server" ID="sm1" />
    <div>
        <table runat="server" id="tbFormatSetting" visible="false">
            <tr>
                <td>
                    Update Print Out Format for
                    <asp:Label runat="server" ID="lbQuoteId" Font-Bold="true" />
                </td>
            </tr>
            <tr>
                <td>
                    <myASCX:QuotationViewOptionUC ID="QuotationViewOptionUC1" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button runat="server" ID="btnUpdateFormatOption" Text="Create PDF" OnClick="btnUpdateFormatOption_Click" />
                </td>
            </tr>
        </table>
       <div id="limg" runat="server" style="width:100%; text-align:center"></div>
    </div>
    </form>
</body>
</html>

