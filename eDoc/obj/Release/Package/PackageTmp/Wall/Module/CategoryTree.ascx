<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CategoryTree.ascx.vb" Inherits="EDOC.CategoryTree" %>
<div class="treeContainer clear">
    <asp:TreeView ID="tvCategory" runat="server"  NodeWrap="True"  ImageSet="Arrows">
        <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />
        <NodeStyle Font-Size="10pt" ForeColor="Black" HorizontalPadding="4px" NodeSpacing="0px" Width="100%"
            VerticalPadding="0px" />
        <ParentNodeStyle Font-Bold="False" />
        <SelectedNodeStyle ForeColor="#FF8000" HorizontalPadding="0px" VerticalPadding="0px" />
    </asp:TreeView>
</div>