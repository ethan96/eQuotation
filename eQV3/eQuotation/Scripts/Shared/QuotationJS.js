/*
 * Author：JJ.Lin
 * Date: 2017/7/31
 * Description：
   共用到的Function會寫在這裡
 **/

//設定哪個teg按下enter後會觸發哪一個按鈕的click事件
function EnterClick(tegID, clickID) {
    $('#' + tegID).keypress(function (e) {
        code = (e.keyCode ? e.keyCode : e.which);
        if (code == 13) {
            $("#" + clickID).click();
        }
    });
}