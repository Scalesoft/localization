

function translate(text: string) {

    return LocalizationManager.getInstance().translate(text);

}

class LocalizationManager {

    private langCookieName = "current-lang";

    private currentLang = "";
    private static instance: LocalizationManager;
    private dictionary: LocalizationDictionary;
    private downloading: boolean;
    private downloadingLanguage: string;

    static getInstance() {
        if (typeof LocalizationManager.instance == "undefined" || LocalizationManager.instance === null) {
            LocalizationManager.instance = new LocalizationManager();
        }

        return LocalizationManager.instance;
    }

    translate(text: string): string {
        if (typeof this.dictionary != "undefined") {
            return this.dictionary.getText(text);
        }

        this.updateLocalizationFile(this.getCurrentLang());

        return this.dictionary.getText(text);
    }


    private updateLocalizationFile(newCurrentLang: string, doneCallback?: () => void) {
        if (this.downloading && this.downloadingLanguage === newCurrentLang) return;
        this.downloading = true;
        this.downloadingLanguage = newCurrentLang;

        delete this.dictionary;

        var xmlhttp = new XMLHttpRequest();

        xmlhttp.onreadystatechange = () => {
            if (xmlhttp.readyState == XMLHttpRequest.DONE) {
                if (xmlhttp.status == 200) {

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
        xmlhttp.open("GET", `/Localize/Translation?lang=${newCurrentLang}`, false);
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
            while (currentCookie.charAt(0) == " ") currentCookie = currentCookie.substring(1);
            if (currentCookie.indexOf(name) == 0) return currentCookie.substring(name.length, currentCookie.length);
        }
        return "";
    }
}

class LocalizationDictionary {
    private data; //key:value json translation dicitionary

    constructor(data: string) {
        this.data = JSON.parse(data);
    }

    getText(text: string): string {
        if (typeof this.data[text] == "undefined") {
            return text;
        }

        return this.data[text];
    }
}