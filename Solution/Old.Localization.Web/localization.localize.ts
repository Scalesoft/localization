

function translate(text: string, scope: string = null) {

    return LocalizationManager.getInstance().translate(text, scope);

}

function translateFormat(text: string, parameters: string[], scope: string = null) {

    return LocalizationManager.getInstance().translateFormat(text, parameters, scope);

}

function configureSiteUrlForTranslation(siteUrl: string) {
    LocalizationManager.getInstance().configureSiteUrl(siteUrl);
}

class LocalizationManager {

    private langCookieName = "current-lang";
    private scopeDelimeter = "-";
    private currentLang = "";
    private static instance: LocalizationManager;
    private dictionary: LocalizationDictionary;
    private downloading: boolean;
    private downloadingLanguage: string;
    private siteUrl: string = "";

    static getInstance() {
        if (typeof LocalizationManager.instance == "undefined" || LocalizationManager.instance == null) {
            LocalizationManager.instance = new LocalizationManager();
        }

        return LocalizationManager.instance;
    }

    public configureSiteUrl(siteUrl: string) {
        this.siteUrl = siteUrl;
    }

    public translate(textKey: string, scope: string = null): string {
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
    }

    public translateFormat(textKey: string, parameters: string[], scope: string = ""): string {

        var translation = this.translate(textKey, scope);

        return !parameters ? translation : this.formatString(translation, parameters);
    }

    private updateLocalizationFile(newCurrentLang: string, doneCallback?: () => void) {
        //if (this.downloading && this.downloadingLanguage === newCurrentLang) return; //Better to download multiple times instead of throw undefined error
        this.downloading = true;
        this.downloadingLanguage = newCurrentLang;

        //delete this.dictionary;

        var xmlhttp = new XMLHttpRequest();

        xmlhttp.onreadystatechange = () => {
            if (xmlhttp.readyState === XMLHttpRequest.DONE) {
                if (xmlhttp.status === 200) {

                    this.dictionary = new LocalizationDictionary(xmlhttp.responseText);
                    this.downloading = false;
                    if (doneCallback) {
                        doneCallback();
                    }
                } else {
                    this.downloading = false;
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
        xmlhttp.open("GET", `${baseUrl}/Localize/Translation?lang=${newCurrentLang}`, false);
        xmlhttp.send();

    }

    getCurrentLang(): string {

        if (this.currentLang === "") {
            const currentlang = this.getCurrentLangFromCookie();
            this.setCurrentlang(currentlang);
        }

        return this.currentLang;

    }

    setCurrentlang(newCurrentLang: string, doneCallback?: () => void) {
        this.currentLang = newCurrentLang;
        this.updateLocalizationFile(newCurrentLang, doneCallback);
    }

    private getCurrentLangFromCookie() {
        const name = this.langCookieName + "=";
        const cookiesList = document.cookie.split(";");
        for (let i = 0; i < cookiesList.length; i++) {
            let currentCookie = cookiesList[i];
            while (currentCookie.charAt(0) === " ") currentCookie = currentCookie.substring(1);
            if (currentCookie.indexOf(name) === 0) return currentCookie.substring(name.length, currentCookie.length);
        }
        return "";
    }

    private formatString(str, obj) {
        return str.replace(/\{\s*([^}\s]+)\s*\}/g, (m, p1, offset, string) => obj[p1]);
    }
}

class LocalizationDictionary {
    private data; //key:value json translation dicitionary

    constructor(data: string) {
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

    getText(text: string): string {
        var textKey = text.toLowerCase();
        if (typeof this.data[textKey] === "undefined") {
            return null;
        }

        return this.data[textKey];
    }
}