/*
 * SimpleModal OSX Style Modal Dialog
 * http://simplemodal.com
 *
 * Copyright (c) 2013 Eric Martin - http://ericmmartin.com
 *
 * Licensed under the MIT license:
 *   http://www.opensource.org/licenses/mit-license.php
 */

$.OSXModalInit = function (id, data,callback) {
    var OSX = {
        container: null,
        init: function () {
            //e.preventDefault();
            //calculate the modal position relative to window
            var winX = $(window).height(); //height of window
            var contX = $("div#" + id + " div.osx-modal-data").innerHeight() + $("div#" + id + " div.osx-modal-title").innerHeight();
            var posX = (winX / 2) - (contX / 2);

            //get the size of data container
            var osxWidth = $("div#" + id + " div.osx-modal-data").innerWidth();

            //remove class "osx-init-container" to show modal
            //$("#" + id).removeClass("osx-init-container");

            if (!$.isNumeric(posX))
                posX = 0;

            $("#" + id).modal({
                overlayId: 'osx-overlay',
                containerId: 'osx-container',
                closeHTML: null,
                minHeight: 80,
                minWidth: osxWidth,
                opacity: 65,
                zIndex: 99999,
                position: [posX + 'px',],
                overlayClose: false,
                onOpen: OSX.open,
                onClose: OSX.close
            });

        },
        open: function (d) {
            var self = this;
            self.container = d.container[0];
            d.overlay.fadeIn('fast', function () {
                $("#" + id, self.container).show();
                var title = $("#" + id + " div.osx-modal-title", self.container);
                title.show();
                d.container.slideDown('fast', function () {
                    var h = $("#" + id + " .osx-modal-data", self.container).height()
						+ title.height()
						+ 20; // padding
                    d.container.animate(
						{ height: h },
						200,
						function () {
						    $("#" + id + " div.close", self.container).show();
						    $("#" + id + " div.osx-modal-data", self.container).show();
						}
					);
                });
            })
            if (data != null) $("#" + id + " div.osx-modal-data").html(data);
            //$("#" + id + " div.osx-modal-data").css("display:block");
        },
        close: function (d) {
            var self = this; // this = SimpleModal object
            d.container.hide("slow", function () {
                $.modal.close();
            })
          
            if (callback && typeof (callback) === "function") {
                callback();
            }
            //d.container.animate(
			//	{ top: "-" + (d.container.height() + 20) },
			//	500,
			//	function () {
			//	    self.close(); // or $.modal.close();
			//	    $.unblockUI();
			//	}
			//);
        }
    };

    OSX.init();
    //alert($("#" + id).html());
};



