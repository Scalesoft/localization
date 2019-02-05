class Localization {
    private mGlobalScope = "global";

    private mCultureCookieName: string = "Localization.Culture";
    private mCurrentCulture: string;

    private mDictionary: { [key: string]: LocalizationDictionary } = {};

    private mPluralizedDictionary: { [key: string]: LocalizationPluralizationDictionary } = {};

    private mSiteUrl: string;

    public translate(text: string, scope?: string, cultureName?: string): ILocalizedString {
        let dictionary = this.getDictionary(scope, cultureName);

        var result = dictionary.translate(text);
        if (result == null) {
            return this.getFallbackTranslation(text, scope, cultureName);
        }

        return result;
    }

    public translateFormat(text: string, parameters: string[], scope?: string, cultureName?: string): ILocalizedString {
        let dictionary = this.getDictionary(scope, cultureName);

        var result = dictionary.translateFormat(text, parameters);
        if (result == null) {
            return this.getFallbackTranslation(text, scope, cultureName);
        }

        return result;
    }

    public translatePluralization(text: string, number: number, scope?: string, cultureName?: string):
        ILocalizedString {
        const dictionary = this.getPluralizationDictionary(scope, cultureName);

        try {
            const result = dictionary.translatePluralization(text, number);
            if (result == null) {
                return this.getFallbackTranslation(text, scope, cultureName);
            }

            return result;
        } catch (exception) {
            return this.handleError(exception, text);
        }
    }

    private getFallbackTranslation(text: string, scope: string, cultureName: string): ILocalizedString {
        console.log(
            `Localized string with key=${text} was not found in dictionary=${scope} with culture=${cultureName}`);
        const localizedString: ILocalizedString = { name: text, value: "X{undefined}", resourceNotFound: true };
        return localizedString;
    }

    private handleError(exception: Error, text: string) {
        console.error(exception.message);
        const localizedString: ILocalizedString = { name: text, value: "X{error}", resourceNotFound: true };
        return localizedString;
    }

    public configureSiteUrl(siteUrl: string) {
        this.mSiteUrl = siteUrl;
    }

    private getDictionary(scope?: string, cultureName?: string): LocalizationDictionary {
        scope = this.checkScope(scope);
        cultureName = this.checkCultureName(cultureName);

        return this.getLocalizationDictionary(scope, cultureName);
    }

    private getPluralizationDictionary(scope?: string, cultureName?: string): LocalizationPluralizationDictionary {
        scope = this.checkScope(scope);
        cultureName = this.checkCultureName(cultureName);

        return this.getPluralizationLocalizationDictionary(scope, cultureName);
    }

    private checkCultureName(cultureName?: string): string {
        if (cultureName) {
            return cultureName;
        }

        return this.getCurrentCulture();
    }

    private checkScope(scope?: string): string {
        if (scope) {
            return scope;
        }

        return this.mGlobalScope;
    }

    private getLocalizationDictionary(scope: string, cultureName: string): LocalizationDictionary {
        let dictionaryKey = this.dictionaryKey(scope, cultureName);
        let dictionary = this.mDictionary[dictionaryKey];
        if (typeof dictionary === "undefined") {
            this.downloadDictionary(scope, cultureName);

            return this.mDictionary[dictionaryKey];
        }

        return dictionary;
    }

    private getPluralizationLocalizationDictionary(scope: string, cultureName: string):
        LocalizationPluralizationDictionary {
        let dictionaryKey = this.dictionaryKey(scope, cultureName);
        let dictionary = this.mPluralizedDictionary[dictionaryKey];
        if (typeof dictionary === "undefined") {
            this.downloadPluralizedDictionary(scope, cultureName);

            return this.mPluralizedDictionary[dictionaryKey];
        }

        return dictionary;
    }

    private dictionaryKey(scope: string, cultureName: string): string {
        return scope.concat("|", cultureName);
    }

    private downloadDictionary(scope: string, cultureName: string) {
        let xmlHttpRequest = new XMLHttpRequest();

        xmlHttpRequest.onreadystatechange = () => {
            if (xmlHttpRequest.readyState === XMLHttpRequest.DONE) {
                if (xmlHttpRequest.status === 200) {
                    let response = xmlHttpRequest.responseText;
                    let dictionaryKey = this.dictionaryKey(scope, cultureName);
                    this.mDictionary[dictionaryKey] = new LocalizationDictionary(response);
                }
            }
        }

        let baseUrl = this.mSiteUrl;
        if (baseUrl && baseUrl.charAt(baseUrl.length - 1) === "/") {
            baseUrl = baseUrl.substring(0, baseUrl.length - 1);
        }
        xmlHttpRequest.open("GET", `${baseUrl}/Localization/Dictionary?scope=${scope}`, false);
        xmlHttpRequest.send();
    }

    private downloadPluralizedDictionary(scope: string, cultureName: string) {
        let xmlHttpRequest = new XMLHttpRequest();

        xmlHttpRequest.onreadystatechange = () => {
            if (xmlHttpRequest.readyState === XMLHttpRequest.DONE) {
                if (xmlHttpRequest.status === 200) {
                    let response = xmlHttpRequest.responseText;
                    let dictionaryKey = this.dictionaryKey(scope, cultureName);
                    this.mPluralizedDictionary[dictionaryKey] = new LocalizationPluralizationDictionary(response);
                }
            }
        }

        let baseUrl = this.mSiteUrl;
        if (baseUrl && baseUrl.charAt(baseUrl.length - 1) === "/") {
            baseUrl = baseUrl.substring(0, baseUrl.length - 1);
        }
        xmlHttpRequest.open("GET", `${baseUrl}/Localization/PluralizedDictionary?scope=${scope}`, false);
        xmlHttpRequest.send();
    }

    public getCurrentCulture(): string {
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
}

class LocalizationDictionary {
    private mDictionary: { [key: string]: ILocalizedString };

    constructor(dictionary: string) {
        this.mDictionary = JSON.parse(dictionary);
    }

    public translate(text: string): ILocalizedString {
        let result = this.mDictionary[text];
        if (typeof result === "undefined") {
            return null;
        }

        return result;
    }

    public translateFormat(text: string, parameters: string[]): ILocalizedString {
        let translation = this.translate(text);

        var formatedText = !parameters ? translation.value : this.formatString(translation, parameters);
        var localizedString: ILocalizedString =
            { name: text, value: formatedText, resourceNotFound: translation.resourceNotFound };

        return localizedString;
    }

    public translatePluralization(text: string, number: number): ILocalizedString {
        let result = this.mDictionary[text];
        if (typeof result === "undefined") {
            return null;
        }

        return result;
    }

    private formatString(str: ILocalizedString, obj: string[]): string {
        return str.value.replace(/\{\s*([^}\s]+)\s*\}/g, (m, p1, offset, string) => obj[p1]);
    }
}

class LocalizationPluralizationDictionary {
    private mDictionary: { [key: string]: IClientPluralizedString };

    constructor(dictionary: string) {
        this.mDictionary = JSON.parse(dictionary);
    }

    public translatePluralization(text: string, number: number): ILocalizedString {
        const pluralizedString = this.mDictionary[text];
        if (typeof pluralizedString === "undefined" || pluralizedString === null) {
            return null;
        }
        const requestedInterval = new PluralizationInterval(number, number);
        for (let key in pluralizedString.pluralized) {
            if (pluralizedString.pluralized.hasOwnProperty(key)) {
                const separatedString = key.split(",");
                const intervalStart = parseInt(separatedString[0], 10);
                const intervalEnd = parseInt(separatedString[1], 10);

                if (isNaN(intervalStart) || isNaN(intervalEnd)) {
                    continue;
                }

                const translationInterval = new PluralizationInterval(intervalStart, intervalEnd);

                if (translationInterval.equals(requestedInterval)) {
                    return pluralizedString.pluralized[key];
                }
            }
        }

        return pluralizedString.defaultLocalizedString;

    }
}

interface ILocalizedString {
    name: string;
    resourceNotFound: boolean;
    value: string;
}

interface IClientPluralizedString {
    pluralized: { [key: string]: ILocalizedString }; // key is pluralization interval joined with a comma
    defaultLocalizedString: ILocalizedString;
}

class PluralizationInterval {
    public readonly x: number;
    public readonly y: number;

    constructor(x: number, y: number) {
        if (x > y) {
            const intervalErrorMsg = "The x value should be less or equal than y.";

            throw new Error(intervalErrorMsg);
        }

        this.x = x;
        this.y = y;
    }

    public isOverlaping(obj: PluralizationInterval): boolean {
        if (!obj) {
            throw new Error("Interval is not defined");
        }

        return this.x <= obj.y && obj.x <= this.y;
    }

    public equals(obj: PluralizationInterval): boolean {
        if (obj == null) {
            return false;
        }

        if (typeof this != typeof obj) {
            return false;
        }

        return this.isOverlaping(obj);
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