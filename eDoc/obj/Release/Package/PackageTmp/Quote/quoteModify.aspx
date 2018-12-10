<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/myMaster.master" CodeBehind="quoteModify.aspx.vb" Inherits="EDOC.quoteModify" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .warntd ul
        {
            text-align: left;
            margin: 0px; margin-top:5px;
            padding: 0px; padding-left:20px;
        }
        .warntd span
        {
            text-align: left;
            display: block;
            margin-left: 30%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div runat="server" id="divLogo">
        <table>
            <tr>
                <td>
                    <asp:Image ImageUrl="http://eq.advantech.com/Images/LogoPi.jpg" runat="server" ID="imgLogo" />
                </td>
                <td align="right">
                    <asp:Label ID="lbAddressInfo" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <div runat="server" id="divMaster">
        <table>
            <tr>
                <td  width="210">
                    <asp:Label runat="server" ID="Label19" Text="PO"></asp:Label>
                    :
                </td>
                <td>
                    <asp:Label runat="server" ID="lbCustomId"></asp:Label>
                </td>
            </tr>
              <tr>
                <td>
                    <asp:Label runat="server" ID="Label23" Text="Quote No"></asp:Label>
                    :
                </td>
                <td>
                    <asp:Label runat="server" ID="lbQuoteID"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label20" Text="<%$ Resources:myRs,CreatedBy %>"></asp:Label>
                    :
                </td>
                <td>
                    <asp:Label runat="server" ID="lbCreatedBy"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label21" Text="<%$ Resources:myRs,QuoteDate %>"></asp:Label>
                    :
                </td>
                <td>
                    <asp:Label runat="server" ID="lbQuoteDate"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label22" Text="<%$ Resources:myRs,PreparedBy %>"></asp:Label>
                    :
                </td>
                <td>
                    <asp:Label runat="server" ID="lbPreparedBy"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label1" Text="<%$ Resources:myRs,QuoteTo %>"></asp:Label>
                    :
                </td>
                <td>
                    <asp:Label runat="server" ID="lbQuoteToName"></asp:Label>  <asp:Label runat="server" ID="lbQuoteToId"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label2" Text="<%$ Resources:myRs,AccountInfo %>"></asp:Label>
                    :
                </td>
                <td>
                    <table width="500">
                        <tr>
                            <td width="200">
                                <asp:Label runat="server" ID="Label13" Text="<%$ Resources:myRs,Office %>"></asp:Label>
                                :
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lbOffice"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="Label14" Text="<%$ Resources:myRs,Currency %>"></asp:Label>
                                :
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lbCurrency"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="Label15" Text="<%$ Resources:myRs,SalesEmail %>"></asp:Label>
                                :
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lbSalesEmail"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="Label16" Text="<%$ Resources:myRs,DirectPhone %>"></asp:Label>
                                :
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lbDirectPhone"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="Label17" Text="<%$ Resources:myRs,Attention %>"></asp:Label>
                                :
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lbAttention"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="Label18" Text="<%$ Resources:myRs,BankAccount %>"></asp:Label>
                                :
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lbBankAccount"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label3" Text="<%$ Resources:myRs,DeliveryDate %>"></asp:Label>
                    :
                </td>
                <td>
                    <asp:Label runat="server" ID="lbDeliveryDate"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label4" Text="<%$ Resources:myRs,ExpiredDate %>"></asp:Label>
                    :
                </td>
                <td>
                    <asp:Label runat="server" ID="lbExpiredDate"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label5" Text="<%$ Resources:myRs,ShippingTerms %>"></asp:Label>
                    :
                </td>
                <td>
                    <asp:Label runat="server" ID="lbShippingTerms"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label6" Text="<%$ Resources:myRs,PaymentTerms %>"></asp:Label>
                    :
                </td>
                <td>
                    <asp:Label runat="server" ID="lbPaymentTerms"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label7" Text="<%$ Resources:myRs,Freight %>"></asp:Label>
                    :
                </td>
                <td>
                    <asp:Label runat="server" ID="lbFreight"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label8" Text="<%$ Resources:myRs,Insurance %>"></asp:Label>
                    :
                </td>
                <td>
                    <asp:Label runat="server" ID="lbInsurance"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label9" Text="<%$ Resources:myRs,SpecialCharge %>"></asp:Label>
                    :
                </td>
                <td>
                    <asp:Label runat="server" ID="lbSpecialCharge"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label10" Text="<%$ Resources:myRs,Tax %>"></asp:Label>
                    :
                </td>
                <td>
                    <asp:Label runat="server" ID="lbTax"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label11" Text="<%$ Resources:myRs,QuoteNotes %>"></asp:Label>
                    :
                </td>
                <td>
                    <asp:Label runat="server" ID="lbQuoteNotes"></asp:Label>
                </td>
            </tr>
            <tr runat="server" id="trRelatedInfo">
                <td>
                    <asp:Label runat="server" ID="Label12" Text="<%$ Resources:myRs,RelatedInformation %>"></asp:Label>
                    :
                </td>
                <td>
                    <asp:Label runat="server" ID="lbRelatedInformation"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
<div runat="server" id="divContent">
    </div>
        <asp:Button runat="server" Text=" Update " ID="btnUpdate" OnClick="btnUpdate_Click" />
    <asp:GridView DataKeyNames="line_no" ID="gv1" runat="server" AllowPaging="false"
        EmptyDataText="no Item." AutoGenerateColumns="false" OnRowDataBound="gv1_RowDataBound" OnDataBound="gv1_DataBound">
        <Columns>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                <HeaderTemplate>
                    <asp:CheckBox ID="chkKey" runat="server" OnClick="GetAllCheckBox(this)" Checked="true" />
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:CheckBox ID="chkKey" runat="server" Checked="true" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                <HeaderTemplate>
                    Seq
                </HeaderTemplate>
                <ItemTemplate>
                    <table>
                        <tr>
                            <td>
                                <asp:LinkButton runat="server" CommandName='<%#Bind("line_no")%>' ID="ibtnSeqUp"
                                    Font-Bold="true" Enabled="false" >↑</asp:LinkButton>
                            </td>
                            <td>
                                <asp:LinkButton runat="server" CommandName='<%#Bind("line_no")%>' ID="ibtnSeqDown"
                                    Font-Bold="true" Enabled="false">↓</asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="line_no" HeaderText="No." ItemStyle-HorizontalAlign="center" />
            <asp:TemplateField HeaderText="Category">
                <ItemTemplate>
                    <asp:TextBox runat="server" ID="txtCategory" Text='<%#Bind("category") %>' BorderWidth="1px"
                        BorderColor="#cccccc" ReadOnly="true" BackColor="#eeeeee" Width="100px"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:HyperLinkField HeaderText="Part No" Target="_blank" DataNavigateUrlFields="modelno"
                DataNavigateUrlFormatString="http://my.advantech.com/product/model_detail.aspx?model_no={0}"
                DataTextField="partno" />
            <asp:TemplateField HeaderText="Description">
                <ItemTemplate>
                    <asp:TextBox runat="server" ID="txtDescription" Text='<%#Bind("description") %>'
                        BorderWidth="1px" BorderColor="#cccccc" ReadOnly="true" BackColor="#eeeeee" Width="100%"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Extended Warranty">
                <ItemTemplate>
                    <asp:DropDownList ID="gv_drpEW" runat="server" AutoPostBack="true" Enabled = "false">
                        <asp:ListItem Text="without EW" Value="0"></asp:ListItem>
                        <asp:ListItem Text="3 months" Value="3"></asp:ListItem>
                        <asp:ListItem Text="6 months" Value="6"></asp:ListItem>
                        <asp:ListItem Text="9 months" Value="9"></asp:ListItem>
                        <asp:ListItem Text="12 months" Value="12"></asp:ListItem>
                        <asp:ListItem Text="15 months" Value="15"></asp:ListItem>
                        <asp:ListItem Text="21 months" Value="21"></asp:ListItem>
                        <asp:ListItem Text="24 months" Value="24"></asp:ListItem>
                        <asp:ListItem Text="36 months" Value="36"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:Label runat="server" Text='<%#currencySign %>' ID="lbEWSign"></asp:Label>
                    <asp:TextBox runat="server" ID="gv_lbEW" Style="text-align: right" BorderWidth="1px"
                        BorderColor="#cccccc" ReadOnly="true" BackColor="#eeeeee" Width="40px"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="rohs" HeaderText="Rohs" ItemStyle-HorizontalAlign="center" />
            <asp:BoundField DataField="classABC" HeaderText="Class" ItemStyle-HorizontalAlign="center" />
            <asp:TemplateField ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="Center">
                <HeaderTemplate>
                    List Price
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%#currencySign %>' ID="lbListPriceSign"></asp:Label>
                    <asp:Label runat="server" Text='<%#FormatNumber(Eval("listpriceX"),2) %>' ID="lbListPrice"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="Center">
                <HeaderTemplate>
                    Unit Price
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%#currencySign %>' ID="lbUnitPriceSign"></asp:Label>
                    <asp:TextBox ID="txtUnitPrice" runat="server" Enabled="false" Text='<%#replace(FormatNumber(Eval("UnitPriceWithWarrantX"),2),",","") %>'
                        Width="60px" Style="text-align: right"></asp:TextBox>
                    <ajaxToolkit:FilteredTextBoxExtender runat="server" ID="ft1" TargetControlID="txtUnitPrice"
                        FilterType="Numbers, Custom" ValidChars="." />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="" HeaderText="Disc." ItemStyle-HorizontalAlign="right" />
            <asp:TemplateField ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="Center">
                <HeaderTemplate>
                    Qty.
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:TextBox ID="txtGVQty" runat="server" Text='<%#Bind("qty") %>' Width="30px" Style="text-align: right"
                        OnTextChanged="txtGVQty_TextChanged"></asp:TextBox>
                    <ajaxToolkit:FilteredTextBoxExtender runat="server" ID="ft2" TargetControlID="txtGVQty"
                        FilterType="Numbers, Custom" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="Center">
                <HeaderTemplate>
                    Req. Date
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:TextBox ID="txtreqdate" runat="server" Text='<%#CDate(Eval("reqdate")).ToShortDateString()%>'
                        Width="60px" Style="text-align: right" Enabled="false"></asp:TextBox>
                    <ajaxToolkit:FilteredTextBoxExtender runat="server" ID="Filteredtextboxextender5"
                        TargetControlID="txtreqdate" FilterType="Numbers, Custom" ValidChars="-/" />
                    <ajaxToolkit:CalendarExtender runat="server" ID="CalendarExtender1" CssClass="cal_Theme1"
                        TargetControlID="txtreqdate" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="Center">
                <HeaderTemplate>
                    Due Date
                </HeaderTemplate>
                <ItemTemplate>
                       <asp:Label runat="server" Text='<%# iif(CDate(Eval("duedate")).toString("yyyy/MM/dd")="1900/01/01","TBD",CDate(Eval("duedate")).toString("yyyy/MM/dd")) %>'
                                                ID="lbDueDate"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="Center">
                <HeaderTemplate>
                    Sub Total
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%#currencySign %>' ID="lbSubTotalSign"></asp:Label>
                    <asp:Label runat="server" Text='<%#replace(FormatNumber(Eval("SubTotalWithWarrantX"),2),",","") %>' ID="lbSubTotal"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="ITP(FOB AESC)" Visible="false">
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%#currencySign %>' ID="lbItpSign"></asp:Label>
                    <asp:TextBox runat="server" ID="txtItp" Text='<%#Eval("newITP") %>' Width="50px"
                        ReadOnly="true"></asp:TextBox>
                    <asp:Button ID="btnSpecialItp" runat="server" Text="Special ITP" Enabled="false" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="Center" Visible="false">
                <HeaderTemplate>
                    Margin
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" ID="lbMargin"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Product Status" ItemStyle-HorizontalAlign="center">
                <ItemTemplate>
                    <asp:Label runat="server" ID="lbProductStatus"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <p style="text-align: right">
        <b>Total:</b><%= currencySign%><asp:Label runat="server" ID="lbtotal" Text="0.00"></asp:Label>
        <b>Total Margin:</b><asp:Label runat="server" ID="lbTotalMargin" Text="0.00"></asp:Label>%</p>
    <p style="text-align: right">
        Total margin is calculated without taking AGS & P-trade items into consideration.</p>
    <table>
    <tr id="trw" runat="server" visible="false">
    <td colspan="2" align="center"  class="warntd">
        <asp:Label ID="labWarn" runat="server" Text="" Font-Bold="True"></asp:Label>
    </td>
    </tr>
        <tr>
<%--        <td align="center" id="tdedit" runat="server" visible="false">
            <asp:Button ID="BTedit" runat="server" Text="Edit this Quote" OnClick="BTedit_Click" />
        </td>--%>
            <td align="center" colspan="2">
                <asp:UpdatePanel ID="upbtnConfirm" runat="server" UpdateMode="Conditional">
                    <ContentTemplate><asp:CheckBox runat ="server" ID="cbxIsTest" Visible="false" />
                        <asp:Button ID="btnConfirm" runat="server" Text="<%$ Resources:myRs,Confirm %>" OnClick="btnConfirm_Click" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>
