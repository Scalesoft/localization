class Localization {
    private mGlobalScope = "global";

    private mCultureCookieName: string = "Localization.Culture";
    private mCurrentCulture: string;

    private readonly mDictionary: { [key: string]: LocalizationDictionary } = {};
    private readonly mDictionaryQueue: { [key: string]: Array<(dictionary: LocalizationDictionary) => void> } = {};

    private mPluralizedDictionary: { [key: string]: LocalizationPluralizationDictionary } = {};
    private readonly mPluralizedDictionaryQueue: { [key: string]: Array<(dictionary: LocalizationPluralizationDictionary) => void> } = {};

    private mSiteUrl: string;

    /**
     * @deprecated Use translateAsync
     */
    public translate(
        text: string, scope?: string, cultureName?: string
    ): ILocalizedString {
        const dictionary = this.getDictionary(scope, cultureName);

        const result = dictionary.translate(text);
        if (result == null) {
            return this.getFallbackTranslation(text, scope, cultureName);
        }

        return result;
    }

    public translateAsync(
        onSuccess: (translation: ILocalizedString) => void, text: string, scope?: string, cultureName?: string
    ) {
        this.getDictionaryAsync((dictionary) => {
            const result = dictionary.translate(text);
            if (result == null) {
                return this.getFallbackTranslation(text, scope, cultureName);
            }

            onSuccess(result);
        }, scope, cultureName);
    }

    /**
     *@deprecated Use translateFormatAsync
     */
    public translateFormat(
        text: string, parameters: string[], scope?: string, cultureName?: string
    ): ILocalizedString {
        const dictionary = this.getDictionary(scope, cultureName);

        const result = dictionary.translateFormat(text, parameters);
        if (result == null) {
            return this.getFallbackTranslation(text, scope, cultureName);
        }

        return result;
    }

    public translateFormatAsync(
        onSuccess: (translation: ILocalizedString) => void, text: string, parameters: string[], scope?: string, cultureName?: string,
    ) {
        this.getDictionaryAsync((dictionary) => {
            const result = dictionary.translateFormat(text, parameters);
            if (result == null) {
                return this.getFallbackTranslation(text, scope, cultureName);
            }

            onSuccess(result);
        }, scope, cultureName);
    }

    /**
     *@deprecated Use translatePluralizationAsync
     */
    public translatePluralization(
        text: string, number: number, scope?: string, cultureName?: string
    ): ILocalizedString {
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

    public translatePluralizationAsync(
        onSuccess: (translation: ILocalizedString) => void, text: string, number: number, scope?: string, cultureName?: string
    ) {
        this.getPluralizationDictionaryAsync(
            dictionary => {
                try {
                    const result = dictionary.translatePluralization(text, number);
                    if (result == null) {
                        return this.getFallbackTranslation(text, scope, cultureName);
                    }

                    onSuccess(result);
                } catch (exception) {
                    onSuccess(this.handleError(exception, text))
                }
            }, scope, cultureName
        );
    }

    private getFallbackTranslation(text: string, scope: string, cultureName: string): ILocalizedString {
        console.log(
            `Localized string with key=${text} was not found in dictionary=${scope} with culture=${cultureName}`,
        );

        return {name: text, value: "X{undefined}", resourceNotFound: true};
    }

    private handleError(exception: Error, text: string) {
        console.error(exception.message);

        return {name: text, value: "X{error}", resourceNotFound: true};
    }

    public configureSiteUrl(siteUrl: string) {
        this.mSiteUrl = siteUrl;
    }

    /**
     *@deprecated Use getDictionaryAsync
     */
    private getDictionary(scope?: string, cultureName?: string): LocalizationDictionary {
        scope = this.checkScope(scope);
        cultureName = this.checkCultureName(cultureName);

        return this.getLocalizationDictionary(scope, cultureName);
    }

    private getDictionaryAsync(onSuccess: (dictionary: LocalizationDictionary) => void, scope?: string, cultureName?: string) {
        scope = this.checkScope(scope);
        cultureName = this.checkCultureName(cultureName);

        return this.getLocalizationDictionaryAsync(scope, cultureName, onSuccess);
    }

    /**
     *@deprecated Use getPluralizationDictionaryAsync
     */
    private getPluralizationDictionary(scope?: string, cultureName?: string): LocalizationPluralizationDictionary {
        scope = this.checkScope(scope);
        cultureName = this.checkCultureName(cultureName);

        return this.getPluralizationLocalizationDictionary(scope, cultureName);
    }

    private getPluralizationDictionaryAsync(
        onSuccess: (dictionary: LocalizationPluralizationDictionary) => void, scope?: string, cultureName?: string
    ) {
        scope = this.checkScope(scope);
        cultureName = this.checkCultureName(cultureName);

        return this.getPluralizationLocalizationDictionaryAsync(scope, cultureName, onSuccess);
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

    /**
     *@deprecated Use getLocalizationDictionaryAsync
     */
    private getLocalizationDictionary(scope: string, cultureName: string): LocalizationDictionary {
        const dictionaryKey = this.dictionaryKey(scope, cultureName);
        const dictionary = this.mDictionary[dictionaryKey];
        if (typeof dictionary === "undefined") {
            this.downloadDictionary(scope, cultureName);

            return this.mDictionary[dictionaryKey];
        }

        return dictionary;
    }

    private getLocalizationDictionaryAsync(
        scope: string, cultureName: string, onSuccess: (dictionary: LocalizationDictionary) => void
    ): void {
        const dictionaryKey = this.dictionaryKey(scope, cultureName);
        const dictionary = this.mDictionary[dictionaryKey];

        if (dictionary === undefined) {
            this.downloadDictionaryAsync(scope, cultureName, onSuccess);
        } else {
            onSuccess(dictionary);
        }
    }

    /**
     *@deprecated Use getPluralizationLocalizationDictionaryAsync
     */
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

    private getPluralizationLocalizationDictionaryAsync(
        scope: string, cultureName: string, onSuccess: (dictionary: LocalizationPluralizationDictionary) => void
    ) {
        let dictionaryKey = this.dictionaryKey(scope, cultureName);
        let dictionary = this.mPluralizedDictionary[dictionaryKey];

        if (dictionary === undefined) {
            this.downloadPluralizedDictionaryAsync(scope, cultureName, onSuccess);
        } else {
            onSuccess(dictionary);
        }
    }

    private dictionaryKey(scope: string, cultureName: string): string {
        return scope.concat("|", cultureName);
    }

    /**
     *@deprecated Use downloadDictionaryAsync
     */
    private downloadDictionary(
        scope: string, cultureName: string
    ): void {
        const dictionaryKey = this.dictionaryKey(scope, cultureName);

        if (this.mDictionaryQueue[scope] === undefined) {
            this.mDictionaryQueue[scope] = [];
        }

        const xmlHttpRequest = new XMLHttpRequest();

        xmlHttpRequest.onreadystatechange = () => {
            if (
                xmlHttpRequest.readyState === XMLHttpRequest.DONE
                && xmlHttpRequest.status === 200
            ) {
                let response = xmlHttpRequest.responseText;

                if (this.mDictionary[dictionaryKey] === undefined) {
                    this.mDictionary[dictionaryKey] = new LocalizationDictionary(response);
                }

                this.processDictionaryQueue(scope, dictionaryKey);
            }
        };

        xmlHttpRequest.open(
            "GET",
            `${this.getBaseUrl()}/Localization/Dictionary?scope=${scope}`,
            false
        );
        xmlHttpRequest.send();
    }

    private downloadDictionaryAsync(
        scope: string, cultureName: string, onSuccess: (dictionary: LocalizationDictionary) => void
    ): void {
        const dictionaryKey = this.dictionaryKey(scope, cultureName);

        if (this.mDictionaryQueue[scope] === undefined) {
            this.mDictionaryQueue[scope] = [];

            const xmlHttpRequest = new XMLHttpRequest();

            xmlHttpRequest.onreadystatechange = () => {
                if (
                    xmlHttpRequest.readyState === XMLHttpRequest.DONE
                    && xmlHttpRequest.status === 200
                ) {
                    let response = xmlHttpRequest.responseText;

                    if (this.mDictionary[dictionaryKey] === undefined) {
                        this.mDictionary[dictionaryKey] = new LocalizationDictionary(response);
                    }

                    this.processDictionaryQueue(scope, dictionaryKey);
                }
            };

            xmlHttpRequest.open(
                "GET",
                `${this.getBaseUrl()}/Localization/Dictionary?scope=${scope}`,
                true
            );
            xmlHttpRequest.send();
        }

        this.mDictionaryQueue[scope].push(onSuccess);

        this.processDictionaryQueue(scope, dictionaryKey);
    }

    private processDictionaryQueue(scope: string, dictionaryKey: string): void {
        if (this.mDictionary[dictionaryKey] !== undefined) {
            let onSuccessQueue = this.mDictionaryQueue[scope].shift();
            while (onSuccessQueue !== undefined) {
                onSuccessQueue(this.mDictionary[dictionaryKey]);

                onSuccessQueue = this.mDictionaryQueue[scope].shift();
            }
        }
    }

    /**
     * @deprecated Use downloadPluralizedDictionaryAsync
     */
    private downloadPluralizedDictionary(
        scope: string, cultureName: string
    ): void {
        const dictionaryKey = this.dictionaryKey(scope, cultureName);

        if (this.mPluralizedDictionaryQueue[scope] === undefined) {
            this.mPluralizedDictionaryQueue[scope] = [];
        }

        const xmlHttpRequest = new XMLHttpRequest();

        xmlHttpRequest.onreadystatechange = () => {
            if (
                xmlHttpRequest.readyState === XMLHttpRequest.DONE
                && xmlHttpRequest.status === 200
            ) {
                let response = xmlHttpRequest.responseText;

                if (this.mPluralizedDictionary[dictionaryKey] === undefined) {
                    this.mPluralizedDictionary[dictionaryKey] = new LocalizationPluralizationDictionary(response);
                }

                this.processPluralizedDictionaryQueue(scope, dictionaryKey);
            }
        };

        xmlHttpRequest.open(
            "GET",
            `${this.getBaseUrl()}/Localization/PluralizedDictionary?scope=${scope}`,
            false
        );
        xmlHttpRequest.send();
    }

    private downloadPluralizedDictionaryAsync(
        scope: string, cultureName: string, onSuccess: (dictionary: LocalizationPluralizationDictionary) => void
    ): void {
        const dictionaryKey = this.dictionaryKey(scope, cultureName);

        if (this.mPluralizedDictionaryQueue[scope] === undefined) {
            this.mPluralizedDictionaryQueue[scope] = [];

            const xmlHttpRequest = new XMLHttpRequest();

            xmlHttpRequest.onreadystatechange = () => {
                if (
                    xmlHttpRequest.readyState === XMLHttpRequest.DONE
                    && xmlHttpRequest.status === 200
                ) {
                    let response = xmlHttpRequest.responseText;

                    if (this.mPluralizedDictionary[dictionaryKey] === undefined) {
                        this.mPluralizedDictionary[dictionaryKey] = new LocalizationPluralizationDictionary(response);
                    }

                    this.processPluralizedDictionaryQueue(scope, dictionaryKey);
                }
            };

            xmlHttpRequest.open(
                "GET",
                `${this.getBaseUrl()}/Localization/PluralizedDictionary?scope=${scope}`,
                true
            );
            xmlHttpRequest.send();
        }

        this.mPluralizedDictionaryQueue[scope].push(onSuccess);

        this.processPluralizedDictionaryQueue(scope, dictionaryKey);
    }

    private processPluralizedDictionaryQueue(scope: string, dictionaryKey: string): void {
        if (this.mPluralizedDictionary[dictionaryKey] !== undefined) {
            let onSuccessQueue = this.mPluralizedDictionaryQueue[scope].shift();
            while (onSuccessQueue !== undefined) {
                onSuccessQueue(this.mPluralizedDictionary[dictionaryKey]);

                onSuccessQueue = this.mPluralizedDictionaryQueue[scope].shift();
            }
        }
    }

    private getBaseUrl(): string {
        let baseUrl = this.mSiteUrl;
        if (baseUrl && baseUrl.charAt(baseUrl.length - 1) === "/") {
            baseUrl = baseUrl.substring(0, baseUrl.length - 1);
        }

        return baseUrl;
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

        return {name: text, value: formatedText, resourceNotFound: translation.resourceNotFound};
    }

    private formatString(str: ILocalizedString, obj: string[]): string {
        return str.value.replace(/\{\s*([^}\s]+)\s*\}/g, (m, p1, offset, string) => obj[p1]);
    }
}

class LocalizationPluralizationDictionary {
    private readonly mDictionary: { [key: string]: IPluralizedString };

    constructor(dictionary: string) {
        this.mDictionary = JSON.parse(dictionary);
    }

    public translatePluralization(text: string, number: number): ILocalizedString {
        const pluralizedString = this.mDictionary[text];
        if (typeof pluralizedString === "undefined" || pluralizedString === null) {
            return null;
        }
        for (let interval of pluralizedString.intervals) {
            const translationInterval = interval.interval;

            if (LocalizationUtils.isInInterval(number, translationInterval)) {
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

interface IPluralizedString {
    intervals: IIntervalWithTranslation[];
    defaultLocalizedString: ILocalizedString;
}

interface IIntervalWithTranslation {
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
                .filter(cookie => cookie.indexOf(name) === 0)
                .map(cookie => decodeURIComponent(cookie.substring(name.length)))[0]
            || null;
    }

    /*
     * Returns true when value is in the pluralization interval
     */
    public static isInInterval(value: number, interval: PluralizationInterval): boolean {
        if (!interval) {
            return false;
        }

        return interval.start <= value && value <= interval.end;
    }
}
