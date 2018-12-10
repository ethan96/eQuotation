<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SiebelJob.aspx.vb" Inherits="EDOC.SiebelJob1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="StartJob" runat="server" Text="Start Siebel job" />&nbsp;&nbsp;
        <asp:Button ID="StopJob" runat="server" Text="Stop Siebel job" /> <br />
        <asp:Label ID="Msg" runat="server" Text="" ForeColor="Tomato"></asp:Label>
    </div>
    </form>
</body>
</html>
