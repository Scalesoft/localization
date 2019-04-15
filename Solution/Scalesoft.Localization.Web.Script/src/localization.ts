class Localization {
    private mGlobalScope = "global";

    private mCultureCookieName: string = "Localization.Culture";
    private mCurrentCulture: string;

    private readonly mDictionary: { [key: string]: LocalizationDictionary } = {};

    private mPluralizedDictionary: { [key: string]: LocalizationPluralizationDictionary } = {};

    private mSiteUrl: string;

    public translate(text: string, scope?: string, cultureName?: string): ILocalizedString {
        const dictionary = this.getDictionary(scope, cultureName);

        const result = dictionary.translate(text);
        if (result == null) {
            return this.getFallbackTranslation(text, scope, cultureName);
        }

        return result;
    }

    public translateFormat(text: string, parameters: string[], scope?: string, cultureName?: string): ILocalizedString {
        const dictionary = this.getDictionary(scope, cultureName);

        const result = dictionary.translateFormat(text, parameters);
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
        const dictionaryKey = this.dictionaryKey(scope, cultureName);
        const dictionary = this.mDictionary[dictionaryKey];
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
        if (typeof this.mCurrentCulture === "undefined") {
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
    private readonly mDictionary: { [key: string]: ILocalizedString };

    constructor(dictionary: string) {
        this.mDictionary = JSON.parse(dictionary);
    }

    public translate(text: string): ILocalizedString {
        const result = this.mDictionary[text];
        if (typeof result === "undefined") {
            return null;
        }

        return result;
    }

    public translateFormat(text: string, parameters: string[]): ILocalizedString {
        const translation = this.translate(text);

        const formatedText = !parameters ? translation.value : this.formatString(translation, parameters);
        const localizedString: ILocalizedString =
            { name: text, value: formatedText, resourceNotFound: translation.resourceNotFound };

        return localizedString;
    }

    private formatString(str: ILocalizedString, obj: string[]): string {
        return str.value.replace(/\{\s*([^}\s]+)\s*\}/g, (m, p1, offset, string) => obj[p1]);
    }
}

class LocalizationPluralizationDictionary {
    private readonly mDictionary: { [key: string]: IClientPluralizedString };

    constructor(dictionary: string) {
        this.mDictionary = JSON.parse(dictionary);
    }

    public translatePluralization(text: string, number: number): ILocalizedString {
        const pluralizedString = this.mDictionary[text];
        if (typeof pluralizedString === "undefined" || pluralizedString === null) {
            return null;
        }
        const requestedInterval = new PluralizationInterval(number, number);
        for (let interval of pluralizedString.intervals) {
            const translationInterval = interval.interval;

            if (LocalizationUtils.isInInterval(requestedInterval, translationInterval)) {
                return interval.localizedString;
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
    intervals: IClientIntervalWithTranslation[];
    defaultLocalizedString: ILocalizedString;
}

interface IClientIntervalWithTranslation {
    interval: PluralizationInterval;
    localizedString: ILocalizedString;
}

class PluralizationInterval {
    public readonly start: number;
    public readonly end: number;

    constructor(start: number, end: number) {
        if (start > end) {
            const intervalErrorMsg = "The start value should be less or equal than end.";

            throw new Error(intervalErrorMsg);
        }

        this.start = start;
        this.end = end;
    }
}

class LocalizationUtils {

    public static getCookie(name: string): string {
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

    private static isOverlaping(inner: PluralizationInterval, outer: PluralizationInterval): boolean {
        if (!inner) {
            throw new Error("Interval is not defined");
        }

        return outer.start <= inner.start && inner.end <= outer.end;
    }

    /*
     * Returns true when inner pluralization interval is in the outer pluralization interval
     */
    public static isInInterval(inner: PluralizationInterval, outer: PluralizationInterval): boolean {
        if (!inner) {
            return false;
        }

        if (typeof outer != typeof inner) {
            return false;
        }

        return this.isOverlaping(inner, outer);
    }
}