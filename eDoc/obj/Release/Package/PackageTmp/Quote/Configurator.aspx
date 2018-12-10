<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/myMaster.master" CodeBehind="Configurator.aspx.vb" Inherits="EDOC.Configurator" %>

<%@ Register Src="~/Ascx/AJPConfiguratorTerms/AJPTerm1.ascx" TagName="AJPTerm1" TagPrefix="myASCX" %>
<%@ Register Src="~/Ascx/AJPConfiguratorTerms/AJPTerm2.ascx" TagName="AJPTerm2" TagPrefix="myASCX" %>
<%@ Register Src="~/Ascx/AJPConfiguratorTerms/AJPTerm3.ascx" TagName="AJPTerm3" TagPrefix="myASCX" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../Js/jquery-ui.js" type="text/javascript"></script>
    <script src="../Js/json2.js" type="text/javascript"></script>
    <link rel="Stylesheet" href="../Js/FancyBox/jquery.fancybox.css" type="text/css" />
    <script type="text/javascript" src="../Js/FancyBox/jquery.fancybox.js"></script>

    <style>
        .fancybox-next span {
            visibility: visible;
        }

        .fancybox-prev span {
            visibility: visible;
        }

        .fancybox-nav {
            width: 5%;
        }

        .fancybox-prev {
            left: -50px;
        }

        .fancybox-next {
            right: -50px;
        }

        .JPNotice_h2 {
            color: #1b1b69;
            text-align: center;
        }

        .JPNotice_div {
            width: 100%;
            height: 90%;
        }

        .JPNotice_ul {
            font-size: 15px;
            text-align: left;
            margin-left: 25%;
            list-style-position: inside;
        }

            .JPNotice_ul li {
                margin: 10px 0;
            }

        .JPNotice_checkbox {
            font-size: 15px;
        }
    </style>

    <script type="text/javascript">

        $(document).ready(function () {
            // if is C-CTOS system, needs to check EOL items and show on screen
            if ("<%=Request("BTOItem")%>".includes("C-CTOS")) {
                GetEOLItems();
            }
        });        

        //===================== Ryan 20161222 For AJP using =====================
        function ShowJPNotice() {
            var gallery = [{
                href: "#divJPNotice01"
            }, {
                href: "#divJPNotice02"
            }, {
                href: "#divJPNotice03"
            }];

            $.fancybox(gallery, {
                'autoSize': false,
                'width': 900,
                'height': 700,
                'autoCenter': true,

                beforeShow: function () {
                    var thisIndex = this.index;

                    // if index = 2 means final page, needs to get term1 and term2 data for preview.
                    if (thisIndex == 2) {
                        var hw_summary = GetAJPTerm1Preview();
                        var sw_summary = GetAJPTerm2Preview();

                        $("textarea#txt_FINAL_HWSummary").val(hw_summary);
                        $("textarea#txt_FINAL_SWSummary").val(sw_summary);
                    }
                }
            });
        }

        function JPConfirm() {
            if (($('input#cb_HW_Preview0').is(':checked') == false) && ($('input#cb_HW_Preview1').is(':checked') == false)) {
                $('#span_HW_1').css({ "backgroundColor": "#FF3030" });
                $('#span_HW_2').css({ "backgroundColor": "#FF3030" });
                FancyBoxGotoIndex(0);
                return false;
            }

            if (($('input#cb_Final_Confirm').is(':checked') == false)) {
                $('#span_Final').css({ "backgroundColor": "#FF3030" });
                return false;
            }

            // if AJP users are forced to select a 207 part in AJPTerm2 auto complete textbox and they didn't select any.
            //if ($('#divExtraSelection').css('display') != 'none')
            //{
            //    var extrapart = $('#txtExtraSelection').val();
            //    if (!extrapart || extrapart == "")
            //    {
            //        alert("Please select a 207- part first.");
            //        FancyBoxGotoIndex(1);
            //        $('#divinnerExtraSelection').css({ "backgroundColor": "#FFFF00" });
            //        return;
            //    }
            //}            

            ProcessingAJPTerm1();
            ProcessingAJPTerm2();
            ProcessingAJPTerm3();
            $.fancybox.close();
            checkAndContinue();
        }

        // Ryan 20170510 Add for AJP special 207- parts validation logic
        function AddAJPExtraSelection(arrCatcomp) {
            if ($('#hdOrg').val() == "JP01")
            {
                var extrapart = $('#txtExtraSelection').val();
                if (extrapart && extrapart != "") {
                    var compNode = {
                        CategoryId: extrapart,
                        CategoryType: "component",
                        ChildComps: []
                    };
                    var catNode = {
                        CategoryId: "Extra Selection",
                        CategoryType: "category",
                        ChildComps: []
                    };
                    catNode.ChildComps.push(compNode);
                    arrCatcomp.ChildComps.push(catNode);
                }
            }           
        }

        function FancyBoxGotoIndex(index) {
            $.fancybox.jumpto([index]);
        }

        function checkAndContinue2() {
            if ($('#hdOrg').val() == "JP01") {
                var blReqNotChecked = CheckRequiredCategory();
                if (blReqNotChecked == false) {

                    // ==============================Ryan 20170509 Add for AJP =============================
                    // Check if all three conditions are met, if so, enable auto complete textbox in AJP term2
                    // 1. if is AGS-CTOS-SYS-A 
                    // 2. select one or more 968T/968Q items (material group= 968MS/SW)
                    // 3. no 206- items are selected
                    var arrCatcomp = {
                        CategoryId: $("#hdBTOId").val(),
                        CategoryType: "category",
                        ChildComps: []
                    };
                    getCheckedComps('tbConfigurator', arrCatcomp);
                    var postData = {
                        RootComp: JSON.stringify(arrCatcomp)
                    };
                    $.ajax({
                        url: "<%= Util.GetRuntimeSiteUrl()%>/Services/AJPTerms.asmx/CheckAJPConfigurationFor207Parts",
                        type: "POST",
                        dataType: 'json',
                        data: postData,
                        async: true,
                        success: function (retData) {
                            if (retData) {                                
                                ShowAJPExtraSelection();
                            }
                            else {                                
                                HideAJPExtraSelection();
                            }
                        },
                    });
                    // ==============================End Ryan 20170509 Adding========================

                    ShowJPNotice();
                }
            }
            else
                checkAndContinue();
        }
        // ===================== End Ryan 20161222 Adding =====================

        function GetEOLItems() {

            var postData = {
                Root: "<%=Request("BTOItem")%>",
                ORGID: "<%=Pivot.CurrentProfile.getCurrOrg%>"
            };
            $.ajax({
                url: "<%= Util.GetRuntimeSiteUrl()%>/Services/eQ25WS.asmx/GetEOLItems",
                type: "POST",
                dataType: 'json',
                data: postData,
                success: function (retData) {
                    if (retData) {
                        if (retData.length > 0) {
                            var EOLItems = "This configuration contains EOL items as below:" + "\n\n";
                            for (i = 0; i < retData.length; i++) {
                                EOLItems += retData[i] + "\n";
                            }
                            EOLItems += "\n" + "Please contact dedicated FAE/AE to update the CTOS document.";
                            alert(EOLItems);
                        }
                    }
                },
                error: function (msg) {
                    console.log("err:" + msg.d);
                }
            });
        }

        function InitReconfigData(rid) {
            busyMode(1);
            var postData = JSON.stringify({ ReConfigId: rid, CompanyId: $('#hdCompanyId').val(), UID: $('#hdUID').val() });
            $.ajax(
                {
                    type: "POST",
                    url: "Configurator.aspx/GetReconfigTree",
                    data: postData,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (retData) {
                        //console.log("call GetReconfigTree ok");
                        var ReconfigTreeObject = $.parseJSON(retData.d);
                        $("#hdBTOId").val(ReconfigTreeObject.BTOItem);
                        $("#tbConfigurator").html(ReconfigTreeObject.ReConfigTreeHtml);
                        $("#hdConfigQty").val(ReconfigTreeObject.ReConfigQty);
                        var priceNodes = $(".divPriceValue");
                        //console.log("priceNodes length:" + priceNodes.length);
                        $.each(priceNodes, function (idx, item) {

                            //20180124 - Comment below out, original html has set checked property, no need to do again and causes bugs.
                            //$($($(item).parent().parent()).children(".compOption")).prop("checked", true);

                            //console.log("check mate");
                        }
                        );
                        calcTotalPriceMaxDueDate();
                        var arrCatcomp = {
                            CategoryId: ReconfigTreeObject.BTOItem,
                            CategoryType: "category",
                            ChildComps: []
                        };
                        $("#tbConfigResult").html(getCheckedComps('tbConfigurator', arrCatcomp));
                        busyMode(0);
                    },
                    error: function (msg) {
                        //console.log("call GetReconfigTree err:" + msg.d);
                        busyMode(0);
                    }
                }
            );
        }

        function InitLoadBOM() {
            appendChildCBom($('#tbConfigurator'), $('#hdBTOId').val(), 0); var $scrollingDiv = $("#configResult"); $scrollingDiv.css("opacity", 0.9);
            $(window).scroll(function () {
                $scrollingDiv
                .stop()
                .animate({ "marginTop": ($(window).scrollTop()) }, "slow");
            });
        }

        function busyMode(modeCode) {
            //(modeCode == 1) ? $("body").css("cursor", "progress") : $("body").css("cursor", "auto");
            var progressNode = $("#UpdateProgress2");
            if (modeCode == 1) {
                progressNode.css("visibility", "visible");
            }
            else {
                progressNode.css("visibility", "hidden");
            }
        }

        function fillChildBOM(inputId, tableId, level) {
            $("#" + inputId).prop('checked', true);
            var inputname = $("#" + inputId).prop('name');
            var selectedInputs = $("input[name='" + inputname + "']");
            $.each(selectedInputs, function (idx, item) {
                if ($(item).prop('checked')) {
                    $(item).attr("checked", "CHECKED");
                }
                else {
                    $(item).removeAttr("checked");
                }
            });

            var categoryValue = $("#" + inputId).attr("compname");
            //$(categoryValue).css("width", "100%");
            //console.log("categoryValue:" + $("#" + inputId));
            calcTotalPriceMaxDueDate();
            var targetTable = $('#' + tableId);
            appendChildCBom(targetTable, categoryValue, level);
            var arrCatcomp = {
                CategoryId: categoryValue,
                CategoryType: "category",
                ChildComps: []
            };
            $("#tbConfigResult").html(getCheckedComps('tbConfigurator', arrCatcomp));
            if (inputId == "" || categoryValue == "") return;
            var priceNode = $("#" + inputId).parent().children(".divPrice")[0]; atpNode = $("#" + inputId).parent().children(".divATP")[0];
            var postData = JSON.stringify({ ComponentCategoryId: categoryValue, ConfigQty: $('#hdConfigQty').val(), Currency: $('#hdCurrency').val(), UID: $('#hdUID').val(), ORG: $('#hdOrg').val() });
            $.ajax(
                {
                    type: "POST",
                    url: "Configurator.aspx/GetCompPriceATP",
                    data: postData,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (retData) {
                        var priceATP = $.parseJSON(retData.d);
                        //Check if item is AGS-EW item                 
                        if (priceATP.IsEw == false) {
                            $(priceNode).html("<b>Price:</b>" + priceATP.CurrencySign + "<div class='divPriceValue' style='display:inline;'>" + priceATP.Price) + "</div>";
                            $(atpNode).html("<b>Available on:</b>" + "<div class='divATPValue' style='display:inline;'>" + priceATP.ATPDate + "</div>");
                        }
                        else {
                            $(priceNode).html("<b>Price:</b><div class='divPriceValue' style='display:inline;'>" + priceATP.Price + "%") + "</div>";
                            $(atpNode).html("<b>Available on:</b>" + "<div class='divATPValue' style='display:inline;'>" + (new Date()).format("yyyy/MM/dd") + "</div>");
                        }
                        calcTotalPriceMaxDueDate();
                    },
                    error: function (msg) {
                        //console.log('err getpriceatp ' + msg.d);
                        busyMode(0);
                    }
                }
            );
        }

        function appendChildCBom(tableElement, CategoryValue, level) {
            //console.log("appendChildCBomCategoryValue:" + CategoryValue);
            tableElement.empty();
            //if (!CategoryValue) { tableElement.empty(); return; }
            level = parseInt(level); var tableId = tableElement.attr('id');
            if (!CategoryValue || CategoryValue == "") {
                tableElement.css('border-style', 'none'); return;
            }
            //console.log("hdIsOneLevel:" + $("#hdIsOneLevel").val());
            if ($("#hdIsOneLevel").val() == "1" && level > 0) { return; }

            busyMode(1);
            var postData = JSON.stringify({ ParentCategoryId: CategoryValue, ConfigQty: $('#hdConfigQty').val(), RBU: $('#hdRBU').val(), ORG: $('#hdOrg').val(), RootId: $("#hdBTOId").val() });
            $.ajax(
                {
                    type: "POST",
                    url: "Configurator.aspx/GetCBOM",
                    data: postData,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (retData) {
                        appendChildCBomSuccess(retData, tableElement, level);
                    },
                    error: function (msg) {
                        //console.log('err GetCBOM ' + msg.d);
                        busyMode(0);
                    }
                }
            );
        }

        var ChildSequence = 0;
        function appendChildCBomSuccess(retData, tableElement, level) {
            var bomRows = $.parseJSON(retData.d);
            //For each category append it to tableElement
            $.each(bomRows, function (index, item) {
                if (item.ChildCategories.length == 0) {
                    if (item.IsCatRequired) {

                        //Ryan 20161220 Do not disable button for AJP
                        //disable order button and alert category is required but has no component to choose
                        if ($('#hdOrg').val() == "JP01")
                            $(".continueBtn").prop('disabled', false);
                        else
                            $(".continueBtn").prop('disabled', true);

                        $($("#divReqCatNoComp").children("b")).text(item.CategoryId);
                        $("#divReqCatNoComp").dialog({
                            modal: true,
                            width: '30%',
                            height: 120,
                            open: function (event, ui) {
                                //setTimeout("$('#divReqCatNoComp').dialog('close')", 7000);
                            }
                        });
                    }
                    //console.log(item.CategoryId + " has no components"); 
                    return true;
                }
                ChildSequence += 1;
                var childTableId = "chtb_" + item.ClientId + "_" + (level + 1) + ChildSequence; selGrpName = "grp_" + childTableId + ChildSequence;
                var compSelection = "<table width='100%' isreq=" + ((item.IsCatRequired) ? "true" : "false") + " catname='" + item.CategoryId + "'>";
                if (item.ChildCategories.length > 0) {
                    tableElement.css('border-style', 'ridge');
                    //Add Select... as the first component selection
                    compSelection +=
                                    "<tr>" +
                                        "<td>" +
                                            ((item.IsExtDesc) ? ("<p style='margin-top:5px; margin-bottom:5px; margin-left:5px'><span style='color:red;'>" + item.ExtDescription + "</span></p>") : "") + // ICC 2015/4/13 Add eStore CTOS extended description before input radio button
                                            "<input type='radio' compname='' name='" + selGrpName + "' onclick=fillChildBOM('','" + childTableId + "','" + (level + 1) + "'); " + ((hasDefaultComp) ? "" : "checked") + ">Select..." +
                                        "</td>" +
                                    "</tr>";
                }

                var hasDefaultComp = false;
                $.each(item.ChildCategories, function (idx, compItem) { if (compItem.IsCompDefault) hasDefaultComp = true; });
                var showHideAnchorId = "showHideAnchor_" + item.ClientId + ChildSequence; trSelId = "trSel_" + item.ClientId + ChildSequence;
                //For each component under current catetory append it under category
                $.each(item.ChildCategories, function (idx, compItem) {
                    //console.log("IsCompDefault:"+compItem.IsCompDefault);
                    //Loop all components and add as current category
                    var inputCompId = "rcomp_" + compItem.ClientId + "_" + idx + ChildSequence + "_" + (level + 1);
                    compSelection +=
                                "<tr>" +
                                    "<td>" +
                                        "<input compname='" + compItem.CategoryId + "' id='" + inputCompId + "' class='compOption' type='radio' name='" + selGrpName + "' onclick=fillChildBOM('" + inputCompId + "','" + childTableId + "','" + (level + 1) + "'); " + ((compItem.IsCompDefault) ? "checked" : "") + " />" +
                                            compItem.CategoryId + " -- " + compItem.Description +
                                            ((compItem.IsCompRoHS == true) ? "&nbsp;<img alt='RoHS' src='../Images/rohs.jpg' />" : "") +
                                            ((compItem.IsHot == true) ? "&nbsp;<img alt='Hot' src='../Images/Hot-orange.gif' />" : "") +
                                            "&nbsp;<div class='divPrice' style='display:inline'></div>" +
                                            "&nbsp;<div class='divATP' style='display:inline'></div>" +
                                    "</td>" +
                                "</tr>";
                    if (compItem.IsCompDefault) {
                        //console.log("setting time out for " + childTableId);
                        setTimeout("fillChildBOM('" + inputCompId + "','" + childTableId + "', '" + (level + 1) + "');", 100);
                    }
                }
                            );
                compSelection +=
                                "<tr>" +
                                    "<td>" +
                                    "   <table class='trChildTable' width='100%' id='" + childTableId + "' style='border-style:none'></table>" +
                                    "</td>" +
                                "</tr>" +
                            "</table>";

                //Frank 20131009
                var isExpand = false;
                if (item.IsCatRequired == true || hasDefaultComp == true) {
                    isExpand = true;
                };

                tableElement.append(
                                "<tr>" +
                                    "<td>" +
                                        "<table width='100%'>" +
                                            "<tr style='background-color:#000080; width:100%'>" +
                                                "<td style='color:White; font-weight:bold; width:100%'>" +
                                                    "<input class='catHeader' type='button' style='width:13px; height:17px' id='" + showHideAnchorId + "' onclick=collapseExpand('" + showHideAnchorId + "','" + trSelId + "'); value='" + ((item.IsCatRequired) ? "-" : "+") + "' /> " +
                                                    item.CategoryId + ((item.IsCatRequired) ? " <font color='red'>(Required)</font>" : "") +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr id='" + trSelId + "' class='trSelection' style='width:100%; display:" + ((isExpand) ? "block" : "none") + "'>" +
                                            "   <td>" + compSelection + "</td>" +
                                            "</tr>" +
                                        "</table>" +
                                    "</td>" +
                                "</tr>");
                $(tableElement).css("width", "100%");
            });
            busyMode(0);
        }

        function collapseExpand(anchorId, trSelId) {
            var anchorNode = $("#" + anchorId); var trSelNode = $("#" + trSelId);
            if (anchorNode.val().indexOf("-") >= 0) {
                trSelNode.css("display", "none"); anchorNode.val("+");
            }
            else {
                trSelNode.css("display", "block"); anchorNode.val("-"); trSelNode.css("width", "100%");
            }
        }

        function collapseExpandAll() {
            //console.log($("#colExpAll").text());
            if ($("#colExpAll").text().indexOf("Collapse") >= 0) {
                $(".trSelection").css("display", "none");
                $(".catHeader").val("+");
                $("#colExpAll").text("Expand All");
            }
            else {
                $(".trSelection").css("display", "block");
                $(".catHeader").val("-");
                $("#colExpAll").text("Collapse All");
            }
            //            var displayValue = ($("#colExpAll").text().indexOf("Collapse")>=0) ? "none" : "block";
            //            $(".trSelection").css("display", displayValue);
            //            ($("#colExpAll").text().indexOf("Collapse") >= 0) ? $("#colExpAll").text("Expand All") : $("#colExpAll").text("Collapse All");
        }

        function checkAndContinue() {
            $(".continueBtn").prop('disabled', true);

            //Ryan 20170203 Move check category code to function "CheckRequiredCategory" (can be shared due to AJP launching)
            var blReqNotChecked = CheckRequiredCategory();

            if (blReqNotChecked == false) {
                //console.log("blReqNotChecked is false");
                var arrCatcomp = {
                    CategoryId: $("#hdBTOId").val(),
                    CategoryType: "category",
                    ChildComps: []
                };
                busyMode(1);
                getCheckedComps('tbConfigurator', arrCatcomp);
                AddAJPExtraSelection(arrCatcomp);

                var postData = JSON.stringify({ RootComp: arrCatcomp, ConfigQty: $('#hdConfigQty').val(), ConfigTreeHtml: $("#tbConfigurator").html(), CompanyId: $('#hdCompanyId').val(), Org: $('#hdOrg').val(), UID: $('#hdUID').val(), ReConfigId: $('#ReConfigId').val(), IsOneLevel: $('#hdIsOneLevel').val() });
                $.ajax(
                {
                    type: "POST",
                    url: "Configurator.aspx/SaveConfigResult",
                    data: postData,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (retData) {
                        //console.log("GetConfigResult ok:" + retData.d);
                        var retMsg = $.parseJSON(retData.d);
                        busyMode(0);
                        if (retMsg.ProcessStatus == true) {
                            window.location.href = "/Quote/QuotationDetail.aspx?UID=<%=Request("UID")%>";
                        }
                        else {
                            alert("Error saved configuration to cart:\n" + retMsg.ProcessMessage.replace("\\n", "\n"));
                            $(".continueBtn").prop('disabled', false);
                        }
                    },
                    error: function (msg) {
                        //console.log('err GetConfigResult ' + msg.d);
                        busyMode(0);
                        //$(thisBtn).prop('disabled', false);
                        $(".continueBtn").prop('disabled', false);
                    }
                }
            );
            }
        }

        function CheckRequiredCategory() {
            var blReqNotChecked = false;
            var reqCats = $('table[isreq=true]');
            $.each(reqCats, function (idx, item) {
                //console.log("req cat id:" + $(item).parent().parent().attr("id"));
                var checkedCompOption = $("> tbody > tr > td > input.compOption:checked", $(item));
                if (checkedCompOption.length == 0) {
                    $("#colExpAll").text("Expand All"); collapseExpandAll();
                    $(window).scrollTop($(item).position().top - 350);
                    $("#reqCatDialog").find(".pcatname").text($(item).attr("catname"));
                    $("#reqCatDialog").dialog({
                        modal: true,
                        width: '50%',
                        //  height: 100,
                        open: function (event, ui) {
                            setTimeout("$('#reqCatDialog').dialog('close')", 3000);
                        }
                    });
                    blReqNotChecked = true;
                    $(".continueBtn").prop('disabled', false);
                }
            });
            return blReqNotChecked;
        }

        function getCheckedComps(tableId, arrCatcomp) {
            var retStr = "<table width='100%'>";
            var trSelectedComps = $("> tbody > tr > td > table > tbody > tr > td > table > tbody > tr > td > input.compOption:checked", $("#" + tableId));
            $.each(trSelectedComps, function (idx, item) {
                var childTable = $("> tr > td > table.trChildTable", $(item).parent().parent().parent());
                if (childTable) {
                    var catname = $($(item).parent().parent().parent().parent()).attr("catname");
                    var compname = $(item).attr("compname");
                    var compNode = {
                        CategoryId: compname,
                        CategoryType: "component",
                        ChildComps: []
                    };
                    var catNode = {
                        CategoryId: catname,
                        CategoryType: "category",
                        ChildComps: []
                    };
                    catNode.ChildComps.push(compNode);
                    arrCatcomp.ChildComps.push(catNode);
                    //console.log("catname:" + catname + ",compname:" + compname);
                    retStr +=
                      "<tr style='display:block'>" +
                        "<td><input style='width:13px; height:17px' type='button' value='-' onclick='showHideNextTr(this);' /></td>" +
                        "<td>" + catname + "</td>" +
                      "</tr>" +
                      "<tr style='display:block'>" +
                        "<td></td>" +
                        "<td>" +
                            "<table width='100%'>" +
                                "<tr>" +
                                    "<td>" + compname + "</td>" +
                                "</tr>" +
                                "<tr>" +
                                    "<td>" + getCheckedComps($(childTable).attr("id"), compNode) + "</td>" +
                                "</tr>" +
                            "</table>" +
                        "</td>" +
                      "</tr>";
                };
            }
            );
            retStr += "</table>";
            return retStr;
        }

        function showHideNextTr(o) {
            var blockOrNone = ($(o).val() == "-") ? "none" : "block"; var newValue = ($(o).val() == "-") ? "+" : "-"; $($(o).parent().parent().next()).css("display", blockOrNone); $(o).val(newValue);
        }

        function calcTotalPriceMaxDueDate() {
            var totalPrice = 0; var maxDd = new Date(); var ewRate = 0.0; var selectedInputs = $('input.compOption:checked');
            $.each(selectedInputs, function (idx, item) {
                var pNode = $(item).parent().children(".divPrice").children(".divPriceValue");
                //console.log("pNode count:" + pNode.length);
                if (pNode.length == 1) {
                    if ($(pNode[0]).text().match("%$")) {
                        ewRate = parseFloat($(pNode[0]).text());
                    }
                    else {
                        totalPrice += parseFloat($(pNode[0]).text());
                    }
                }
                var aNode = $(item).parent().children(".divATP").children(".divATPValue");
                if (aNode.length == 1) {
                    var cDate = new Date($(aNode).text()); var curMaxDate = maxDd; if ((cDate - curMaxDate) > 0) maxDd = cDate;
                }
            }
            );
            //console.log("ewRate:" + ewRate);
            totalPrice = totalPrice * (1 + ewRate * 0.01);
            $($(".totalPrice")[0]).text(totalPrice.toFixed(2)); $($(".maxDueDate")[0]).text(maxDd.format("yyyy/MM/dd"));
        }

        function showHideConfigResultDiv(o) {
            if ($(o).val() == "Hide") {
                $("#tbConfigResult").css("display", "none"); $("#tbConfigResult").css("right", "100px"); $("#tbConfigTable").css("display", "none"); $("#configResult").css("width", "30px"); $("#configResult").css("height", "1px"); $(o).val("Show");
            }
            else {
                $("#tbConfigResult").css("display", "block"); $("#tbConfigResult").css("right", "10px"); $("#tbConfigTable").css("display", "block"); $("#configResult").css("width", "300px"); $("#configResult").css("height", "350px"); $(o).val("Hide");
            }
        }

    </script>
    <input type="hidden" id="hdBTOId" />
    <input type="hidden" value="1" id="hdConfigQty" />
    <input type="hidden" value="0" id="hdIsOneLevel" />
    <input type="hidden" value="" id="hdCurrency" />
    <input type="hidden" value="" id="hdCompanyId" />
    <input type="hidden" value="" id="hdOrg" />
    <input type="hidden" value="" id="hdRBU" />
    <input type="hidden" value="" id="hdUID" />
    <input type="hidden" value="" id="ReConfigId" />
    <table width="100%">
        <tr>
            <td>
                <span style="width: 41%;" id="page_path" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <table width="100%">
                    <tr>
                        <td align="left" style="width: 15%">
                            <a onclick="collapseExpandAll();" id="colExpAll" href="javascript:void(0);">Collapse
                                All</a>
                        </td>
                        <td style="width: 35%" align="right">
                            <input type="button" value="Click to Continue" onclick="checkAndContinue2();" class="continueBtn" /><br />
                        </td>
                        <td>
                            <asp:Label ID="Msg" runat="server" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <table width="100%" id="tbConfigurator" style="border-style: ridge; height: 400px">
                    <tr>
                        <th>Loading...
                        </th>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center">
                <input type="button" value="Click to Continue" onclick="checkAndContinue2();" class="continueBtn" />
            </td>
        </tr>
    </table>
    <div id="configResult" style="display: block; background-color: #EBEBEB; width: 300px; height: 350px; position: fixed; bottom: 25%; right: 10px;">
        <input type="button" value="Hide" onclick="showHideConfigResultDiv(this);" class="continueBtn" />
        <table width="100%" style="display: block" id="tbConfigTable">
            <tr>
                <td>
                    <table>
                        <tr>
                            <th align="left">
                                <font color="black">Total Price:</font>
                            </th>
                            <td class="totalPriceCurrSign"></td>
                            <td class="totalPrice"></td>
                            <th align="left">
                                <font color="black">Max Due Date:</font>
                            </th>
                            <td class="maxDueDate"></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <input type="button" value="Click to Continue" onclick="checkAndContinue2();" class="continueBtn" />
                </td>
            </tr>
            <tr>
                <td>
                    <div style="width: 99%; height: 300px; overflow-x: auto; overflow-y: scroll;" id="tbConfigResult">
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div id="divReqCatNoComp" style="background-color: #EBEBEB; width: 30%; display: none; border-style: double; border-color: #FFA500">
        Category <b></b>is required but there is currently no available component.        
    </div>
    <div id="reqCatDialog" style="background-color: #EBEBEB; width: 50%; border-style: double; border-color: #FFA500; display: none">
        <h3 style="color: Black">Please select one component of category:</h3>
        <p class="pcatname" style="color: Red; font-weight: bold">
        </p>
    </div>
    <div id="fakeDiv" style="display: none"></div>

    <asp:Panel ID="divJP" runat="server" Visible="false">
        <div style="display: none;">
            <div id="divJPNotice01" class="JPNotice_div">
                <myASCX:AJPTerm1 ID="AJPTerm1" runat="server" />
            </div>
            <div id="divJPNotice02" class="JPNotice_div">
                <myASCX:AJPTerm2 ID="AJPTerm2" runat="server" />
            </div>
            <div id="divJPNotice03" class="JPNotice_div">
                <myASCX:AJPTerm3 ID="AJPTerm3" runat="server" />
            </div>
        </div>
    </asp:Panel>

</asp:Content>
