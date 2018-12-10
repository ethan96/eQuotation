<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ProductRelatedInfo.aspx.vb"
    Inherits="EDOC.ProductRelatedInfo" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Product Related Info</title>
    <script src="<%= Util.GetRuntimeSiteUrl()%>/Js/json2.js" type="text/javascript"></script>
    <script src="<%= Util.GetRuntimeSiteUrl()%>/Js/jquery-latest.min.js" type="text/javascript"></script>
    <script src="<%= Util.GetRuntimeSiteUrl()%>/Js/SiteUtil.js" type="text/javascript"></script>
</head>
<script type="text/javascript">
    function getPlantATP(strPN, strPlant, IsLocal) {
        if (strPN == '') {
            return;
        }
        if (IsLocal) {
            var divATP = $('#divLocalATP');
        } else {
            var divATP = $('#divACLATP');
        }
        var postData = JSON.stringify({ strPartNo: strPN, strPlant: strPlant });
        $.ajax({
            type: "POST",
            url: "ProductRelatedInfo.aspx/GetACLATP",
            data: postData,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                if ($.trim(msg.d) != "") {
                    //console.log('called atp ok');
                    var ATPTotalInfo = $.parseJSON(msg.d);
                    divATP.html('');
                    divATP.append("<tr ><th colspan='2' style='color:Black'>Plant: " + strPlant + "</th></tr>");
                    divATP.append("<tr class='HeaderStyle'><th>Available Date</th><th>Qty</th></tr>");
                    if (ATPTotalInfo.ATPRecords.length > 0) {
                        $.each(ATPTotalInfo.ATPRecords, function (i, item) {
                            divATP.append('<tr><td>' + item.AvailableDate + '</td><td>' + item.Qty + '</td></tr>');
                        });
                    }
                    else {
                        divATP.html("<tr><td><b>" + strPlant + " No  inventory" + "</b></td></tr>");
                    }
                    $("body").css("cursor", "auto");
                }
                else {
                    divATP.html("<tr><td><b>" + strPlant + " No  inventory" + "</b></td></tr>");
                }
            },
            error: function (msg) {
                divATP.html(msg.d);
                $("body").css("cursor", "auto");
            }
        }
            );
    }
    function getProductInfo(strPN, strPlant, IsLocal) {
        if (strPN == '') { return; }
        var postData = JSON.stringify({ strPartNo: strPN, strPlant: strPlant });
        $.ajax({
            type: "POST",
            url: "ProductRelatedInfo.aspx/GetProductInfo",
            data: postData,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {

                if ($.trim(msg.d) != "") {

                    var partinfo = $.parseJSON(msg.d);
                    var sb = new StringBuilder();
                    sb.append(" <tr> <td align='right' class='black'  nowrap='nowrap' width='100'> Part No : </td>");
                    sb.append(" <td nowrap='nowrap'> " + partinfo.PartNo + "</td>");
                    sb.append("</tr>");
                    sb.append(" <tr> <td align='right' class='black'  nowrap='nowrap' >  Product Status :</td>");
                    sb.append(" <td> " + partinfo.ProductStatus + " </td> </tr>");
                    sb.append("<tr> <td align='right' class='black'  nowrap='nowrap' >   ABCD Indicator :</td>");
                    sb.append(" <td> " + partinfo.Indicator + " </td> </tr>");
                    sb.append(" <tr> <td align='right' class='black'  nowrap='nowrap' >  MOQ :</td>");
                    sb.append(" <td>" + partinfo.MOQ + " </td> </tr>");
                    sb.append(" <tr> <td align='right' class='black'  nowrap='nowrap' >   PLM Notice :</td>");
                    sb.append(" <td> " + partinfo.PLMNotice + " </td></tr>");
                    sb.append(" <tr> <td align='right' class='black'  nowrap='nowrap' >   EOL :</td>");
                    sb.append(" <td> " + partinfo.IsApplyingForPhaseOutStatus + " </td></tr>");
                    $('#divProduct').html(sb.toString());

                }
                else {
                    $('#divProduct').html("<tr><td>Invalid Part NO</td></tr>");
                }
            },
            error: function (msg) {
                $('#divProduct').html("");
                alert(msg.d);
                $("body").css("cursor", "auto");
            }
        }
            );
    }
    $(document).ready(function () {

        //        var partno = $.getUrlParam('partno').toUpperCase();
        //        var orgid = $.getUrlParam('orgid').toUpperCase();
        //        var plant1 = "";
        //        var plant2 = ""
        //        if ($.trim(orgid) == "US01") {
        //            plant1 = "USH1";
        //            plant2 = "TWH1";
        //        }
        //        if ($.trim(orgid) == "EU10") {
        //            plant1 = "EUH1";
        //            plant2 = "TWH1";
        //        }
        //        if (partno != "" && plant1 != "" && plant2 != "") {
        //            var loadingimg = "<tr><td><img src='../images/LoadingRed.gif'/></td></tr>"
        //            $('#divProduct').html(loadingimg);
        //            $('#divLocalATP').html(loadingimg);
        //            $('#divACLATP').html(loadingimg);
        //            getProductInfo(partno, plant1, true);
        //            getPlantATP(partno, plant1, true);
        //            getPlantATP(partno, plant2, false);

        //        }
    });
</script>
<style type="text/css">
    body
    {
        margin-left: 0px;
        margin-top: 0px;
        margin-right: 0px;
        margin-bottom: 0px;
    }
    #divLocalATP, span, td, th
    {
        font-family: arial,\5B8B\4F53,Arial Narrow,serif;
        font-size: 12px;
    }
    .HeaderStyle
    {
        color: white;
        background-color: #FF6600;
    }
    .black
    {
        color: #000000;
        font-weight: bold;
    }
    td, tbody
    {
        vertical-align: top;
    }
</style>
<body>
    <form id="form1" runat="server" style="height: 100%;">
    <div id="ProductRelatedInfoFrame">
    </div>
    <table border="0" align="left" style="height: 100%; border-width: thin; border-style: solid;
        vertical-align: top;">
        <tr>
            <td valign="top" align="left">
                <table id="divProduct" width="230" valign="top">
                </table>
            </td>
            <td align="left" valign="top">
                <table id="divLocalATP" valign="top">
                </table>
            </td>
            <td align="left" valign="top">
                <table id="divACLATP" valign="top">
                </table>
            </td>
            <td align="left" valign="top">
                <asp:Label ID="ShippingVia" runat="server" Text="" />
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
