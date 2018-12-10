<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/myMaster.master" CodeBehind="CreditInfo.aspx.vb" Inherits="EDOC.CreditInfo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<style type="text/css"> 
  .style1 {
            border-color:black;
            border-width:1px;
            border-style:Solid;
            width:70%;
            border-collapse:collapse;
            
        }

  .style2 {
            border-color:black;
            border-width:1px;
            border-style:Solid;
            width:15%;
            border-collapse:collapse;
            background-color:#F85C12;
            color:white;
            font-size:14px;
        }

  .style3 {
            border-color:black;
            border-width:1px;
            border-style:Solid;
            width:35%;
            border-collapse:collapse;
            font-size:14px;
        }

</style>

<center>
<table width="80%" class="style1">
        <tr>
            <th align="left" colspan="4" style="color: black;font-size:14px;">Customer's Credit Info</th>
        </tr>
        <tr >
            <td class="style2">
                ORG :
            </td>
            <td class="style3">
                <%= Request("ORG")%>
            </td>
            <td class="style2">
                ERP ID :
            </td>
            <td class="style3">
                <%= Request("ID")%>
            </td>
        </tr>
        <tr >
            <td class="style2">
                Credit Control Area :
            </td>
            <td class="style3">
                <asp:Label ID="lbCreditControlArea" runat="server" Text="" />
            </td>
            <td class="style2">
                Currency :
            </td>
            <td class="style3">
                <asp:Label ID="lbCurrency" runat="server" Text="" />
            </td>
        </tr>

        <tr>
            <td class="style2">
                Credit Limit :
            </td>
            <td class="style3">
                <asp:Label ID="lbCreditLimit" runat="server" Text="" />
            </td>
            <td class="style2">
                Credit Exposure :
            </td>
            <td class="style3">
                <asp:Label ID="lbCreditExposure" runat="server" Text="" />
            </td>
        </tr>
        <tr>
            <td class="style2">
                <%--Percentage :--%>
                Credit limit used :
            </td>
            <td class="style3">
                <asp:Label ID="lbPercentage" runat="server" Text="" />
            </td>
            <td class="style2">
                Receivables :
            </td>
            <td class="style3">
                <asp:Label ID="lbReceivables" runat="server" Text="" />
            </td>
        </tr>
        <tr>
            <td class="style2">
                Special Liability :
            </td>
            <td class="style3">
                <asp:Label ID="lbSpecialLiability" runat="server" Text="" />
            </td>
            <td class="style2">
                Sales Value :
            </td>
            <td class="style3">
                <asp:Label ID="lbSalesValue" runat="server" Text="" />
            </td>
        </tr>
        <tr>
            <td class="style2">
                Risk Category :
            </td>
            <td class="style3">
                <asp:Label ID="lbRiskCategory" runat="server" Text="" />
            </td>
            <td class="style2">
                Blocked :
            </td>
            <td class="style3">
                <asp:Label ID="lbBlocked" runat="server" Text="" />
            </td>
        </tr>
    </table>
<%--    <table width="100%" style="border-color:#D7D0D0;border-width:1px;border-style:Solid;width:100%;border-collapse:collapse;">
        <tr>
            <td>
                <asp:GridView runat="server" ID="gvCustCredit" Width="100%" AutoGenerateColumns="false">
                    <Columns>
                        <asp:TemplateField HeaderText="Credit Control Area" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <%#Eval("CreditControlArea")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Currency" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <%#Eval("Currency")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Credit Limit" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <%#Eval("CreditLimit")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Credit Exposure" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <%#Eval("CreditExposure")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Credit Percentage" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <%#Eval("Percentage")%>%
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Receivables" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <%#Eval("Receivables")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Special Liability" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <%#Eval("SpecialLiability")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Sales Value" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <%#Eval("SalesValue")%>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Risk Category" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <%#Eval("RiskCategory")%>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Blocked" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <%#Eval("Blocked")%>
                            </ItemTemplate>
                        </asp:TemplateField>


                    </Columns>
                </asp:GridView>
            </td>
        </tr><asp:GridView runat="server"></asp:GridView>
    </table>--%>
    <br/>
    <asp:GridView runat="server" ID="gv1" AutoGenerateColumns="false" AllowPaging="false"
        Width="80%" OnRowDataBound="gv1_RowDataBound" Visible="false">
        <Columns>
            <asp:TemplateField ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                <HeaderTemplate>
                    No.
                </HeaderTemplate>
                <ItemTemplate>
                    <%# Container.DataItemIndex + 1 %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="Invoice NO" DataField="ar_no" SortExpression="ar_no"
                ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Reference" DataField="re_nO" SortExpression="re_nO"
                ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Shipping Date" DataField="ar_date" SortExpression="ar_date"
                ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Currency" DataField="currency" SortExpression="currency"
                ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Amount" DataField="amount" SortExpression="amount" ItemStyle-HorizontalAlign="Right" />
            <asp:BoundField HeaderText="Open Amount" DataField="local_amount" SortExpression="local_amount"
                ItemStyle-HorizontalAlign="Right" />
            <asp:BoundField HeaderText="Due Date" DataField="ar_due_date" SortExpression="ar_due_date"
                ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Over Due" DataField="ar_status" SortExpression="ar_status" />
        </Columns>
        <%--<FixRowColumn FixColumns="-1" FixRows="-1" TableHeight="400px" FixRowType="Header" />--%>
        <PagerSettings PageButtonCount="10" Position="TopAndBottom" />
    </asp:GridView>

</center>
</asp:Content>
