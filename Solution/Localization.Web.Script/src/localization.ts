class Localization {
    private mGlobalScope = "global";

    private mCultureCookieName: string = "Localization.Culture";
    private mCurrentCulture: string;

    private mDownloading: boolean;
    private mDownloadingCulture: string;
    private mDownloadingScope: string;

    private mDictionary: { [key: string] : LocalizationDictionary } = {};

    private mSiteUrl: string;

    public translate(text: string, scope?: string, cultureName?: string): string {
        let dictionary = this.getDictionary(scope, cultureName);

        return dictionary.translate(text);
    }

    public translateFormat(text: string, parameters: string[], scope?: string, cultureName?: string): string {
        let dictionary = this.getDictionary(scope, cultureName);

        return dictionary.translateFormat(text, parameters);
    }

    public configureSiteUrl(siteUrl: string) {
        this.mSiteUrl = siteUrl;
    }

    private getDictionary(scope?: string, cultureName?: string): LocalizationDictionary {
        scope = this.checkScope(scope);
        cultureName = this.checkCultureName(cultureName);

        return this.getLocalizationDictionary(scope, cultureName);
    }

    private checkCultureName(cultureName?: string): string {
        if (cultureName) {
            return cultureName;
        }

        return this.getCurrentCulture();
    }

    private checkScope(scope?: string): string{
        if (scope) {
            return scope;
        }

        return this.mGlobalScope;
    }

    private getLocalizationDictionary(scope: string, cultureName: string) : LocalizationDictionary {
        let dictionaryKey = this.dictionaryKey(scope, cultureName);
        let dictionary = this.mDictionary[dictionaryKey];
        if (dictionary === null) {
            this.downloadDictionary(scope, cultureName);

            return this.mDictionary[dictionaryKey];
        }

        return dictionary;
    }

    private dictionaryKey(scope: string, cultureName: string): string {
        return scope.concat("|", cultureName);
    }

    private downloadDictionary(scope: string, cultureName: string){
        this.markDownloading(true, scope, cultureName);

        let xmlHttpRequest = new XMLHttpRequest();

        xmlHttpRequest.onreadystatechange = () => {
            if (xmlHttpRequest.readyState === XMLHttpRequest.DONE) {
                if (xmlHttpRequest.status === 200) {
                    let response = xmlHttpRequest.responseText;
                    let dictionaryKey = this.dictionaryKey(scope, cultureName);
                    this.mDictionary[dictionaryKey] = new LocalizationDictionary(response);
                }

                this.markDownloading(false, scope, cultureName);
            }
        }

        let baseUrl = this.mSiteUrl;
        if (baseUrl && baseUrl.charAt(baseUrl.length - 1) === "/") {
            baseUrl = baseUrl.substring(0, baseUrl.length - 1);
        }
        xmlHttpRequest.open("GET", `${baseUrl}/Localization/Dictionary?scope=${scope}`, false);
        xmlHttpRequest.send();
    }

    private  getCurrentCulture(): string {
        if (this.mCurrentCulture === "") {
            const currentCulture = this.getCurrentCultureCookie();
            this.setCurrentCulture(currentCulture);
        }

        return this.mCurrentCulture;
    }

    private setCurrentCulture(culture: string) {
        this.mCurrentCulture = culture;
    }

    private getCurrentCultureCookie(): string {
        return LocalizationUtils.getCookie(this.mCultureCookieName);
    }

    private markDownloading(downloading: boolean, scope: string, cultureName: string) {       
        if (downloading) {
            this.mDownloadingScope = scope;
            this.mDownloadingCulture = cultureName;
        } else {
            this.mDownloadingScope = "";
            this.mDownloadingCulture = "";
        }

        this.mDownloading = downloading;
    }
}

class LocalizationDictionary {
    private mDictionary;

    constructor(dictionary: string) {
        this.mDictionary = JSON.parse(dictionary);
    }

    public translate(text: string): string {
        let result = this.mDictionary[text];
        if (typeof result === "undefined") {
            return null;
        }

        return result;
    }

    public translateFormat(text: string, parameters: string[]): string {
        let translation = this.translate(text);

        return !parameters ? translation : this.formatString(translation, parameters);
    }

    private formatString(str, obj) {
        return str.replace(/\{\s*([^}\s]+)\s*\}/g, (m, p1, offset, string) => obj[p1]);
    }
}

class LocalizationUtils {
    
    static getCookie(name: string): string {
        name = name + "=";
        return document.cookie
            .split(";")
            .map(c => c.trim())
            .filter(cookie => {
                return cookie.indexOf(name) === 0;
            })
            .map(cookie => {
                return decodeURIComponent(cookie.substring(name.length));
            })[0] ||
            null;
    }

}