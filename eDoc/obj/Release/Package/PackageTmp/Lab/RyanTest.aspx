<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RyanTest.aspx.vb" %>

<%@ Register Src="~/Ascx/JPAOnlineQuoteTemplateV3.ascx" TagName="AJP" TagPrefix="myASCX" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <myASCX:AJP ID="AJP" runat="server" />
        </div>
    </form>
</body>
</html>
