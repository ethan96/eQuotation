<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/myMaster.master" CodeBehind="Catalog.aspx.vb" Inherits="EDOC.Catalog" %>
<%@ Register Src="~/ascx/CheckUID.ascx" TagName="CheckUID" TagPrefix="myASCX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<myASCX:CheckUID runat="server" ID="ascxCheckUID" />
    <asp:Panel runat="server" ID="p_catalog">
    </asp:Panel>
</asp:Content>
