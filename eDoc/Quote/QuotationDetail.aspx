<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/myMaster.master"
    CodeBehind="QuotationDetail.aspx.vb" Inherits="EDOC.QuotationDetail1" %>

<%@ Register Src="~/ascx/PickProduct.ascx" TagName="PickProduct" TagPrefix="myASCX" %>
<%@ Register Src="~/ascx/PickCTOSAssemblyInstructionDoc.ascx" TagName="PickCTOSAssemblyDoc" TagPrefix="myASCX" %>
<%@ Register Src="~/ascx/QuotationViewOption.ascx" TagName="QuotationViewOptionUC" TagPrefix="myASCX" %>
<%@ Register Src="~/Ascx/USVolumeDiscount.ascx" TagName="USVolumeDiscount" TagPrefix="myASCX" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .tree table {
            width: auto;
        }

        .tree td {
            border: 0px;
            padding: 2px;
        }
    </style>
    <%= EDOC.MyUtil.GetOrgStyle()%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="../Css/token-input-facebook.css" rel="stylesheet" />
    <script src="../Js/jquery.tokeninput.js"></script>
    <style>
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
            width: 330px;
            display: inline-block;
        }

            ul.token-input-list-facebook li input {
                border: 0;
                padding: 3px 8px;
                background-color: white;
                margin: 2px 0;
                -webkit-appearance: caret;
                width: 240px;
            }
    </style>
    <script type="text/javascript">
        $(document).ready(function ($) {

       <%--   if (window.history && window.history.pushState) {
              window.history.pushState('forward', null, 'QuotationDetail.aspx?UID=<%=HDquoteid.ClientId%>');
              $(window).on('popstate', function () {
                  saveQuote();
              });

          }--%>
        });
        function saveQuote() {
            $("#<%=btnUpdate.ClientId%>").trigger("click");
            window.history.go(-1);
            //  window.location = "QuotationMaster.aspx?UID=" + $("#<%=HDquoteid.ClientId%>").val(); 
        }
        $(document).ready(function () {
            $(".UnitPriceClass").keydown(function (event) {
                if ((countdot($(this).val(), ".") > 0) && (event.keyCode == 190 || event.keyCode == 110)) {
                    //alert($(this).val() + " error!");
                    return false;
                }
            });
            <%--<% If  Role.IsTWAonlineSales() Then %>--%>
            <% If _IsAddMultiParts Then%>
            setupPNAutoSuggestion();
            <% End If%>
        });
        function setupPNAutoSuggestion() {
            $("#<%=txtPartNo.ClientId%>").tokenInput("../Services/tokenInput.ashx?orgid=<%=Pivot.CurrentProfile.getCurrOrg%>&IsUSAOnlineEP=<%=Me._IsUSAOnlineEP%>", {
                theme: "facebook", searchDelay: 100, minChars: 2, tokenDelimiter: ";", hintText: "Type Part No...", preventDuplicates: false, resizeInput: false
            });
        }
        function AddItem(val) {
            $("#<%=txtPartNo.ClientID %>").tokenInput("add", { id: val, name: val });
        }
        function countdot(s1, letter) {
            var result = s1.split(letter);
            if (result && result.length > 0)
                return result.length - 1;
            else
                return 0;
        }
    </script>
    <script type="text/javascript">
        function OnBeforeAdd() {

            if ("<%=Pivot.CurrentProfile.getCurrOrg %>" == "KR01") {
                if ($.trim($('#<%=txtPartNo.ClientID %>').val()) == "") {
                    return false;
                }

                var flag = true;

                var postData = { _InputParts: $('#<%=txtPartNo.ClientID %>').val() };
                $.ajax({
                    url: "<%= Util.GetRuntimeSiteUrl()%>/Services/eQ25WS.asmx/IsAKRPartsNeedConfirm",
                    type: "POST",
                    dataType: 'json',
                    data: postData,
                    async: false,
                    success: function (retData) {
                        if (retData) {
                            if (retData.length > 0) {
                                var IssueItems = "Below items are not with product status A:" + "\n\n";
                                for (i = 0; i < retData.length; i++) {
                                    IssueItems += retData[i] + "\n";
                                }
                                IssueItems += "\nClick OK to continue quoting these items.\n";

                                if (confirm(IssueItems) == true) {
                                    flag = true;
                                    return getACLATP('');
                                }
                                else {
                                    flag = false;
                                }
                            }
                            else {
                                flag = true;
                                return getACLATP('');
                            }
                        }
                    },
                    error: function (msg) {
                        console.log("err:" + msg.d);
                    }
                });
                return flag;
            }
            else if ("<%=Role.IsAonlineUsa%>" == "True") {
                if ($.trim($('#<%=txtPartNo.ClientID %>').val()) == "") {
                    return false;
                }
                else if ($('#<%=txtPartNo.ClientID %>').val().toUpperCase() == "AGS-CTOS-PAP-OS") {
                    var updateflag = false;
                    var returnflag = true;

                    var postData = { _QuoteID: $("#<%=HDquoteid.ClientID %>").val(), _ParentLineNo: $('#<%=Me.drpParentItem.ClientID%>').val() };
                    $.ajax({
                        url: "<%= Util.GetRuntimeSiteUrl()%>/Services/eQ25WS.asmx/IsBTOSParentSBCBTO",
                        type: "POST",
                        dataType: 'json',
                        data: postData,
                        async: false,
                        success: function (retData) {
                            if (retData) {
                                var msg = "Adding AGS-CTOS-PAP-OS will replace SBC-BTO as PAP-OS-BTO.\n\nClick OK to continue.\n";
                                if (confirm(msg) == true) {
                                    returnflag = true;
                                    updateflag = true;
                                    getACLATP('');
                                }
                                else {
                                    updateflag = false;
                                    returnflag = false;
                                }
                            }
                            else {
                                returnflag = true;
                                updateflag = false;
                            }
                        }
                    });

                    // if user confirm to do replace, call ajax to update BTOS parent 
                    if (updateflag == true) {
                        $.ajax({
                            url: "<%= Util.GetRuntimeSiteUrl()%>/Services/eQ25WS.asmx/UpdateSBCBTOtoPAPOSBTO",
                            type: "POST",
                            dataType: 'json',
                            data: postData,
                            async: false,
                            success: function (retData) {
                                if (retData) {
                                    // update ok
                                    returnflag = true;
                                }
                                else {
                                    // update failed 
                                    returnflag = false;
                                    alert('Update failed, please contact eQ.Helpdesk@advantech.com for more information.');
                                }
                            }
                        });
                    }

                    return returnflag;
                }
                else {
                    return getACLATP('');
                }
        }
        else {
            return getACLATP('');
        }
}


function getACLATP(PartNo) {
    var strPN = $('#<%=txtPartNo.ClientID %>').val();
        if (strPN.indexOf(";") >= 0) return;
        if (PartNo != '') {
            strPN = PartNo;
        }
        if ($.trim(strPN) == "") { return false; }
        // $('#ProductRelatedInfoFrame').attr('src', '../includes/ProductRelatedInfo.aspx?partno=' + strPN + '&orgid=<%=Pivot.CurrentProfile.getCurrOrg %>');
        var loadingimg = "<img src='../images/LoadingRed.gif'/>"
        $('#ProductRelatedInfoFrame').html(loadingimg);

        var postData = JSON.stringify({ strPartNo: strPN, strOrgid: "<%=Pivot.CurrentProfile.getCurrOrg %>", AccountCurrency: "<%=MasterRef.currency %>" });
            $.ajax({
                type: "POST",
                url: "<%= Util.GetRuntimeSiteUrl()%>/includes/ProductRelatedInfo.aspx/GetProductATP",
                data: postData,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var retMsg = $.parseJSON(msg.d);
                    if (retMsg.ProcessStatus == true) {
                        $('#ProductRelatedInfoFrame').html(retMsg.ProcessMessage);

                    }
                    else {
                        $('#ProductRelatedInfoFrame').html('Internal Error! Please Contact <a href="mailto:MyAdvantech@advantech.com">MyAdvantech Team</a> to report this issue, thank you.');
                    }
                    //                    if ($.trim(msg.d) != "") {
                    //                        $('#ProductRelatedInfoFrame').html(msg.d);
                    //                    } else {
                    //                        $('#ProductRelatedInfoFrame').html('No information');
                    //                    }
                },
                error: function (msg) {
                    $('#ProductRelatedInfoFrame').html('Internal Error! Please Contact <a href="mailto:MyAdvantech@advantech.com">MyAdvantech Team</a> to report this issue, thank you.');
                    //                    if ($.trim(msg.d) != "") {
                    //                        $('#ProductRelatedInfoFrame').html(msg.d);
                    //                    } 
                }
            }
            );
        }


        function getACLATP_bak(PartNo) {
            $("body").css("cursor", "progress");

            var strPN = $('#<%=txtPartNo.ClientID %>').val();

                if (PartNo != '') {
                    strPN = PartNo;
                }

                if (strPN == '') {
                    $("body").css("cursor", "default");
                    return;
                }
                //console.log('strPN:' + strPN);
                var postData = JSON.stringify({ strPartNo: strPN });
                $.ajax({
                    type: "POST",
                    url: "QuotationDetail.aspx/GetACLATP",
                    data: postData,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        if ($.trim(msg.d) != "") {
                            //console.log('called atp ok');
                            var ATPTotalInfo = $.parseJSON(msg.d);
                            var divATP = $('#divACLATP');
                            divATP.html('');
                            //console.log(ATPTotalInfo.PartNo);
                            if (ATPTotalInfo.ATPRecords.length > 0) {
                                //  alert(divATP.html());
                                divATP.append("<tr><th colspan='2' style='color:Black'>ACL Inventory</th></tr>");
                                divATP.append("<tr><th style='color:Black'>Available Date</th><th style='color:Black'>Qty</th></tr>");
                                $.each(ATPTotalInfo.ATPRecords, function (i, item) {
                                    divATP.append('<tr><td>' + item.AvailableDate + '</td><td>' + item.Qty + '</td></tr>');
                                });

                            }
                            $("body").css("cursor", "auto");
                        }
                    },
                    error: function (msg) {
                        //console.log('err calling atp ' + msg.d);
                        $("body").css("cursor", "auto");
                    }
                }
                );
            }
    </script>
    <asp:HiddenField runat="server" ID="Horg" />
    <asp:HiddenField runat="server" ID="HisFrCH" />
    <asp:HiddenField runat="server" ID="HisNeedSpADAM" />
    <%--    <asp:HiddenField runat="server" ID="HisANASales" />
    --%>
    <asp:HiddenField runat="server" ID="HCurrency" />
    <asp:HiddenField runat="server" ID="HDquoteid" />
    <table>
        <tr>
            <td>
                <table runat="server" id="tbFlow" class="Ind">
                    <tr>
                        <td>
                            <asp:ImageButton runat="server" ID="ibtnMaster" ImageUrl="~/Images/Master.gif" OnClick="ibtnMaster_Click" />
                        </td>
                        <td>
                            <asp:ImageButton runat="server" ID="ibtnDetail" ImageUrl="~/Images/Detail.gif" OnClick="ibtnDetail_Click" />
                        </td>
                        <td>
                            <asp:ImageButton runat="server" ID="ibtnPreview" ImageUrl="~/Images/Preview.gif"
                                OnClick="ibtnPreview_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table style="width: auto">
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <th align="left" style="color: #333333" runat="server" id="TitleQuoteType" visible="false">Quote Type:
                                    </th>
                                    <td runat="server" id="tdQuoteType" visible="false">
                                        <asp:Label ID="lbQuoteType" Font-Bold="false" runat="server"></asp:Label>
                                    </td>
                                    <th align="left" style="color: #333333">Quote to:
                                    </th>
                                    <td>
                                        <asp:Label ID="lbCompanyName" Font-Bold="false" runat="server"></asp:Label>
                                    </td>
                                    <th align="left" style="color: #333333">ERP ID:
                                    </th>
                                    <td>
                                        <asp:Label ID="lbCompanyId" Font-Bold="false" runat="server"></asp:Label>
                                    </td>
                                    <th align="left" style="color: #333333">SAP Sales Org:
                                    </th>
                                    <td>
                                        <asp:Label ID="lbSAPOrg" Font-Bold="false" runat="server"></asp:Label>
                                    </td>
                                    <th align="left" style="color: #333333">
                                        <span id="span1" runat="server">Siebel Org:</span>
                                    </th>
                                    <td>
                                        <asp:Label ID="lbRBU" Font-Bold="false" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:HyperLink runat="server" ID="hyCustomerCreditInfo" Text="Customer's Credit Info"
                                            Visible="false" Target="_blank" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table>
        <tr>
            <td align="left">
                <table>
                    <tr valign="top">
                        <td style="width: 300px">
                            <table style="width: 300px">
                                <tr>
                                    <td>
                                        <asp:Image runat="server" ID="imgComp" ImageUrl="~/images/Product.gif" />
                                    </td>
                                    <td>
                                        <asp:LinkButton runat="server" ID="lbtnConfig" Text="Configure a System" OnClick="lbtnConfig_Click" OnClientClick="return confirmconfigforJP();" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="position: relative;">
                                        <asp:Panel DefaultButton="btnAdd" runat="server" ID="pldd">
                                            <asp:UpdatePanel ID="UPForm" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table class="TbNoWrap" style="table-layout: fixed; width: 500px;">
                                                        <tr>
                                                            <td style="width: 110px;">
                                                                <asp:Label runat="server" ID="Label3" Text="Choose Parent Item" />
                                                                :
                                                            </td>
                                                            <td colspan="2">
                                                                <asp:DropDownList runat="server" ID="drpParentItem">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <%--                                                                <div id="divReconfigBtn"><< （To reconfig by choosing a system on left）
                                                                </div>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label runat="server" ID="L1" Text="<%$ Resources:myRs,PartNo %>" />
                                                                :
                                                            </td>
                                                            <td colspan="3">
                                                                <asp:TextBox runat="server" ID="txtPartNo" Width="270"></asp:TextBox>
                                                                <%--   onkeydown="return OnTxtPersonInfoKeyDown();"--%>
                                                                <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server"
                                                                    TargetControlID="txtPartNo" ServicePath="~/Services/AutoComplete.asmx" ServiceMethod="GetPartNo" MinimumPrefixLength="2"
                                                                    CompletionSetCount="15">
                                                                </ajaxToolkit:AutoCompleteExtender>
                                                                <%-- <asp:ImageButton ID="ibtnPickPartNo" runat="server" ImageUrl="~/Images/search.gif"
                                                                    OnClick="ibtnPickPartNo_Click" />--%>
                                                                <img src="../Images/search.gif" onclick="return PickPN(this);" dataid="0" class="cursor" alt="Pick Part Part NO" />
                                                                <asp:ImageButton runat="server" ID="imgPartNoAdd" ImageUrl="~/images/edss_showall.gif"
                                                                    OnClick="imgPartNoAdd_Click" />
                                                                <asp:Panel ID="PLPickProduct" runat="server" Style="display: none" CssClass="modalPopup">
                                                                    <div style="text-align: right;">
                                                                        <asp:ImageButton ID="CancelButtonProduct" runat="server" ImageUrl="~/Images/del.gif" />
                                                                    </div>
                                                                    <div>
                                                                        <asp:UpdatePanel ID="UPPickProduct" runat="server" UpdateMode="Conditional">
                                                                            <ContentTemplate>
                                                                                <myASCX:PickProduct ID="ascxPickProduct" runat="server" />
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </div>
                                                                </asp:Panel>
                                                                <asp:LinkButton ID="lbDummyProduct" runat="server"></asp:LinkButton>
                                                                <ajaxToolkit:ModalPopupExtender ID="MPPickProduct" runat="server" TargetControlID="lbDummyProduct"
                                                                    PopupControlID="PLPickProduct" BackgroundCssClass="modalBackground" CancelControlID="CancelButtonProduct"
                                                                    DropShadow="true" />
                                                            </td>
                                                        </tr>
                                                        <tr runat="server" id="trUnitPriceAdd">
                                                            <td>
                                                                <asp:Label runat="server" ID="Label2" Text="Unit Price" />
                                                                :
                                                            </td>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtUnitPriceAdd" Width="50px" Text="" class="UnitPriceClass" />
                                                                <ajaxToolkit:FilteredTextBoxExtender runat="server" ID="ft1" TargetControlID="txtUnitPriceAdd"
                                                                    FilterType="Numbers, Custom" ValidChars="." />
                                                            </td>
                                                            <td style="text-align: left;">
                                                                <asp:Label runat="server" ID="lbAddPlant" Text="Plant:" Visible="false" />
                                                                <asp:DropDownList ID="dlAddPlant" runat="server" Visible="false">
                                                                    <asp:ListItem Text="USH1" Value="USH1" Selected="True" />
                                                                    <asp:ListItem Text="USH2" Value="USH2" />
                                                                    <asp:ListItem Text="TWH1" Value="TWH1" />
                                                                    <asp:ListItem Text="TWH2" Value="TWH2" />
                                                                    <asp:ListItem Text="UBH1" Value="UBH1" />
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label runat="server" ID="L2" Text="<%$ Resources:myRs,Qty %>" />
                                                                :
                                                            </td>
                                                            <td colspan="3">
                                                                <asp:TextBox runat="server" ID="txtQty" Width="50px" Text="1" />
                                                                <ajaxToolkit:FilteredTextBoxExtender runat="server" ID="ft3" TargetControlID="txtQty"
                                                                    FilterType="Numbers, Custom" />
                                                                <asp:Button ID="btnAdd" runat="server" Text="<%$ Resources:myRs,Add %>" OnClick="btnAdd_Click"
                                                                    OnClientClick="return OnBeforeAdd();" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td runat="server" id="tdAddItemType" visible="false" colspan="4">Add As:
                                                                <asp:DropDownList ID="dlAddItemType" runat="server">
                                                                    <asp:ListItem Text="BTOS Root Item (Line No 100, 200, 300...etc.,)" Value="-1" Enabled="false" />
                                                                    <asp:ListItem Text="Component (Line No 1, 2, 3...etc.,)" Value="0" Selected="True" />
                                                                    <asp:ListItem Text="BTOS Loose Item (Line No 101, 102, 103...etc.,)" Value="1" />
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4" style="width: 400px; word-break: break-all; white-space: normal;">
                                                                <asp:Label runat="server" ID="lbAddErrMsg" ForeColor="Tomato" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </asp:Panel>

                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top" align="left">
                            <ajaxToolkit:TabContainer runat="server" ID="TabPMD" ActiveTabIndex="0">
                                <ajaxToolkit:TabPanel ID="TabPanel1" runat="server" HeaderText="Product Related Info">
                                    <ContentTemplate>
                                        <div id="ProductRelatedInfoFrame" style="height: 160px; overflow: auto;">
                                        </div>
                                    </ContentTemplate>
                                </ajaxToolkit:TabPanel>
                                <ajaxToolkit:TabPanel ID="TabPanel3" runat="server" HeaderText="Volume Discount" Visible="false">
                                    <ContentTemplate>
                                        <asp:UpdatePanel ID="upUSVD" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <div style="max-width: 800px; height: 160px; overflow-y: auto">
                                                    <myASCX:USVolumeDiscount ID="ascxUSVD" runat="server"></myASCX:USVolumeDiscount>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </ContentTemplate>
                                </ajaxToolkit:TabPanel>
                                <ajaxToolkit:TabPanel ID="TabPanel2" runat="server" HeaderText="Margin & Disc">
                                    <ContentTemplate>
                                        <table style="width: auto">
                                            <tr valign="top">
                                                <td valign="top">
                                                    <fieldset>
                                                        <legend>
                                                            <asp:Label ID="txtMarginDisc" runat="server" Style="font-weight: bold; font-size: 12px; color: #666666" Text="Margin & Disc"></asp:Label></legend>
                                                        <table style="width: auto">
                                                            <tr valign="top">
                                                                <td valign="top">
                                                                    <table>
                                                                        <tr valign="top">
                                                                            <td align="left">
                                                                                <asp:RadioButtonList ID="rbtnMD" runat="server" RepeatColumns="1">
                                                                                    <asp:ListItem Value="Dic" Selected="True">Disc</asp:ListItem>
                                                                                    <asp:ListItem Value="Mar" Enabled="false">Margin</asp:ListItem>
                                                                                </asp:RadioButtonList>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtDMValue" runat="server" Width="40px" Style="text-align: right" />
                                                                                %
                                                                                <ajaxToolkit:FilteredTextBoxExtender runat="server" ID="ftDM" TargetControlID="txtDMValue"
                                                                                    FilterType="Numbers, Custom" ValidChars="." />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr valign="top">
                                                                <td align="center">
                                                                    <asp:Button runat="server" Text=" Confirm " ID="btnConfirmMD" OnClick="btnConfirmMD_Click" />
                                                                    <asp:Button runat="server" Text=" Reset " ID="btnReset" OnClick="btnReset_Click" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </td>
                                                <td runat="server" id="tdMinimumPrice" valign="top" visible="false">
                                                    <fieldset>
                                                        <legend><span style="font-weight: bold; font-size: 12px; color: #666666">Price Grade</span></legend>
                                                        <table style="width: auto">
                                                            <tr valign="top">
                                                                <td align="center">
                                                                    <asp:Button runat="server" Text=" Apply Minimum Price " ID="btnApplyMinimumPrice" />
                                                                    <asp:Button runat="server" Text=" Reset " ID="btnResetOriginalPrice" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </ajaxToolkit:TabPanel>
                            </ajaxToolkit:TabContainer>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <hr />
    <p runat="server" id="pEX" visible="false" style="font-size: larger; font-weight: bold; color: #0000ff">
        Exchange Rate:<asp:Label runat="server" ID="lbEXR" ForeColor="#ff0000"></asp:Label>
        (<asp:Label runat="server" ID="lbEXRB"></asp:Label>)
    </p>
    <table>
        <tr>
            <td>
                <asp:ImageButton runat="server" ID="imgXls" ImageUrl="~/Images/excel.gif" AlternateText="Download"
                    OnClick="imgXls_Click" />&nbsp;
                <asp:Button runat="server" Text=" Delete " ID="btnDel" OnClick="btnDel_Click" />
                &nbsp;
                <asp:Button runat="server" Text=" Update " ID="btnUpdate" OnClick="btnUpdate_Click" />&nbsp;
                <asp:Button runat="server" Text=" Maintain BreakPoints " ID="btnBreakPoints" Visible="false" OnClientClick="return MaintainBreakPoints(this);" />&nbsp;
            </td>
            <td align="right" runat="server" id="twPriceGrade" visible="false">Price Grade:<asp:TextBox ID="TBpriceGrade" runat="server" />
                <ajaxToolkit:AutoCompleteExtender runat="server" ID="autoComp1" TargetControlID="TBpriceGrade"
                    ServiceMethod="GetPriceGrades" MinimumPrefixLength="0" />
                <asp:Button ID="BTgetDiscountPrice" runat="server" Text="Apply" />
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="upGV1" runat="server" UpdateMode="Conditional" OnPreRender="upGV1_PreRender">
        <ContentTemplate>
            <asp:GridView DataKeyNames="line_no" ID="gv1" runat="server" AllowPaging="false"
                EmptyDataText="Current quote items list is empty." AutoGenerateColumns="false"
                OnRowDataBound="gv1_RowDataBound">
                <Columns>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        <HeaderTemplate>
                            <asp:CheckBox ID="chkKey" runat="server" OnClick="GetAllCheckBox(this)" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkKey" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        <HeaderTemplate>
                            Seq
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table>
                                <tr>
                                    <td style="border-width: 0px;">
                                        <asp:LinkButton runat="server" CommandName='<%#Bind("line_no")%>' ID="ibtnSeqUp"
                                            Font-Bold="true" OnClick="ibtnSeqUp_Click">↑</asp:LinkButton>
                                    </td>
                                    <td style="border-width: 0px;">
                                        <asp:LinkButton runat="server" CommandName='<%#Bind("line_no")%>' ID="ibtnSeqDown"
                                            Font-Bold="true" OnClick="ibtnSeqDown_Click">↓</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="No." ItemStyle-HorizontalAlign="center" ItemStyle-Width="80">
                        <ItemStyle Wrap="False" />
                        <ItemTemplate>
                            <%#Eval("line_no") %>
                            <asp:Label ID="lbDisplayLinenoSplitSign" runat="server" Text="/" Visible="false" />
                            <asp:TextBox ID="txtDisplayLineno" runat="server" Text='<%#Bind("DisplayLineno") %>' Width="30px" Visible="false"
                                Style="text-align: right" OnTextChanged="txtDisplayLineno_TextChanged" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Category">
                        <ItemTemplate>
                            <asp:TextBox runat="server" ID="txtCategory" TextMode="MultiLine" Rows="2" Columns="1"
                                Text='<%#Bind("category") %>' BorderWidth="1px" BorderColor="#cccccc" Width="100px"
                                OnTextChanged="txtCategory_TextChanged"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Part No">
                        <ItemTemplate>
                            <asp:HyperLink runat="server" ID="hlPartNo" Text='<%#Bind("partno") %>' NavigateUrl='<%# "~/Product/ModelDetailTranslation.aspx?part_no=" & HttpUtility.UrlEncode(Eval("partno")) %>'
                                Target="_blank" />
                            <span class="pnmore"><span onclick="return PickPN(this);" lineno='<%#Eval("line_no") %>' dataid='<%#Eval("ItemType") %>' runat="server"
                                id="PickImg">
                                <img src="../Images/pickPN.png" alt="Change a PartNO" /></span>&nbsp;&nbsp;
                                <asp:ImageButton runat="server" ID="imgPartNo" ImageUrl="~/images/edss_showall.gif"
                                    OnClientClick='<%# Eval("partno", "getACLATP(""{0}"");") %>' OnClick="imgPartNo_Click1" />&nbsp;&nbsp;
                             <% If (_IsTWOnline Or _IsCNAonlineUser) AndAlso Not String.IsNullOrEmpty(Quote2ErpID) Then%>
                                <img src="../Images/historyV2.png" width="16" height="16" alt="Pricing History" title="Pricing History" onclick="popPricingHistory('<%#Eval("partno")%>','<%=Quote2ErpID%>');" /></span><% End If%>
                            <asp:HyperLink runat="server" ID="hlReconfigure" Text='Reconfigure' NavigateUrl='<%# "ReConfigureCTOSCheck.aspx?ReConfigId=" & Eval("RECFIGID") & "&UID=" & Request("UID") %>' />
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Virtual Part No">
                        <ItemTemplate>
                            <asp:TextBox runat="server" ID="txtcustMaterial" TextMode="SingleLine" Text='<%#Bind("VirtualPartNo") %>'
                                BorderWidth="1px" Width="100px" OnTextChanged="txtcustMaterial_TextChanged"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Description">
                        <ItemTemplate>
                            <asp:Button ID="BT_PickCTOAssemblyInstructionDoc" runat="server" Text="Add CTO" OnClick="BT_PickCTOAssemblyInstructionDoc_Click"
                                Visible="false" />
                            <asp:TextBox runat="server" ID="txtDescription" TextMode="MultiLine" Rows="2" Columns="1" Class="PartDescCssClass"
                                Text='<%#Bind("description") %>' BorderWidth="1px" Width="100px" ReadOnly="true"></asp:TextBox>
                            <asp:ImageButton ID="lnkView" runat="server" ImageUrl="~/Images/edit.gif" Class='<%#Eval("line_no") & "," & Eval("partno") & "," & Eval("id")%>' Visible="false" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Extended Warranty">
                        <ItemTemplate>
                            <asp:DropDownList ID="gv_drpEW" runat="server" AutoPostBack="true" OnSelectedIndexChanged="gv_drpEW_SelectedIndexChanged">
                                <%--    <asp:ListItem Text="without EW" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="3 months" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="6 months" Value="6"></asp:ListItem>
                                        <asp:ListItem Text="9 months" Value="9"></asp:ListItem>
                                        <asp:ListItem Text="12 months" Value="12"></asp:ListItem>
                                        <asp:ListItem Text="15 months" Value="15"></asp:ListItem>
                                        <asp:ListItem Text="21 months" Value="21"></asp:ListItem>
                                        <asp:ListItem Text="24 months" Value="24"></asp:ListItem>
                                        <asp:ListItem Text="36 months" Value="36"></asp:ListItem>--%>
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="rohs" HeaderText="Rohs" ItemStyle-HorizontalAlign="center" />
                    <asp:BoundField DataField="classABC" HeaderText="Class" ItemStyle-HorizontalAlign="center" />
                    <asp:TemplateField ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="Center">
                        <HeaderTemplate>
                            List Price
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%#Me.HCurrency.Value %>' ID="lbListPriceSign"></asp:Label>
                            <asp:Label runat="server" Text='<%#FormatNumber(Eval("listprice"), 2) %>' ID="lbListPrice"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="Center">
                        <HeaderTemplate>
                            <asp:Label runat="server" ID="lbUnitPrice" />
                        </HeaderTemplate>
                        <ItemStyle Wrap="False" />
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%#Me.HCurrency.Value %>' ID="lbUnitPriceSign" />
                            <asp:TextBox ID="txtUnitPrice" runat="server" Text='<%#Replace(FormatNumber(Eval("newunitprice"), 2), ",", "") %>'
                                Width="60px" Style="text-align: right" OnTextChanged="txtUnitPrice_TextChanged"
                                class="UnitPriceClass" />
                            <ajaxToolkit:FilteredTextBoxExtender runat="server" ID="ft1" TargetControlID="txtUnitPrice"
                                FilterType="Numbers, Custom" ValidChars="." />
                            <asp:Label ID="lbDisplayUnitPriceSplitSign" runat="server" Text="/" Visible="false" />
                            <asp:TextBox ID="txtDisplayUnitPrice" runat="server" Text='<%#Bind("DisplayUnitPrice") %>' Visible="false"
                                Width="60px" Style="text-align: right" OnTextChanged="txtDisplayUnitPrice_TextChanged" class="UnitPriceClass" />
                            <%--<asp:Label ID="LbABRTax" runat="server" Text="" Visible="false" />--%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="" HeaderText="Disc." ItemStyle-HorizontalAlign="right" />
                    <asp:TemplateField ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="Center">
                        <HeaderTemplate>
                            Qty.
                        </HeaderTemplate>
                        <ItemStyle Wrap="False" />
                        <ItemTemplate>
                            <asp:TextBox ID="txtGVQty" runat="server" Text='<%#Bind("qty") %>' Width="30px" Style="text-align: right"
                                OnTextChanged="txtGVQty_TextChanged"></asp:TextBox>
                            <ajaxToolkit:FilteredTextBoxExtender runat="server" ID="ft2" TargetControlID="txtGVQty"
                                FilterType="Numbers, Custom" />
                            <asp:Label ID="lbDisplayQtySplitSign" runat="server" Text="/" Visible="false" />
                            <asp:TextBox ID="txtDisplayQty" runat="server" Text='<%#Bind("DisplayQty") %>' Width="30px" Visible="false"
                                Style="text-align: right" OnTextChanged="txtDisplayQty_TextChanged" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="Center">
                        <HeaderTemplate>
                            <asp:Label runat="server" ID="lbReqDate">Req. Date</asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="txtreqdate" runat="server" Text='<%#CDate(Eval("reqdate")).ToShortDateString()%>'
                                Width="60px" Style="text-align: right" OnTextChanged="txtreqdate_TextChanged"></asp:TextBox>
                            <ajaxToolkit:FilteredTextBoxExtender runat="server" ID="Filteredtextboxextender5"
                                TargetControlID="txtreqdate" FilterType="Numbers, Custom" ValidChars="-/" />
                            <ajaxToolkit:CalendarExtender runat="server" ID="CalendarExtender1" CssClass="cal_Theme1"
                                TargetControlID="txtreqdate" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="Center">
                        <HeaderTemplate>
                            <asp:Label runat="server" ID="lbHDueDate">Due Date</asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lbIDueDate" Text='<%# TransferLocalTime(Eval("duedate"))%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="Center">
                        <HeaderTemplate>
                            Sub Total
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%#Me.HCurrency.Value %>' ID="lbSubTotalSign" />
                            <asp:Label runat="server" Text="" ID="lbSubTotal" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label runat="server" ID="lbITP"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%#Me.HCurrency.Value %>' ID="lbItpSign"></asp:Label>
                            <asp:TextBox runat="server" ID="txtItp" Text='<%#Eval("newITP") %>' Width="50px"
                                ReadOnly="true" BackColor="#ebebe4"></asp:TextBox>
                            <asp:Button ID="btnSpecialItp" runat="server" Text="Special ITP" OnClick="btnSpecialItp_Click" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="Center">
                        <HeaderTemplate>
                            Margin
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lbMargin"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center">
                        <HeaderTemplate>
                            SPR No
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lbSPRNO" Text='<%#Eval("sprno") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="deliveryPlant" Visible="false" />
                    <asp:TemplateField HeaderText="Delivery Plant">
                        <ItemTemplate>
                            <asp:DropDownList ID="gv_drpDeliveryPlant" runat="server" AutoPostBack="true" OnSelectedIndexChanged="gv_drpDeliveryPlant_SelectedIndexChanged">
                                <asp:ListItem Text="USH1" Value="USH1" Selected="True" />
                                <asp:ListItem Text="USH2" Value="USH2" />
                                <%--                                <asp:ListItem Text="TWH1" Value="TWH1" />
                                <asp:ListItem Text="TWH2" Value="TWH2" />--%>
                                <asp:ListItem Text="UBH1" Value="UBH1" />
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center">
                        <HeaderTemplate>
                            BreakPoints:<asp:Label runat="server" ID="lbBreakPointScales" Text="" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lbBreakPointsPrice" Text="" />
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>
            </asp:GridView>
            <asp:Table ID="Table1" BorderWidth="0" runat="server" HorizontalAlign="Right" Width="500px">
                <asp:TableRow runat="server" ID="rowSUBTotal" Visible="false">
                    <asp:TableCell HorizontalAlign="Right">Quote Total Net Price:</asp:TableCell><asp:TableCell HorizontalAlign="Right">
                        <%= Me.HCurrency.Value%>&nbsp;<asp:Label ID="lbABRQuoteSubTotal" runat="server" Text=""></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow runat="server" ID="rowTotalICMS" Visible="false">
                    <asp:TableCell Width="400px" HorizontalAlign="Right">ICMS:</asp:TableCell>
                    <asp:TableCell Width="100px" HorizontalAlign="Right">
                        <%= Me.HCurrency.Value%>&nbsp;<asp:Label ID="lbTotalICMS" runat="server" Text=""></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow runat="server" ID="rowTotalICMSOrig" Visible="false">
                    <asp:TableCell Width="400px" HorizontalAlign="Right">ICMS (Orig):</asp:TableCell>
                    <asp:TableCell Width="100px" HorizontalAlign="Right">
                        <%= Me.HCurrency.Value%>&nbsp;<asp:Label ID="lbTotalICMSOrig" runat="server" Text=""></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow runat="server" ID="rowTotalICMSDest" Visible="false">
                    <asp:TableCell Width="400px" HorizontalAlign="Right">ICMS (Dest):</asp:TableCell>
                    <asp:TableCell Width="100px" HorizontalAlign="Right">
                        <%= Me.HCurrency.Value%>&nbsp;<asp:Label ID="lbTotalICMSDest" runat="server" Text=""></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow runat="server" ID="rowTotalICMSSpec" Visible="false">
                    <asp:TableCell Width="400px" HorizontalAlign="Right">ICMS (Spec):</asp:TableCell>
                    <asp:TableCell Width="100px" HorizontalAlign="Right">
                        <%= Me.HCurrency.Value%>&nbsp;<asp:Label ID="lbTotalICMSSpec" runat="server" Text=""></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow runat="server" ID="rowTotalIPI" Visible="false">
                    <asp:TableCell HorizontalAlign="Right">IPI:</asp:TableCell>
                    <asp:TableCell HorizontalAlign="Right">
                        <%= Me.HCurrency.Value%>&nbsp;<asp:Label ID="lbTotalIPI" runat="server" Text=""></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow runat="server" ID="rowTotalPIS" Visible="false">
                    <asp:TableCell HorizontalAlign="Right">PIS:</asp:TableCell><asp:TableCell HorizontalAlign="Right">
                        <%= Me.HCurrency.Value%>&nbsp;<asp:Label ID="lbTotalPIS" runat="server" Text=""></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow runat="server" ID="rowTotalCONGIS" Visible="false">
                    <asp:TableCell HorizontalAlign="Right">CONGIS:</asp:TableCell><asp:TableCell HorizontalAlign="Right">
                        <%= Me.HCurrency.Value%>&nbsp;<asp:Label ID="lbTotalCOFINS" runat="server" Text=""></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Right" runat="server" ID="rowTotalHeader" Font-Bold="true">Total:</asp:TableCell>
                    <asp:TableCell HorizontalAlign="Right" Width="30%">
                        <%= Me.HCurrency.Value%>&nbsp;<asp:Label ID="lbtotal" runat="server" Text=""></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow runat="server" ID="rowTotalMargin">
                    <asp:TableCell HorizontalAlign="Right" Font-Bold="true">Total Margin:</asp:TableCell>
                    <asp:TableCell HorizontalAlign="Right" Width="30%">
                        <asp:Label runat="server" ID="lbTotalMargin" Text="0.00"></asp:Label>%
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow runat="server" ID="rowTotalITP" Visible="false">
                    <asp:TableCell HorizontalAlign="Right" Font-Bold="true">Total ITP:</asp:TableCell>
                    <asp:TableCell HorizontalAlign="Right" Width="30%">
                        <%= Me.HCurrency.Value%>&nbsp;<asp:Label runat="server" ID="lbTotalITP" Text="0.00"></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow runat="server" ID="rowStandardMargin" Visible="false">
                    <asp:TableCell HorizontalAlign="Right">Total Margin Advantech product:</asp:TableCell>
                    <asp:TableCell HorizontalAlign="Right" Width="30%">
                        <asp:Label runat="server" ID="lbStandardMargin" Text="0.00"></asp:Label>%
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow runat="server" ID="rowPTDMargin" Visible="false">
                    <asp:TableCell HorizontalAlign="Right">Total Margin P-td product:</asp:TableCell>
                    <asp:TableCell HorizontalAlign="Right" Width="30%">
                        <asp:Label runat="server" ID="lbPTDMargin" Text="0.00"></asp:Label>%
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow runat="server" ID="rowMarginInformation">
                    <asp:TableCell HorizontalAlign="Right" ColumnSpan="2">Total margin is calculated without taking AGS & P-trade items into consideration.</asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <%--            <p style="text-align: right" runat="server" id="quotetotal">
                <b>Total:</b><%= Me.HCurrency.Value%><asp:Label runat="server" ID="lbtotal" Text="0.00"></asp:Label><span runat="server" id="spMarg1"><b>Total Margin:</b>
                    <asp:Label runat="server" ID="lbTotalMargin" Text="0.00"></asp:Label>% </span><span runat="server" id="spTotalITP" visible="false"><b>Total ITP:</b> <%= Me.HCurrency.Value%><asp:Label runat="server" ID="lbTotalITP" Text="0.00"></asp:Label></span>
            </p>--%>
            <%--            <span runat="server" id="spStandardMargin" visible="false">
                <p style="text-align: right">
                    Total Margin Advantech product:<asp:Label runat="server" ID="lbStandardMargin" Text="0.00"></asp:Label>%
                </p>
            </span>--%>
            <%--            <span runat="server" id="spPTDMargin" visible="false">
                <p style="text-align: right">
                    Total Margin P-td product:<asp:Label runat="server" ID="lbPTDMargin" Text="0.00"></asp:Label>%
                </p>
            </span>--%>
            <%--            <span runat="server" id="spMarg2">
                <p style="text-align: right">
                    Total margin is calculated without taking AGS & P-trade items into consideration.
                </p>
            </span>--%>
            <asp:Panel ID="PLSITP" runat="server" Style="display: none" CssClass="modalPopup">
                <div style="text-align: right;">
                    <asp:ImageButton ID="ibtnCCSITP" runat="server" ImageUrl="~/Images/del.gif" />
                </div>
                <div>
                    <asp:UpdatePanel ID="UPSITP" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td>
                                        <%--<b>Line :</b>--%>
                                    </td>
                                    <td>
                                        <asp:HiddenField runat="server" ID="hLine" />
                                        <%-- <asp:TextBox ID="txtLine" runat="server" ReadOnly="true"></asp:TextBox>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lbITPMsg" runat="server" Visible="false" ForeColor="Red"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td>
                                        <b>
                                            <asp:Label ID="lbITPTitle" runat="server" Text="ITP:" />
                                        </b>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSITP" runat="server"></asp:TextBox><ajaxToolkit:FilteredTextBoxExtender runat="server" ID="Filteredtextboxextender5"
                                            TargetControlID="txtSITP" FilterType="Numbers, Custom" ValidChars="." />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <b>SPR# :</b> </td>
                                    <td>
                                        <asp:TextBox ID="txtSPRNO" runat="server" MaxLength="20"></asp:TextBox></td>
                                </tr>
                            </table>
                            <table width="100%">
                                <tr>
                                    <td align="center">
                                        <div id="itpmsg" style="color: tomato;"></div>
                                        <asp:Button ID="btnSITP" runat="server" Text="Update" OnClick="btnSITP_Click" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </asp:Panel>
            <asp:LinkButton ID="lbtnSITP" runat="server"></asp:LinkButton><ajaxToolkit:ModalPopupExtender ID="MPSITP" runat="server" TargetControlID="lbtnSITP"
                PopupControlID="PLSITP" BackgroundCssClass="modalBackground" CancelControlID="ibtnCCSITP"
                DropShadow="true" />
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnDel" />
            <asp:AsyncPostBackTrigger ControlID="btnUpdate" />
            <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
    <table>
        <tr>
            <td>
                <table id="tbOptions" runat="server" style="width: auto">
                    <tr>
                        <td>
                            <b>Options:</b> </td>
                        <td>
                            <asp:CheckBox runat="server" ID="cbxListPrice" Text="List Price" ForeColor="Black" />
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="cbxDisc" Text="Discount%" ForeColor="Black" />
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="cbxDueDate" Text="Due Date" ForeColor="Black" />
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="cbxLumpSum" Text="LumpSumOnly" ForeColor="Black" />
                        </td>
                        <td class="hide EUshow">
                            <asp:CheckBox runat="server" ID="cbShowTotal" Text="Show Freight and Total" Checked="true" ForeColor="Black" />
                        </td>
                        <td class="hide EUshow">
                            <asp:CheckBox runat="server" ID="cbShowFreight" Text="Auto Calculate Freight" Checked="true" ForeColor="Black" />
                        </td>
                        <td>
                            <myASCX:QuotationViewOptionUC ID="QuotationViewOptionUC1" runat="server" Visible="False" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="upbtnConfirm" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table>
                <tr>
                    <td>
                        <asp:Label runat="server" ForeColor="Red" ID="lbConfirmMsg" Text=""></asp:Label></td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="btnConfirm" runat="server" Text="<%$ Resources:myRs,Confirm %>" OnClick="btnConfirm_Click" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Panel ID="PLConfirm" runat="server" Style="display: none" CssClass="modalPopup">
        <div style="text-align: right;">
            <%--<asp:ImageButton ID="CancelButtonConfirm" runat="server" ImageUrl="~/Images/del.gif" />--%>
        </div>
        <div>
            <asp:UpdatePanel ID="UPConfirm" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <table>
                        <tr>
                            <th align="left" id="gptd">This quote must be approved through Approval Flow due to below issues:
                                <ul>
                                    <li runat="server" id="ApprovalFlowType1">Below GP</li>
                                    <li runat="server" id="ApprovalFlowType2">Expiration date is more than 30 days</li>
                                </ul>
                            </th>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:GridView ID="gvbelowgpitems" runat="server" AutoGenerateColumns="false" Width="98%" Visible="false">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Part NO">
                                            <ItemTemplate><%#Eval("partno") %></ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Unit Price" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate><%#Me.HCurrency.Value %><%#Eval("newUnitPrice") %></ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Margin" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate><%#Eval("Margin")%>%</ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Mini. Price" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate><%#Me.HCurrency.Value %><%#Eval("MinimumPrice")%></ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr valign="top">
                            <td>Reason:
                                <asp:TextBox ID="txtRelatedInfo" runat="server" TextMode="MultiLine" Columns="50"
                                    Rows="5" MaxLength="28" Width="600" OnPreRender="txtRelatedInfo_PreRender"></asp:TextBox></td>
                        </tr>
                        <tr valign="top" runat="server" id="TRNotifyCC" visible="false">
                            <td>Also notify the approval result to:
                                <asp:TextBox ID="txtNotifyEmail" runat="server" Columns="50" />
                                <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server"
                                    TargetControlID="txtNotifyEmail" ServicePath="~/Services/AutoComplete.asmx" ServiceMethod="getEmployee" MinimumPrefixLength="2"
                                    CompletionSetCount="15">
                                </ajaxToolkit:AutoCompleteExtender>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
            <table>
                <tr>
                    <td align="center">
                        <asp:Button ID="CancelButtonConfirm" runat="server" Text="Cancel" />
                        <asp:Button ID="btnRealConfirm" runat="server" Text="<%$ Resources:myRs,Confirm %>"
                            OnClick="btnRealConfirm_Click" OnClientClick="return checkreason();" />
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
    <asp:LinkButton ID="lbDummyConfirm" runat="server"></asp:LinkButton><ajaxToolkit:ModalPopupExtender ID="MPConfirm" runat="server" TargetControlID="lbDummyConfirm"
        PopupControlID="PLConfirm" BackgroundCssClass="modalBackground" CancelControlID="CancelButtonConfirm"
        DropShadow="true" />
    <asp:UpdatePanel ID="UP_QuotationNote" runat="server" UpdateMode="Conditional" Visible="false">
        <ContentTemplate>
            <table>

                <tr runat="server" id="TR_Leadtime_warranty_AENC" visible="false">
                    <td width="200px" align="right">Lead Time: </td>
                    <td width="150px" align="left">
                        <asp:TextBox Width="200px" runat="server" ID="TextBoxLeadTime" />
                    </td>
                    <td width="50px" align="center">
                        <%--                        <asp:Button ID="Button1" runat="server" Text="Save" OnClick="BT_UpdateSalesNote_Click"
                            Visible="false" />
                        <br />
                        <asp:Label ID="Label1" runat="server" Text="" ForeColor="Tomato"></asp:Label>--%>
                    </td>
                    <td width="100px" align="right">Warranty: </td>
                    <td width="150px" align="left">
                        <asp:TextBox Width="200px" runat="server" ID="TextBoxWarranty" />
                    </td>
                    <td width="50px" align="center">
                        <%--                        <asp:Button ID="Button2" runat="server" Text="Save" OnClick="BT_UpdateOrderNote_Click"
                            Visible="false" />
                        <br />--%>
                        <asp:Label ID="Label4" runat="server" Text="" ForeColor="Tomato"></asp:Label></td>
                    <td width="200px" align="center"></td>
                </tr>


                <tr>
                    <td width="200px" align="right">Sales Note From Customer:
                        <br />
                        (Internal) </td>
                    <td width="150px" align="left">
                        <asp:TextBox TextMode="MultiLine" Width="300px" Height="80px" runat="server" ID="txtSalesNote"
                            onblur="return checkdate(this,'3000')"></asp:TextBox></td>
                    <td width="50px" align="center">
                        <asp:Button ID="BT_UpdateSalesNote" runat="server" Text="Save" OnClick="BT_UpdateSalesNote_Click"
                            Visible="false" />
                        <br />
                        <asp:Label ID="LB_SalesNote" runat="server" Text="" ForeColor="Tomato"></asp:Label></td>
                    <td width="100px" align="right">External Note: </td>
                    <td width="150px" align="left">
                        <asp:TextBox TextMode="MultiLine" Width="300px" Height="80px" runat="server" ID="txtOrderNote"
                            onblur="return checkdate(this,'1000')"></asp:TextBox></td>
                    <td width="50px" align="center">
                        <asp:Button ID="BT_UpdateOrderNote" runat="server" Text="Save" OnClick="BT_UpdateOrderNote_Click"
                            Visible="false" />
                        <br />
                        <asp:Label ID="LB_OrderNote" runat="server" Text="" ForeColor="Tomato"></asp:Label></td>
                    <td width="200px" align="center"></td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="BT_UpdateSalesNote" />
            <asp:AsyncPostBackTrigger ControlID="BT_UpdateOrderNote" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:LinkButton ID="lbDummyConfigConfirm" runat="server"></asp:LinkButton><asp:Panel ID="PLPickCTOSDoc" runat="server" Style="display: none" CssClass="modalPopup"
        Width="70%" Height="60%">
        <div style="text-align: right;">
            <asp:ImageButton ID="CancelButtonCTOSDoc" runat="server" ImageUrl="~/Images/del.gif" />
        </div>
        <div>
            <asp:UpdatePanel ID="UPPickCTOSDoc" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <myASCX:PickCTOSAssemblyDoc ID="ascxPickCTOSAssDoc1" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </asp:Panel>
    <asp:LinkButton ID="lbDummyCTOSAssDoc" runat="server"></asp:LinkButton><ajaxToolkit:ModalPopupExtender ID="MPCTOSAssDoc" runat="server" TargetControlID="lbDummyCTOSAssDoc"
        PopupControlID="PLPickCTOSDoc" BackgroundCssClass="modalBackground" CancelControlID="CancelButtonCTOSDoc"
        DropShadow="false" />
    <div id="divPickPN" title="Pick Part NO" style="text-align: center; display: none;">
        <iframe id="Myframe" frameborder="0" width="100%" height="100%" scrolling="no"></iframe>
    </div>
    <div id="divSODetail" style="display: none; overflow: auto">
        <table width="100%">
            <tr>
                <td align="left">
                    <table align="left" class="ptb">
                        <tr>
                            <td><b>Part No.</b></td>
                            <td>
                                <input type="text" id="txtQueryOHPN" style="width: 150px" onkeypress="Click_btnQueryOrderHistory();" />
                            </td>
                            <td>
                                <input type="button" id="btnQueryOrderHistory" onclick='qOrderHistoryAgain()' value="Search" />
                            </td>
                        </tr>
                    </table>
                    <script type="text/javascript">
                        function qOrderHistoryAgain() {
                            popPricingHistory($("#txtQueryOHPN").val(), "<%=Quote2ErpID%>");
                        }

                        function Click_btnQueryOrderHistory() {
                            if (event.keyCode == 13) {
                                qOrderHistoryAgain();
                            }
                        }

                    </script>
                </td>
            </tr>
            <tr>
                <td>
                    <table width="100%" border="1" class="pricehistory gtd" bordercolor='#000000' style="border-color: #D7D0D0; border-width: 1px; border-style: Solid; border-collapse: collapse;">
                        <thead>
                            <tr class="HeaderStyle" style="color: White; background-color: #FF6600;">
                                <th style="height: 25px; color: White;">SO No.</th>
                                <th style="color: White;">Order Date</th>
                                <th style="color: White;">Line No.</th>
                                <th style="color: White;">Part No.</th>
                                <th style="color: White;">Unit Price</th>
                                <th style="color: White;">Qty.</th>
                                <th style="color: White;">Amount</th>
                            </tr>
                        </thead>
                        <tbody id="soDetailList" />
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <div id="DialogPartDescription" style="display: none;">
        <b>Line No. :</b> <span id="SpanLineNo"></span>
        <br />
        <b>Part No. :</b> <span id="SpanPartNo"></span>
        <br />
        <b>Description:</b>
        <asp:TextBox ID="TxtPartDescription" runat="server" Height="100" Width="400" TextMode="MultiLine" />
    </div>

    <asp:Panel ID="NCNR_Panel" runat="server" Style="display: none" CssClass="modalPopup" Width="30%">
        <div>
            <asp:UpdatePanel ID="NCNR_UpdatePanel" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <table>
                        <tr>
                            <td align="center" title="NCNR">
                                <asp:Label runat="server" Font-Size="10pt" Text="Subject to NCNR agreement.<br />　　"></asp:Label></td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
            <table style="height: 70%">
                <tr>
                    <td align="center">
                        <asp:Button ID="NCNR_Confirm_Button" runat="server" Text="Confirm" OnClick="btnConfirm_Click" />
                        <asp:Button ID="NCNR_Cancel_Button" runat="server" Text="Cancel" />
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
    <asp:LinkButton ID="NCNR_LinkButton" runat="server"></asp:LinkButton><ajaxToolkit:ModalPopupExtender ID="NCNR_Popup" runat="server" PopupControlID="NCNR_Panel" TargetControlID="NCNR_LinkButton"
        BackgroundCssClass="modalBackground" CancelControlID="NCNR_Cancel_Button" />

    <div id="divBreakPoints" style="display: none" title="BreakPoints">
        <iframe id="BreakPointsIframe" src="" width="100%" height="100%" frameborder="0"></iframe>
    </div>


    <script type="text/javascript">

        function MaintainBreakPoints(_this) {
            var obj = $(_this);
            $("#divBreakPoints").dialog(
	        	{
	        	    modal: true, width: '750', height: '450',
	        	    open: function (type, data) {
	        	        busyMode(true);
	        	        $('#BreakPointsIframe').attr('src', '<%=Util.GetRuntimeSiteUrl%>/Includes/QuoteByMOQ.aspx?UID=<%=UID%>');
	        	    },
	        	    close: function (type, data) {
	        	        window.location.reload();
	        	    }
	        	}
	       );
                return false;
            }


            $('#BreakPointsIframe').on('load', function () {
                busyMode(false);
            });


            $(document).on("click", "[id*=lnkView]", function () {
                //$("#id").html($(".Id", $(this).closest("tr")).html());
                //$("#name").html($(".Name", $(this).closest("tr")).html());
                //$("#description").html($(".Description", $(this).closest("tr")).html());

                //$('li.item-a').closest('ul').css('background-color', 'red');
                var _SpanLineNo = document.getElementById('SpanLineNo');
                var _SpanPartNo = document.getElementById('SpanPartNo');
                var txtareaPartDesc = document.getElementById('<%=Me.TxtPartDescription.ClientID %>');

                var sourcetextarea = $(this).closest("td").find(".PartDescCssClass");
                var _str1 = $(this).attr('Class');
                _SpanLineNo.innerHTML = _str1.split(',')[0];
                _SpanPartNo.innerHTML = _str1.split(',')[1];

                var _QuoteDetailID = _str1.split(',')[2] || "";
                var _DescriptionVal = $(sourcetextarea).val();
                //alert(_str1);
                // txtareaPartDesc.innerText = sourcetextarea.val();
                $(txtareaPartDesc).val($(sourcetextarea).val());
                $("#DialogPartDescription").dialog({
                    title: "Edit Description",
                    buttons: {
                        Save: function () {
                            //ICC 2016/2/26 Use web service to update description.
                            var errMsg = 'Internal Error! Update QuotationDetail description error! Please Contact <a href="mailto:MyAdvantech@advantech.com">MyAdvantech Team</a> to report this issue, thank you.';
                            if (_QuoteDetailID.length > 0) {
                                var postData = JSON.stringify({ ID: _QuoteDetailID, description: $(txtareaPartDesc).val() });
                                $.ajax(
                                    {
                                        async: false,
                                        type: "POST", url: "<%=IO.Path.GetFileName(Request.PhysicalPath) %>/UpdateDescription", data: postData, contentType: "application/json; charset=utf-8", dataType: "json",
                                        success: function (retData) {
                                            var result = $.parseJSON(retData.d);
                                            if (result == "Error") {
                                                $("html, body").html(errMsg);
                                            }
                                        },
                                        error: function (retData) {
                                            $("html, body").html(errMsg);
                                        }
                                    });
                                }
                                else $("html, body").html(errMsg);
                            $(this).dialog('close');
                            location.reload();
                        },
                        Cancel: function () {
                            $(this).dialog('close');
                        }
                    },
                    modal: true,
                    width: "450px"
                });
                return false;
            });

                function GetAllCheckBox(cbAll) {
                    var items = document.getElementsByTagName("input");
                    for (i = 0; i < items.length; i++) {
                        if (items[i].type == "checkbox") {
                            items[i].checked = cbAll.checked;
                        }
                    }
                }
                function OnTxtPersonInfoKeyDown() {
                    var H = document.getElementById('<%=Me.Horg.ClientID %>');

                    var acNameClientId = '<%=Me.AutoCompleteExtender1.ClientID %>';

                    var acName = $find(acNameClientId);


                    if (acName != null) {
                        acName.set_contextKey(H.value);
                    }

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

                $("#<%= btnDel.ClientID %>").click(function () { return confirm("Are you sure you want to delete?"); });

                function confirmconfigforJP() {

            <% If _IsJPOnline Then%>
                    var flag = true;
                    var postData = JSON.stringify({ UID: "<%=Request("UID")%>" });
                    $.ajax(
                        {
                            async: false,
                            type: "POST", url: "<%=IO.Path.GetFileName(Request.PhysicalPath) %>/CheckBTOSExist", data: postData, contentType: "application/json; charset=utf-8", dataType: "json",
                            success: function (retData) {
                                var result = $.parseJSON(retData.d);
                                if (result == false) {
                                    flag = true;
                                }
                                else {
                                    flag = confirm('Configure a new system will delete all existing items and systems in quote, are you sure?');
                                }
                            },
                            error: function (retData) {
                                flag = confirm('Configure a new system will delete all existing items and systems in quote, are you sure?');
                            }
                        });
                        return flag;
                <% Else%>
                    return true;
            <% End If%>


                }
    </script>
    <script language="javascript" type="text/javascript">
        var lineno = 0;
        function PickPN(_this) {
            var obj = $(_this);
            var btos = $(_this).attr("dataid"); //ICC 2015/6/11 Add new tag to save btos parent data. 
            $("#divPickPN").dialog(
	        	{
	        	    modal: true, width: '750', height: '450',
	        	    open: function (type, data) {

	        	        if (obj.get(0).tagName.toUpperCase() == "SPAN") {
	        	            $('#Myframe').attr('src', '../Includes/Products.aspx?dotype=2&Btos=' + btos + '&currency=<%=MasterRef.currency %>');
	        	            lineno = obj.attr("lineno");
	        	        }
	        	        else {
	        	            $('#Myframe').attr('src', '../Includes/Products.aspx?dotype=1&Btos=' + btos + '&currency=<%=MasterRef.currency %>');
	        	        }
	        	    },
	        	    close: function (type, data) {

	        	    }
	        	}
	       );
            }
            function PickPnEnd(_PN) {
                $("#divPickPN").dialog("close");
                // $("#<%=txtPartNo.ClientID %>").val(_PN);
                //ICC 2015/6/29 Only ATW & ACN users can use tokenInput, other users please use jQuery default function to set val in part no textbox.
                <%--<% If Role.IsTWAonlineSales() OrElse Role.IsCNAonlineSales() Then%>--%>
            <% If _IsAddMultiParts Then%>
                $("#<%=txtPartNo.ClientID %>").tokenInput("add", { id: _PN, name: _PN });
            <% Else%>
                $("#<%=txtPartNo.ClientID %>").val(_PN);
            <% End If%>
                return false;
            }
            function PickPnEnd2(_PN) {
                $("#divPickPN").dialog("close");
                showloadingV2("Processing, Please wait...", 0);
                ////////////////////////
                var _quoteid = $("#<%=HDquoteid.ClientID %>").val();
                var postData = JSON.stringify({ Quoteid: _quoteid, Lineno: lineno, Partno: _PN });
                $.ajax(
                    {
                        type: "POST", url: "<%=IO.Path.GetFileName(Request.PhysicalPath) %>/UpdatePartNO", data: postData, contentType: "application/json; charset=utf-8", dataType: "json",
                        success: function (retData) {
                            var lists = $.parseJSON(retData.d);
                            if (lists.status == 1) {
                                showloadingV2(lists.message, 1);
                                setTimeout('myrefresh()', 1000);
                            }
                            else {
                                showloadingV2(lists.message, 4);
                            }
                        },
                        error: function (retData) {
                        }
                    });
                /////////////////////////
            }
            function myrefresh() {
                window.location.reload();
            }
    </script>
    <script type="text/javascript">
        var _isbtnAdd = false;
        function pageLoad(sender, e) {

            if (e.get_isPartialLoad() == false) {
                Sys.WebForms.PageRequestManager.getInstance().add_initializeRequest(InitRequestHandler);
                Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
            }

        }
        function InitRequestHandler(sender, e) {
             <%--document.getElementById('<%=btnAdd.ClientID%>').disabled = true;--%>
            if (document.getElementById('<%=btnAdd.ClientID%>') != null) {
                document.getElementById('<%=btnAdd.ClientID%>').disabled = true;
            }


            if (e.get_postBackElement().id == "ContentPlaceHolder1_btnAdd") {
                _isbtnAdd = true;
            }
            else if (e.get_postBackElement().id == "ContentPlaceHolder1_btnDel") //ICC 2015/4/20  Add btnDel in this event. When update panel refresh form, ATW's partno textbox should applied facebook theme and style.
            {
                _isbtnAdd = true;
            }
            else {
                _isbtnAdd = false;
            }
        }
        function EndRequestHandler(sender, e) {
             <%--document.getElementById('<%=btnAdd.ClientID%>').disabled = false;--%>
            if (document.getElementById('<%=btnAdd.ClientID%>') != null) {
                document.getElementById('<%=btnAdd.ClientID%>').disabled = false;
            }
            <%--<% If  Role.IsTWAonlineSales() Then %>--%>
             <% If _IsAddMultiParts Then%>
            if (_isbtnAdd) setupPNAutoSuggestion();
             <% End If%>
        }

        function popPricingHistory(PartNo, ErpID) {
            busyMode(true); var tbDetail = $("#soDetailList"); tbDetail.empty();
            var pn = PartNo;
            //console.log("pn:" + pn);
            //console.log("ErpID:" + ErpID);

            var postData = JSON.stringify({ PartNo: pn, ErpID: ErpID });
            $.ajax(
                {
                    type: "POST", url: "<%=IO.Path.GetFileName(Request.PhysicalPath)%>/GetOrderHistory", data: postData, contentType:
                                "application/json; charset=utf-8", dataType: "json",
                    success: function (retData) {
                        //
                        var orderlines = $.parseJSON(retData.d); var linesHtml = "";
                        //var orderlines = odObj.OrderLines;
                        //console.log("ha1");
                        $.each(orderlines, function (idx, item) {
                            //console.log("item.ORDER_QTY:"+item.ORDER_QTY);
                            linesHtml +=
                                "<tr>" +
                                    "<td align='center'>" + item.SO_NO + "</td>" +
                                    "<td align='center'>" + item.ORDER_DATE + "</td>" +
                                    "<td align='center'>" + item.LINE_NO + "</td>" +
                                    "<td align='center'>" + item.PART_NO + "</td>" +
                                    "<td align='center'>" + item.UNIT_PRICE + "</td>" +
                                    "<td align='center'>" + item.ORDER_QTY + "</td>" +
                                    "<td align='center'>" + item.AMOUNT + "</td>" +
                                "</tr>";
                        }
                        );
                        //console.log("ha2");
                        if (orderlines.length == 0) { linesHtml = "<td colspan='7' align='center' style='font-style:italic'>No Historical Ordering Data</td>"; }
                        tbDetail.append(linesHtml);

                        $("#divSODetail").dialog({
                            modal: true,
                            width: $(window).width() - 100,
                            height: $(window).height() - 100,
                            open: function (event, ui) { },
                            title: "Order History"
                        }
                        );

                        busyMode(false);
                    },
                    error: function (msg) {
                        console.log("call GetSODetail err:" + msg.d);
                        busyMode(false);
                    }
                });
        }
        function busyMode(mode) {
            (mode == true) ? $("#UpdateProgress2").css("visibility", "visible") : $("#UpdateProgress2").css("visibility", "hidden");
            // (mode == true) ? $("#imgLoading").css("style", "block") : $("#imgLoading").css("style", "none");
        }
        function checkreason() {
            var _ISAKR = "<%=_IsKRAonlineUser%>";
            var _LowGPSalesReason = $("#<%=txtRelatedInfo.ClientID%>").val().trim().replace(/^\s+|\s+$/g, "");
            if (_ISAKR == 'True' && _LowGPSalesReason == '') {
                alert("Please input low GP reason for GP approval flow.");
                return false;
            }
            //alert(_LowGPSalesReason);

        }
    </script>
</asp:Content>
