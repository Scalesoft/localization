function translate(text, scope) {
    if (scope === void 0) { scope = null; }
    return LocalizationManager.getInstance().translate(text, scope);
}
function translateFormat(text, parameters, scope) {
    if (scope === void 0) { scope = null; }
    return LocalizationManager.getInstance().translateFormat(text, parameters, scope);
}
function configureSiteUrlForTranslation(siteUrl) {
    LocalizationManager.getInstance().configureSiteUrl(siteUrl);
}
var LocalizationManager = (function () {
    function LocalizationManager() {
        this.langCookieName = "current-lang";
        this.scopeDelimeter = "-";
        this.currentLang = "";
        this.siteUrl = "";
    }
    LocalizationManager.getInstance = function () {
        if (typeof LocalizationManager.instance == "undefined" || LocalizationManager.instance == null) {
            LocalizationManager.instance = new LocalizationManager();
        }
        return LocalizationManager.instance;
    };
    LocalizationManager.prototype.configureSiteUrl = function (siteUrl) {
        this.siteUrl = siteUrl;
    };
    LocalizationManager.prototype.translate = function (textKey, scope) {
        if (scope === void 0) { scope = null; }
        if (typeof this.dictionary == "undefined") {
            this.updateLocalizationFile(this.getCurrentLang());
        }
        var translationKey = scope == null ? textKey : scope + this.scopeDelimeter + textKey;
        var translation = this.dictionary.getText(translationKey);
        if (translation == null) {
            if (scope == null) {
                throw new Error(this.formatString("Given translation key '{0}' does not exit in resource file", [textKey]));
            }
            throw new Error(this.formatString("Given translation key '{0}' with scope '{1}' does not exit in resource file", [textKey, scope]));
        }
        return translation;
    };
    LocalizationManager.prototype.translateFormat = function (textKey, parameters, scope) {
        if (scope === void 0) { scope = ""; }
        var translation = this.translate(textKey, scope);
        return !parameters ? translation : this.formatString(translation, parameters);
    };
    LocalizationManager.prototype.updateLocalizationFile = function (newCurrentLang, doneCallback) {
        var _this = this;
        //if (this.downloading && this.downloadingLanguage === newCurrentLang) return; //Better to download multiple times instead of throw undefined error
        this.downloading = true;
        this.downloadingLanguage = newCurrentLang;
        //delete this.dictionary;
        var xmlhttp = new XMLHttpRequest();
        xmlhttp.onreadystatechange = function () {
            if (xmlhttp.readyState === XMLHttpRequest.DONE) {
                if (xmlhttp.status === 200) {
                    _this.dictionary = new LocalizationDictionary(xmlhttp.responseText);
                    _this.downloading = false;
                    if (doneCallback) {
                        doneCallback();
                    }
                }
                else {
                    _this.downloading = false;
                    if (doneCallback) {
                        doneCallback();
                    }
                }
            }
        };
        var baseUrl = this.siteUrl;
        if (baseUrl && baseUrl.charAt(baseUrl.length - 1) === "/") {
            baseUrl = baseUrl.substring(0, baseUrl.length - 1);
        }
        xmlhttp.open("GET", baseUrl + "/Localize/Translation?lang=" + newCurrentLang, false);
        xmlhttp.send();
    };
    LocalizationManager.prototype.getCurrentLang = function () {
        if (this.currentLang === "") {
            var currentlang = this.getCurrentLangFromCookie();
            this.setCurrentlang(currentlang);
        }
        return this.currentLang;
    };
    LocalizationManager.prototype.setCurrentlang = function (newCurrentLang, doneCallback) {
        this.currentLang = newCurrentLang;
        this.updateLocalizationFile(newCurrentLang, doneCallback);
    };
    LocalizationManager.prototype.getCurrentLangFromCookie = function () {
        var name = this.langCookieName + "=";
        var cookiesList = document.cookie.split(";");
        for (var i = 0; i < cookiesList.length; i++) {
            var currentCookie = cookiesList[i];
            while (currentCookie.charAt(0) === " ")
                currentCookie = currentCookie.substring(1);
            if (currentCookie.indexOf(name) === 0)
                return currentCookie.substring(name.length, currentCookie.length);
        }
        return "";
    };
    LocalizationManager.prototype.formatString = function (str, obj) {
        return str.replace(/\{\s*([^}\s]+)\s*\}/g, function (m, p1, offset, string) { return obj[p1]; });
    };
    return LocalizationManager;
}());
var LocalizationDictionary = (function () {
    function LocalizationDictionary(data) {
        this.data = JSON.parse(data);
        for (var key in this.data) {
            if (this.data.hasOwnProperty(key)) {
                if (key.toLocaleLowerCase() !== key) {
                    this.data[key.toLowerCase()] = this.data[key];
                    delete this.data[key];
                }
            }
        }
    }
    LocalizationDictionary.prototype.getText = function (text) {
        var textKey = text.toLowerCase();
        if (typeof this.data[textKey] === "undefined") {
            return null;
        }
        return this.data[textKey];
    };
    return LocalizationDictionary;
}());
//# sourceMappingURL=localization.localize.js.map