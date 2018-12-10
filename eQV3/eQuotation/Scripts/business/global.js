
(function ($) {
    $.fn.onEnter = function (func) {
        this.bind('keypress', function (e) {
            if (e.keyCode == 13) {
                func.apply(this, [e]);
                return false;
            }
        });
        return this;
    };
})(jQuery);

$(document).ready(function () {

    //set global behaviour when submit buttons are clicked
    $("input[type=submit]").on("click", function () { $.blockUI(); });
    $("button[type=submit]").on("click", function () { $.blockUI(); });

    //handle close box-error
    $("#close-box-error").on("click", function () {
        $("div.box-error").hide("slow");
    });

    function getError(req, status, err) {
        alert(err + " :: The connection was fail. Please try again.");
        $("#wrapper-content").html("");
        $.unblockUI();
    }

    $.pushHTML = function (data, fnShow) {

        var containerId = "B8C08DE1-B263-4019-AC92-AA286B22281D";

        //identify the data content
        //error data will be identified by GUI: B8C08DE1-B263-4019-AC92-AA286B22281D
        if (data.toString().indexOf(containerId) == -1)
        {
            //it is NOT an error handling output
           if (typeof fnShow == "function")
                fnShow.call(this);
        }
        else
        {
            //it is error handling output
            $("div.data-error").html(data);
            $("div.box-error").show("slow");
            setTimeout("$('div.box-error').hide('slow')", 5000); //Hidden after 5 seconds 
            //play beep sound
            if (window.HTMLAudioElement)
            {
                var oAudio = document.getElementById("beep-error");
                oAudio.play();
            }
        }
    }


});

//// DO on hash change
//$(window).on('hashchange', function () {
//    checkURL();
//});

function checkURL(url, opt) {

    container = $('#wrapper-content');
    // Do this if url exists (for page refresh, etc...)
    //var url = getURL();

    //make sure that it has valid url
    if (!url.split("?")[0]) return;

    if (url) {
        // parse url to jquery
        loadURL(url, container, opt);
    } 

}

function getURL() {
    //get the url by removing the hash
    //var url = location.hash.replace(/^#/, '');
    var url = location.href.split('#').splice(1).join('#');

    //BEGIN: IE11 Work Around
    if (!url) {

        try {
            var documentUrl = window.document.URL;
            if (documentUrl) {
                if (documentUrl.indexOf('#', 0) > 0 && documentUrl.indexOf('#', 0) < (documentUrl.length + 1)) {
                    url = documentUrl.substring(documentUrl.indexOf('#', 0) + 1);

                }

            }

        } catch (err) { }
    }
    //END: IE11 Work Around

    return url;
}

function getQuery(key)
{
    var hashUrl = getURL();
    var query = null;

    if (hashUrl.indexOf("?", 0) >= 0)
    {
        var allqs = hashUrl.split("?");
        $.each(allqs[1].split("&"), function (idx, value) {
            if (value.indexOf("=", 0) >= 0)
            {
                var qs = value.split("=");
                if (qs[0] == key) { query = qs[1]; return; }
            }
        });
    }
    
    return query;
}

function setQuery(key, val)
{
    var hashUrl = getURL();
    var replaced = false;
    var url = null;

    if (hashUrl.indexOf("?", 0) >= 0)
    {
        var allqs = hashUrl.split("?"); url = allqs[0];

        $.each(allqs[1].split("&"), function (idx, value)
        {
            if (value.indexOf("=", 0) >= 0)
            {
                var qs = value.split("=");
                if (qs[0] == key) {
                    url = (url.indexOf("?") >= 0) ? url + "&" + key + "=" + val : url + "?" + key + "=" + val;
                    replaced = true;
                }
                else {
                    url = (url.indexOf("?") >= 0) ? url + "&" + value : url + "?" + value;
                }
            }
        });
    }

    if (!replaced)
        url = (url.indexOf("?") >= 0) ? url + "&" + key + "=" + val : url + "?" + key + "=" + val;
    
    if (url) window.location.hash = url;
}

/*
 * LOAD AJAX PAGES
 */
function loadURL(url, container, opt, newUrl) {
    //console.log(container)
    
    $.ajax({
        type: "GET",
        url: url,
        dataType: 'html',
        cache: true, // (warning: setting it to false will cause a timestamp and will call the request twice)
        beforeSend: function () {

            // empty container and var to start garbage collection (frees memory)
            pagefunction = null;

            ////clean SCAN FUNCTION CONTAINER
            //$.IFS_SCANNER = {};

            //place cog
            $("#LOADING_STATUS").html("<i class='fa fa-cog fa-spin fa-2x'></i>");

            //block UI
            $.blockUI();

        },
        success: function (data) {

            // dump data to container
            $.pushHTML(data, function () {
                container.removeData().html("");
                container.css({
                    opacity: '0.0'
                }).html(data).delay(50).animate({
                    opacity: '1.0'
                }, 300);

                //change hash URL
                if (opt) {
                    if (newUrl)
                        window.location.hash = newUrl;
                    else
                        window.location.hash = url;
                }
            });

            //unblock UI
            $.unblockUI();

            //clear cog
            $("#LOADING_STATUS").html("");

            // clear data var
            data = null;
            container = null;
        },
        error: function (xhr, ajaxOptions, thrownError) {

            var strHTML = "";
            strHTML = strHTML + "<div class='main-content'>";
            strHTML = strHTML + "<div class='topbox titlebox'><h4>Page Error!!!</h4></div>";
            strHTML = strHTML + "<div class='content'>";
            strHTML = strHTML + "<div style='padding:10px'><h4><i class='fa fa-warning'></i> Error 404! Page not found.</h4></div>";
            strHTML = strHTML + "</div>";
            strHTML = strHTML + "</div>";

            container.html(strHTML);

            //clear cog
            $("#LOADING_STATUS").html("");

            //unblock UI
            $.unblockUI();
        },
        async: true
    });

}

function updateURL(url)
{
    var strURL = null;

    //get current TAB & MODE value from query string
    var tab = getQuery("tab");
    var mode = getQuery("mode");

    //set default value in case of NULL
    if (!tab) tab = 0;
    if (!mode) mode = 1;

    //update value of TAB key
    if (url.indexOf("?", 0) >= 0)
        strURL = url + "&tab=" + tab;
    else
        strURL = url + "?tab=" + tab;

    //update value of MODE key
    strURL = strURL + "&mode=" + mode;
    //window.location.hash = strURL;

    return strURL;
}

//update main content and change URL
function initContent(url)
{
    checkURL(updateURL(url), true);
}

//update main content without changing URL
function updateContent(url)
{
    loadURL(url, $("#wrapper-content"), false);
}

//update content from redirected URL
function redirectContent(url, redirectUrl)
{
    loadURL(url, $("#wrapper-content"), true, updateURL(redirectUrl));
}

//update specific area or section and block UI
function updateSection(url, contId)
{
    loadURL(url, $("#" + contId), false);
}

//update container in silent (without block UI)
function updateSectionAsync(url, contId)
{
    //validate serial number
    $.get(url, function (data) {
        $.pushHTML(data, function () {
            $("#" + contId).html(data);
        });
    })
}

//delete content with and changing URL
function deleteContent(url, redirectUrl)
{
    if (confirm("Are you sure want to delete?"))
    {
        if (redirectUrl)
            loadURL(url, $("#wrapper-content"), true, updateURL(redirectUrl));
        else
            loadURL(url, $("#wrapper-content"), false);
    };
}

//delete modal content and subsequently execute functon fn
function deleteModal(url, fn)
{
    if (confirm("Are you sure want to delete?"))
    {
        $.blockUI();
        $.get(url, function (data) {
            $.pushHTML(data, function () {
                if (typeof fn == "function") fn.call(this, data);
            });
            $.unblockUI();
        });
    };
}

//do action, close modal and change URL
function redirectModal(url, newUrl) {
    $.blockUI();
    $.get(url, function (data) {
        $.pushHTML(data, function () {
            $.modal.close();
            $("#wrapper-content").html(data);
            window.location.hash = updateURL(newUrl);
        });
        $.unblockUI();
    });
}

//open modal window
function openModal(url, name)
{
    $.blockUI();
    $.get(url, function (data) {
        $.pushHTML(data, function () {
            $.OSXModalInit(name, data);
        });
        $.unblockUI();
    });
}

//display document on container
function displayDocument(url, contId)
{
    $.blockUI();
    $.get(url, function (data) {
        $.pushHTML(data, function () {
            var hg = parseInt($("#" + contId).css("height").replace("px", ""));
            var success = new PDFObject({ url: data.url, height: hg + "px" }).embed(contId);
        });
        $.unblockUI();
    })
}

//handle scanning event
function barcodeScanner(url, contId, func)
{
    //block the updated section
    $('#' + contId).block({
        message: '<div style="padding:10px"><h4>Processing...</h4></div>',
        css: { border: '1px solid #c00' }
    });
    
    //validate serial number
    $.get(url, function (data) {
        $.pushHTML(data, function () {
            $("#" + contId).html(data);
        });
        $('#' + contId).unblock();
        
        if (typeof func == "function") func.call(null);
    })
}

//adjust height of container navigation
function adjheight() {
    //adjust height of navigator space
    var hnav = $("#div-main-nav").css("height");
    var sh = parseInt(hnav.replace("px", ""));
    $("#div-main-nav-child").css("height", (sh - 10) + "px");

}