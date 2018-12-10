function ShowMasterErr(title, errStr, loadingSeconds) {
    var _title = "Alert Messages";
    if ($.trim(title) != "") { _title = title; }

    $("#divMasterAlertWindow").dialog(
		{
		    title: _title,
		    modal: true, width: '50%',
		    open: function (type, data) {
		        $("#divMasterAlertWindow").find(".errMsg").text(errStr);
		        ////扩展自动关闭功能
		        if ($.isNumeric(loadingSeconds) && loadingSeconds > 0) {
		            setTimeout(function () { $("#divMasterAlertWindow").dialog("close") }, loadingSeconds * 1000);
		        }
		        ///扩展结束
		    },
		    close: function (type, data) { $("#divMasterAlertWindow").find(".errMsg").empty(); }
		}
	   );
}

function ShowFreight() {
    $("#divFreightWindow").dialog(
       {
           title: " Please choose a freight",
           modal: false, width: '430',
           draggable: false,
           open: function (type, data) {
               $("#divFreightWindow").parent().appendTo("#dialog_target");
           },
           close: function (type, data) { }
       }
      );
}
function CloseFreight() {
    $("#divFreightWindow").dialog("close");
}

function ShowMsg(id, msg) {
    $("#" + id).html(msg);
}
//function autoClose(title) {
//    --t;
//    if (t > 0) {
//        document.getElementById("ui-id-1").innerHTML = title + ".&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; After " + t + " seconds will be shut down."
//        setTimeout(function () {
//            autoClose(title);
//        }, 1000);
//    }
//    else
//     {
//           $("#divMasterAlertWindow").dialog("close");
//     }
//}
function onFocusFun(element, elementValue) {
    if (element.value == elementValue) {
        element.value = "";
        element.style.color = "";
    }
}
function onblurFun(element, elementValue) {
    if (element.value == '') {
        element.style.color = "#808080";
        element.value = elementValue;
    }
}
////////////////////////////////////////////
(function ($) {
    $.getUrlParam = function (name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return unescape(r[2]); return null;
    }
})(jQuery);
///////////////////////////////////
jQuery.fn.center = function () {
    this.css("position", "absolute");
    this.css("top", Math.max(0, (($(window).height() - this.outerHeight()) / 2) +
                                                  $(window).scrollTop()) + "px");
    this.css("left", Math.max(0, (($(window).width() - this.outerWidth()) / 2) +
                                                  $(window).scrollLeft()) + "px");
    return this;
}
///////////////////////////////////////
function showloadingV2(alertstr, loadingSeconds) {
    $("#loading").text(alertstr);
    $("#loading").center();
    $("#loading").show();
    if ($.isNumeric(loadingSeconds) && loadingSeconds > 0) {
        setTimeout(function () { hideloading(); }, loadingSeconds * 1000);
    }
}
function hideloading() {
    $("#loading").empty();
    $("#loading").hide();
}
/////////////////////////////////////
function StringBuilder() {
    this._strings = new Array();
}
StringBuilder.prototype.append = function (str) {
    this._strings.push(str);
}
StringBuilder.prototype.toString = function () {
    return this._strings.join('');
}