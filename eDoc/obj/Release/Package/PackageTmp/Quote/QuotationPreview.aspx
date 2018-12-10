<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/myMaster.master" CodeBehind="QuotationPreview.aspx.vb" Inherits="EDOC.QuotationPreview" %>
<%@ Register Src="~/ascx/CheckUIDisVisiable.ascx" TagName="CheckUID" TagPrefix="myASCX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:UpdatePanel ID="UP_QuotationPreview" runat="server" UpdateMode="Conditional"
        Visible="true">
        <ContentTemplate>
            <table width="300px" id="view_type_option" runat="server">
                <tr>
                    <td width="10%" align="center">
                        <asp:Label ID="Label1" runat="server" Text="View Type:"></asp:Label>
                    </td>
                    <td width="90%">
                        <asp:RadioButtonList ID="RadioButtonList_PriviewMode" runat="server" RepeatDirection="Horizontal"
                            OnSelectedIndexChanged="RadioButtonList_PriviewMode_SelectedIndexChanged" AutoPostBack="True"
                            Width="300px">
                            <asp:ListItem Value="true">Internal User</asp:ListItem>
                            <asp:ListItem Selected="True" Value="false">External User</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
            </table>
            <myASCX:CheckUID runat="server" ID="ascxCheckUID" />
            <div runat="server" id="divContent">
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="RadioButtonList_PriviewMode" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
