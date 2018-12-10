
//*START GLOBAL VARIABLES*//

$.IFS_LIBRARY_CODE = {};
$.IFS_SCANNER = {};                 //function containing business logic
$.IFS_IDENTIFIER = "";              //variable to identify the state of current action
$.IFS_EMPL_PREFIX = "IS0";          //prefix identified to identify employee code
$.IFS_EMPL_LEN = 8;                 //length of employee code
$.IFS_EMPL_SINGLE = "XXE01";        //indicate timelog active  XXE01 = single; 
$.IFS_EMPL_BATCH = "XXE02";         //indicate timelog actiove XXE02 = batch;
$.IFS_DOC_IDENT = "XXD01";          //indicate opening document

//*START SCANNER CONFIGURATION*/
//SOURCE: http://a.kabachnik.info/jquery-scannerdetection-tutorial.html
$(document).scannerDetection({

    timeBeforeScanTest: 200, 
    startChar: [120, 16], 
    endChar: [0, 13], 
    avgTimeByChar: 40,
    ignoreIfFocusOn: 'input, textarea',
    onKeyDetect: function (event) { console.log(event.which); },
    onComplete: function (textcode) {
        
        textcode = textcode.toUpperCase();

        //check code in library
        var libraryCode = false; 
        $.each($.IFS_LIBRARY_CODE, function (key, value) {
            
            if (value.CODE.toUpperCase() == textcode.toUpperCase())
            {
                //timelog in batch
                if (value.CODE == "SCAN00R2" && $.IFS_IDENTIFIER == $.IFS_EMPL_SINGLE)
                    openModal("production/timegroup/" + $("#SCANNER_OUTPUT").text(), "TimelogGroup");

                //go to station
                if (value.DOMAIN == "Factory" && value.URL != "")
                {
                    $.blockUI();
                    $.get(value.URL, function (data) {
                        $.pushHTML(data, function () {
                            window.location.hash = value.URL;
                            $("#wrapper-content").html(data);
                        });
                        $.unblockUI();
                    })
                }

                //close error message modal
                if (value.CODE == "SCAN00R3") {
                    $("div.box-error").hide("slow");
                }

                //set textcode on global bar
                $("#SCANNER_OUTPUT").text(value.NAME);

                //set identifier
                $.IFS_IDENTIFIER = value.IDENTIFIER;

                libraryCode = true;
                return true;
            }
        });

        //return function if code is listed in library
        if (libraryCode) {
            libraryCode = false;
            return libraryCode;
        }

        //check if code is an EMPLOYEE-ID
        //initiate to log working time
        if (textcode.substring(0, $.IFS_EMPL_PREFIX.length) == $.IFS_EMPL_PREFIX && textcode.length == $.IFS_EMPL_LEN)
        {
            $.blockUI();
            $.IFS_IDENTIFIER = $.IFS_EMPL_SINGLE;
            $("#SCANNER_OUTPUT").text(textcode);
            $.unblockUI();
            return true;
        }

        //start to log working time if EMPL IDENTIFIER is active
        if ($.IFS_IDENTIFIER == $.IFS_EMPL_SINGLE)
        {
            $.IFS_TIMELOG(textcode);
            return true;
        }

        //ready to move an object forward
        if ($.IFS_IDENTIFIER == "XXN01" && $.IS_STATION()) {

            var XSTATID = $("#STATION_ID").val();
            if (XSTATID) {
                updateContent("production/forward/" + textcode + "?stat=" + XSTATID);
            }
            $.IFS_IDENTIFIER = "";
            $("#SCANNER_OUTPUT").text("");
            return true;
        }

        //ready to move an object backward
        if ($.IFS_IDENTIFIER == "XXP01" && $.IS_STATION()) {

            var XSTATID = $("#STATION_ID").val();
            if (XSTATID) {
                updateContent("production/backward/" + textcode + "?stat=" + XSTATID);
            }
            $.IFS_IDENTIFIER = "";
            $("#SCANNER_OUTPUT").text("");
            return true;
        }

        //go to local implemented function
        //scanningEventHandler("onScanning", textcode);
        $.IFS_SCANNER(textcode);

    }

});

//*END SCANNER CONFIGURATION*/

//*START TIMELOG FUNCTION*/
$.IFS_TIMELOG = function (textcode) {
    //alert($.IS_STATION());
    //get scanned employee id previously
    var emplid = $("#SCANNER_OUTPUT").text();

    if (emplid)
    {
        $.blockUI();
        var XSTATID = $("#STATION_ID").val();
        $.post("production/timelog/" + emplid + "?wo=" + textcode + "&stat=" + XSTATID, function (data) {
            $.pushHTML(data, function () {

                //reset global identifier
                $.IFS_IDENTIFIER = "";

                //show the status of logging
                $("#SCANNER_OUTPUT").text(emplid + ": " + data.status);

                //retrieve WO info if user is in STATION and user is first person who is check-in
                if (XSTATID && data.init) initContent("production/Station/" + XSTATID + "?arg=" + textcode);

            })
            $.unblockUI();
        })
    }
}
//*END TIMELOG FUNCTION*/

//*CHECK IF PAGE IS STATION
$.IS_STATION = function () {

    if ($("#STAT_MGR_GUID").val() == "D9C64D7D-0DE5-4247-97E0-59DDEAED3CD2")
        return true;
    else
        return false;
}

//*CHECK IF PAGE IS STATION*/