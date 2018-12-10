function showDialogById(th, dialogTitle, dialogWidth, dialogHeight) {
    dialogWidth = dialogWidth || 200;
    if (dialogWidth == undefined)
        dialogWidth = 465;
    dialogHeight = dialogHeight || 'auto';
    if (dialogHeight == undefined)
        dialogHeight = 'auto';
    $("#" + th).dialog({ width: dialogWidth, height: dialogHeight, modal: true, title: dialogTitle });
}

function selectAllBox(th) {
    $(".checkAllBox :input[type='checkbox']").each(function (i) {
        this.checked = th.checked;
    });
}

function checkDeleteFile() {
    if ($(".checkAllBox :input[type='checkbox'][checked='true']").length == 0) {
        alert("Please select delete file.");
        return false;
    }
    return true;
}

function showContainer(th, content) {
    if (th.checked)
        $("#" + content).show();
    else
        $("#" + content).hide();
}

function checkChineEnglishStrLength2(content, maxNumber) {
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
function checkHtmlInputFu2(content, obj, messageId, maxTextNumber) {//文本框内容,文本框对象,提示信息Id,最大字符
    //var maxTextNumber = 500; //最大字符
    var featureTextLength = content.replace(/[^\x00-\xff]/g, "**").length;
    if (featureTextLength > maxTextNumber && obj != null) {
        obj.value = checkChineEnglishStrLength(content, maxTextNumber); //母板页导入js
        featureTextLength = maxTextNumber;
    }
    $("#" + messageId).html(featureTextLength + " / ");
}