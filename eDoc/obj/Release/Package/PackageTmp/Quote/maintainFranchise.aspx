<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/myMaster.master" CodeBehind="maintainFranchise.aspx.vb" Inherits="EDOC.maintainFranchise" %>
<%@ Register Src="~/ascx/PickAccount.ascx" TagName="PickAccount" TagPrefix="myASCX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:UpdatePanel runat="server" ID="upF" UpdateMode="Conditional">
<ContentTemplate>
    <asp:Panel runat="server" ID="plForm">
    </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>

<table><tr><td align="center">
<asp:Button runat="server" ID="btnAddEdit" Text="Add/Edit" OnClick="btnAddEdit_Click" /><asp:Label runat="server" ID="lbMsg" ForeColor="Red"></asp:Label>
</td></tr></table>
    <asp:GridView ID="gv" runat="server" AutoGenerateColumns="true" EmptyDataText="No raw data be found."
        OnRowDataBound="gv_RowDataBound">
    </asp:GridView>
    <asp:Panel ID="PLps" runat="server" Style="display: none" CssClass="modalPopup">
        <div style="text-align: right;">
            <asp:ImageButton ID="Cps" runat="server" ImageUrl="~/Images/del.gif" />
        </div>
        <div>
            <asp:UpdatePanel ID="UPps" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <table>
                        <tr>
                            <td colspan="2">
                                System will automatically create a login account for eMail '<asp:Label runat="server"
                                    ID="lbEmail"></asp:Label>' , please input the initial login password below:
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Password:
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtPs"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Button runat="server" ID="btnConfirm" Text="Confirm" OnClick="btnConfirm_Click" />
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </asp:Panel>
    <asp:LinkButton ID="LBps" runat="server"></asp:LinkButton>
    <ajaxToolkit:ModalPopupExtender ID="MPps" runat="server" TargetControlID="LBps" PopupControlID="PLps"
        BackgroundCssClass="modalBackground" CancelControlID="Cps" DropShadow="true" />
    <asp:Panel ID="PLPickAccount" runat="server" Style="display: none;" CssClass="modalPopup">
        <div style="text-align: right;">
            <asp:ImageButton ID="CancelButtonAccount" runat="server" ImageUrl="~/Images/del.gif" />
        </div>
        <div>
            <asp:UpdatePanel ID="UPPickAccount" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <myascx:pickaccount id="ascxPickAccount" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </asp:Panel>
    <asp:LinkButton ID="lbDummyAccount" runat="server" />
    <ajaxToolkit:ModalPopupExtender ID="MPPickAccount" runat="server" TargetControlID="lbDummyAccount"
        PopupControlID="PLPickAccount" BackgroundCssClass="modalBackground" CancelControlID="CancelButtonAccount" />
</asp:Content>
