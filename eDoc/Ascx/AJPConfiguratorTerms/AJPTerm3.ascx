<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="AJPTerm3.ascx.vb" Inherits="EDOC.AJPTerm3" %>

<style type="text/css">
    .margin_left20 {
        margin: 0 0 0 20px;
    }

    .margin_left30 {
        margin: 0 0 0 30px;
    }

    .margin_left40 {
        margin: 0 0 0 40px;
    }

    .mrgin_top10 {
        margin: 10px 0;
    }

    .mrgin_0_30 {
        margin: 0 30px;
    }

    .mrgin_0_40 {
        margin: 0 40px;
    }

    .mrgin_10_30 {
        margin: 10px 30px 0px 30px;
    }

    .text_center {
        text-align: center;
    }

    .text_left {
        text-align: left;
    }

    .text_right {
        text-align: right;
    }

    .v_align {
        vertical-align: top;
    }

    .font_size_small {
        font-size: small;
    }

    .font_size_larger {
        font-size: x-large;
    }

    .font_size_large {
        font-size: large;
    }

    .font_bold {
        font-weight: bold;
    }

    .text_decoration {
        text-decoration: underline;
    }

    .css_table {
        display: table;
    }

    .css_tr {
        display: table-row;
    }

    .css_td {
        display: table-cell;
    }

    .w99p {
        width: 99%;
    }

    .w90p {
        width: 90%;
    }

    .w48p {
        width: 48%;
    }

    .w80p {
        width: 80%;
    }

    .h35p {
        height: 35%;
    }

    .h40p {
        height: 40%;
    }

    .h45p {
        height: 45%;
    }

    .bg_blue {
        background-color: #c6d9fe;
    }

    .border {
        border-style: solid;
        border-color: #385d8a;
    }

    .border_none {
        border-style: none;
    }

    #triangle-right {
        width: 50px;
        height: 0;
        border-style: solid;
        border-width: 5px 0 5px 8.7px;
        border-color: transparent transparent transparent #4f81bd;
    }
</style>

<script src="../Js/jquery-ui.js" type="text/javascript"></script>
<script src="../Js/json2.js" type="text/javascript"></script>

<script>
    $(document).ready(function () {
        $('#cb_Final_Confirm').click(function () {
            if (($('input#cb_Final_Confirm').is(':checked') == true)) {
                $("#btnAJPConfirm").focus();
            }
        });
    });

    function ProcessingAJPTerm3() {
        var FINAL_HWSummary = $('#txt_FINAL_HWSummary').val();
        var FINAL_SWSummary = $('#txt_FINAL_SWSummary').val();
        var postData = {
            QuoteID: "<%=Request("UID")%>",
            FINAL_HWSummary: FINAL_HWSummary,
            FINAL_SWSummary: FINAL_SWSummary
        };
        $.ajax({
            url: "<%= Util.GetRuntimeSiteUrl()%>/Services/AJPTerms.asmx/AJPTerm3",
            type: "POST",
            dataType: 'json',
            data: postData,
            async: false,
            success: function (retData) {
                if (retData) {
                }
            },
            error: function (msg) {
                console.log("err:" + msg.d);
            }
        });
    }
</script>

<div class="font_size_small">
    お客様のシステムは以下の設定で組み立てを行います。<br />
    もし、設定に間違いが無い場合には下にある<br />
    <span class="margin_left30">□注文を確定する</span><br />
    のチェックボックスをチェックして、次に進んでください。<br />
    <br />
    もし、変更をやり直す場合には、ブラウザの[戻る]ボタンで戻り、改めて設定内容の記入をお願いします。<br />
    <div class="mrgin_0_30 bg_blue border" style="height: 400px;">
        <p class="font_bold mrgin_10_30">ハードウェア設定内容のまとめ</p>
        <textarea rows="15" cols="" class="border w90p h45p bg_blue mrgin_0_40" id="txt_FINAL_HWSummary" readonly="readonly"></textarea>
        <p class="font_bold mrgin_10_30">ソフトウェア設定内容のまとめ</p>
        <textarea rows="10" cols="" class="border w90p h35p bg_blue mrgin_0_40" id="txt_FINAL_SWSummary" readonly="readonly"></textarea>
    </div>
    <br />
    <div class="css_table margin_left40">
        <div class="css_tr">
            <div class="css_td">
                <span class="margin_left30 font_size_large font_bold" id="span_Final" style="background-color: yellow">
                    <input type="checkbox" id="cb_Final_Confirm" />この構成内容を確定、次へ進む</span>
            </div>
            <div class="css_td">
                <span class="margin_left40">
                    <img src="../../Images/Current.png" style="width: 50px; height: 30px;" />
                </span>
            </div>
            <div class="css_td">
                <span class="margin_left40 font_size_large font_bold text_decoration">次に進む</span><br />
                <span class="margin_left40 text_decoration">システムの送り先、決済情報を確認します</span>
            </div>
        </div>
    </div>
    <p class="text_center  text_decoration mrgin_top10">
        <a href="javascript:void(0);" onclick="FancyBoxGotoIndex(0)">ハードウェア設定をやり直すためには、このリンクをクリックしてください（１／３に戻ります）</a>
    </p>
    <p class="text_center  text_decoration mrgin_top10">
        <a href="javascript:void(0);" onclick="FancyBoxGotoIndex(1)">ソフトウェア設定をやり直すためにはこのリンクをクリックしてください（２／３に戻ります）</a>
    </p>
    <p class="text_center font_size_larger font_bold mrgin_top10">３／３</p>
    <p class="text_center font_size_larger font_bold mrgin_top10">
        <input class="text_center" type="button" value="Confirm" id="btnAJPConfirm" onclick="JPConfirm()" />
    </p>
</div>
