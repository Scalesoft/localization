
function showLocalizedText() {
    var localized = translate("JSHello");
    $("#jslocalization").html(localized);
    $("#jslocalization").append("<br>");

    var localizedWithScope = translate("Hello with date", "", [Date.now()]);
    $("#jslocalization").append(localizedWithScope);
    $("#jslocalization").append("<br>");

    var localizedWithDateAndScope = translate("Hello with date and scope", "global", [Date.now(), "global"]);
    $("#jslocalization").append(localizedWithDateAndScope);
}