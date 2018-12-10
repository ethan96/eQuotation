<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="VersionInfo.aspx.vb" Inherits="EDOC.VersionInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
<form runat="server">
<li>Port：<asp:Label ID="lbport" runat="server" /></li>
<li>IsTesting：<asp:Label ID="lbIsTesting" runat="server" /></li>
<li>eQ DB：<asp:Label ID="lbeQDB" runat="server" /></li>
<li>Publish Path：<asp:Label ID="lbPubPath" runat="server" /></li>
<li>Role.IsUsaUser：<asp:Label ID="RoleIsUSAonlineSales" runat="server" /></li>
<li>Role.IsUSAACSales：<asp:Label ID="RoleIsUSAACSales" runat="server" /></li>
<li>Util.GetRuntimeSiteUrl()：<asp:Label ID="GetRuntimeSiteUrl" runat="server" /></li>
 <img src='<%=Util.GetRuntimeSiteUrl() %>/Images/Advantech logo.jpg' alt="Advantech eStore" />
 <img src='/Images/Advantech logo.jpg' alt="Advantech eStore" />
 <br>
    <asp:Button ID="Button1" runat="server" Text="Button" />
    </form>
</body>
</html>
