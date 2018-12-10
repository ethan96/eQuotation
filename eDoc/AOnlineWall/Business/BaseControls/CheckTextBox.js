$(document).ready(function () {
    $("textarea[typeX=textAearBoxCSS]").each(function () {
        checkHtmlInputFu($(this));
        $(this).keyup(function () {
            checkHtmlInputFu($(this));
        });
    });
});
function checkHtmlInputFu(content) {//文本框内容,文本框对象,提示信息Id,最大字符
    //var maxTextNumber = 500; //最大字符
    var maxTextNumber = parseInt(content.attr("MaxLengthX"));
    var featureTextLength = content.val().replace(/[^\x00-\xff]/g, "**").length;
    if (featureTextLength > maxTextNumber) {
        content.val(checkChineEnglishStrLength(content.val(), maxTextNumber)); //母板页导入js
        featureTextLength = maxTextNumber;
    }
    $("#" + content.attr("id") + "_Message").html(featureTextLength + " / " + maxTextNumber);
}
//输入内容和长度  返回固定字节
function checkChineEnglishStrLength(content, maxNumber) {
    var txtLen = 0, txtIndex = 0;
    for (var i = 0; i < content.length; i++) {
        var word = content.charAt(i); //修复特殊字符
        if (/[^\x00-\xff]/g.test(word))
            txtLen += 2;
        else
            txtLen++;

        if (txtLen >= maxNumber) {
            if (/[^\x00-\xff]/g.test(word))
                return content.substring(0, txtIndex);
            return content.substring(0, txtIndex + 1);
        }
        txtIndex++;
    }
    return content;
}