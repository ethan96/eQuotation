<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ProductReport.aspx.vb" Inherits="EDOC.ProductReport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:TextBox ID="TBpartno" runat="server"></asp:TextBox>
        <asp:Button ID="Button1" runat="server" Text="Search" />
        <asp:GridView ID="GridView1" runat="server"></asp:GridView>


    </form>
</body>
</html>
