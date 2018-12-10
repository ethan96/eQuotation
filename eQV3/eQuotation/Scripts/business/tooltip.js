

function IFS_tooltip() {
    $(".tooltip").each(function (index) {
        var newString = $(this).val().replace(/\n/g, '<br />');
        $(this).prop('title', newString);
    });
    $('.tooltip').tooltipster({
        position: "top",
        contentAsHTML: true
    });

};
