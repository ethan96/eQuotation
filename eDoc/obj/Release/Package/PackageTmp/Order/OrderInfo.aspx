<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/myMaster.master"
    CodeBehind="OrderInfo.aspx.vb" Inherits="EDOC.OrderInfo" %>

<%@ Register Src="~/ASCX/Order/PartialDeliver.ascx" TagName="PartialDeliver" TagPrefix="uc2" %>
<%@ Register Src="~/ASCX/Order/OrderAddress.ascx" TagName="OrderAddress" TagPrefix="uc1" %>
<%@ Register Src="~/ASCX/Order/AuthCreditResult.ascx" TagName="AuthCreditResult"
    TagPrefix="uc3" %>
<%@ Register Src="~/ASCX/Order/PickCalendar.ascx" TagName="Calendar" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"><center>
<fieldset style="margin-top:3%; width:1000px;">
	<legend><span style="font-weight:bold; font-size:16px; color:#666666">Order Information</span></legend>
    <table width="100%" class="NoBord"  style="margin-top:1%;">
        <tr id="orderaddressesforus" runat="server">
            <td colspan="2">
                <table>
                
                    <tr>
                        <td style="vertical-align:top">
                        <fieldset>
	                        <legend style="font-weight:bold; font-size:14px; color:#666666 ">Sold to</legend>
                            <uc1:OrderAddress ID="soldtoaddress" runat="server" IsCanPick="false" Type="SOLDTO" />
                            </fieldset>
                        </td>
                        <td style="vertical-align:top">
                        <fieldset>
	                        <legend style="font-weight:bold; font-size:14px; color:#666666 ">Ship to</legend>
                            <uc1:OrderAddress ID="shiptoaddress" runat="server" Type="S" /></fieldset>
                        </td>
                        <td id="tdbilltoascx" runat="server" visible="false" style="vertical-align:top">
                         <fieldset>
	                        <legend style="font-weight:bold; font-size:14px; color:#666666 ">Bill to</legend>
                            <uc1:OrderAddress ID="billtoaddress" runat="server" Type="B" /></fieldset>
                        </td>
                        <td id="tdendcustomer" runat="server" visible="false" style="vertical-align:top">
                         <fieldset>
	                        <legend style="font-weight:bold; font-size:14px; color:#666666 ">End Customer</legend>
                            <uc1:OrderAddress ID="endcustomer" runat="server" Type="EM" /></fieldset>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="h5" style="width: 25%">
                <asp:Literal runat="server" ID="litRD">Required Date</asp:Literal>:
            </td>
            <td>
                <asp:UpdatePanel runat="server" ID="UpReqDate" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox runat="server" ID="txtreqdate"></asp:TextBox>
                        (
                        <asp:Literal runat="server" ID="litRDF">yyyy/MM/dd </asp:Literal>)
                        <asp:Panel ID="PLPickCalendar" runat="server" Style="display: none" CssClass="modalPopup">
                            <div style="text-align: right;">
                                <asp:ImageButton ID="CancelCalendar" runat="server" ImageUrl="~/Images/del.gif" />
                            </div>
                            <div>
                                 <asp:UpdatePanel runat="server" ID="UPASCXCAL" UpdateMode="Conditional">
                        <ContentTemplate>
                                <uc1:Calendar ID="ascxCalendar" runat="server" /></ContentTemplate></asp:UpdatePanel>
                            </div>
                        </asp:Panel>
                        <ajaxToolkit:ModalPopupExtender ID="MPPickCalendar" runat="server" TargetControlID="txtreqdate"
                            PopupControlID="PLPickCalendar" BackgroundCssClass="modalBackground" CancelControlID="CancelCalendar"
                            DropShadow="true" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr runat="server" id="trDelPlant" visible="false">
            <td class="h5" style="width: 25%">
                Delivery Plant:
            </td>
            <td>
                <asp:DropDownList ID="drpDelPlant" runat="server">
                    <asp:ListItem Value="SGH1">SGH1</asp:ListItem>
                    <asp:ListItem Value="TWH1">TWH1</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="h5" style="width: 25%">
                PO Number:
            </td>
            <td>
                <table>
                    <tr>
                        <td>
                            <asp:TextBox runat="server" ID="txtPONo" AutoPostBack="true" OnTextChanged="txtPONo_TextChanged" />
                        </td>
                        <td>
                            <asp:UpdatePanel runat="server" ID="upPoDuplicateMsg" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Label runat="server" ID="lbPoDuplicateMsg" Font-Bold="true" ForeColor="Tomato"
                                        Font-Size="X-Small" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="txtPONo" EventName="TextChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="h5">
                PO Date:
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtPODate" ReadOnly="true"></asp:TextBox>
                <ajaxToolkit:CalendarExtender TargetControlID="txtPODate" runat="server" Format="yyyy/MM/dd"
                    ID="calDate" />
            </td>
        </tr>
        <tr id="TRAttention" runat="server" visible="false">
            <td class="h5">
                Attention:
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtAttention"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="left">
                <table border="0">
                    <tr>
                        <td align="left" width="20%" >
                            <uc2:PartialDeliver ID="PartialDeliver1" runat="server" />
                        </td>
                        <td width="80%" runat="server" visible="false" id="tbExempt">
                            <asp:CheckBox ID="cbxIsTaxExempt" runat="server" Text="Tax Exempt?" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr runat="server" id="tyEarlyShip" visible="false">
            <td colspan="2">
                <asp:CheckBox runat="server" ID="cbEarlyShipmentAllowed" Text=" Early Shipment Allowed?"
                    Font-Bold="true" />
            </td>
        </tr>
        <tr>
            <td class="h5" >
                <span runat="server" id="spShipc">Ship Condition:</span> <span runat="server" id="SpanInct">
                    Incoterm:</span>
            </td>
            <td>
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td width="10%">
                            <asp:DropDownList runat="server" ID="drpShipCondition">
                            </asp:DropDownList>
                            <asp:DropDownList runat="server" ID="drpIncoterm">
                            </asp:DropDownList>
                        </td>
                        <td class="h5" width="10%" style="padding-left: 19px; padding-right: 4px;">
                            Ship via:
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtIncoterm" onblur="return checkdate(this,'28')"></asp:TextBox>
                            ( Maximum 28 Characters )
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <%--<tr>
            <td class="h5">
                Incoterm:
            </td>
            <td>
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:DropDownList runat="server" ID="drpIncoterm">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>--%>
        <tr runat="server" id="trPayTerm" visible="false" valign="top">
            <td class="h5">
                Payment Term:<br />
                (Visible to internal user only)
            </td>
            <td>
                <table>
                    <tr valign="top">
                        <td>
                            <asp:DropDownList runat="server" ID="dlPayterm" OnSelectedIndexChanged="dlPayterm_SelectedIndexChanged"
                                AutoPostBack="True" />
                        </td>
                    </tr>
                    <tr valign="top">
                        <td>
                            <asp:UpdatePanel runat="server" ID="upCreditCard" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table align="left" border="0" cellpadding="0" cellspacing="0" runat="server" id="tbCreditCardInfo"
                                        visible="false">
                                        <tr>
                                            <td class="h5" width="20%" align="left">
                                                Card Type:
                                            </td>
                                            <td width="10%">
                                                <asp:DropDownList runat="server" ID="dlCCardType">
                                                    <asp:ListItem Value="AMEX" Text="American Express" />
                                                    <asp:ListItem Value="DISC" Text="Discover" />
                                                    <asp:ListItem Value="MC" Text="Master -/Euro Card" />
                                                    <asp:ListItem Value="VISA" Text="Visa Card" />
                                                </asp:DropDownList>
                                            </td>
                                            <td class="h5" width="20%">
                                                Credit Card Number:
                                            </td>
                                            <td width="50%" style="padding-left: 5px;">
                                                <asp:TextBox runat="server" ID="txtCreditCardNumber" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="h5" align="left">
                                                Holder's Name:
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtCCardHolder" />
                                            </td>
                                            <td class="h5" align="left">
                                                Expire Date:
                                            </td>
                                            <td style="padding-left: 5px;">
                                                <table>
                                                    <tr>
                                                        <td width="10%">
                                                            <asp:DropDownList runat="server" ID="dlCCardExpYear" />
                                                        </td>
                                                        <td width="90%">
                                                            <asp:DropDownList runat="server" ID="dlCCardExpMonth">
                                                                <asp:ListItem Text="January" Value="1" />
                                                                <asp:ListItem Text="February" Value="2" />
                                                                <asp:ListItem Text="March" Value="3" />
                                                                <asp:ListItem Text="April" Value="4" />
                                                                <asp:ListItem Text="May" Value="5" />
                                                                <asp:ListItem Text="June" Value="6" />
                                                                <asp:ListItem Text="July" Value="7" />
                                                                <asp:ListItem Text="August" Value="8" />
                                                                <asp:ListItem Text="September" Value="9" />
                                                                <asp:ListItem Text="October" Value="10" />
                                                                <asp:ListItem Text="November" Value="11" />
                                                                <asp:ListItem Text="December" Value="12" />
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="h5" align="left">
                                                CVV Code:
                                            </td>
                                            <td colspan="3" style="padding-left: 5px;">
                                                <asp:TextBox runat="server" ID="txtCCardVerifyValue" Width="45" AutoPostBack="true" />
                                            </td>
                                        </tr>
                                        <tr valign="top">
                                            <td>
                                                <asp:LinkButton runat="server" ID="lBtnAuthCcInfo" Text="Verify Credit Card" OnClick="lBtnAuthCcInfo_Click" />
                                                <br />
                                                <asp:CheckBox runat="server" ID="ckbUserNewBillAddress" AutoPostBack="true" Text="Use New Bill Address"
                                                    OnCheckedChanged="ckbUserNewBillAddress_OnCheckedChanged" />
                                            </td>
                                            <td colspan="3">
                                                <table><tr><td width="40%">
                                                <uc1:OrderAddress ID="newbilladdress" runat="server" Editable="true" Visible="false" />
                                                <td  width="60%"></td>
                                                </td></tr></table>
                                            </td>
                                        </tr>
                                        <tr style="height: 10px">
                                            <td colspan="2">
                                                <asp:Label runat="server" ID="lbCCardMsg" Font-Bold="true" ForeColor="Tomato" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="dlPayterm" EventName="SelectedIndexChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="trSE" runat="server" visible="false">
            <td class="h5">
                Sales Employee
            </td>
            <td>
                <table>
                    <tr>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlSE" Width="150">
                            </asp:DropDownList>
                        </td>
                        <td runat="server" id="tdE2name" visible="false">
                            Sales Employee2
                        </td>
                        <td runat="server" id="tdE2" visible="false">
                            <asp:DropDownList runat="server" ID="ddlSE2" Width="150">
                            </asp:DropDownList>
                        </td>
                        <td runat="server" id="tdE3name" visible="false">
                            Sales Employe3
                        </td>
                        <td runat="server" id="tdE3" visible="false">
                            <asp:DropDownList runat="server" ID="ddlSE3" Width="150">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="trKeyInPerson" runat="server" visible="false">
            <td class="h5">
                Key In Person
            </td>
            <td>
                <asp:DropDownList runat="server" ID="ddlKeyInPerson" Width="150">
                </asp:DropDownList>
            </td>
        </tr>
        <tr id="trDSGSO" runat="server" visible="false">
            <td class="h5">
            </td>
            <td>
                <asp:UpdatePanel runat="server" ID="upDistChannDiv" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table>
                            <tr>
                                <td class="h5">
                                    Distribution Channel:
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="dlDistChann" AutoPostBack="true" OnSelectedIndexChanged="dlDistChann_SelectedIndexChanged">
                                        <asp:ListItem Text="Select..." Value="" Selected="True" />
                                        <asp:ListItem Value="10" />
                                        <asp:ListItem Value="20" />
                                        <asp:ListItem Value="30" />
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <table runat="server" id="tbDivSalesGrpOffice" visible="false">
                                        <tr>
                                            <td class="h5">
                                                Division:
                                            </td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddlDivision" />
                                            </td>
                                            <td class="h5">
                                                Sales Group:
                                            </td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddlSalesGroup" />
                                            </td>
                                            <td class="h5">
                                                Sales Office:
                                            </td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddlSalesOffice" />
                                            </td>
                                            <td class="h5">
                                                District:
                                            </td>
                                            <td>
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
        <tr>
            <td class="h5">
                Order Note (External Note):<br />
                (Maximum 1000 Characters)
            </td>
            <td>
                <asp:TextBox TextMode="MultiLine" Width="300px" Height="80px" runat="server" ID="txtOrderNote"
                    onblur="return checkdate(this,'1000')"></asp:TextBox>
            </td>
        </tr>
        <tr id="trSN" runat="server">
            <td class="h5">
                Sales Note From Customer:<br />
                (Maximum 300 Characters)
            </td>
            <td>
                <asp:UpdatePanel runat="server" ID="upSalesNote" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox TextMode="MultiLine" Width="300px" Height="80px" runat="server" ID="txtSalesNote"
                            onblur="return checkdate(this,'3000')"></asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="lBtnAuthCcInfo" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr id="trOPN" runat="server">
            <td class="h5">
                EU OP Note:<br />
                (Maximum 100 Characters)
            </td>
            <td>
                <asp:TextBox TextMode="MultiLine" Width="300px" Height="80px" runat="server" ID="txtOPNote"
                    onblur="return checkdate(this,'100')"></asp:TextBox>
            </td>
        </tr>
        <tr runat="server" id="trBillInfo" visible="false">
            <td class="h5">
                Billing Instruction Info:
            </td>
            <td>
                <asp:UpdatePanel runat="server" ID="upBillingInstructionInfo" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox Width="300px" runat="server" ID="txtBillingInstructionInfo" TextMode="MultiLine"
                            Height="80px" onblur="return checkdate(this,'2000')" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="lBtnAuthCcInfo" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr runat="server" id="trFreTax">
            <td class="h5">
                Freight Fee:
            </td>
            <td>
                <table>
                    <tr>
                        <td width="15%">
                            Freight(taxable):
                        </td>
                        <td width="85%">
                            <asp:TextBox ID="txtFtTax" runat="server"></asp:TextBox>
                            <ajaxToolkit:FilteredTextBoxExtender runat="server" ID="ft1" TargetControlID="txtFtTax"
                                FilterType="Numbers, Custom" ValidChars="." />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Free Freight Charge:
                        </td>
                        <td>
                            <asp:TextBox ID="txtFtFre" runat="server"></asp:TextBox>
                            <ajaxToolkit:FilteredTextBoxExtender runat="server" ID="Filteredtextboxextender1"
                                TargetControlID="txtFtFre" FilterType="Numbers, Custom" ValidChars="." />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </fieldset> 
    <table width="100%">
        <tr>
         
            <td align="center">
                
                <asp:Button runat="server" Text="Next" ID="btnPIPreview" OnClick="btnPIPreview_Click"
                    Width="150px" /><asp:CheckBox ID="cbxIsDir" runat="server" Checked="false" Text="Direct2SAP" />
                    <asp:Label ID="lbConMsg" runat="server" Text=""></asp:Label>
                  
            </td>
        </tr>
    </table>
    <script type="text/javascript">
   
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
    </script>
    <asp:UpdatePanel runat="server" ID="upCreditCardAuthInfo" UpdateMode="Conditional">
        <ContentTemplate>
            <ajaxToolkit:AlwaysVisibleControlExtender ID="AlwaysVisibleControlExtender1" runat="server"
                TargetControlID="PanelCreditAuthInfo" HorizontalSide="Center" VerticalSide="Middle"
                HorizontalOffset="0" VerticalOffset="0" />
            <asp:Panel runat="server" ID="PanelCreditAuthInfo" Visible="false" Width="340px"
                Height="125px" BackColor="LightGray" HorizontalAlign="Center">
                <table align="center" width="100%">
                    <tr>
                        <td align="right">
                            <asp:LinkButton runat="server" ID="lnkCloseCreditCardAuthInfo" Text="Close" OnClick="lnkCloseCreditCardAuthInfo_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <uc3:AuthCreditResult ID="AuthCreditResult1" runat="server" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="lBtnAuthCcInfo" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel></center>
</asp:Content>
