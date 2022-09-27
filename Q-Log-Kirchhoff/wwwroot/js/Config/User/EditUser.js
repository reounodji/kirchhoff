document.getElementById("User").className += " active";

$(document).ready(function () {
    setupInfoPopOver();
});

function setupInfoPopOver() {
    $('[data-toggle="popover"]').popover();
    var data = "<ul>";
    data += "<li>" + localizations['PasswordRequirements'] + "</li>";
    data += "<li>" + localizations['NameRequired'] + "</li>";
    data += "</ul>";
    var popover = $('#InfoPopover');
    popover.attr('data-content', data);
}