<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/myMaster.master"
    CodeBehind="QuotationMaster.aspx.vb" Inherits="EDOC.QuotationMaster1" %>

<%@ Register Assembly="EDOC" Namespace="EDOC" TagPrefix="uc1" %>
<%@ Register Src="~/ascx/PickAccount.ascx" TagName="PickAccount" TagPrefix="myASCX" %>
<%@ Register Src="~/ascx/PickAttention.ascx" TagName="PickAttention" TagPrefix="myASCX" %>
<%@ Register Src="~/ascx/PickShipToBillToSAP.ascx" TagName="PickShipToBillTo" TagPrefix="myASCX" %>
<%@ Register Src="~/ascx/PickAddressSiebel.ascx" TagName="PickADS" TagPrefix="myASCX" %>
<%@ Register Src="~/ascx/PickSiebelOpportunity.ascx" TagName="Pickopty1" TagPrefix="myASCX" %>
<%@ Register Src="~/ascx/PickSignature.ascx" TagName="PickSignature1" TagPrefix="myASCX" %>
<%@ Register Src="~/Ascx/UCCreditInfo.ascx" TagName="CustomerCreditInfo" TagPrefix="myASCX" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%= EDOC.MyUtil.GetOrgStyle()%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <% If Request("UID") IsNot Nothing AndAlso Not String.IsNullOrEmpty(Request("UID")) AndAlso (_IsTWAonlineUser Or _IsCNAonlineUser) Then%>
    <table id="tbFlow" class="Ind">
        <tr>
            <td>
                <a href="quotationMaster.aspx?UID=<%=Request("UID")%>">
                    <img src="../Images/Master.gif" alt="" /></a>
            </td>
            <td><a href="QuotationDetail.aspx?VIEW=0&UID=<%=Request("UID")%>">
                <img src="../Images/Detail.gif" alt="" /></a>

            </td>
            <td><a href="Quotation2Siebel.aspx?UID=<%=Request("UID")%>">
                <img src="../Images/Preview.gif" alt="" /></a>
            </td>
        </tr>
    </table>
    <% End If%>
    <%--ICC 2017/01/20 For brand new quote, set focus to account name textbox.--%>
    <%If Me.isBrandNewQuote = True AndAlso Not Me.ascxPickAccount.TxtAccountName Is Nothing Then%>
    <script type="text/javascript">
        $(function () {
            $('#<%=Me.ascxPickAccount.TxtAccountName.ClientID%>').focus();
        });
    </script>
    <%End If%>
    <asp:UpdatePanel ID="UPForm" runat="server" UpdateMode="Conditional" OnPreRender="UPForm_PreRender">
        <ContentTemplate>
            <table>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="Label1" Text="<%$ Resources:myRs,QuoteTo %>" />
                        <span style="color: #ff0000">*</span> :
                    </td>
                    <td>Name&nbsp;:&nbsp;<asp:TextBox runat="server" ID="txtQuoteToName" ReadOnly="true" BackColor="#ebebe4"
                        Width="150" />
                        &nbsp;&nbsp; ERP ID&nbsp;:&nbsp;<asp:TextBox runat="server" ID="hfErpId" ReadOnly="true" BackColor="#ebebe4"
                            Width="80" />&nbsp;
                        <asp:ImageButton ID="ibtnPickAccount" runat="server" ImageUrl="~/Images/search.gif"
                            OnClick="ibtnPickAccount_Click" />
                        <asp:HiddenField ID="hfAccountRowId" runat="server" />
                        <asp:HiddenField ID="hfQuoteNo" runat="server" />
                        &nbsp;&nbsp;SAP Sales Org&nbsp;:&nbsp;<asp:DropDownList ID="drpOrg" runat="server">
                        </asp:DropDownList>
                        <asp:Panel ID="PLPickAccount" runat="server" Style="display: none;" CssClass="modalPopup">
                            <div style="text-align: right;">
                                <asp:ImageButton ID="CancelButtonAccount" runat="server" ImageUrl="~/Images/del.gif" />
                            </div>
                            <div>
                                <asp:UpdatePanel ID="UPPickAccount" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <myASCX:PickAccount ID="ascxPickAccount" runat="server" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </asp:Panel>
                        <asp:LinkButton ID="lbDummyAccount" runat="server" />
                        <ajaxToolkit:ModalPopupExtender ID="MPPickAccount" runat="server" TargetControlID="lbDummyAccount"
                            PopupControlID="PLPickAccount" BackgroundCssClass="modalBackground" CancelControlID="CancelButtonAccount" />
                        <asp:HiddenField ID="hfCreatedDate" runat="server" />
                    </td>
                </tr>
                <tr style="display: none">
                    <td colspan="2">Street:<asp:Label runat="server" ID="lbSoldToStreet" />| City:<asp:Label runat="server"
                        ID="lbSoldToCity" />| State:<asp:Label runat="server" ID="lbSoldToState" />|
                        Zipcode:<asp:Label runat="server" ID="lbSoldToZipcode" />| Country:<asp:Label runat="server"
                            ID="lbSoldToCountry" />| Attention:<asp:Label runat="server" ID="lbSoldToAttention" />|
                        Tel:<asp:Label runat="server" ID="lbSoldToTel" />
                        <asp:HiddenField ID="HFsoldtofax" runat="server" />
                    </td>
                </tr>
                <tr runat="server" id="trCreditBlockMsg" visible="false">
                    <td colspan="2" align="center">
                        <asp:GridView runat="server" ID="gvERPIDCreditBlockMsg" AutoGenerateColumns="false"
                            ShowHeader="false">
                            <Columns>
                                <asp:BoundField HeaderText="Error Message" DataField="Message" ItemStyle-HorizontalAlign="Center"
                                    ItemStyle-ForeColor="Red" ItemStyle-Font-Bold="true" />
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr id="trDSGSO" runat="server" visible="false">
                    <td>Distribution Channel :
                    </td>
                    <td style="padding: 0px">
                        <table cellpadding="0px" style="width: auto">
                            <tr>
                                <td style="padding: 0px; border: 0px">
                                    <asp:UpdatePanel runat="server" ID="upDistChannDiv" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td style="border: 0px">
                                                        <asp:DropDownList runat="server" ID="dlDistChann" AutoPostBack="true" OnSelectedIndexChanged="dlDistChann_SelectedIndexChanged">
                                                            <asp:ListItem Text="Select..." Value="" />
                                                            <asp:ListItem Value="10" />
                                                            <asp:ListItem Value="20" />
                                                            <asp:ListItem Value="30" Selected="True" />
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td style="padding: 0px">
                                                        <table runat="server" id="tbDivSalesGrpOffice" visible="false">
                                                            <tr>
                                                                <td style="width: auto; border: 0px">Division:
                                                                </td>
                                                                <td style="width: auto; border: 0px">
                                                                    <asp:TextBox runat="server" ID="txtSalesDivision" Width="30px" />
                                                                </td>
                                                                <td style="width: auto; border: 0px">Sales Group:
                                                                </td>
                                                                <td style="width: auto; border: 0px">
                                                                    <asp:TextBox runat="server" ID="txtSalesGroup" Width="30px" />
                                                                </td>
                                                                <td style="width: auto; border: 0px">Sales Office:
                                                                </td>
                                                                <td style="width: auto; border: 0px">
                                                                    <asp:TextBox runat="server" ID="txtSalesOffice" Width="30px" />
                                                                </td>
                                                                <td style="width: auto; border: 0px">District:
                                                                </td>
                                                                <td style="width: auto; border: 0px">
                                                                    <asp:TextBox runat="server" ID="txtSalesDistrict" Width="30px" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr runat="server" id="trSB" visible="false">
                    <td valign="top">
                        <asp:Label runat="server" ID="Label2" Text="Ship To & Bill To" />:
                    </td>
                    <td>
                        <asp:Panel ID="Panel2" runat="server" CssClass="collapsePanelHeader">
                            <div style="padding: 5px; cursor: pointer; vertical-align: middle;">
                                <div style="float: left; vertical-align: middle;">
                                    <asp:ImageButton ID="Image1" runat="server" ImageUrl="~/images/expand.jpg" AlternateText="Expand..." />
                                </div>
                            </div>
                        </asp:Panel>
                        <asp:Panel ID="Panel1" runat="server" CssClass="collapsePanel" Height="0" Width="1021px">
                            <table border="0" runat="server" id="tbSBSAP" width="1020px">
                                <tr>
                                    <td style="width: 33%">
                                        <table width="100%" style="background-color: #fafafa">
                                            <tr>
                                                <td>Ship to Name :
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtShipToName" ReadOnly="true" BackColor="#ebebe4" />
                                                    <asp:ImageButton ID="ibtnPickShipTo" runat="server" ImageUrl="~/Images/search.gif"
                                                        OnClick="ibtnPickShipTo_Click" />
                                                    <asp:Panel ID="PLSB" runat="server" Style="display: none" CssClass="modalPopup">
                                                        <div style="text-align: right;">
                                                            <asp:ImageButton ID="CSB" runat="server" ImageUrl="~/Images/del.gif" />
                                                        </div>
                                                        <div>
                                                            <asp:UpdatePanel ID="UPSB" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <myASCX:PickShipToBillTo ID="PickSB" runat="server" />
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </asp:Panel>
                                                    <asp:LinkButton ID="LBSB" runat="server"></asp:LinkButton>
                                                    <ajaxToolkit:ModalPopupExtender ID="MPSB" runat="server" TargetControlID="LBSB" PopupControlID="PLSB"
                                                        BackgroundCssClass="modalBackground" CancelControlID="CSB" DropShadow="true" />
                                                    <asp:CheckBox runat="server" ID="cbNewShipTo" AutoPostBack="true" OnCheckedChanged="cbNewShipTo_CheckedChanged"
                                                        Text="New" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Ship to ERP ID :
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtShipToERPID" ReadOnly="true" BackColor="#ebebe4" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Street1:
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtShipToStreet" ReadOnly="true" Width="165px" onkeyup="return checkdate(this,'35')" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Street2:
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtShipToStreet2" ReadOnly="true" Width="165px" onkeyup="return checkdate(this,'35')" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td>
                                                    <table width="165px">
                                                        <tr align="center">
                                                            <th align="left" style="color: Black">City
                                                            </th>
                                                            <th align="left" style="color: Black">State
                                                            </th>
                                                            <th align="left" style="color: Black">Zipcode
                                                            </th>
                                                            <th align="left" style="color: Black">Country
                                                            </th>
                                                        </tr>
                                                        <tr align="center">
                                                            <td align="center">
                                                                <asp:TextBox runat="server" ID="txtShipToCity" Width="60px" ReadOnly="true" />
                                                            </td>
                                                            <td align="center">
                                                                <asp:TextBox runat="server" ID="txtShipToState" Width="20px" ReadOnly="true" />
                                                            </td>
                                                            <td align="center">
                                                                <asp:TextBox runat="server" ID="txtShipToZip" Width="60px" ReadOnly="true" />
                                                            </td>
                                                            <td align="center">
                                                                <asp:TextBox runat="server" ID="txtShipToCountry" Width="20px" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Attention:
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtShipToAttention" Width="150px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Tel:
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtShipToTel" Width="150px" />
                                                    <asp:HiddenField ID="HFshiptofax" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td style="width: 33%" id="tdShipto" runat="server">
                                        <table width="100%" style="background-color: #fafafa">
                                            <tr>
                                                <td>Bill to Name :
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtBillName" ReadOnly="true" BackColor="#ebebe4" />
                                                    <asp:ImageButton ID="ibtnPickBill" runat="server" ImageUrl="~/Images/search.gif"
                                                        OnClick="ibtnPickBill_Click" />
                                                    <asp:CheckBox runat="server" Text="New" ID="cbNewBillTo" AutoPostBack="true" OnCheckedChanged="cbNewBillTo_CheckedChanged" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Bill to ERP ID :
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtBillID" ReadOnly="true" BackColor="#ebebe4" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Street1 :
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtBillToStreet" ReadOnly="true" Width="165px" onkeyup="return checkdate(this,'35')" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Street2:
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtBillToStreet2" ReadOnly="true" Width="165px" onkeyup="return checkdate(this,'35')" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td>
                                                    <table>
                                                        <tr align="center">
                                                            <th align="left" style="color: Black">City
                                                            </th>
                                                            <th align="left" style="color: Black">State
                                                            </th>
                                                            <th align="left" style="color: Black">Zipcode
                                                            </th>
                                                            <th align="left" style="color: Black">Country
                                                            </th>
                                                        </tr>
                                                        <tr align="center">
                                                            <td align="center">
                                                                <asp:TextBox runat="server" ID="txtBillToCity" Width="60px" ReadOnly="true" />
                                                            </td>
                                                            <td align="center">
                                                                <asp:TextBox runat="server" ID="txtBillToState" Width="20px" ReadOnly="true" />
                                                            </td>
                                                            <td align="center">
                                                                <asp:TextBox runat="server" ID="txtBillToZip" Width="60px" ReadOnly="true" />
                                                            </td>
                                                            <td align="center">
                                                                <asp:TextBox runat="server" ID="txtBillToCountry" Width="20px" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Attention:
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtBillToAttention" Width="150px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Tel:
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtBillToTel" Width="150px" />
                                                    <asp:HiddenField ID="HFbilltofax" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td id="tdEM" runat="server" visible="false" style="width: 33%">
                                        <table width="100%" style="background-color: #fafafa">
                                            <tr>
                                                <td>End Customer Name :
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtEMName" ReadOnly="true" BackColor="#ebebe4" />
                                                    <asp:ImageButton ID="ibtnPickEM" runat="server" ImageUrl="~/Images/search.gif"
                                                        OnClick="ibtnPickEM_Click" />
                                                    <asp:Button runat="server" Text="Clear" ID="btnClear" OnClick="btnClear_Click" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>End Customer ERP ID :
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtEMERPID" ReadOnly="true" BackColor="#ebebe4" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Street1:
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtEMStreet" ReadOnly="true" Width="165px" onkeyup="return checkdate(this,'35')" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Street2:
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtEMStreet2" ReadOnly="true" Width="165px" onkeyup="return checkdate(this,'35')" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td>
                                                    <table width="165px">
                                                        <tr align="center">
                                                            <th align="left" style="color: Black">City
                                                            </th>
                                                            <th align="left" style="color: Black">State
                                                            </th>
                                                            <th align="left" style="color: Black">Zipcode
                                                            </th>
                                                            <th align="left" style="color: Black">Country
                                                            </th>
                                                        </tr>
                                                        <tr align="center">
                                                            <td align="center">
                                                                <asp:TextBox runat="server" ID="txtEMCity" Width="60px" ReadOnly="true" />
                                                            </td>
                                                            <td align="center">
                                                                <asp:TextBox runat="server" ID="txtEMState" Width="20px" ReadOnly="true" />
                                                            </td>
                                                            <td align="center">
                                                                <asp:TextBox runat="server" ID="txtEMZip" Width="60px" ReadOnly="true" />
                                                            </td>
                                                            <td align="center">
                                                                <asp:TextBox runat="server" ID="txtEMCountry" Width="20px" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Attention:
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtEMAttention" Width="150px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Tel:
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtEMTel" Width="150px" />
                                                    <asp:HiddenField ID="HFEMfax" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <%--<table border="0" runat="server" id="tbSBSiebel">
                                <tr>
                                    <td>
                                        <table style="background-color: #fafafa">
                                            <tr>
                                                <td>
                                                    Ship to Address :
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" TextMode="MultiLine" Width="180px" Rows="3" Columns="1"
                                                        ID="txtShipAddresSiebel" ReadOnly="true" BackColor="#ebebe4" />
                                                    <asp:ImageButton ID="ibtnSA" runat="server" ImageUrl="~/Images/search.gif" OnClick="ibtnSA_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                        <table style="background-color: #fafafa">
                                            <tr>
                                                <td>
                                                    Bill to Address :
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" TextMode="MultiLine" Width="180px" Rows="3" Columns="1"
                                                        ID="txtBillAddresSiebel" ReadOnly="true" BackColor="#ebebe4" />
                                                    <asp:ImageButton ID="ibtnBA" runat="server" ImageUrl="~/Images/search.gif" OnClick="ibtnBA_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <asp:Panel ID="PLADS" runat="server" Style="display: none" CssClass="modalPopup">
                                <div style="text-align: right;">
                                    <asp:ImageButton ID="CADS" runat="server" ImageUrl="~/Images/del.gif" />
                                </div>
                                <div>
                                    <asp:UpdatePanel ID="UPADS" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <myASCX:PickADS ID="pickADS" runat="server" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </asp:Panel>
                            <asp:LinkButton ID="LBD" runat="server"></asp:LinkButton>
                            <ajaxToolkit:ModalPopupExtender ID="MPADS" runat="server" TargetControlID="LBD" PopupControlID="PLADS"
                                BackgroundCssClass="modalBackground" CancelControlID="CADS" DropShadow="true" />--%>
                        </asp:Panel>
                        <ajaxToolkit:CollapsiblePanelExtender ID="cpeDemo" runat="Server" TargetControlID="Panel1"
                            ExpandControlID="Panel2" CollapseControlID="Panel2" Collapsed="True" TextLabelID=""
                            ImageControlID="Image1" ExpandedText="" CollapsedText="" ExpandedImage="~/images/collapse.jpg"
                            CollapsedImage="~/images/expand.jpg" SuppressPostBack="true" SkinID="CollapsiblePanelDemo" />
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <table style="width: 630px">
                            <tr>
                                <td style="width: 120px">
                                    <asp:Label runat="server" ID="Label20" Text="<%$ Resources:myRs,CreatedBy %>" />
                                </td>
                                <td style="width: 220px">
                                    <asp:TextBox runat="server" ID="txtCreatedBy" ReadOnly="true" BackColor="#ebebe4"
                                        Width="200px" />
                                </td>
                                <td style="width: 80px">
                                    <asp:Label runat="server" ID="Label21" Text="<%$ Resources:myRs,QuoteDate %>" />
                                </td>
                                <td style="width: 200px">
                                    <asp:TextBox runat="server" ID="txtQuoteDate" ReadOnly="true" BackColor="#ebebe4"
                                        Width="130" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="Label4" Text="<%$ Resources:myRs,ExpiredDate %>" />
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtExpiredDate" BackColor="#ebebe4" onkeypress="return false;"
                                        Width="80" />
                                    <ajaxToolkit:CalendarExtender runat="server" ID="CalExt1" Enabled="false" TargetControlID="txtExpiredDate"
                                        Format="MM/dd/yyyy" />
                                    <ajaxToolkit:FilteredTextBoxExtender ID="Filteredtextboxextender6" runat="server"
                                        FilterType="Numbers, Custom" TargetControlID="txtExpiredDate" ValidChars="-/" />
                                    <asp:DropDownList ID="DDLExpiredDays" runat="server" Visible="false" onchange="javascript:onDDLExpiredDaysChange(this)">
                                        <asp:ListItem Selected="False" Value="30">30 Days</asp:ListItem>
                                        <asp:ListItem Value="60">60 Days</asp:ListItem>
                                        <asp:ListItem Value="90">90 Days</asp:ListItem>
                                        <asp:ListItem Value="120">120 Days</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label runat="server" Visible="false" ID="lbPreparedBy" Text="<%$ Resources:myRs,PreparedBy %>" />
                                </td>
                                <td>
                                    <asp:TextBox runat="server" Visible="false" ID="txtPreparedBy" Width="200px" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <table runat="server" id="tbOfficeGroup">
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="Label13" Text="Sales Office" />
                                </td>
                                <td>
                                    <%--<asp:TextBox runat="server" ID="txtOffice" ReadOnly="true" Width="120" BackColor="#ebebe4" />--%>
                                    <asp:HiddenField ID="h_office" runat="server" />
                                    <asp:DropDownList ID="drpSalesOffice" runat="server" Enabled="False" AutoPostBack="true"
                                        BackColor="#ebebe4" OnSelectedIndexChanged="drpSalesOffice_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lbGroup" Text="Sales Group" />
                                </td>
                                <td>
                                    <asp:DropDownList ID="drpGroup" runat="server" Width="120px">
                                        <asp:ListItem Value="" Selected="true" Text="Select..." />
                                        <asp:ListItem Value="eAutomation" />
                                        <asp:ListItem Value="ePlatform" />
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                        <table style="width: 520px">
                            <tr runat="server" id="trCurrencyRow">
                                <td style="width: 120px">
                                    <asp:Label runat="server" ID="Label14" Text="<%$ Resources:myRs,Currency %>" />
                                </td>
                                <td style="width: 400px">
                                    <asp:DropDownList ID="drpCurrency" runat="server" Visible="true">
                                        <asp:ListItem Value="" Selected="true" Text="Select..." />
                                        <asp:ListItem Value="USD" />
                                        <asp:ListItem Value="CNY" />
                                        <asp:ListItem Value="JPY" />
                                        <asp:ListItem Value="EUR" />
                                        <asp:ListItem Value="GBP" />
                                        <asp:ListItem Value="TWD" />
                                        <asp:ListItem Value="AUD" />
                                        <asp:ListItem Value="KRW" />
                                        <asp:ListItem Value="BRL" />
                                        <asp:ListItem Value="AUD" />
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 120px">
                                    <asp:Label runat="server" ID="Label15" Text="Prepared For" />
                                    :<br />
                                    (Internal Sales)
                                </td>
                                <td style="text-align: left; width: 400px">
                                    <asp:TextBox runat="server" ID="txtSalesEmail" onkeydown="return OnTxtPersonInfoKeyDown4();"
                                        Width="200px" ReadOnly="true" BackColor="#ebebe4" />
                                    <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" TargetControlID="txtSalesEmail"
                                        ServicePath="~/Services/AutoComplete.asmx" ServiceMethod="GetContact" MinimumPrefixLength="2">
                                    </ajaxToolkit:AutoCompleteExtender>
                                    <asp:HiddenField runat="server" ID="hfSalesRowId" />
                                    <asp:Label ID="LabelAlternativeSalesEmail" runat="server" Text=" Alternative Sales Email:" Visible="false" />
                                    <asp:TextBox runat="server" ID="txtSalesEmail1" onkeydown="return OnTxtPersonInfoKeyDown1();"
                                        Width="140px" Visible="false" />
                                    <ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server"
                                        TargetControlID="txtSalesEmail1" WatermarkText="Type SalesEmail1 Here" WatermarkCssClass="watermarked" />
                                    <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" TargetControlID="txtSalesEmail1"
                                        ServicePath="~/Services/AutoComplete.asmx" ServiceMethod="GetContact" MinimumPrefixLength="2">
                                    </ajaxToolkit:AutoCompleteExtender>
                                </td>
                            </tr>
                            <tr runat="server" id="trSalesContactNum">
                                <td>
                                    <asp:Label runat="server" ID="Label1SalesContactNumber" Text="Sales Contact Number" />
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtDirectPhone" />
                                </td>
                            </tr>
                            <tr runat="server" id="trCustomerContact">
                                <td>
                                    <asp:Label runat="server" ID="Label17" Text="Customer Contact" />
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtAttention" />
                                    <asp:TextBox runat="server" ID="txtAttentionName" Visible="false" Enabled="false" />
                                    <asp:ImageButton ID="ibtnPickAttention" runat="server" ImageUrl="~/Images/search.gif"
                                        OnClick="ibtnPickAttention_Click" />
                                    <asp:HiddenField ID="hfAttentionRowId" runat="server" />
                                    <asp:Panel ID="PLPickAttention" runat="server" Style="display: none" CssClass="modalPopup">
                                        <div style="text-align: right;">
                                            <asp:ImageButton ID="CancelButtonAttention" runat="server" ImageUrl="~/Images/del.gif" />
                                        </div>
                                        <div>
                                            <asp:UpdatePanel ID="UPPickAttention" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <myASCX:PickAttention ID="ascxPickAttention" runat="server" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </asp:Panel>
                                    <asp:LinkButton ID="lbDummyAttention" runat="server"></asp:LinkButton>
                                    <ajaxToolkit:ModalPopupExtender ID="MPPickAttention" runat="server" TargetControlID="lbDummyAttention"
                                        PopupControlID="PLPickAttention" BackgroundCssClass="modalBackground" CancelControlID="CancelButtonAttention"
                                        DropShadow="true" />
                                </td>
                            </tr>
                            <tr runat="server" id="trBankInfo">
                                <td>
                                    <asp:Label runat="server" ID="Label18" Text="<%$ Resources:myRs,BankAccount %>" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtBankInfo" runat="server" TextMode="MultiLine" Columns="50" Rows="5"
                                        MaxLength="28" Width="400px" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr runat="server" id="trInsideSales" visible="false">
                    <td>Inside Sales
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlAENCInsideSales" runat="server">
                            <%-- <asp:ListItem Value="Esther.Owens@Advantech.com">Esther Owens</asp:ListItem>
                            <asp:ListItem Value="Helen.Liu@Advantech.com">Helen Liu</asp:ListItem>
                            <asp:ListItem Value="Joe.Veltri@Advantech.com">Joe Veltri</asp:ListItem>
                            <asp:ListItem Value="Kayla.Nakai@Advantech.com">Kayla Nakai</asp:ListItem>
                            <asp:ListItem Value="Marady.Pek@Advantech.com">Marady Pek</asp:ListItem>
                            <asp:ListItem Value="Maria.Arellano@Advantech.com">Maria Arellano</asp:ListItem>
                            <asp:ListItem Value="Peter.Kim@Advantech.com">Peter Kim</asp:ListItem>
                            <asp:ListItem Value="Rocio.Arellano@Advantech.com">Rocio Arellano</asp:ListItem>
                            <asp:ListItem Value="Thuyn@Advantech.com">Thuy Tumacder</asp:ListItem>
                            <asp:ListItem Value="Valeriem@Advantech.com">Valerie Morgan</asp:ListItem>
                            <asp:ListItem Value="Vanessa.Mcdaniel@Advantech.com">Vanessa McDaniel</asp:ListItem>
                            <asp:ListItem Value="Yvonne.Torres@Advantech.com">Yvonne Torres</asp:ListItem>--%>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr runat="server" id="trDeliveryDate">
                    <td>
                        <asp:Label runat="server" ID="Label3" Text="<%$ Resources:myRs,DeliveryDate %>" />
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtDeliveryDate" Width="80" />
                        <asp:CheckBox ID="cbxDelDateFlag" runat="server" Text=" To Be Verified" />
                        <%--To Be Verified--%>
                        <ajaxToolkit:FilteredTextBoxExtender runat="server" ID="Filteredtextboxextender5"
                            TargetControlID="txtDeliveryDate" FilterType="Numbers, Custom" ValidChars="-/" />
                        <ajaxToolkit:CalendarExtender runat="server" ID="CalendarExtender1" CssClass="cal_Theme1"
                            TargetControlID="txtDeliveryDate" />
                    </td>
                </tr>
                <tr runat="server" id="trExempt" visible="false">
                    <td>Tax:
                    </td>
                    <td>
                        <asp:CheckBox ID="cbxIsTaxExempt" runat="server" Text="Tax Exempt?" />
                        <%-- <asp:CheckBox ID="cbxExempt" runat="server" Text="(Is/Not)" />--%>
                    </td>
                </tr>
                <tr runat="server" id="trShipTerm">
                    <td>
                        <asp:Label runat="server" ID="Label5" Text="Ship Condition" />
                    </td>
                    <td>
                        <asp:DropDownList ID="dlShipTerm" runat="server" Width="150px" Visible="true" />
                    </td>
                </tr>
                <tr runat="server" id="trIncoterm" visible="false">
                    <td>Incoterm
                    </td>
                    <td>
                        <table style="width: 520px">
                            <tr>
                                <td style="width: 160px">
                                    <asp:TextBox runat="server" ID="txtInco1" Width="120px" Text="FB1" Enabled="false" Visible="false" />
                                    <asp:DropDownList ID="DDLInco1" runat="server">
                                        <asp:ListItem Text="FB1" Value="FB1" Selected="True" />
                                        <asp:ListItem Text="EXW" Value="EXW" />
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 60px">ship via
                                </td>
                                <td style="width: 120px">
                                    <asp:TextBox runat="server" ID="txtInco2" Width="120px" MaxLength="28" onkeyup="return checkdate(this,'28')" />
                                </td>
                                <td style="width: 180px">( Maximum 28 Characters )
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="Label6" Text="<%$ Resources:myRs,PaymentTerms %>" />
                    </td>
                    <td>
                        <asp:DropDownList ID="dlPaymentTerm" runat="server" Width="250px" Visible="true" />
                    </td>
                </tr>
                <%--<tr runat="server" id="trEmployee" class="EUhide">--%>
                <tr runat="server" id="trEmployee">
                    <td>
                        <asp:Label runat="server" ID="Label22" Text="Sales Employee" />
                    </td>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <asp:DropDownList ID="drpSE" runat="server" Width="250px" Visible="true" />
                                </td>
                                <td>Sales Employee2
                                </td>
                                <td>
                                    <asp:DropDownList ID="drpSE2" runat="server" Width="250px" Visible="true" />
                                </td>
                                <td>Sales Employee3
                                </td>
                                <td>
                                    <asp:DropDownList ID="drpSE3" runat="server" Width="250px" Visible="true" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr runat="server" id="trAJPEmployee" visible="false">
                    <td>
                        <asp:Label runat="server" ID="Label19" Text="Sales Employee (AJP)" />
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtAJPSalesEmployee" Width="250px" />
                    </td>
                </tr>
                <tr runat="server" id="trRefPO" visible="false">
                    <td>PO#:
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtRefPONO" Width="100px" />
                        <asp:Label ID="lbUploadPOFile" runat="server" Text="Upload PO File:" />
                        <asp:FileUpload runat="server" ID="updPO" /><asp:Button runat="server" ID="btnUploadPO"
                            Text="Upload PO" />
                    </td>
                </tr>
                <tr runat="server" id="trSourcefrom" visible="false">
                    <td colspan="2">
                        <div class="GlobalHide USshow">
                            Source from:     &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:DropDownList ID="ddlQuoteSource" runat="server">
                                <asp:ListItem Text="eStore"></asp:ListItem>
                                <asp:ListItem Text="Amazon"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </td>
                </tr>
                <tr runat="server" id="trCareOn" visible="false">
                    <td>C/O:
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtCareOn" Width="100px" />
                    </td>
                </tr>
                <tr runat="server" id="trABRQuoteType" visible="false">
                    <td>Quote Type:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlABRQuoteType" runat="server">
                            <asp:ListItem Text="ZQTI" Value="ZQTI"></asp:ListItem>
                            <asp:ListItem Text="ZQTC" Value="ZQTC"></asp:ListItem>
                            <asp:ListItem Text="ZQTR" Value="ZQTR"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr runat="server" id="trFreightInsuSPTax">
                    <td></td>
                    <td>
                        <table>
                            <tr>
                                <td width="200">
                                    <asp:Label runat="server" ID="Label10" Text="<%$ Resources:myRs,Tax %>" />
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtTax" Width="50px" Style="text-align: right;" />%
                                    <ajaxToolkit:FilteredTextBoxExtender runat="server" ID="Filteredtextboxextender4"
                                        TargetControlID="txtTax" FilterType="Numbers, Custom" ValidChars="." />
                                </td>
                                <td width="200">
                                    <asp:Label runat="server" ID="LabelFreight" Text="<%$ Resources:myRs,Freight %>" />
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtFreight" Width="50px" />
                                    <ajaxToolkit:FilteredTextBoxExtender runat="server" ID="Filteredtextboxextender1"
                                        TargetControlID="txtFreight" FilterType="Numbers, Custom" ValidChars=".TBD" />
                                </td>
                            </tr>
                            <tr runat="server" id="trSpecialChargeInsurance">
                                <td width="200">
                                    <asp:Label runat="server" ID="LabelSpecialCharge" Text="<%$ Resources:myRs,SpecialCharge %>" />
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtSpecialCharge" Width="50px" />
                                    <ajaxToolkit:FilteredTextBoxExtender runat="server" ID="Filteredtextboxextender3"
                                        TargetControlID="txtSpecialCharge" FilterType="Numbers, Custom" ValidChars="." />
                                </td>
                                <td width="200">
                                    <asp:Label runat="server" ID="LabelInsurance" Text="<%$ Resources:myRs,Insurance %>" />
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtInsurance" Width="50px" />
                                    <ajaxToolkit:FilteredTextBoxExtender runat="server" ID="Filteredtextboxextender2"
                                        TargetControlID="txtInsurance" FilterType="Numbers, Custom" ValidChars="." />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr runat="server" id="txtQuoteDesc">
                    <td>
                        <asp:Label runat="server" ID="Label_Description" Text="<%$ Resources:myRs,QuoteDescription %>" />
                        <span runat="server" id="SP_star1" style="color: #ff0000">*</span> :
                    </td>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <asp:TextBox runat="server" ID="txtCustomId" BackColor="Orange" Width="250px" />
                                </td>
                                <td>
                                    <div id="divEngineer" runat="server" visible="false" style="width: 700px">
                                        <asp:Label runat="server" ID="LabelEngineer" Text="Engineer:" />
                                        <asp:TextBox runat="server" ID="TBEngineer" BackColor="Orange" />

                                        <asp:Label runat="server" ID="Label_EngineerTel" Text="Engineer Tel:" />
                                        <asp:TextBox runat="server" ID="TBEngineerTEL" BackColor="Orange" />

                                        <asp:Label runat="server" ID="Label_Warrenty" Text="Warranty:" />
                                        <asp:TextBox runat="server" ID="txtWarranty" BackColor="Orange" Text="2 Years" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr runat="server" id="trQuoteNote">
                    <td>
                        <asp:Label runat="server" ID="Label11" Text="<%$ Resources:myRs,QuoteNotes %>" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtQuoteNote" runat="server" TextMode="MultiLine" Columns="50" Rows="5"
                            MaxLength="28" Width="600" />
                    </td>
                </tr>
                <tr runat="server" id="trLowGPReason" class="TWhide CNhide">
                    <td>
                        <asp:Label runat="server" ID="Label12" ForeColor="#FFA500" Text="<%$ Resources:myRs,RelatedInformation %>" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtRelatedInfo" runat="server" TextMode="MultiLine" Columns="50"
                            Rows="5" MaxLength="28" Width="600" BackColor="Orange" />
                    </td>
                </tr>
                <tr runat="server" id="trEmailGreeting" class="TWhide CNhide">
                    <td>
                        <asp:Label runat="server" ID="Label8" Text="<%$ Resources:myRs,EmailGreeting %>" />
                    </td>
                    <td>
                        <uc1:NoToolBarEditor runat="server" ID="txtEmailGreeting" Width="615px" Height="200px" AutoFocus="false" />
                    </td>
                </tr>
                <tr runat="server" id="trSignature">
                    <td>
                        <asp:Label runat="server" ID="Label16" Text="<%$ Resources:myRs,Signature %>" />
                    </td>
                    <td>
                        <asp:Image ID="imgSignature" runat="server" Visible="false" />
                        <asp:ImageButton runat="server" ID="ImgButtonRemoveSignature" ImageUrl="~/Images/del.gif"
                            AlternateText="Remove Signature" />
                        <asp:ImageButton runat="server" ID="ImgButtonPickSignature" ImageUrl="~/Images/search.gif"
                            AlternateText="Pick Signature" />
                        <asp:HiddenField ID="HFSignatureID" runat="server" />
                        <%--Pick Signsture Panel--%>
                        <asp:Panel ID="PLPickSignature" runat="server" Style="display: none" CssClass="modalPopup">
                            <div style="text-align: right;">
                                <asp:ImageButton ID="CancelButtonSignature" runat="server" ImageUrl="~/Images/del.gif" />
                            </div>
                            <div>
                                <asp:UpdatePanel ID="UPPickSignature" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <myASCX:PickSignature1 ID="ascxPickSignature1" runat="server" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </asp:Panel>
                        <asp:LinkButton ID="lbDummySignature" runat="server" />
                        <ajaxToolkit:ModalPopupExtender ID="MPPickSignature" runat="server" TargetControlID="lbDummySignature"
                            PopupControlID="PLPickSignature" BackgroundCssClass="modalBackground" CancelControlID="CancelButtonSignature"
                            DropShadow="false" />
                        <span style="padding-left: 10px;">
                            <asp:HyperLink ID="Hysignature" runat="server" NavigateUrl="~/User/Signature.aspx">Upload my signature</asp:HyperLink></span>
                    </td>
                </tr>
                <tr runat="server" id="trSpecialTandC" class="TWhide CNhide">
                    <td>
                        <asp:Label runat="server" ID="Label9" Text="<%$ Resources:myRs,SpecialTandC %>" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtSpecialTandC" runat="server" TextMode="MultiLine" Columns="50"
                            Rows="5" MaxLength="28" Width="600" BackColor="Orange" />
                    </td>
                </tr>
                <tr runat="server" id="trRepeatedOrder" visible="false">
                    <td>
                        <asp:Label runat="server" ID="Label23" Text="<%$ Resources:myRs,isRepeatedOrder %>" />
                    </td>
                    <td>
                        <asp:CheckBox ID="cbxIsRepeatedOrder" runat="server" />
                    </td>
                </tr>
                <tr runat="server" id="trReVisionControl" visible="false">
                    <td>
                        <asp:Label runat="server" ID="Label7" Text="<%$ Resources:myRs,Revision %>" />
                    </td>
                    <td>
                        <asp:Button ID="ButtonCreateNewRevision" runat="server" Text="Create New Revision" />
                        Revision Number&nbsp;:&nbsp;
                        <asp:DropDownList ID="DDLRevisionNumber" runat="server" AutoPostBack="true">
                        </asp:DropDownList>
                        <asp:Button ID="ButtonMakeItActive" runat="server" Text="Make This Version Active" />
                        <asp:CheckBox ID="cbxIsActive" runat="server" Text="Active" Visible="false" />
                    </td>
                </tr>
            </table>
            <div style="display: none;">
                <div id="divCustomerCreditInfo">
                    <myASCX:CustomerCreditInfo ID="ascxCustomerCreditInfo" runat="server" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <table runat="server" id="tableopty">
        <tr>
            <td style="background-color: #FFFFFF; height: 20px; width: 10%;" valign="top">
                <div align="right">
                    <font color="#ffa500">*Opportunity:</font><br />
                </div>
            </td>
            <td style="background-color: #FFFFFF; height: 20px; width: 90%;" align="left" colspan="3">
                <asp:UpdatePanel runat="server" ID="UP_Opty" UpdateMode="Conditional" ChildrenAsTriggers="false">
                    <ContentTemplate>
                        <asp:Label ID="LabelOptyName" runat="server" Text="Opportunity Name:" Visible="false" />
                        <asp:TextBox runat="server" ID="txtOptyName" MaxLength="300" Width="150px" ReadOnly="true"
                            BackColor="#ebebe4" Visible="false" />
                        <asp:Label ID="LabelOptyStage" runat="server" Text="Stage:" Visible="false" />
                        <asp:DropDownList ID="DDLOptyStage" runat="server" Visible="false">
                            <asp:ListItem Selected="True">25% Proposing/Quoting</asp:ListItem>
                            <asp:ListItem>50% Negotiating</asp:ListItem>
                            <asp:ListItem>75% Waiting for PO/Approval</asp:ListItem>
                            <asp:ListItem>100% Won-PO Input in SAP</asp:ListItem>
                        </asp:DropDownList>
                        <asp:ImageButton runat="server" ID="ibtn_PickOpty" ImageUrl="~/Images/search.gif"
                            OnClick="ibtn_PickOpty_Click" />
                        <asp:Button ID="ButtonNewOpty" runat="server" Text="New Opportunity" OnClick="ButtonNewOpty_Click" />
                        <asp:CheckBox ID="cbx_NewOpty" Text="New Opportunity" runat="server" OnCheckedChanged="cbx_NewOpty_CheckedChanged"
                            AutoPostBack="true" Visible="false" />
                        <asp:Label ID="Label24" runat="server" Text="Opportunity ID:" Visible="false" />
                        <asp:TextBox runat="server" ID="txtOptyRowID" Width="150px" ReadOnly="true" BackColor="#ebebe4"
                            Visible="false" />
                    </ContentTemplate>
                </asp:UpdatePanel>
                (Opportunity will not be created until quotation is released to customer.)
            </td>
        </tr>
    </table>
    <%--Pick Opportunity Panel--%>
    <asp:Panel ID="PLPickOpty" runat="server" Style="display: none" CssClass="modalPopup">
        <div style="text-align: right;">
            <asp:ImageButton ID="CancelButtonOpty" runat="server" ImageUrl="~/Images/del.gif" />
        </div>
        <div>
            <asp:UpdatePanel ID="UPPickOpty" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <myASCX:Pickopty1 ID="ascxPickopty1" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </asp:Panel>
    <asp:LinkButton ID="lbDummyOpty" runat="server"></asp:LinkButton>
    <ajaxToolkit:ModalPopupExtender ID="MPPickOpty" runat="server" TargetControlID="lbDummyOpty"
        PopupControlID="PLPickOpty" BackgroundCssClass="modalBackground" CancelControlID="CancelButtonOpty"
        DropShadow="false" />
    <asp:HiddenField ID="HFOriginalQuoteID" runat="server" />
    <asp:HiddenField ID="HFAKRQuoteListPriceOnly" runat="server" />
    <table>
        <tr>
            <td align="center">
                <asp:Button ID="btnConfirm" runat="server" Text="<%$ Resources:myRs,Confirm %>" OnClick="btnConfirm_Click" />
            </td>
        </tr>
    </table>


    <%--<input type="button" id="btnOpenDialog" value="Open Confirm Dialog" />--%>
    <div id="dialog-confirm"></div>
    <div id="MessageDialog"></div>

    <%--Ryan 20170822 Add for FancyBox--%>
    <link rel="Stylesheet" href="../Js/FancyBox/jquery.fancybox.css" type="text/css" />
    <script type="text/javascript" src="../Js/FancyBox/jquery.fancybox.js"></script>

    <%--Ryan 20170214 Add for tokeninput--%>
    <link href="../Css/token-input-facebook.css" rel="stylesheet" />
    <script src="../Js/jquery.tokeninput.js"></script>
    <style type="text/css">
        ul.token-input-list-facebook {
            overflow: hidden;
            height: auto !important;
            height: 1%;
            border: 1px solid #8496ba;
            cursor: text;
            font-size: 12px;
            font-family: Verdana;
            min-height: 1px;
            z-index: 999;
            margin: 0;
            padding: 0;
            background-color: #fff;
            list-style-type: none;
            clear: left;
            width: 350px;
            display: inline-flex;
        }

            ul.token-input-list-facebook li:hover {
                background-color: #ffffff;
            }
    </style>

    <script type="text/javascript">
        $(document).ready(function () {
            TokenInputforAJP();
        });

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    TokenInputforAJP();
                }
            });
        };

        function TokenInputforAJP() {

            //only execute for AJP
            <% If _IsJPAonlineUser Then%>

            // if AJP Employee text box is empty, needs to check if this quotation has set employee before and bring it back.
            if (!$("#<%=txtAJPSalesEmployee.ClientID%>").val()) {
                var postData = { QuoteID: "<%=Request("UID")%>" };
                $.ajax({
                    url: "<%= Util.GetRuntimeSiteUrl()%>/Services/AutoComplete.asmx/GetDefaultEmployee",
                    type: "POST",
                    dataType: 'json',
                    data: postData,
                    success: function (retData) {
                        if (retData.id && retData.name) {
                            $("#<%=txtAJPSalesEmployee.ClientID%>").tokenInput("add", { id: retData.id, name: retData.name });
                        }
                    },
                    error: function (msg) {
                    }
                });
            }


            $("#<%=txtAJPSalesEmployee.ClientID%>").tokenInput("<%=System.IO.Path.GetFileName(Request.ApplicationPath) %>/Services/AutoComplete.asmx/GetTokenInputSalesEmployee?ORGID=JP01", {
                theme: "facebook", searchDelay: 200, minChars: 1, tokenDelimiter: ";", hintText: "Type Name",
                tokenLimit: 1, preventDuplicates: true, resizeInput: false, resultsLimit: 5,
                resultsFormatter: function (data) {
                    return "<li style='border-bottom: 1px solid #003377;'>" + "<span style='font-weight: bold;font-size: 14px;'>" + data.name + "</span><br/>" + "<span style='color:gray;'>" + data.id + "</span></li>";
                },
                onAdd: function (data) {
                    $("#<%=txtAJPSalesEmployee.ClientID%>").val(data.id);
                }
            });

            <% End If%>
        }

        function OnTxtPersonInfoKeyDown4() {

            var acNameClientId = '<%=Me.AutoCompleteExtender4.ClientID %>';

            var acName = $find(acNameClientId);


            if (acName != null)
                acName.set_contextKey("");
        }
        function OnTxtPersonInfoKeyDown1() {

            var acNameClientId = '<%=Me.AutoCompleteExtender1.ClientID %>';

            var acName = $find(acNameClientId);


            if (acName != null)
                acName.set_contextKey("");
        }
        function checkdate(id, Maximum) {
            if (id.value.length > Maximum) {
                alert('More than ' + Maximum + ' characters')
                id.focus()
                return false
            }
            else {
                return true
            }
        }

        function onDDLExpiredDaysChange(e) {

            var numberOfDaysToAdd = e.options[e.selectedIndex].value;
            var someDate = new Date();
            someDate.setDate(someDate.getDate() + parseInt(numberOfDaysToAdd));

            var dd = someDate.getDate();
            var mm = someDate.getMonth() + 1;
            var y = someDate.getFullYear();

            var someFormattedDate = mm + '/' + dd + '/' + y;

            var expireddate = document.getElementById('<%=Me.txtExpiredDate.ClientID%>');
            expireddate.value = someFormattedDate;
            return true;
        }



        function fnOpenNormalDialog() {
            var strSC = '20104050';
            var strDT = 'E10';
            var postData = JSON.stringify({ SalesCode: strSC, DistrictOnForm: strDT });
            $.ajax({
                type: "POST",
                url: "<%= Util.GetRuntimeSiteUrl()%>/Services/quoteExit.asmx/IsMatchSalesDistrict",
                data: postData,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var retMsg = $.parseJSON(msg.d);
                    retMsg.pro
                    if (retMsg.ProcessStatus == true) {
                        alert('1111');
                        //$("#dialog-confirm").html(retMsg.ProcessMessage);
                    }
                    else {
                        alert('222');
                        //$('#ProductRelatedInfoFrame').html('Internal Error! Please Contact <a href="mailto:MyAdvantech@advantech.com">MyAdvantech Team</a> to report this issue, thank you.');
                    }
                },
                error: function (msg) {
                }
            }
            );





            //$("#dialog-confirm").html("Confirm Dialog Box");

            //// Define the Dialog and its properties.
            //$("#dialog-confirm").dialog({
            //    resizable: false,
            //    modal: true,
            //    title: "Modal",
            //    height: 250,
            //    width: 400,
            //    buttons: {
            //        "Yes": function () {
            //            $(this).dialog('close');
            //            return true;
            //        },
            //        "No": function () {
            //            $(this).dialog('close');
            //            return false;
            //        }
            //    }
            //});
        }

        $('#btnOpenDialog').click(fnOpenNormalDialog);

        function callback(value) {
            if (value) {
                alert("Confirmed");
            } else {
                alert("Rejected");
            }
        }

        function checkANADistrict(districtID, seID) {
            var district = document.getElementById(districtID).value;
            var se = document.getElementById(seID);
            var seValue = se.options[se.selectedIndex].value;

            $.ajax({
                type: "POST",
                url: "<%= Util.GetRuntimeSiteUrl()%>/Services/quoteExit.asmx/CheckANASalesDistrict",
                data: JSON.stringify({ SalesID: seValue, District: district }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data.d === false) {
                        $("#MessageDialog")
                            .html("<h3> " + district + " is not in your assigned district list. Do you want to modify it ?</h3>").dialog(
                            "option",
                            "buttons", {
                                "Yes": function () {
                                    $(this).dialog("close");
                                    $("#" + districtID)
                                        .css({ "border": "1px solid red" })
                                        .focus();
                                    return false;
                                },
                                "No": function () {
                                    $("#" + districtID).css({ "border": "1px solid red" });
                                    $(this).dialog("close");
                                }
                            }
                        );
                        $("#MessageDialog").dialog("open");
                    }
                    else
                        $("#" + districtID).css({ "border": "" });
                },
                error: function (err) {
                    $("body").html('Internal Error! Please Contact <a href="mailto:MyAdvantech@advantech.com">MyAdvantech Team</a> to report this issue, thank you.');
                    return false;
                }
            });
        };

        //ICC 2015/10/16 Check district data is valid. Use sales employee's sales_code to find it is in dbo.ANA_AOL_SALES_DISTRICT or not. This is for AAC group.
        function pageLoad(sender, e) {

            //Check district
            <% If _IsUSAonline AndAlso _IsUSAOnlineEP Then%>
                <% If Not Me.txtSalesDistrict Is Nothing AndAlso Not Me.txtSalesGroup Is Nothing AndAlso Not Me.drpSE Is Nothing Then%>
            var districtID = '<%=Me.txtSalesDistrict.ClientID%>';
            var seID = '<%=Me.drpSE.ClientID%>';

            $("#" + districtID).blur(function () {
                checkANADistrict(districtID, seID);
            });
            $("#" + seID).change(function () {
                checkANADistrict(districtID, seID);
            });
            <% End If%>
            <% End If%>

            //Check PO no. exists
            <%--<% If _IsUSAAC Then%>
                <% If Not Me.txtRefPONO Is Nothing AndAlso Not Me.drpOrg Is Nothing Then%>
                var PoNoID = '<%=Me.txtRefPONO.ClientID%>';
                var OrgID = '<%=Me.drpOrg.ClientID%>';
                var ERPID = '<%=Me.hfErpId.ClientID%>';
                $("#" + PoNoID).blur(function () {
                    var PoNo = document.getElementById(PoNoID).value;
                    if ($.trim(PoNo).length > 0) {
                        var Org = document.getElementById(OrgID);
                        var OrgValue = Org.options[Org.selectedIndex].value;
                        var ERP = document.getElementById(ERPID).value;
                        $.ajax({
                            type: "POST",
                            url: "<%= Util.GetRuntimeSiteUrl()%>/Services/quoteExit.asmx/IsPONumberExisting",
                            data: JSON.stringify({ ORGID: OrgValue, ERPID: ERP, PO: PoNo }),
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (data) {
                                if (data.d === true) {
                                    $("#MessageDialog")
                                        .html("<h3>The PO# '" + PoNoID + "' already exists!</h3>").dialog(
                                        "option",
                                        "buttons", {
                                            "Confirm": function () {
                                                $(this).dialog("close");
                                                $("#" + PoNoID)
                                                    .css({ "border": "1px solid red" });
                                                return false;
                                            }
                                        }
                                    );
                                    $("#MessageDialog").dialog("open");
                                }
                                else
                                    $("#" + PoNoID).css({ "border": "" });
                            },
                            error: function (err) {
                                $("body").html('Internal Error! Please Contact <a href="mailto:MyAdvantech@advantech.com">MyAdvantech Team</a> to report this issue, thank you.');
                                return false;
                            }
                        });
                    }
                    else
                        $("#" + PoNoID).css({ "border": "" });
                
                });
                <%End If%>
            <% End If%>--%>


            $("#MessageDialog").dialog({
                model: true,
                title: "Message",
                autoOpen: false,
                width: "auto",
                resizable: false
            });
        }

        function ShowFancyBox() {
            var gallery = [{
                href: "#divCustomerCreditInfo"
            }];

            $.fancybox(gallery, {
                'autoSize': true,
                'autoCenter': true
            });
        }
    </script>
</asp:Content>
