<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/myMaster.master" CodeBehind="CustomBTOS_ATW.aspx.vb" Inherits="EDOC.CustomBTOS_ATW" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:TreeView  runat="server"  ID="Tree1" OnSelectedNodeChanged="Tree1_SelectedNodeChanged" Width="101px" ImageSet="Arrows" Height="273px"  >
        <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />
        <NodeStyle Font-Names="Verdana"  Font-Size="12pt" ForeColor="Black" HorizontalPadding="0px" NodeSpacing="0px" VerticalPadding="0px" />
        <ParentNodeStyle Font-Bold="False"  />
        <SelectedNodeStyle Font-Underline="True" ForeColor="#5555DD" HorizontalPadding="0px" VerticalPadding="0px" />
    </asp:TreeView>


</asp:Content>
