<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/myMaster.master" CodeBehind="ReConfigureCTOSCheck.aspx.vb" Inherits="EDOC.ReConfigureCTOSCheck" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField runat="server" ID="hdReconfigId" />
    <asp:HiddenField runat="server" ID="hdCompRadioName" />
    <table width="100%">
        <tr>
            <td>
                Component:&nbsp;<asp:Label runat="server" ID="lbObsoleteItem" Font-Bold="true" />&nbsp;is phased out. <br />
                Please pick an alternative item from category:<asp:Label runat="server" ID="lbCatName" Font-Bold="true" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel runat="server" ID="up1" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table>
                            <tr valign="top">
                                <td colspan="2">
                                    <asp:ListBox runat="server" ID="lBoxAltItems" SelectionMode="Single" AutoPostBack="true"
                                        Width="400px" OnSelectedIndexChanged="lBoxAltItems_SelectedIndexChanged" DataTextField="category_id"
                                        DataValueField="category_id" />
                                </td>
                            </tr>
                            <tr>
                                <th align="left">
                                    Price:
                                </th>
                                <td>
                                    <asp:Label runat="server" ID="lbSelAltPrice" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td><asp:Button runat="server" ID="btnConfirm" Text="Confirm" OnClick="btnConfirm_Click" /></td>
        </tr>
        <tr>
            <td><asp:Label runat="server" ID="lbMsg" ForeColor="Tomato" Font-Bold="true" /></td>
        </tr>
    </table>
</asp:Content>
