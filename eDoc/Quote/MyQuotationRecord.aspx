<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/myMaster.master" CodeBehind="MyQuotationRecord.aspx.vb" Inherits="EDOC.MyQuotationRecord" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <asp:Panel runat="server" ID="pldummy" DefaultButton="btnSH">
        <table>
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label3" Text="Quote No"></asp:Label>
                    :
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtQuoteId"></asp:TextBox>
                </td>
                <td>
                    <asp:Label runat="server" ID="lbRoleName" Text="Quote Desc"></asp:Label>
                    :
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtCustomId"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="lbRoleValue" Text="<%$ Resources:myRs,AccountName %>"></asp:Label>
                    :
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtAccountName"></asp:TextBox>
                </td>
                <td>
                    <asp:Label runat="server" ID="Label1" Text="<%$ Resources:myRs,CompanyID %>"></asp:Label>
                    :
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtCompanyID"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label4" Text="<%$ Resources:myRs,SalesOrderNO %>"></asp:Label>
                    :
                </td>
                <td>
                    <asp:TextBox runat="server" ID="TextBox_SONO"></asp:TextBox>
                </td>
                <td>
                    <asp:Label runat="server" ID="Label5" Text="<%$ Resources:myRs,PurchaseOrderNO %>"></asp:Label>
                    :
                </td>
                <td>
                    <asp:TextBox runat="server" ID="TextBox_PONO"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="left" style="color: Black">
                    Quotation Create Date
                </td>
                <td>
                    From:&nbsp;<ajaxToolkit:CalendarExtender runat="server" ID="CalExt1" TargetControlID="txtQuoteCreateFrom"
                        Format="yyyy/MM/dd" />
                    <asp:TextBox runat="server" ID="txtQuoteCreateFrom" Width="80px" />
                </td>
                <td style="color: Black">
                    To:
                </td>
                <td>
                    <ajaxToolkit:CalendarExtender runat="server" ID="CalExt2" TargetControlID="txtQuoteCreateTo"
                        Format="yyyy/MM/dd" />
                    <asp:TextBox runat="server" ID="txtQuoteCreateTo" Width="80px" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label2" Text="<%$ Resources:myRs,Status %>"></asp:Label>
                    :
                </td>
                <td>
                    <asp:DropDownList ID="drpStatus" runat="server">
                        <asp:ListItem Value="">All</asp:ListItem>
                        <asp:ListItem Value="0">Draft</asp:ListItem>
                        <asp:ListItem Value="1">Finish</asp:ListItem>
                    </asp:DropDownList>
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
                    <asp:Button runat="server" ID="btnSH" OnClick="btnSH_Click" Text="Search" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString="<%$ connectionStrings: EQ %>"
        SelectCommand=""></asp:SqlDataSource>
    Delete the draft of
    <asp:TextBox ID="txtMonth" runat="server" MaxLength="2"></asp:TextBox>
    month ago.<asp:Button ID="btnDeleteBatch" runat="server" Text="Delete" OnClick="btnDeleteBatch_Click" />
    <ajaxToolkit:FilteredTextBoxExtender runat="server" ID="Fn" TargetControlID="txtMonth"
        FilterType="Numbers, Custom" />
    <asp:GridView DataKeyNames="quoteId" ID="GridView1" AllowSorting="true" AllowPaging="true"
        PageIndex="0" PageSize="16" runat="server" AutoGenerateColumns="false" OnPageIndexChanging="GridView1_PageIndexChanging"
        Width="100%" OnRowDataBound="GridView1_RowDataBound" OnSorting="GridView1_Sorting">
        <Columns>
<%--            <asp:BoundField HeaderText="Quote ID" DataField="quoteId" ItemStyle-HorizontalAlign="Left"
                SortExpression="quoteId" />
--%>            
            <asp:BoundField HeaderText="Quote No" DataField="quoteNo" ItemStyle-HorizontalAlign="Left"
                SortExpression="quoteNo" />
            <asp:BoundField HeaderText="Quote Desc" DataField="customId" ItemStyle-HorizontalAlign="Left"
                SortExpression="customId" />
            <asp:BoundField HeaderText="ERP ID" DataField="quoteToErpId" ItemStyle-HorizontalAlign="Left"
                SortExpression="quoteToErpId" />
            <asp:TemplateField>
                <HeaderTemplate>
                    <asp:Label runat="server" ID="lbsqid" Text="<%$ Resources:myRs,SiebelQuoteId %>"></asp:Label>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" ID="lbSiebelQuoteId" Text='<%#Bind("siebelQuoteId")%>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="Account Name" DataField="quoteToName" ItemStyle-HorizontalAlign="Left"
                SortExpression="quoteToName" />
            <asp:TemplateField>
                <HeaderTemplate>
                    <asp:Label runat="server" ID="lbUpdateErpId" Text="<%$ Resources:myRs,GetErpIdFromSiebel %>"></asp:Label>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Button runat="server" ID="btnGetErpId" Text="Get" OnClick="btnGetErpId_Click" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderTemplate>
                    <asp:Label runat="server" ID="lbUpdateErpId" Text="GP Flow"></asp:Label>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" ID="lbGpFlow"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="Quote Date" DataField="quoteDate" ItemStyle-HorizontalAlign="Left"
                SortExpression="quoteDate" />
            <%--<asp:BoundField HeaderText="Quote Date" DataField="quoteDate" DataFormatString="{0:d}" ItemStyle-HorizontalAlign="Left" SortExpression="quoteDate"/>--%>
            <asp:TemplateField>
                <HeaderTemplate>
                    <asp:Label runat="server" ID="lbStatus" Text="Status"></asp:Label>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" ID="lbStatusV" Text='<%#Bind("DOCSTATUS")%>' />
                </ItemTemplate>
            </asp:TemplateField>
            <%--<asp:TemplateField>
                <HeaderTemplate>
                <asp:Label runat="server" ID="lbHdEdit" Text="<%$ Resources:myRs,Add2Cart %>"></asp:Label>
                  </HeaderTemplate>
                <ItemTemplate>
               <asp:Button runat="server" ID="btnAdd2Cart" Text="Add2Cart" />
                </ItemTemplate>
            </asp:TemplateField>--%>
            <asp:TemplateField>
                <HeaderTemplate>
                    <asp:Label runat="server" ID="lbHdEdit" Text="<%$ Resources:myRs,Order %>"></asp:Label>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Button runat="server" ID="btnOrder" Text="Order" OnClick="btnOrder_Click" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderTemplate>
                    <asp:Label runat="server" ID="lbHdEdit" Text="<%$ Resources:myRs,Detail %>"></asp:Label>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:ImageButton ImageUrl="~/Images/search.gif" runat="server" ID="ibtnDetail" OnClick="ibtnDetail_Click" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderTemplate>
                    <asp:Label runat="server" ID="lbHdEdit" Text="<%$ Resources:myRs,Pdf %>"></asp:Label>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:ImageButton ImageUrl="~/Images/pdf_icon.jpg" runat="server" ID="ibtnPdf" OnClick="ibtnPdf_Click" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                <HeaderTemplate>
                    <asp:Label runat="server" ID="lbfw" Text="Forward"></asp:Label>
                </HeaderTemplate>
                <ItemTemplate>
                    <%--  <asp:Button Text="Go" runat="server" ID="btnFW" OnClick="btnFW_Click" />--%>
                    <span onclick="openwin('<%# Eval("quoteId") %>')" style="text-decoration: underline; cursor: pointer;">Go </span>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="Expired Date" DataField="expiredDate" DataFormatString="{0:yyyy/MM/dd}"
                ItemStyle-HorizontalAlign="Left" SortExpression="expiredDate" />
            <asp:TemplateField>
                <HeaderTemplate>
                    <asp:Label runat="server" ID="lbOrder_log" Text="Order Log"></asp:Label>
                </HeaderTemplate>
                <ItemTemplate>
                    <div align="center">
                        <asp:HyperLink ID="HyperLink_CheckAll" runat="server" NavigateUrl='<%#Bind("quoteId","QuotationToOrderLog.aspx?quoteid={0}")%>'
                            Target="_blank">Check All</asp:HyperLink>
                        <asp:GridView ID="GridView_OrderInfo" runat="server" AutoGenerateColumns="false"
                            ShowHeader="false">
                            <Columns>
                                <asp:TemplateField HeaderText="Order Info" ItemStyle-Width="100%" ItemStyle-VerticalAlign="Top">
                                    <ItemTemplate>
                                        <table>
                                            <tr>
                                                <td align="left">
                                                    SO:
                                                </td>
                                                <td>
                                                    <%#Eval("SO_NO")%>
                                                </td>
                                                <td align="left">
                                                    PO:
                                                </td>
                                                <td>
                                                    <%#Eval("PO_NO")%>
                                                </td>
                                                <td align="left">
                                                    Order Date:
                                                </td>
                                                <td>
                                                    <%#Format(Eval("ORDER_DATE"), "yyyy/MM/dd")%>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderTemplate>
                    <asp:Label runat="server" ID="lbHdEdit" Text="<%$ Resources:myRs,Edit %>"></asp:Label>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:ImageButton ImageUrl="~/Images/edit.gif" runat="server" ID="ibtnEdit" OnClick="ibtnEdit_Click" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderTemplate>
                    <asp:Label runat="server" ID="lbHdDelete" Text="<%$ Resources:myRs,Delete %>"></asp:Label>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:ImageButton ImageUrl="~/Images/del.gif" runat="server" ID="ibtnDelete" OnClick="ibtnDelete_Click"
                        OnClientClick="return confirm('Are you sure to delete this Quotation?')" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <script>
        function openwin(QuoteID) {
            var url = "QuoteForward.aspx?QuoteID=" + QuoteID;
            var iTop = (window.screen.height - 30 - 310) / 2;
            var iLeft = (window.screen.width - 10 - 546) / 2;
            window.open(url, 'newwindow', 'height=310, width=546, top=' + iTop + ',left=' + iLeft + ', toolbar=no, menubar=no, scrollbars=no, resizable=no,location=no, status=no')
        }
    </script>
</asp:Content>
