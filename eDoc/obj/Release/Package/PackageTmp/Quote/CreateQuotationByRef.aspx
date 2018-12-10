<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/myMaster.master" CodeBehind="CreateQuotationByRef.aspx.vb" Inherits="EDOC.CreateQuotationByRef" %>

<%@ Register Src="~/ascx/PickQuoteCopy.ascx" TagName="PickQuoteCopy" TagPrefix="myASCX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UPForm" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table>
                <tr>
                    <td>
                        <span style="color: #ff0000">*</span>
                        <asp:Label runat="server" ID="Label19" Text="<%$ Resources:myRs,CopyFromId %>"></asp:Label>
                        :
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtRefId"></asp:TextBox>
                        <asp:ImageButton ID="ibtnPickQuoteCopy" runat="server" ImageUrl="~/Images/search.gif"
                            OnClick="ibtnPickQuoteCopy_Click" />
                        <asp:Panel ID="PLPickQuoteCopy" runat="server" Style="display: none" CssClass="modalPopup">
                            <div style="text-align: right;">
                                <asp:ImageButton ID="CancelButtonQuoteCopy" runat="server" ImageUrl="~/Images/del.gif" />
                            </div>
                            <div>
                                <asp:UpdatePanel ID="UPPickQuoteCopy" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <myASCX:PickQuoteCopy ID="ascxPickQuoteCopy" runat="server" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </asp:Panel>
                        <asp:LinkButton ID="lbDummyQuoteCopy" runat="server"></asp:LinkButton>
                        <ajaxToolkit:ModalPopupExtender ID="MPPickQuoteCopy" runat="server" TargetControlID="lbDummyQuoteCopy"
                            PopupControlID="PLPickQuoteCopy" BackgroundCssClass="modalBackground" CancelControlID="CancelButtonQuoteCopy"
                            DropShadow="true" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <table>
        <tr>
            <td align="center">
                <asp:Button ID="btnConfirm" runat="server" Text="<%$ Resources:myRs,Confirm %>" OnClick="btnConfirm_Click" OnClientClick="return EUCopyPurpose();" />
            </td>
        </tr>
    </table>
    <div style="display: none">
        <div id="divCopyPurpose" style="width: 100%; height: 90%;">
            <div style="height: 80%; width: 100%; text-align: center;">
                <h2 style="color: #1b1b69; text-align: center;">Please select your copy purpose.</h2>
                <asp:RadioButtonList ID="CopyPurpose" runat="server" Style="margin-left: 25%; width: 75%">
                    <asp:ListItem Value="1" Selected="True">Create New Quotation/Opty</asp:ListItem>
                    <asp:ListItem Value="2">Revise Quotation</asp:ListItem>
                </asp:RadioButtonList>
            </div>
            <div style="height: 20%; text-align: center">
                <asp:Button runat="server" ID="btnCopyPurpose" Text="Submit" OnClick="btnConfirm_Click" />
            </div>
        </div>
    </div>

    <link rel="Stylesheet" href="../Js/FancyBox/jquery.fancybox.css" type="text/css" />
    <script type="text/javascript" src="../Js/FancyBox/jquery.fancybox.js"></script>
    <script type="text/javascript">
        function EUCopyPurpose(node) {
        <% If _IsEUUser Then%>

            if ($('#<%=txtRefId.ClientID%>').val() == "") {                
                alert("Please input a quote number first.");
                return false;
            }

            var gallery = [{
                href: "#divCopyPurpose"
            }];

            $.fancybox(gallery, {
                'autoSize': false,
                'width': 350,
                'height': 200,
                'autoCenter': true
            });
            return false;
                <% Else%>
            return true;
                <% End If%>
        }
    </script>
</asp:Content>

