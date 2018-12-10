<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/myMaster.master" CodeBehind="rGPControl.aspx.vb" Inherits="EDOC.rGPControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table>
<%--        <tr>
            <td style="width:25%">
                <asp:Label runat="server" Text="Year: (yyyy, ex: 2018)"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtYear" runat="server" MaxLength="4"></asp:TextBox>
                <ajaxToolkit:FilteredTextBoxExtender runat="server" ID="ft3" TargetControlID="txtYear" FilterType="Numbers, Custom" />
            </td>
        </tr>--%>
        <tr>
            <td style="width:25%">
                <asp:Label runat="server" Text="Start Date: (yyyyMMdd, ex: 20180101)"></asp:Label>
            </td>
            <td>
               <asp:TextBox ID="txtStartDate" runat="server" MaxLength="8"></asp:TextBox>
               <ajaxToolkit:FilteredTextBoxExtender runat="server" ID="FilteredTextBoxExtender1" TargetControlID="txtStartDate" FilterType="Numbers, Custom" />
            </td>
        </tr>
                <tr>
            <td>
                <asp:Label runat="server" Text="End Date: (yyyyMMdd, ex: 20201231)"></asp:Label>
            </td>
            <td>
               <asp:TextBox ID="txtEndDate" runat="server" MaxLength="8"></asp:TextBox>
               <ajaxToolkit:FilteredTextBoxExtender runat="server" ID="FilteredTextBoxExtender2" TargetControlID="txtEndDate" FilterType="Numbers, Custom" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:ImageButton runat="server" ID="imgXls" ImageUrl="~/Images/excel.gif" AlternateText="Download" OnClick="imgXls_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
