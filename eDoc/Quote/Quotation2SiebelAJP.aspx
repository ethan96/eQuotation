<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/myMaster.master" CodeBehind="Quotation2SiebelAJP.aspx.vb" Inherits="EDOC.Quotation2SiebelAJP" %>

<%@ Register Src="~/Ascx/JPAOnlineQuoteTemplateV4.ascx" TagName="QuotationPreview" TagPrefix="myASCX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UP_QuotationPreview" runat="server" UpdateMode="Conditional" Visible="true">
        <ContentTemplate>
            <table style="width: 1024px" id="view_type_option" runat="server" align="center">
                <tr>
                    <td style="width: 80px" valign="middle">View Type：
                    </td>
                    <td style="width: 844px">
                        <asp:RadioButtonList ID="RadioButtonList_PriviewMode" runat="server" RepeatDirection="Horizontal"
                            OnSelectedIndexChanged="RadioButtonList_PriviewMode_SelectedIndexChanged" AutoPostBack="True"
                            Width="300px">
                            <asp:ListItem Selected="True" Value="true">Internal User</asp:ListItem>
                            <asp:ListItem Value="false">External User</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td valign="middle">
                        <asp:DropDownList ID="drpAJPOffice" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpAJPOffice_SelectedIndexChanged" Width="150px">
                            <asp:ListItem Value="0">-</asp:ListItem>
                            <asp:ListItem Value="1">Tokyo Office</asp:ListItem>
                            <asp:ListItem Value="2">Osaka Office</asp:ListItem>
                            <asp:ListItem Value="3">Nagoya Office</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td runat="server" id="tdpdf" align="left">
                        <asp:ImageButton ID="ImageBtPdf" runat="server" ImageUrl="~/Images/pdf.gif" OnClick="ImageBtPdf_Click" />
                    </td>
                </tr>
            </table>
            &nbsp;&nbsp;
            <table style="width: 1024px" align="center">
                <tr>
                    <td align="center" style="width: 600px; border: 5px solid">
                        <myASCX:QuotationPreview ID="ASCXQuote" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="height: 15px"></td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button runat="server" ID="btnConfirm" Text="Confirm" OnClick="btnConfirm_Click" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="RadioButtonList_PriviewMode" />
            <asp:PostBackTrigger ControlID="ImageBtPdf" />
        </Triggers>
    </asp:UpdatePanel>

</asp:Content>
