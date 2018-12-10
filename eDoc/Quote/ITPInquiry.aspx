<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/myMaster.master" CodeBehind="ITPInquiry.aspx.vb" Inherits="EDOC.ITPInquiry" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <table width="100%">
        <tr><td><h2>AEU ITP Inquiry</h2></td></tr>
        <tr>
            <td>
                <asp:Panel runat="server" ID="PanelSearch" DefaultButton="btnSearch">
                    <table width="300px">
                        <tr>
                            <th align="left" style="color:Black">
                                Part No:
                            </th>
                            <td>
                                <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" TargetControlID="txtPN"
                                        ServicePath="~/Services/AutoComplete.asmx" ServiceMethod="GetPartNo" MinimumPrefixLength="2" CompletionInterval="200"/>
                                <asp:TextBox runat="server" ID="txtPN" Width="250px" />&nbsp;<asp:Button runat="server" ID="btnSearch" Text="Query" OnClick="btnSearch_Click" />
                            </td>
                        </tr>
                        <tr style="display:none">
                            <th align="left" style="color:Black">Currency:</th>
                            <td>
                                <asp:DropDownList runat="server" ID="dlCurr">
                                    <asp:ListItem Text="EUR" Selected="True" />
                                    <asp:ListItem Text="GBP" />
                                    <asp:ListItem Text="USD" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="height:20px">
                                <asp:UpdatePanel runat="server" ID="up2" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label runat="server" ID="lbMsg" ForeColor="Tomato" />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>                                
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td>
                <table width="100%">
                    <tr>
                        <td>
                            <asp:ImageButton runat="server" ID="btnXls" ImageUrl="~/Images/excel.gif" AlternateText="Download Excel" OnClick="btnXls_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:UpdatePanel runat="server" ID="up1" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:GridView runat="server" ID="gv1" Width="400px" AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Part No.">
                                                <ItemTemplate>
                                                    <a target="_blank" href='http://my.advantech.com/DM/ProductDashboard.aspx?PN=<%#Eval("part_no") %>'>
                                                        <%#Eval("part_no")%></a>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="Currency" DataField="CURRENCY" />
                                            <asp:BoundField HeaderText="ITP" DataField="ITP" />
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
