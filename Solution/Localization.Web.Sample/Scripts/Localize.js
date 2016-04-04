function translate(text) {
    return LocalizationManager.getInstance().translate(text);
}
var LocalizationManager = (function () {
    function LocalizationManager() {
        this.langCookieName = "current-lang";
        this.currentLang = "";
    }
    LocalizationManager.getInstance = function () {
        if (typeof LocalizationManager.instance == "undefined" || LocalizationManager.instance === null) {
            LocalizationManager.instance = new LocalizationManager();
        }
        return LocalizationManager.instance;
    };
    LocalizationManager.prototype.translate = function (text) {
        if (typeof this.dictionary != "undefined") {
            return this.dictionary.getText(text);
        }
        this.updateLocalizationFile(this.getCurrentLang());
        return this.dictionary.getText(text);
    };
    LocalizationManager.prototype.updateLocalizationFile = function (newCurrentLang, doneCallback) {
        var _this = this;
        if (this.downloading && this.downloadingLanguage === newCurrentLang)
            return;
        this.downloading = true;
        this.downloadingLanguage = newCurrentLang;
        delete this.dictionary;
        var xmlhttp = new XMLHttpRequest();
        xmlhttp.onreadystatechange = function () {
            if (xmlhttp.readyState == XMLHttpRequest.DONE) {
                if (xmlhttp.status == 200) {
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
        xmlhttp.open("GET", "/Localize/Translation?lang=" + newCurrentLang, false);
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
            while (currentCookie.charAt(0) == " ")
                currentCookie = currentCookie.substring(1);
            if (currentCookie.indexOf(name) == 0)
                return currentCookie.substring(name.length, currentCookie.length);
        }
        return "";
    };
    return LocalizationManager;
})();
var LocalizationDictionary = (function () {
    function LocalizationDictionary(data) {
        this.data = JSON.parse(data);
    }
    LocalizationDictionary.prototype.getText = function (text) {
        if (typeof this.data[text] == "undefined") {
            return text;
        }
        return this.data[text];
    };
    return LocalizationDictionary;
})();
//# sourceMappingURL=Localize.js.map