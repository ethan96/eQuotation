<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Wall/Wall.Master" CodeBehind="WallHome.aspx.vb" Inherits="EDOC.WallHome" %>
<%@ Register Src="~/Wall/Module/CategoryTree.ascx" TagName="CategoryTree" TagPrefix="Aonline" %>
<%@ Register Src="~/Wall/Module/WallContent.ascx" TagName="WallContent" TagPrefix="Aonline" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMenu" runat="server">
    <Aonline:CategoryTree id="CategoryTree1" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="server">
    <Aonline:WallContent id="WallContent1" runat="server" />
</asp:Content>