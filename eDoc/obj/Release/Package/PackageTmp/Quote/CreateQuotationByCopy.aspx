<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/myMaster.master"
    CodeBehind="CreateQuotationByCopy.aspx.vb" Inherits="EDOC.CreateQuotationByCopy" %>

<%@ Register Src="~/ascx/PickQuoteCopy.ascx" TagName="PickQuoteCopy" TagPrefix="myASCX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table>
        <tr>
            <td width="134">
                Copy From:
            </td>
            <td colspan="5">
                <asp:RadioButtonList ID="Qtype" runat="server" RepeatDirection="Horizontal" Width="450"
                    Font-Bold="True" AutoPostBack="True" OnSelectedIndexChanged="Qtype_SelectedIndexChanged">
                    <asp:ListItem Value="1" Selected="True">SAP Order</asp:ListItem>
                    <asp:ListItem Value="0">SAP Quotation</asp:ListItem>
                    <asp:ListItem Value="2">Old Quote</asp:ListItem>
                    <asp:ListItem Value="3">eStore Quotation</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
    </table>
    <asp:Panel runat="server" ID="Porder">
        <asp:Panel runat="server" ID="pldummy" DefaultButton="btnSH">
            <table>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="Label3" Text="Order No" />
                        :
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtOrderNo" />
                    </td>
                    <td>
                        <asp:Label runat="server" ID="Label4" Text="PO No" />
                        :
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtPONO" />
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lbRoleName" Text="ERP ID" />
                        :
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtCompanyId" />
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lbAccountName" Text="Company Name" />
                        :
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtCompanyName" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Org:
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lbSearchOrg" />
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lbRoleValue" Text="From" />
                        :
                    </td>
                    <td>
                        <ajaxToolkit:CalendarExtender runat="server" ID="CalendarExtender1" TargetControlID="txtOrderFrom"
                            Format="yyyy/MM/dd" />
                        <asp:TextBox runat="server" ID="txtOrderFrom" />
                    </td>
                    <td>
                        <asp:Label runat="server" ID="Label2" Text="To" />
                        :
                    </td>
                    <td>
                        <ajaxToolkit:CalendarExtender runat="server" ID="CalendarExtender2" TargetControlID="txtOrderTo"
                            Format="yyyy/MM/dd" />
                        <asp:TextBox runat="server" ID="txtOrderTo" />
                    </td>
                   <td>
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td align="center">
                        <asp:Button runat="server" ID="btnSH" Text="Search" OnClick="btnSH_Click" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:GridView ID="GridView1" AllowSorting="True" DataKeyNames="SONO" PageSize="15"
            Font-Size="8pt" runat="server" AutoGenerateColumns="False" Width="100%" OnRowDataBound="GridView1_RowDataBound"
            OnPageIndexChanging="GridView1_PageIndexChanging" OnSelectedIndexChanging="GridView1_SelectedIndexChanging"
            AllowPaging="True">
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:Label runat="server" ID="l1" Text="Order No" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%#ParseSONO(Eval("SONO"))%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:Label runat="server" ID="l3" Text="PO NO" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="l4" Text='<%#Eval("PONO") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:Label runat="server" ID="l5" Text="Sold To" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="l6" Text='<%#Eval("SoldToID") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Company Name">
                    <ItemTemplate>
                        <%#Eval("COMPANYNAME") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-HorizontalAlign="Center" Visible="false">
                    <HeaderTemplate>
                        <asp:Label runat="server" ID="L7" Text="Org" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="l8" Text='<%#Eval("ORG_ID") %>' />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        <asp:Label runat="server" ID="l9" Text="Order Date" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="l10" Text='<%#ParseSAPDate(Eval("ORDERDATE")) %>' />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:TemplateField>
                <asp:ButtonField ButtonType="Button" CommandName="Select" HeaderText="Detail" ShowHeader="True"
                    Text="Detail" ItemStyle-HorizontalAlign="Center" />
                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        <asp:Label runat="server" ID="lbQuote" Text="Action" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Button runat="server" ID="btnQuote" Text="Copy to a new Quote" OnClick="btnQuote_Click" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:TemplateField>
            </Columns>
            <SelectedRowStyle BackColor="#FFD393" />
        </asp:GridView>
        <hr />
        <asp:Label ID="LabDetail" runat="server" Visible="false" Font-Bold="True">Order Detail:</asp:Label>
        <asp:GridView DataKeyNames="lineno" ID="gv1" runat="server" AllowPaging="false" EmptyDataText="no Item."
            Font-Size="8" AutoGenerateColumns="false" AllowSorting="True">
            <Columns>
                <asp:BoundField DataField="Lineno" HeaderText="No." ItemStyle-HorizontalAlign="center" />
                <asp:BoundField DataField="Partno" HeaderText="Part No" ItemStyle-HorizontalAlign="left" />
                <asp:TemplateField HeaderText="Qty">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="l12" Text='<%#Eval("Qty") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        Req Date
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="l12" Text='<%#Eval("ReqDate") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        Unit Price
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="l13" Text='<%#Eval("UnitPrice") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        Amount
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="l14" Text='<%#Eval("Amount") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:TextBox runat="server" ID="TBhide" Height="0px" Width="0px" CssClass="hide" />
    </asp:Panel>
    <asp:Panel runat="server" ID="PQuote">
        <myASCX:PickQuoteCopy ID="ascxPickQuoteCopy" runat="server" />
    </asp:Panel>
    <asp:Panel runat="server" ID="PeStore">
        <asp:Panel runat="server" ID="Panel2" DefaultButton="btnSH">
            <table>
                <tr>
                    <td>
                        Quoteid :
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="TBQuoteid" />
                    </td>
                    <td>
                        Customer Name
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="TBCustomerName" />
                    </td>
                    <td>
                        <asp:Label runat="server" ID="Label6" Text="Creator" />
                        :
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="TBCreator" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="Label8" Text="From" />
                        :
                    </td>
                    <td>
                        <ajaxToolkit:CalendarExtender runat="server" ID="CalendarExtender3" TargetControlID="TBfrom"
                            Format="yyyy/MM/dd" />
                        <asp:TextBox runat="server" ID="TBfrom" />
                    </td>
                    <td>
                        <asp:Label runat="server" ID="Label9" Text="To" />
                        :
                    </td>
                    <td>
                        <ajaxToolkit:CalendarExtender runat="server" ID="CalendarExtender4" TargetControlID="TBto"
                            Format="yyyy/MM/dd" />
                        <asp:TextBox runat="server" ID="TBto" />
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td align="center">
                        <asp:Button runat="server" ID="BTeStore" Text="Search" OnClick="BTeStore_Click" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <%--OnRowDataBound="GridView1_RowDataBound"--%>
        <asp:GridView ID="GridView2" AllowSorting="True" DataKeyNames="quoteid" PageSize="15"
            Font-Size="8pt" runat="server" AutoGenerateColumns="False" Width="100%" OnPageIndexChanging="GridView2_PageIndexChanging"
            OnSelectedIndexChanging="GridView2_SelectedIndexChanging" AllowPaging="True">
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:Label runat="server" ID="l1" Text="Quote No" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%#Eval("quoteid")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <%--         <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:Label runat="server" ID="l3" Text="PO NO" />
                    </HeaderTemplate>
                    <ItemTemplate>
                    <asp:Label runat="server" ID="l4" Text='<%#Eval("PONO") %>' />
                    </ItemTemplate>
                </asp:TemplateField>--%>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:Label runat="server" ID="l5" Text="Sold To" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="l6" Text='<%#Eval("quoteToErpId") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Company Name">
                    <ItemTemplate>
                        <%# Eval("quoteToName")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-HorizontalAlign="Center" Visible="false">
                    <HeaderTemplate>
                        <asp:Label runat="server" ID="L7" Text="Org" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="l8" Text='<%#Eval("ORG") %>' />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        <asp:Label runat="server" ID="l9" Text="Creator" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Eval("createdBy") %>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="left"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        <asp:Label runat="server" ID="l9" Text="Quote Date" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Eval("createdDate","{0:yyyy-MM-dd}") %>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:TemplateField>
                <asp:ButtonField ButtonType="Button" CommandName="Select" HeaderText="Detail" ShowHeader="True"
                    Text="Detail" ItemStyle-HorizontalAlign="Center" />
                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        <asp:Label runat="server" ID="lbQuote" Text="Action" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Button runat="server" ID="btnQuote" Text="Copy to a new Quote" OnClick="btnQuoteStore_Click" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:TemplateField>
            </Columns>
            <SelectedRowStyle BackColor="#FFD393" />
        </asp:GridView>
        <hr />
        <asp:Label ID="LabeStoreDetail" runat="server" Visible="false" Font-Bold="True">Quote Detail:</asp:Label>
        <asp:GridView DataKeyNames="line_No" ID="GridView3" runat="server" AllowPaging="false"
            EmptyDataText="no Item." Font-Size="8" AutoGenerateColumns="false" AllowSorting="True">
            <Columns>
                <asp:BoundField DataField="line_No" HeaderText="No." ItemStyle-HorizontalAlign="center" />
                <asp:BoundField DataField="Partno" HeaderText="Part No" ItemStyle-HorizontalAlign="left" />
                <asp:TemplateField HeaderText="Qty">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="l10" Text='<%#Eval("Qty") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        Req Date
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="l10" Text='<%#Eval("ReqDate") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        Unit Price
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="l10" Text='<%#Eval("UnitPrice","{0:f2}") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        Amount
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="l10" Text='<%#Eval("Amount","{0:f2}") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:TextBox runat="server" ID="TBhide2" Height="0px" Width="0px" CssClass="hide" />
    </asp:Panel>
</asp:Content>
