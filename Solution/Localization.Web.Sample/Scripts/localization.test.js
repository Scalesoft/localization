
function showLocalizedText() {
    var localized = translate("JSHello");
    $("#jslocalization").html(localized);
    $("#jslocalization").append("<br>");

    var localizedWithScope = translate("Hello", "greetings");
    $("#jslocalization").append(localizedWithScope);
    $("#jslocalization").append("<br>");

    var localizedWithFormat = translateFormat("HelloWithDate", [Date.now()]);
    $("#jslocalization").append(localizedWithFormat);
    $("#jslocalization").append("<br>");

    var localizedWithScopeAndFormat = translateFormat("HelloWithDateAndScope", [Date.now(), "global"], "global");
    $("#jslocalization").append(localizedWithScopeAndFormat);
}