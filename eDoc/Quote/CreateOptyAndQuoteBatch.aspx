<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/myMaster.master" CodeBehind="CreateOptyAndQuoteBatch.aspx.vb" Inherits="EDOC.CreateOptyAndQuoteBatch" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div>
    <asp:TextBox ID="txtQuoteId" runat="server"></asp:TextBox>
    <asp:Button ID="btnSingle" text = "Single" runat ="server" OnClick="btnSingle_Click" />
    <asp:Button ID="btnBatch" runat ="server" Text="Batch" OnClick="btnBatch_Click" />
    </div>
</asp:Content>
