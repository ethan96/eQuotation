<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/myMaster.master" CodeBehind="ChangeUser.aspx.vb" Inherits="EDOC.ChangeUser" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:TextBox ID="txtUser" runat="server"></asp:TextBox><asp:Button ID="btnChange"
        runat="server" Text="Change" OnClick="btnChange_Click" />
</asp:Content>
