<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/myMaster.master" CodeBehind="OrderFromCombinedQuotation.aspx.vb" Inherits="EDOC.OrderFromCombinedQuotation" %>
<%@ Register Src="~/ascx/PickQuoteFinished.ascx" TagName="PickQuote" TagPrefix="myASCX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
        .sp td
        {
            text-align: center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <table class="sp">
        <tr>
            <td>
                <asp:Label runat="server" ID="Label19" Text="<%$ Resources:myRs,QuoteId %>"></asp:Label>
            </td>
            <td>
            </td>
            <td>
                <asp:Label runat="server" ID="Label1" Text="<%$ Resources:myRs,QuoteId %>"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="UPForm" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox runat="server" ID="txtRefId" Width="120px"></asp:TextBox>
                  
                <asp:ImageButton ID="ibtnPickQuote" runat="server" ImageUrl="~/Images/search.gif"
                    OnClick="ibtnPickQuote_Click" />
                      </ContentTemplate>
                </asp:UpdatePanel>
                <asp:Panel ID="PLPickQuote" runat="server" Style="display: none" CssClass="modalPopup">
                    <div style="text-align: right;">
                        <asp:ImageButton ID="CancelButtonQuote" runat="server" ImageUrl="~/Images/del.gif" />
                    </div>
                    <div>
                        <asp:UpdatePanel ID="UPPickQuote" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <myASCX:PickQuote ID="ascxPickQuote" runat="server" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </asp:Panel>
                <asp:LinkButton ID="lbDummyQuote" runat="server"></asp:LinkButton>
                <ajaxToolkit:ModalPopupExtender ID="MPPickQuote" runat="server" TargetControlID="lbDummyQuote"
                    PopupControlID="PLPickQuote" BackgroundCssClass="modalBackground" CancelControlID="CancelButtonQuote"
                    DropShadow="true" />
            </td>
            <td>
                <asp:Button ID="btnCombine" runat="server" Text="  >>  " OnClick="btnCombine_Click" /><br />
                <asp:Button ID="btnRemove" runat="server" Text="  <<  " OnClick="btnRemove_Click" />
            </td>
            <td>
                <asp:ListBox ID="lboxId" runat="server" Width="120px"></asp:ListBox>
            </td>
        </tr>
    </table>
    <table>
        <tr>
            <td align="center">
                <asp:Button ID="btnOrder" runat="server" Text="<%$ Resources:myRs,Order %>" OnClick="btnOrder_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
