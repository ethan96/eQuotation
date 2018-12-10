<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TestMing.aspx.vb" Inherits="EDOC.TestMing" %>



<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">


    <asp:Literal ID="Literal1" runat="server"></asp:Literal>
    <asp:GridView ID="GridView1" runat="server">
    </asp:GridView>
    <asp:Button ID="Button1" runat="server" Text="清空ITP" />

    <asp:Button ID="Button2" runat="server" Text="Button" />

    </form>
</body>
</html>
