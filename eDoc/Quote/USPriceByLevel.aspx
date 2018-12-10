<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/myMaster.master" CodeBehind="USPriceByLevel.aspx.vb" Inherits="EDOC.USPriceByLevel" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <table width="100%">
        <tr>
            <td>
                <asp:Panel runat="server" ID="Panel1" DefaultButton="btnQuery">
                    <table>
                        <tr>
                            <th align="left" style="color:Black">
                                Part No:
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtPN" />
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btnQuery" Text="Search" OnClick="btnQuery_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:GridView runat="server" ID="gv1" Width="100%">
                    <Columns>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
</asp:Content>
