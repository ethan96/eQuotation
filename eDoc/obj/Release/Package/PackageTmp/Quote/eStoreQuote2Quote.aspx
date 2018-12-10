<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="eStoreQuote2Quote.aspx.vb" Inherits="EDOC.eStoreQuote2Quote" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">

    <asp:TextBox ID="TBquoteid" runat="server"></asp:TextBox>
    <asp:Button ID="BtCopy" runat="server" Text="Button" />
      <asp:GridView runat="server" ID="gv1">
    </asp:GridView>
    <asp:GridView runat="server" ID="gv2">
    </asp:GridView>
    <asp:GridView runat="server" ID="gv3">
    </asp:GridView>
    </form>
</body>
</html>
