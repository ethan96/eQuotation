<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/myMaster.master" CodeBehind="CustomBtos.aspx.vb" Inherits="EDOC.CustomBtos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <style type="text/css">
        .tree table
        {
            width: auto;
        }
        .tree td
        {
            border: 0px;
            padding: 2px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:TreeView runat="server" ID="treeBTOSList" CssClass="tree" ExpandDepth="0" OnSelectedNodeChanged="treeBTOSList_SelectedNodeChanged">
</asp:TreeView>

    <asp:Panel ID="PLA" runat="server" Style="display: none" CssClass="modalPopup">
        <div style="text-align: right;">
            <asp:ImageButton ID="CancelButtonA" runat="server" ImageUrl="~/Images/del.gif" />
        </div>
        <div>
            <asp:UpdatePanel ID="UPA" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                  <B>BTOS Item : </B><asp:label runat="server" id="lbItem"></asp:label>
                  <table><tr><td align="center"><asp:Button ID="btnConfig" runat="server" Text="Configure" OnClick="btnConfig_Click" /></td></tr></table>
                  <div id="divC" runat="server"></div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </asp:Panel>
    <asp:LinkButton ID="lbDummyA" runat="server"></asp:LinkButton>
    <ajaxToolkit:ModalPopupExtender ID="MPA" runat="server" TargetControlID="lbDummyA"
        PopupControlID="PLA" BackgroundCssClass="modalBackground" CancelControlID="CancelButtonA"
        DropShadow="false" />
</asp:Content>
