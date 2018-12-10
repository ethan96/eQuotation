<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/myMaster.master" CodeBehind="QuoteApproval.aspx.vb" Inherits="EDOC.QuoteApproval" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div runat="server" id="divContent">
    </div>
    <table>
        <tr>
            <td align="center">
                <asp:Button runat="server" ID="btnProcess" Text="Process" Enabled="false" OnClick="btnProcess_Click" />
            </td>
        </tr>
    </table>
    <asp:Panel ID="PLProcess" runat="server" Style="display: none" CssClass="modalPopup">
        <div style="text-align: right;">
            <asp:ImageButton ID="ibtnCProcess" runat="server" ImageUrl="~/Images/del.gif" />
        </div>
        <div>
            <asp:UpdatePanel ID="UPProcess" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <table>
                        <tr>
                            <td>
                                <b>Comment :</b>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtComment" runat="server" TextMode="MultiLine" Width="200px" Height="80px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <table width="100%">
                        <tr>
                            <td align="center">
                                <asp:Button ID="btnApprove" runat="server" Text="Approve" OnClick="btnApprove_Click" /> | <asp:Button ID="btnReject" runat="server" Text="Reject" OnClick="btnReject_Click" />
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </asp:Panel>
    <asp:LinkButton ID="lbtnProcess" runat="server"></asp:LinkButton>
    <ajaxToolkit:ModalPopupExtender ID="MPProcess" runat="server" TargetControlID="lbtnProcess"
        PopupControlID="PLProcess" BackgroundCssClass="modalBackground" CancelControlID="ibtnCProcess"
        DropShadow="true" />
</asp:Content>
