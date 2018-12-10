<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/myMaster.master" CodeBehind="MarginReport.aspx.vb" Inherits="EDOC.MarginReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ImageButton runat="server" ID="imgDownload" ImageUrl="~/Images/excel.gif" AlternateText="Download" OnClick="imgDownload_Click"/>
</asp:Content>
