<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="AJPTerm2.ascx.vb" Inherits="EDOC.AJPTerm2" %>

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

    .h40p {
        height: 40%;
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

<script>
    $(document).ready(function () {
        Term2SetValuetoForm();

        $('#txtExtraSelection').tokenInput("<%=System.IO.Path.GetFileName(Request.ApplicationPath) %>/Services/AutoComplete.asmx/GetTokenInput207Parts?ORGID=JP01", {
            theme: "facebook", searchDelay: 200, minChars: 1, tokenDelimiter: ";", hintText: "Type a 207- part",
            tokenLimit: 1, preventDuplicates: true, resizeInput: false, resultsLimit: 7, zindex:9999,
            resultsFormatter: function (data) {
                return "<li style='border-bottom: 1px solid #003377;'>" + "<span style='font-weight: bold;font-size: 14px;'>" + data.name + "</span><br/>" + "<span style='color:gray;'>" + data.id + "</span></li>";
            },
            onAdd: function (data) {
                $('#txtExtraSelection').val(data.name);
            }
        });
    });

    function ShowAJPExtraSelection() {
        $('#divExtraSelection').show();
    }

    function HideAJPExtraSelection()
    {
        $('#divExtraSelection').hide();
    }


    function ProcessingAJPTerm2() {

        var SW_OS_Installation = "";
        if ($("input:radio[name='rb_SW_OS_Installation']:checked").val() == 1)
            SW_OS_Installation = $('#txt_SW_OS_Installation').val();

        var SW_Username = "";
        if ($("input:radio[name='rb_SW_Username']:checked").val() == 1)
            SW_Username = $('#txt_SW_Username').val();

        var SW_OS_Timezone = "";
        if ($("input:radio[name='rb_SW_OS_Timezone']:checked").val() == 1)
            SW_OS_Timezone = $('#txt_SW_OS_Timezone').val();

        var SW_OS_Input = "";
        if ($("input:radio[name='rb_SW_OS_Input']:checked").val() == 1)
            SW_OS_Input = $('#txt_SW_OS_Input').val();

        var SW_IP_Settings = "";
        if ($("input:radio[name='rb_SW_IP_Settings']:checked").val() == 1)
            SW_IP_Settings = $('#txt_SW_IP_Settings').val();

        var SW_Settings = "";
        if ($("input:radio[name='rb_SW_Settings']:checked").val() == 1)
            SW_Settings = $('#txt_SW_Settings').val();

        var SW_Others = $('#txt_SW_Others').val();

        var postData = {
            QuoteID: "<%=Request("UID")%>",
                    SW_OS_Installation: SW_OS_Installation,
                    SW_Username: SW_Username,
                    SW_OS_Timezone: SW_OS_Timezone,
                    SW_OS_Input: SW_OS_Input,
                    SW_IP_Settings: SW_IP_Settings,
                    SW_Settings: SW_Settings,
                    SW_Others: SW_Others
                };
                $.ajax({
                    url: "<%= Util.GetRuntimeSiteUrl()%>/Services/AJPTerms.asmx/AJPTerm2",
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

    function GetAJPTerm2Preview() {
        var result = "";

        if ($('#txt_SW_OS_Installation').val().length > 0)
            result += "注文したＯＳをインストールします: " + $('#txt_SW_OS_Installation').val() + "\n";

        if ($('#txt_SW_Username').val().length > 0)
            result += "ユーザー名の設定について: " + $('#txt_SW_Username').val() + "\n";

        if ($('#txt_SW_OS_Timezone').val().length > 0)
            result += "OSのタイムゾーンの設定について: " + $('#txt_SW_OS_Timezone').val() + "\n";

        if ($('#txt_SW_OS_Input').val().length > 0)
            result += "キーボード配列の設定について: " + $('#txt_SW_OS_Input').val() + "\n";

        if ($('#txt_SW_IP_Settings').val().length > 0)
            result += "ＩＰアドレスの設定について: " + $('#txt_SW_IP_Settings').val() + "\n";

        if ($('#txt_SW_Settings').val().length > 0)
            result += "その他の設定について: " + $('#txt_SW_Settings').val() + "\n";

        if ($('#txt_SW_Others').val().length > 0)
            result += "その他: " + $('#txt_SW_Others').val() + "\n";

        return result;
    }

    function SW_Preview() {

        if ($('input#cb_SW_Preview').is(':checked') == false) {
            $("input[name='rb_SW_OS_Installation']").attr("disabled", false);
            $("input[name='rb_SW_Username']").attr("disabled", false);
            $("input[name='rb_SW_OS_Timezone']").attr("disabled", false);
            $("input[name='rb_SW_OS_Input']").attr("disabled", false);
            $("input[name='rb_SW_IP_Settings']").attr("disabled", false);
            $("input[name='rb_SW_Settings']").attr("disabled", false);
            $("input[name='rb_SW_Others']").attr("disabled", false);

            $('#txt_SW_OS_Installation').attr("disabled", false);
            $('#txt_SW_Username').attr("disabled", false);
            $('#txt_SW_OS_Timezone').attr("disabled", false);
            $('#txt_SW_OS_Input').attr("disabled", false);
            $('#txt_SW_IP_Settings').attr("disabled", false);
            $('#txt_SW_Settings').attr("disabled", false);
            $('#txt_SW_Others').attr("disabled", false);
        }
        else {
            $("input[name='rb_SW_OS_Installation'][value='0']").prop("checked", true);
            $("input[name='rb_SW_Username'][value='0']").prop("checked", true);
            $("input[name='rb_SW_OS_Timezone'][value='0']").prop("checked", true);
            $("input[name='rb_SW_OS_Input'][value='0']").prop("checked", true);
            $("input[name='rb_SW_IP_Settings'][value='0']").prop("checked", true);
            $("input[name='rb_SW_Settings'][value='0']").prop("checked", true);

            $("input[name='rb_SW_OS_Installation']").attr("disabled", true);
            $("input[name='rb_SW_Username']").attr("disabled", true);
            $("input[name='rb_SW_OS_Timezone']").attr("disabled", true);
            $("input[name='rb_SW_OS_Input']").attr("disabled", true);
            $("input[name='rb_SW_IP_Settings']").attr("disabled", true);
            $("input[name='rb_SW_Settings']").attr("disabled", true);
            $("input[name='rb_SW_Others']").attr("disabled", true);

            $('#txt_SW_OS_Installation').attr("disabled", true);
            $('#txt_SW_Username').attr("disabled", true);
            $('#txt_SW_OS_Timezone').attr("disabled", true);
            $('#txt_SW_OS_Input').attr("disabled", true);
            $('#txt_SW_IP_Settings').attr("disabled", true);
            $('#txt_SW_Settings').attr("disabled", true);
            $('#txt_SW_Others').attr("disabled", true);
        }
    }

    function Term2SetValuetoForm() {
        var postData = {
            QuoteID: "<%=Request("UID")%>"
        };
        $.ajax({
            url: "<%= Util.GetRuntimeSiteUrl()%>/Services/AJPTerms.asmx/GetAJPTermValue",
            type: "POST",
            dataType: 'json',
            data: postData,
            success: function (retData) {
                if (retData) {
                    if (retData.SW_OS_Installation.length > 0) {
                        $('#txt_SW_OS_Installation').val(retData.SW_OS_Installation);
                        $("input[name='rb_SW_OS_Installation'][value='1']").prop("checked", true);
                    }

                    if (retData.SW_Username.length > 0) {
                        $('#txt_SW_Username').val(retData.SW_Username);
                        $("input[name='rb_SW_Username'][value='1']").prop("checked", true);
                    }

                    if (retData.SW_OS_Timezone.length > 0) {
                        $('#txt_SW_OS_Timezone').val(retData.SW_OS_Timezone);
                        $("input[name='rb_SW_OS_Timezone'][value='1']").prop("checked", true);
                    }

                    if (retData.SW_OS_Input.length > 0) {
                        $('#txt_SW_OS_Input').val(retData.SW_OS_Input);
                        $("input[name='rb_SW_OS_Input'][value='1']").prop("checked", true);
                    }

                    if (retData.SW_IP_Settings.length > 0) {
                        $('#txt_SW_IP_Settings').val(retData.SW_IP_Settings);
                        $("input[name='rb_SW_IP_Settings'][value='1']").prop("checked", true);
                    }

                    if (retData.SW_Settings.length > 0) {
                        $('#txt_SW_Settings').val(retData.SW_Settings);
                        $("input[name='rb_SW_Settings'][value='1']").prop("checked", true);
                    }

                    if (retData.SW_Others.length > 0) {
                        $('#txt_SW_Others').val(retData.SW_Others);
                    }
                }
            },
            error: function (msg) {
                console.log("err:" + msg.d);
            }
        });
    }
</script>

<div class="font_size_small">
    <p class="text_decoration w48p text_center  font_bold">システム設定項目</p>
    <br />
    <div id="divExtraSelection" class="css_tr">
        <div id="divinnerExtraSelection" style="margin-left: 25px">
            <b style="color: red">Image Item Selection:&nbsp;</b>
            <input type="text" id="txtExtraSelection" style="width: 100px" /><br />
        </div>
        <div class="css_tr">
            <div class="css_td">&nbsp;</div>
        </div>
    </div>
    <div class="css_table">
        <div class="css_tr">
            <div class="css_td text_right">
                0. 
            </div>
            <div class="css_td w48p">
                ＯＳのインストールを行いません。<br />
            </div>
            <div class="css_td w48p">
                <span class="">
                    <input type="checkbox" id="cb_SW_Preview" onclick="SW_Preview()" />ＯＳをインストールしない場合は、このチェックボックスをチェックして次へ進んでください。</span><br />
            </div>
        </div>
        <div class="css_tr">
            <div class="css_td">&nbsp;</div>
        </div>
        <div class="css_tr">
            <div class="css_td text_right">
                1. 
            </div>
            <div class="css_td w48p">
                注文したＯＳをインストールします。<br />
            </div>
            <div class="css_td w48p">
                <input type="radio" name="rb_SW_OS_Installation" value="0" checked="checked" />特に変更はありません。<br />
                <input type="radio" name="rb_SW_OS_Installation" value="1" />別途支給のＯＳをインストールします。<br />
                指定のインストールイメンージがある場合は以下に詳細を記入してください）
                <input class="margin_left20 w90p bg_blue" type="text" id="txt_SW_OS_Installation" />
            </div>
        </div>
        <div class="css_tr">
            <div class="css_td">&nbsp;</div>
        </div>
        <div class="css_tr">
            <div class="css_td text_right">
                2. 
            </div>
            <div class="css_td w48p">
                ユーザー名の設定について<br />
                Windows Embeddedの場合：機種名（ARK, UNOなど）を使用します<br />
                Windows Embedded以外： MyUser
            </div>
            <div class="css_td w48p">
                <input type="radio" name="rb_SW_Username" value="0" checked="checked" />特に変更はありません。<br />
                <input type="radio" name="rb_SW_Username" value="1" />以下のように設定してください。
                <input class="margin_left20 w90p bg_blue" type="text" id="txt_SW_Username" />
            </div>
        </div>
        <div class="css_tr">
            <div class="css_td">&nbsp;</div>
        </div>
        <div class="css_tr">
            <div class="css_td text_right">
                3. 
            </div>
            <div class="css_td w48p">
                OSのタイムゾーンの設定について<br />
                タイムゾーンを日本に設定します。<br />
                日付、時刻を日本で設定します。
            </div>
            <div class="css_td w48p">
                <input type="radio" name="rb_SW_OS_Timezone" value="0" checked="checked" />特に変更はありません。<br />
                <input type="radio" name="rb_SW_OS_Timezone" value="1" />以下のように設定してください。
                <input class="margin_left20 w90p bg_blue" type="text" id="txt_SW_OS_Timezone" />
            </div>
        </div>
        <div class="css_tr">
            <div class="css_td">&nbsp;</div>
        </div>
        <div class="css_tr">
            <div class="css_td text_right">
                4. 
            </div>
            <div class="css_td w48p">
                キーボード配列の設定について<br />
                インストールするＯＳが日本語の場合には日本語配列にします。<br />
            </div>
            <div class="css_td w48p">
                <input type="radio" name="rb_SW_OS_Input" value="0" checked="checked" />特に変更はありません。<br />
                <input type="radio" name="rb_SW_OS_Input" value="1" />以下のように設定してください。
                <input class="margin_left20 w90p bg_blue" type="text" id="txt_SW_OS_Input" />
            </div>
        </div>
        <div class="css_tr">
            <div class="css_td">&nbsp;</div>
        </div>
        <div class="css_tr">
            <div class="css_td text_right">
                5. 
            </div>
            <div class="css_td w48p">
                ＩＰアドレスの設定について<br />
                ＤＨＣＰを設定します。<br />
            </div>
            <div class="css_td w48p">
                <input type="radio" name="rb_SW_IP_Settings" value="0" checked="checked" />特に変更はありません。<br />
                <input type="radio" name="rb_SW_IP_Settings" value="1" />以下のように設定してください。
                <input class="margin_left20 w90p bg_blue" type="text" id="txt_SW_IP_Settings" />
            </div>
        </div>
        <div class="css_tr">
            <div class="css_td">&nbsp;</div>
        </div>
        <div class="css_tr">
            <div class="css_td text_right">
                6. 
            </div>
            <div class="css_td w48p">
                その他の設定について<br />
                Ａｄｍｉｎｉｓｔｒａｔｏｒを有効化します。<br />
                作成したユーザを削除します。<br />
                再起動後、ユーザーフォルダを削除します。<br />
                Windowsログを削除します。<br />
                WindowsOSの場合、Ｓｙｓｐｒｅｐを実行します。ただし、Windows8/8.1/10の場合には実行しません。<br />
            </div>
            <div class="css_td w48p">
                <input type="radio" name="rb_SW_Settings" value="0" checked="checked" />特に変更はありません。<br />
                <input type="radio" name="rb_SW_Settings" value="1" />以下のように設定してください。
                <textarea rows="5" cols="" class="margin_left20 w90p bg_blue" id="txt_SW_Settings"></textarea>
            </div>
        </div>
        <div class="css_tr">
            <div class="css_td">&nbsp;</div>
        </div>
        <div class="css_tr">
            <div class="css_td text_right">
                7. 
            </div>
            <div class="css_td w48p">
                その他<br />
                何か他に連絡事項がある場合には、ここに記入してください。<br />
            </div>
            <div class="css_td w48p">
                <input type="radio" checked="checked" name="rb_SW_Others" />以下は追加の要望事項です。<br />
                <textarea rows="5" cols="" class="margin_left20 w90p bg_blue" id="txt_SW_Others"></textarea>
            </div>
        </div>
    </div>
    <br />
    <p class="text_center font_size_larger font_bold mrgin_top10">以上ですべての設定を完了しました。</p>
    <p class="text_center font_size_larger font_bold mrgin_top10">設定内容は次のページでご確認ください。</p>
    <p class="text_center font_size_larger font_bold mrgin_top10">２／３</p>
</div>
