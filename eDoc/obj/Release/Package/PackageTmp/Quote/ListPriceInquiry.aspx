<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/myMaster.master" CodeBehind="ListPriceInquiry.aspx.vb" Inherits="EDOC.ListPriceInquiry" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table width="100%">
        <tr>
            <td>
                <h2>
                    List Price Inquiry</h2>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel runat="server" ID="PanelSearch" DefaultButton="btnSearch">
                    <table width="300px">
                        <tr>
                            <th align="left" style="color: Black">
                                Part No:
                            </th>
                            <td>
                                <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" TargetControlID="txtPN"
                                    ServicePath="~/Services/AutoComplete.asmx" ServiceMethod="GetPartNo" MinimumPrefixLength="2"
                                    CompletionInterval="200" />
                                <asp:TextBox runat="server" ID="txtPN" Width="250px" />&nbsp;<asp:Button runat="server"
                                    ID="btnSearch" Text="Query" OnClick="btnSearch_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel runat="server" ID="upOrgCurcyDivision" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="200px">
                                            <tr align="left">
                                                <th align="left" style="color: Black; width: 50px">
                                                    Org:
                                                </th>
                                                <td style="width: 100px">
                                                    <asp:DropDownList runat="server" ID="dlOrg" AutoPostBack="true" OnSelectedIndexChanged="dlOrg_SelectedIndexChanged">
                                                        <asp:ListItem Value="TW01" />
                                                        <asp:ListItem Value="AU01" />
                                                        <asp:ListItem Value="KR01" />
                                                        <asp:ListItem Value="CN10" />
                                                        <asp:ListItem Value="EU10" Selected="True" />
                                                        <asp:ListItem Value="JP01" />
                                                        <asp:ListItem Value="SG01" />
                                                        <asp:ListItem Value="US01" />
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <th align="left" style="color: Black; width: 50px">
                                                    Currency:
                                                </th>
                                                <td style="width: 100px">
                                                    <asp:DropDownList runat="server" ID="dlCurr" />
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <th align="left" runat="server" id="thDivision" visible="false" style="color: Black;
                                                    width: 50px">
                                                    Division:
                                                </th>
                                                <td runat="server" id="tdDivision" visible="false" style="width: 100px">
                                                    <asp:DropDownList runat="server" ID="dlDivision">
                                                        <asp:ListItem Value="10" Selected="True" />
                                                        <asp:ListItem Value="20" />
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="height: 20px">
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
                            <asp:ImageButton runat="server" ID="btnXls" ImageUrl="~/Images/excel.gif" AlternateText="Download Excel"
                                OnClick="btnXls_Click" />
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
                                            <asp:BoundField HeaderText="List Price" DataField="LIST_PRICE" />
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
