<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="AJPTerm1.ascx.vb" Inherits="EDOC.Software" %>

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

    .highlight {
        background-color: yellow;
    }
</style>

<script src="../Js/jquery-ui.js" type="text/javascript"></script>
<script src="../Js/json2.js" type="text/javascript"></script>

<script>
    $(document).ready(function () {
        Term1SetValuetoForm();
    });


    function ProcessingAJPTerm1() {
        var HW_COMUSB = "";
        if ($("input:radio[name='rb_HW_COMUSB']:checked").val() == 1)
            HW_COMUSB = $('#txt_HW_COMUSB').val();

        var HW_Storage = "";
        if ($("input:radio[name='rb_HW_Storage']:checked").val() == 1)
            HW_Storage = $('#txt_HW_Storage').val();

        var HW_SATA = "";
        if ($("input:radio[name='rb_HW_SATA']:checked").val() == 1)
            HW_SATA = $('#txt_HW_SATA').val();

        var HW_Disk_Division = "";
        if ($("input:radio[name='rb_HW_Disk_Division']:checked").val() == 1)
            HW_Disk_Division = $('#txt_HW_Disk_Division').val();

        var HW_Cable = "";
        if ($("input:radio[name='rb_HW_Cable']:checked").val() == 1)
            HW_Cable = $('#txt_HW_Cable').val();

        var HW_BIOS = "";
        if ($("input:radio[name='rb_HW_BIOS']:checked").val() == 1)
            HW_BIOS = $('#txt_HW_BIOS').val();

        var HW_OS_License = "";
        if ($("input:radio[name='rb_HW_OS_License']:checked").val() == 1)
            HW_OS_License = $('#txt_HW_OS_License').val();

        var HW_OS_Activation = "";
        if ($("input:radio[name='rb_HW_OS_Activation']:checked").val() == 1)
            HW_OS_Activation = $('#txt_HW_OS_Activation').val();

        var HW_Others = $('#txt_HW_Others').val();

        var postData = {
            QuoteID: "<%=Request("UID")%>",
            HW_COMUSB: HW_COMUSB,
            HW_Storage: HW_Storage,
            HW_SATA: HW_SATA,
            HW_Disk_Division: HW_Disk_Division,
            HW_Cable: HW_Cable,
            HW_BIOS: HW_BIOS,
            HW_OS_License: HW_OS_License,
            HW_OS_Activation: HW_OS_Activation,
            HW_Others: HW_Others,
            IFSSOP: $('#<%=ddlIFS.ClientID %> OPTION:selected').val()
        };
        $.ajax({
            url: "<%= Util.GetRuntimeSiteUrl()%>/Services/AJPTerms.asmx/AJPTerm1",
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

    function GetAJPTerm1Preview() {
        var result = "";

        if ($('#txt_HW_COMUSB').val().length > 0)
            result += "ＣＯＭ／ＵＳＢブラケットの設置位置: " + $('#txt_HW_COMUSB').val() + "\n";

        if ($('#txt_HW_Storage').val().length > 0)
            result += "ストレージの取り付け順序: " + $('#txt_HW_Storage').val() + "\n";

        if ($('#txt_HW_SATA').val().length > 0)
            result += "ＳＡＴＡケーブルの接続順序について: " + $('#txt_HW_SATA').val() + "\n";

        if ($('#txt_HW_Disk_Division').val().length > 0)
            result += "パーティションの指示: " + $('#txt_HW_Disk_Division').val() + "\n";

        if ($('#txt_HW_Cable').val().length > 0)
            result += "ケーブル整理・固定: " + $('#txt_HW_Cable').val() + "\n";

        if ($('#txt_HW_BIOS').val().length > 0)
            result += "ＢＩＯＳ設定: " + $('#txt_HW_BIOS').val() + "\n";

        if ($('#txt_HW_OS_License').val().length > 0)
            result += "ＯＳライセンスシールは本体シリアル番号の近くに貼付する: " + $('#txt_HW_OS_License').val() + "\n";

        if ($('#txt_HW_OS_Activation').val().length > 0)
            result += "出荷時にＯＳのアクティベーションは行いません: " + $('#txt_HW_OS_Activation').val() + "\n";

        if ($('#txt_HW_Others').val().length > 0)
            result += "その他: " + $('#txt_HW_Others').val() + "\n";

        return result;
    }

    function HW_Preview0() {
        // This function means AJP Term1 all fields are setted to default and no things need to change.

        if ($('input#cb_HW_Preview0').is(':checked')) {
            $("#cb_HW_Preview1").removeAttr("checked");

            $("input[name='rb_HW_COMUSB'][value='0']").prop("checked", true);
            $("input[name='rb_HW_COMUSB']").attr("disabled", true);
            $('#txt_HW_COMUSB').attr("disabled", true);

            $("input[name='rb_HW_Storage'][value='0']").prop("checked", true);
            $("input[name='rb_HW_Storage']").attr("disabled", true);
            $('#txt_HW_Storage').attr("disabled", true);

            $("input[name='rb_HW_SATA'][value='0']").prop("checked", true);
            $("input[name='rb_HW_SATA']").attr("disabled", true);
            $('#txt_HW_SATA').attr("disabled", true);

            $("input[name='rb_HW_Disk_Division'][value='0']").prop("checked", true);
            $("input[name='rb_HW_Disk_Division']").attr("disabled", true);
            $('#txt_HW_Disk_Division').attr("disabled", true);

            $("input[name='rb_HW_Cable'][value='0']").prop("checked", true);
            $("input[name='rb_HW_Cable']").attr("disabled", true);
            $('#txt_HW_Cable').attr("disabled", true);

            $("input[name='rb_HW_BIOS'][value='0']").prop("checked", true);
            $("input[name='rb_HW_BIOS']").attr("disabled", true);
            $('#txt_HW_BIOS').attr("disabled", true);

            $("input[name='rb_HW_OS_License'][value='0']").prop("checked", true);
            $("input[name='rb_HW_OS_License']").attr("disabled", true);
            $('#txt_HW_OS_License').attr("disabled", true);

            $("input[name='rb_HW_OS_Activation'][value='0']").prop("checked", true);
            $("input[name='rb_HW_OS_Activation']").attr("disabled", true);
            $('#txt_HW_OS_Activation').attr("disabled", true);

            $("input[name='rb_HW_Others']").attr("disabled", true);
            $('#txt_HW_Others').attr("disabled", true);
        }
        else {
            HW_Preview1();
        }
    }

    function HW_Preview1() {
        // This function means AJP Term1 has something to be changed.

        $("#cb_HW_Preview0").removeAttr("checked");

        $("input[name='rb_HW_COMUSB']").attr("disabled", false);
        $("input[name='rb_HW_Storage']").attr("disabled", false);
        $("input[name='rb_HW_SATA']").attr("disabled", false);
        $("input[name='rb_HW_Disk_Division']").attr("disabled", false);
        $("input[name='rb_HW_Cable']").attr("disabled", false);
        $("input[name='rb_HW_BIOS']").attr("disabled", false);
        $("input[name='rb_HW_OS_License']").attr("disabled", false);
        $("input[name='rb_HW_OS_Activation']").attr("disabled", false);
        $("input[name='rb_HW_Others']").attr("disabled", false);

        $('#txt_HW_COMUSB').attr("disabled", false);
        $('#txt_HW_Storage').attr("disabled", false);
        $('#txt_HW_SATA').attr("disabled", false);
        $('#txt_HW_Disk_Division').attr("disabled", false);
        $('#txt_HW_Cable').attr("disabled", false);
        $('#txt_HW_BIOS').attr("disabled", false);
        $('#txt_HW_OS_License').attr("disabled", false);
        $('#txt_HW_OS_Activation').attr("disabled", false);
        $('#txt_HW_Others').attr("disabled", false);
    }

    function Term1SetValuetoForm() {
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

                    if (retData.HW_COMUSB.length > 0) {
                        $('#txt_HW_COMUSB').val(retData.HW_COMUSB);
                        $("input[name='rb_HW_COMUSB'][value='1']").prop("checked", true);
                    }

                    if (retData.HW_Storage.length > 0) {
                        $('#txt_HW_Storage').val(retData.HW_Storage);
                        $("input[name='rb_HW_Storage'][value='1']").prop("checked", true);
                    }

                    if (retData.HW_SATA.length > 0) {
                        $('#txt_HW_SATA').val(retData.HW_SATA);
                        $("input[name='rb_HW_SATA'][value='1']").prop("checked", true);
                    }

                    if (retData.HW_Disk_Division.length > 0) {
                        $('#txt_HW_Disk_Division').val(retData.HW_Disk_Division);
                        $("input[name='rb_HW_Disk_Division'][value='1']").prop("checked", true);
                    }

                    if (retData.HW_Cable.length > 0) {
                        $('#txt_HW_Cable').val(retData.HW_Cable);
                        $("input[name='rb_HW_Cable'][value='1']").prop("checked", true);
                    }

                    if (retData.HW_BIOS.length > 0) {
                        $('#txt_HW_BIOS').val(retData.HW_BIOS);
                        $("input[name='rb_HW_BIOS'][value='1']").prop("checked", true);
                    }

                    if (retData.HW_OS_License.length > 0) {
                        $('#txt_HW_OS_License').val(retData.HW_OS_License);
                        $("input[name='rb_HW_OS_License'][value='1']").prop("checked", true);
                    }

                    if (retData.HW_OS_Activation.length > 0) {
                        $('#txt_HW_OS_Activation').val(retData.HW_OS_Activation);
                        $("input[name='rb_HW_OS_Activation'][value='1']").prop("checked", true);
                    }

                    if (retData.HW_Others.length > 0)
                        $('#txt_HW_Others').val(retData.HW_Others);
                }
            },
            error: function (msg) {
                console.log("err:" + msg.d);
            }
        });

    }
</script>

<div class="font_size_small">
    <p class="text_decoration w99p text_center font_size_large font_bold">My Advantech Work order entry image</p>
    <div id="divIFSSelection" class="css_tr">
        <div id="divinnerIFSSelection" style="margin-left: 25px">
            <b style="color: red">SOP:&nbsp;</b>
            <asp:DropDownList ID="ddlIFS" runat="server"></asp:DropDownList>
        </div>
        <div class="css_tr">
            <div class="css_td">&nbsp;</div>
        </div>
    </div>
    システムの組み立てに際して特に指定が無い場合には、以下を標準として組み立てを行います。<br />
    変更が必要無い場合には<br />
    <span class="margin_left30 font_bold" id="span_HW_1" style="background-color: yellow">
        <input type="checkbox" id="cb_HW_Preview0" onclick="HW_Preview0()" />特に変更はありません</span><br />
    にチェックしてご注文内容を確認してください。<br />
    <br />
    もし、ご希望の設定がある場合には、<br />
    <span class="margin_left30 font_bold" id="span_HW_2" style="background-color: yellow">
        <input type="checkbox" id="cb_HW_Preview1" onclick="HW_Preview1()" />以下のように設定してください。</span><br />
    にチェックして、設定内容詳細を記入してください。<br />
    <span class="text_decoration margin_left40 font_bold">システム設定項目</span><br />
    <br />
    <div class="css_table">
        <div class="css_tr">
            <div class="css_td text_right v_align">
                1. 
            </div>
            <div class="css_td w48p">
                <div class="css_table">
                    <div class="css_tr">
                        <div class="css_td w80p v_align">
                            <p>ＣＯＭ／ＵＳＢブラケットの設置位置</p>
                            <p>シャーシのスロット側から見たとき、</p>
                            <p>ＣＯＭ実装位置はＣＰＵボード左側、また、</p>
                            <p>
                                ＵＳＢ実装位置はＣＰＵ右側に組み立てます。
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            </p>
                        </div>
                        <div class="css_td">
                            <img src="../Images/AJPConfirmTermJPG1.jpg" style="width: 120px; height: 90px;" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="css_td w48p v_align">
                <input type="radio" name="rb_HW_COMUSB" value="0" checked="checked" />特に変更はありません。<br />
                <input type="radio" name="rb_HW_COMUSB" value="1" />以下のように設定してください。
                <input class="margin_left20 w90p bg_blue" type="text" id="txt_HW_COMUSB" />
            </div>
        </div>
        <div class="css_tr">
            <div class="css_td">&nbsp;</div>
        </div>
        <div class="css_tr">
            <div class="css_td text_right v_align">
                2. 
            </div>
            <div class="css_td w48p">
                <div class="css_table">
                    <div class="css_tr">
                        <div class="css_td w80p v_align">
                            <p>ストレージの取り付け順序</p>
                            <p>ＯＤＤはシャーシの一番上の５インチベイに設置します。</p>
                        </div>
                        <div class="css_td">
                            <img src="../Images/AJPConfirmTermJPG2.jpg" style="width: 120px; height: 90px;" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="css_td w48p v_align">
                <input type="radio" name="rb_HW_Storage" value="0" checked="checked" />特に変更はありません。<br />
                <input type="radio" name="rb_HW_Storage" value="1" />以下のように設定してください。
                <input class="margin_left20 w90p bg_blue" type="text" id="txt_HW_Storage" />
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
                ＳＡＴＡケーブルの接続順序について<br />
                組み立てるストレージの数に応じて、ＨＤＤをＳＡＴＡ１・・・の順番に、すべてのＨＤＤを接続します。<br />
                ＯＤＤは最後に接続します。<br />
            </div>
            <div class="css_td w48p">
                <input type="radio" name="rb_HW_SATA" value="0" checked="checked" />特に変更はありません。<br />
                <input type="radio" name="rb_HW_SATA" value="1" />以下のように設定してください。
                <input class="margin_left20 w90p bg_blue" type="text" id="txt_HW_SATA" />
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
                パーティションの指示<br />
                ストレージ全体をＣドライブとして設定します。<br />
                複数個のＨＤＤのご注文の場合、１台当たりの最大容量でパーティションを作成し、Ｄドライブ・・・と設定します。<br />
                ＲＡＩＤ設定、またパーティションサイズを変更する場合は必ず指示をご連絡ください。<br />
            </div>
            <div class="css_td w48p">
                <input type="radio" name="rb_HW_Disk_Division" value="0" checked="checked" />特に変更はありません。<br />
                <input type="radio" name="rb_HW_Disk_Division" value="1" />以下のように設定してください。
                <input class="margin_left20 w90p bg_blue" type="text" id="txt_HW_Disk_Division" />
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
                ケーブル整理・固定<br />
                ＦＡＮに巻き込まないようにケーブルを整理する。<br />
                特に指定が無い場合には、ケーブルタイを使用した整理、また固定等は行いません。<br />
            </div>
            <div class="css_td w48p">
                <input type="radio" name="rb_HW_Cable" value="0" checked="checked" />特に変更はありません。<br />
                <input type="radio" name="rb_HW_Cable" value="1" />以下のように設定してください。
                <input class="margin_left20 w90p bg_blue" type="text" id="txt_HW_Cable" />
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
                ＢＩＯＳ設定<br />
                ＢＩＯＳのデフォルト設定を適用します。<br />
                また、設定日時は日本時間を標準として設定します。<br />
            </div>
            <div class="css_td w48p">
                <input type="radio" name="rb_HW_BIOS" value="0" checked="checked" />特に変更はありません。<br />
                <input type="radio" name="rb_HW_BIOS" value="1" />以下のように設定してください。
                <input class="margin_left20 w90p bg_blue" type="text" id="txt_HW_BIOS" />
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
                ＯＳライセンスシールは本体シリアル番号の近くに貼付する。<br />
            </div>
            <div class="css_td w48p">
                <input type="radio" name="rb_HW_OS_License" value="0" checked="checked" />特に変更はありません。<br />
                <input type="radio" name="rb_HW_OS_License" value="1" />以下のように設定してください。
                <input class="margin_left20 w90p bg_blue" type="text" id="txt_HW_OS_License" />
            </div>
        </div>
        <div class="css_tr">
            <div class="css_td">&nbsp;</div>
        </div>
        <div class="css_tr">
            <div class="css_td text_right">
                8. 
            </div>
            <div class="css_td w48p">
                出荷時にＯＳのアクティベーションは行いません。<br />
            </div>
            <div class="css_td w48p">
                <input type="radio" name="rb_HW_OS_Activation" value="0" checked="checked" />特に変更はありません。<br />
                <input type="radio" name="rb_HW_OS_Activation" value="1" />以下のように設定してください。
                <input class="margin_left20 w90p bg_blue" type="text" id="txt_HW_OS_Activation" />
            </div>
        </div>
        <div class="css_tr">
            <div class="css_td">&nbsp;</div>
        </div>
        <div class="css_tr">
            <div class="css_td text_right">
                9. 
            </div>
            <div class="css_td w48p">
                その他<br />
                何か他に連絡事項がある場合には、ここに記入してください。<br />
                ⇒ 引き続きソフトウェアのインストールについて確認を行います。<br />
            </div>
            <div class="css_td w48p">
                <input type="radio" name="rb_HW_Others" checked="checked" />以下のように設定してください。<br />
                <textarea rows="5" cols="" class="margin_left20 w90p bg_blue" id="txt_HW_Others"></textarea>
            </div>
        </div>
        <div class="css_tr">
            <div class="css_td">&nbsp;</div>
        </div>
    </div>
    <br />
    <p class="text_center font_size_larger font_bold mrgin_top10">１／３</p>
</div>
