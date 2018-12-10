<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/myMaster.master" CodeBehind="ChartOver_old.aspx.vb" Inherits="EDOC.ChartOver_old" %>
<%@ Register src="../Ascx/NVCHART.ascx" tagname="NVCHART" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table>
        <tr>
            <td>
                <table style="width: auto">
                    <tr>
                      <td>
                           Year:
                        </td>
                        <td>
                           <asp:DropDownList runat="server" ID="drpYear">
                          <%--  <asp:ListItem Text="2012" Value="2012"></asp:ListItem>
                            <asp:ListItem Text="2013" Value="2013" Selected="True"></asp:ListItem>--%>
                            </asp:DropDownList>
                        </td>
                         <td>
                          Month From:
                        </td>
                        <td>
                           <asp:DropDownList runat="server" ID="drpMF">
                            <asp:ListItem Text="1" Value="1"></asp:ListItem>
                            <asp:ListItem Text="2" Value="2"></asp:ListItem>
                            <asp:ListItem Text="3" Value="3"></asp:ListItem>
                            <asp:ListItem Text="4" Value="4"></asp:ListItem>
                            <asp:ListItem Text="5" Value="5"></asp:ListItem>
                            <asp:ListItem Text="6" Value="6"></asp:ListItem>
                            <asp:ListItem Text="7" Value="7"></asp:ListItem>
                            <asp:ListItem Text="8" Value="8"></asp:ListItem>
                            <asp:ListItem Text="9" Value="9"></asp:ListItem>
                            <asp:ListItem Text="10" Value="10"></asp:ListItem>
                            <asp:ListItem Text="11" Value="11"></asp:ListItem>
                            <asp:ListItem Text="12" Value="12"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                           Month To:
                        </td>
                        <td>
                          <asp:DropDownList runat="server" ID="drpMT">
                            <asp:ListItem Text="1" Value="1"></asp:ListItem>
                            <asp:ListItem Text="2" Value="2"></asp:ListItem>
                            <asp:ListItem Text="3" Value="3"></asp:ListItem>
                            <asp:ListItem Text="4" Value="4"></asp:ListItem>
                            <asp:ListItem Text="5" Value="5"></asp:ListItem>
                            <asp:ListItem Text="6" Value="6"></asp:ListItem>
                            <asp:ListItem Text="7" Value="7"></asp:ListItem>
                            <asp:ListItem Text="8" Value="8"></asp:ListItem>
                            <asp:ListItem Text="9" Value="9"></asp:ListItem>
                            <asp:ListItem Text="10" Value="10"></asp:ListItem>
                            <asp:ListItem Text="11" Value="11"></asp:ListItem>
                            <asp:ListItem Text="12" Value="12"></asp:ListItem></asp:DropDownList>
                        </td>
                        <td>
                            Org:
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="drpOrg" AutoPostBack="true">
                            <asp:ListItem Text="US01" Value="US01"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            RBU:
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="drpRBU">
                            </asp:DropDownList>
                        </td>
                       
                        <td><asp:Button runat="server" ID="btnQuery" Text="Query" OnClick="btnQuery_Click" /></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr><td align="right"><uc1:NVCHART ID="NVCHART1" runat="server" /></td></tr>
    </table>
<asp:ImageButton runat="server" ID="imgXls" ImageUrl="~/Images/excel.gif" AlternateText="Download" OnClick="imgXls_Click" />
  <asp:Panel runat="server" ID="plC">
  
  </asp:Panel>
</asp:Content>
