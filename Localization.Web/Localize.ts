

function localize(text: string) {

    return LocalizationManager.getInstance().localize(text);

}

class LocalizationManager {
    
    private langCookieName = "current-lang";

    private currentLang = "";
    private static instance: LocalizationManager;
    private dictionary: LocalizationDictionary;
    private updateDicitonaryDoneCallbacks:{(): void;} [] = [];

    public static getInstance() {
        if (LocalizationManager.instance === null) {
            LocalizationManager.instance = new LocalizationManager();
        }

        return LocalizationManager.instance;
    }

    public localize(text: string): string {
        if (typeof this.dictionary != "undefined") {
            return this.dictionary.getText(text);
        }
        
        this.updateDicitonaryDoneCallbacks.push(() => {
            return this.localize(text);
        });
    }


    private updateLocalizationFile(newCurrentLang:string, doneCallback: ()=> void) {
        delete this.dictionary;

        var xmlhttp = new XMLHttpRequest();

        onreadystatechange = ()=> {
            if (xmlhttp.readyState == XMLHttpRequest.DONE) {
                if (xmlhttp.status == 200) {

                    this.dictionary = new LocalizationDictionary(xmlhttp.responseText);

                    for (var i = 0; i < this.updateDicitonaryDoneCallbacks.length; i++) {
                        var callback = this.updateDicitonaryDoneCallbacks[i];
                        callback();
                    }

                    this.updateDicitonaryDoneCallbacks = [];
                }
                else {
                    
                }
            }
        }

        xmlhttp.open("GET", "/Localize/Translation?lang="+newCurrentLang, true);
        xmlhttp.send();

    }

    public getCurrentLang():string {

        if (this.currentLang === "") {
            var currentlang = this.getCurrentLangFromCookie();
            this.setCurrentlang(currentlang);
        }

        return this.currentLang;

    }

    public setCurrentlang(newCurrentLang: string, doneCallback?: ()=> void) {
        this.currentLang = newCurrentLang;
        this.updateLocalizationFile(newCurrentLang, doneCallback);
    }

    private getCurrentLangFromCookie() {
        var name = this.langCookieName + "=";
        var cookiesList = document.cookie.split(';');
        for (var i = 0; i < cookiesList.length; i++) {
            var currentCookie = cookiesList[i];
            while (currentCookie.charAt(0) == ' ') currentCookie = currentCookie.substring(1);
            if (currentCookie.indexOf(name) == 0) return currentCookie.substring(name.length, currentCookie.length);
        }
        return "";
    }
}

class LocalizationDictionary {
    private data;   //key:value json translation dicitionary

    constructor(data: string) {
        this.data = data;
    }

    getText(text: string): string {
        if (typeof this.data[text] == "undefined") {
            return text;
        }
        
        return this.data[text];
    }
}