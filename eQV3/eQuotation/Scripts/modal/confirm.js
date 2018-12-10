/*
* SimpleModal Confirm Modal Dialog
* http://www.ericmmartin.com/projects/simplemodal/
* http://code.google.com/p/simplemodal/
*
* Copyright (c) 2008 Eric Martin - http://ericmmartin.com
*
* Licensed under the MIT license:
*   http://www.opensource.org/licenses/mit-license.php
*
* Revision: $Id$
*
*/


function confirm(message, callback) {
    $('#confirm').modal({
        close: false,
        overlayId: 'confirmModalOverlay',
        containerId: 'confirmModalContainer',
        onShow: function (dialog) {
            dialog.data.find('.message').append(message);

            // if the user clicks "yes"
            dialog.data.find('.yes').click(function () {
                // call the callback
                if ($.isFunction(callback)) {
                    callback.apply();
                }
                // close the dialog
                $.modal.close();
            });

            //if the user click "no"
            dialog.data.find('.no').click(function () {
                $.modal.close();
                $.unblockUI();
            });

            //if the user click "cross"
            dialog.data.find('.modalCloseX').click(function () {
                $.modal.close();
                $.unblockUI();
            });
        }
    });
}