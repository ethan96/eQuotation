<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Order2Quote.aspx.vb" Inherits="EDOC.Order2Quote" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    SoNo:<asp:TextBox runat="server" ID="TBsono"></asp:TextBox>
    <asp:Button runat="server" Text="Get" ID="BT1" OnClick="BT1_Click" />
    <asp:GridView runat="server" ID="gv1">
    </asp:GridView>
    <asp:GridView runat="server" ID="gv2">
    </asp:GridView>
    <asp:GridView runat="server" ID="gv3">
    </asp:GridView>
    </form>
</body>
</html>