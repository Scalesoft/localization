const LocalizationStatusSuccess = (text: string, scope: string): ILocalizationStatus => ({
    success: true,
    message: "Success",
    text,
    scope,
});

const LocalizationDictionaryStatusSuccess = (scope: string): IDictionaryError => ({
    scope,
    context: null,
});

class Localization {
    private mGlobalScope = "global";

    private mCultureCookieName: string = "Localization.Culture";
    private mCurrentCulture: string;

    private readonly mDictionary: { [key: string]: LocalizationDictionary } = {};
    private readonly mDictionaryQueue: { [key: string]: Array<(dictionary: LocalizationDictionary) => void> } = {};

    private mPluralizedDictionary: { [key: string]: LocalizationPluralizationDictionary } = {};
    private readonly mPluralizedDictionaryQueue: { [key: string]: Array<(dictionary: LocalizationPluralizationDictionary) => void> } = {};

    private readonly mLocalizationConfiguration: ILocalizationConfiguration;

    private mErrorHandlerCalled = false;

    public constructor(localizationConfiguration?: ILocalizationConfiguration) {
        this.mLocalizationConfiguration = localizationConfiguration === undefined || localizationConfiguration === null
            ? {
                errorResolution: LocalizationErrorResolution.Key,
            } : localizationConfiguration;
    }

    private callErrorHandler(errorStatus: ILocalizationError) {
        if (!this.mErrorHandlerCalled) {
            this.mErrorHandlerCalled = true;

            if (this.mLocalizationConfiguration.onError !== undefined) {
                this.mLocalizationConfiguration.onError(errorStatus);
            }
        }
    }

    private getTranslationOnError(text: string, scope: string): ILocalizedString | null {
        switch (this.mLocalizationConfiguration.errorResolution) {
            case LocalizationErrorResolution.Null:
                return null;

            case LocalizationErrorResolution.Key:
                return {
                    name: text,
                    resourceNotFound: true,
                    value: text,
                };
        }
    }

    /**
     * @deprecated Use translateAsync or getDictionaryAsync() => translate
     */
    public translate(
        text: string,
        scope?: string,
        cultureName?: string,
    ): ILocalizedString {
        const dictionary = this.getDictionary(scope, cultureName);

        return dictionary.translate(
            text,
            () => dictionary.getFallbackTranslation(text, scope, cultureName),
        );
    }

    public translateAsync(
        text: string,
        scope?: string,
        cultureName?: string,
    ): Promise<ILocalizationResult> {
        return new Promise((resolve, reject) => {
            this.getDictionaryAsync(scope, cultureName)
                .then((dictionaryResponse) => {
                    const result = dictionaryResponse.result.translate(
                        text,
                        () => dictionaryResponse.result.getFallbackTranslation(text, scope, cultureName),
                    );

                    resolve(
                        new LocalizationResult(
                            result,
                            LocalizationStatusSuccess(text, scope),
                        )
                    );
                }, (dictionaryResponse: ILocalizationDictionaryResult<LocalizationDictionary>) => {
                    const errorStatus = {
                        success: false,
                        message: "Unable to load required dictionary",
                        errorType: 'loadDictionary',
                        text,
                        scope: dictionaryResponse.status.scope,
                        context: dictionaryResponse.status.context,
                    };

                    this.callErrorHandler(errorStatus);

                    reject(
                        new LocalizationResult(
                            this.getTranslationOnError(text, scope),
                            LocalizationStatusSuccess(text, scope),
                        )
                    );
                });
        });
    }

    /**
     *@deprecated Use translateFormatAsync or getDictionaryAsync() => translateFormat
     */
    public translateFormat(
        text: string,
        parameters: string[],
        scope?: string,
        cultureName?: string,
    ): ILocalizedString {
        const dictionary = this.getDictionary(scope, cultureName);

        return dictionary.translateFormat(
            text,
            parameters,
            () => dictionary.getFallbackTranslation(text, scope, cultureName),
        );
    }

    public translateFormatAsync(
        text: string,
        parameters: string[],
        scope?: string,
        cultureName?: string,
    ): Promise<ILocalizationResult> {
        return new Promise((resolve, reject) => {
            this.getDictionaryAsync(scope, cultureName)
                .then((dictionaryResponse) => {
                    const result = dictionaryResponse.result.translateFormat(
                        text,
                        parameters,
                        () => dictionaryResponse.result.getFallbackTranslation(text, scope, cultureName),
                    );

                    resolve(
                        new LocalizationResult(
                            result,
                            LocalizationStatusSuccess(text, scope),
                        )
                    );
                }, (dictionaryResponse: ILocalizationDictionaryResult<LocalizationDictionary>) => {
                    const errorStatus = {
                        success: false,
                        message: "Unable to load required dictionary",
                        errorType: 'loadDictionary',
                        text,
                        scope: dictionaryResponse.status.scope,
                        context: dictionaryResponse.status.context,
                    };

                    this.callErrorHandler(errorStatus);

                    reject(
                        new LocalizationResult(
                            this.getTranslationOnError(text, scope),
                            LocalizationStatusSuccess(text, scope),
                        )
                    );
                });
        });
    }

    /**
     *@deprecated Use translatePluralizationAsync or getPluralizationDictionaryAsync() => translatePluralization
     */
    public translatePluralization(
        text: string,
        number: number,
        scope?: string,
        cultureName?: string,
    ): ILocalizedString {
        const dictionary = this.getPluralizationDictionary(scope, cultureName);

        try {
            return dictionary.translatePluralization(
                text,
                number,
                () => dictionary.getFallbackTranslation(text, scope, cultureName),
            );
        } catch (exception) {
            return this.handleError(exception, text);
        }
    }

    public translatePluralizationAsync(
        text: string,
        number: number,
        scope?: string,
        cultureName?: string,
    ): Promise<ILocalizationResult> {
        return new Promise((resolve, reject) => {
            this.getPluralizationDictionaryAsync(scope, cultureName)
                .then((dictionaryResponse) => {
                    try {
                        const result = dictionaryResponse.result.translatePluralization(
                            text,
                            number,
                            () => dictionaryResponse.result.getFallbackTranslation(text, scope, cultureName),
                        );

                        resolve(
                            new LocalizationResult(
                                result,
                                LocalizationStatusSuccess(text, scope),
                            )
                        );
                    } catch (exception) {
                        reject(
                            new LocalizationResult(
                                this.handleError(exception, text),
                                {
                                    success: false,
                                    message: exception.message,
                                    errorType: 'exception',
                                    text,
                                    scope,
                                },
                            )
                        );
                    }
                }, (dictionaryResponse: ILocalizationDictionaryResult<LocalizationDictionary>) => {
                    const errorStatus = {
                        success: false,
                        message: "Unable to load required pluralization dictionary",
                        errorType: 'loadDictionary',
                        text,
                        scope: dictionaryResponse.status.scope,
                        context: dictionaryResponse.status.context,
                    };

                    this.callErrorHandler(errorStatus);

                    reject(
                        new LocalizationResult(
                            this.getTranslationOnError(text, scope),
                            LocalizationStatusSuccess(text, scope),
                        )
                    );
                });
        });
    }

    private handleError(exception: Error, text: string) {
        console.error(exception.message);

        return {name: text, value: "X{error}", resourceNotFound: true};
    }

    public configureSiteUrl(siteUrl: string) {
        this.mLocalizationConfiguration.siteUrl = siteUrl;
    }

    /**
     *@deprecated Use getDictionaryAsync
     */
    private getDictionary(scope?: string, cultureName?: string): LocalizationDictionary {
        scope = this.checkScope(scope);
        cultureName = this.checkCultureName(cultureName);

        return this.getLocalizationDictionary(scope, cultureName);
    }

    public getDictionaryAsync(
        scope?: string,
        cultureName?: string,
    ): Promise<ILocalizationDictionaryResult<LocalizationDictionary>> {
        scope = this.checkScope(scope);
        cultureName = this.checkCultureName(cultureName);

        return new Promise((resolve, reject) => {
            this.getLocalizationDictionaryAsync(scope, cultureName, (dictionary: LocalizationDictionary) => {
                resolve(
                    {
                        result: dictionary,
                        status: LocalizationDictionaryStatusSuccess(scope),
                    }
                );
            }, (status: IDictionaryError) => {
                reject(
                    {
                        result: null,
                        status,
                    }
                );
            });
        });
    }

    /**
     *@deprecated Use getPluralizationDictionaryAsync
     */
    private getPluralizationDictionary(scope?: string, cultureName?: string): LocalizationPluralizationDictionary {
        scope = this.checkScope(scope);
        cultureName = this.checkCultureName(cultureName);

        return this.getPluralizationLocalizationDictionary(scope, cultureName);
    }

    public getPluralizationDictionaryAsync(
        scope?: string,
        cultureName?: string,
    ): Promise<ILocalizationDictionaryResult<LocalizationPluralizationDictionary>> {
        scope = this.checkScope(scope);
        cultureName = this.checkCultureName(cultureName);

        return new Promise((resolve, reject) => {
            this.getPluralizationLocalizationDictionaryAsync(scope, cultureName, (dictionary: LocalizationPluralizationDictionary) => {
                resolve(
                    {
                        result: dictionary,
                        status: LocalizationDictionaryStatusSuccess(scope),
                    }
                );
            }, (status: IDictionaryError) => {
                reject(
                    {
                        result: null,
                        status,
                    }
                );
            });
        });
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
        scope: string,
        cultureName: string,
        onSuccess: (dictionary: LocalizationDictionary) => void,
        onFailed: (scope: IDictionaryError) => void,
    ): void {
        const dictionaryKey = this.dictionaryKey(scope, cultureName);
        const dictionary = this.mDictionary[dictionaryKey];

        if (dictionary === undefined) {
            this.downloadDictionaryAsync(scope, cultureName, onSuccess, onFailed);
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
        scope: string,
        cultureName: string,
        onSuccess: (dictionary: LocalizationPluralizationDictionary) => void,
        onFailed: (scope: IDictionaryError) => void,
    ) {
        let dictionaryKey = this.dictionaryKey(scope, cultureName);
        let dictionary = this.mPluralizedDictionary[dictionaryKey];

        if (dictionary === undefined) {
            this.downloadPluralizedDictionaryAsync(scope, cultureName, onSuccess, onFailed);
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
        scope: string,
        cultureName: string,
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
        scope: string, cultureName: string,
        onSuccess: (dictionary: LocalizationDictionary) => void,
        onFailed: (scope: IDictionaryError) => void,
    ): void {
        /*
        this.getDownloadPromise(
            (response) => new LocalizationDictionary(response),
            this.mDictionary,
            "Dictionary",
            scope,
            cultureName
        );
        */

        const dictionaryKey = this.dictionaryKey(scope, cultureName);

        if (this.mDictionaryQueue[scope] === undefined) {
            this.mDictionaryQueue[scope] = [];

            const xmlHttpRequest = new XMLHttpRequest();

            xmlHttpRequest.onreadystatechange = () => {
                if (xmlHttpRequest.readyState !== XMLHttpRequest.DONE) {
                    return;
                }

                if (xmlHttpRequest.status === 200) {
                    let response = xmlHttpRequest.responseText;

                    if (this.mDictionary[dictionaryKey] === undefined) {
                        this.mDictionary[dictionaryKey] = new LocalizationDictionary(response);
                    }

                    this.processDictionaryQueue(scope, dictionaryKey);
                } else {
                    onFailed(
                        {
                            scope,
                            context: xmlHttpRequest,
                        }
                    );
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
        scope: string,
        cultureName: string,
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
        scope: string,
        cultureName: string,
        onSuccess: (dictionary: LocalizationPluralizationDictionary) => void,
        onFailed: (scope: IDictionaryError) => void,
    ): void {
        /*
        this.getDownloadPromise(
            (response) => new LocalizationPluralizationDictionary(response),
            this.mPluralizedDictionary,
            "PluralizedDictionary",
            scope,
            cultureName
        );
        */

        const dictionaryKey = this.dictionaryKey(scope, cultureName);

        if (this.mPluralizedDictionaryQueue[scope] === undefined) {
            this.mPluralizedDictionaryQueue[scope] = [];

            const xmlHttpRequest = new XMLHttpRequest();

            xmlHttpRequest.onreadystatechange = () => {
                if (xmlHttpRequest.readyState !== XMLHttpRequest.DONE) {
                    return;
                }

                if (xmlHttpRequest.status === 200) {
                    let response = xmlHttpRequest.responseText;

                    if (this.mPluralizedDictionary[dictionaryKey] === undefined) {
                        this.mPluralizedDictionary[dictionaryKey] = new LocalizationPluralizationDictionary(response);
                    }

                    this.processPluralizedDictionaryQueue(scope, dictionaryKey);
                } else {
                    onFailed(
                        {
                            scope,
                            context: xmlHttpRequest,
                        }
                    );
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

    /**
     * TODO Use in downloadPluralizedDictionaryAsync and downloadDictionaryAsync when synchronous methods are removed
     */
    private getDownloadPromise(
        dictionaryFactory: (response: string) => BaseLocalizationDictionary<IPluralizedString>,
        dictionaryCache: { [key: string]: BaseLocalizationDictionary<IPluralizedString> },
        path: string,
        scope: string,
        cultureName: string,
    ): Promise<BaseLocalizationDictionary<IPluralizedString>> {
        const dictionaryKey = this.dictionaryKey(scope, cultureName);

        return new Promise((resolve, reject) => {
            const xmlHttpRequest = new XMLHttpRequest();

            xmlHttpRequest.open(
                "GET",
                `${this.getBaseUrl()}/Localization/${path}?scope=${scope}`,
                true,
            );
            xmlHttpRequest.send();

            xmlHttpRequest.onreadystatechange = () => {
                if (xmlHttpRequest.readyState !== XMLHttpRequest.DONE) {
                    return;
                }

                if (
                    xmlHttpRequest.status >= 200 && xmlHttpRequest.status < 300
                ) {
                    let response = xmlHttpRequest.responseText;

                    if (dictionaryCache[dictionaryKey] === undefined) {
                        dictionaryCache[dictionaryKey] = dictionaryFactory(response);
                    }

                    resolve(dictionaryCache[dictionaryKey]);
                } else {
                    reject({
                        scope,
                        status: xmlHttpRequest.status,
                        statusText: xmlHttpRequest.statusText,
                        context: xmlHttpRequest,
                    });
                }
            };
        });
    }

    private getBaseUrl(): string {
        let baseUrl = this.mLocalizationConfiguration.siteUrl;
        if (baseUrl && baseUrl.charAt(baseUrl.length - 1) === "/") {
            baseUrl = baseUrl.substring(0, baseUrl.length - 1);
        }

        return baseUrl;
    }

    public getCurrentCulture(): string {
        if (this.mCurrentCulture === undefined) {
            const parsedCookieValue = this.getParsedCultureCookie();
            const currentCulture = parsedCookieValue.currentCulture === null
                ? parsedCookieValue.defaultCulture
                : parsedCookieValue.currentCulture;

            this.setCurrentCulture(currentCulture);
        }

        return this.mCurrentCulture;
    }

    private setCurrentCulture(culture: string) {
        this.mCurrentCulture = culture;
    }

    private getParsedCultureCookie(): ILocalizationCookie {
        const currentCultureCookieValue = this.getCurrentCultureCookie();
        const parsedCookieValue = JSON.parse(currentCultureCookieValue) as ILocalizationCookie;

        if (
            parsedCookieValue.defaultCulture === undefined
            || parsedCookieValue.currentCulture === undefined
        ) {
            console.error(
                `Unexpected value of the cookie ${this.mCultureCookieName}. Expected object with properties 'defaultCulture', and 'currentCulture'.`,
                parsedCookieValue
            );
        }

        return parsedCookieValue;
    }

    private getCurrentCultureCookie(): string {
        return LocalizationUtils.getCookie(this.mCultureCookieName);
    }
}

abstract class BaseLocalizationDictionary<TResponse> {
    protected readonly mDictionary: { [key: string]: TResponse };

    protected constructor(dictionary: string) {
        this.mDictionary = JSON.parse(dictionary);
    }

    public getFallbackTranslation(text: string, scope: string, cultureName: string): ILocalizedString {
        console.log(
            `Localized string with key=${text} was not found in dictionary=${scope} with culture=${cultureName}`,
        );

        return {name: text, value: "X{undefined}", resourceNotFound: true};
    }
}

class LocalizationDictionary extends BaseLocalizationDictionary<ILocalizedString> {
    constructor(dictionary: string) {
        super(dictionary);
    }

    public translate(text: string, fallback: () => ILocalizedString): ILocalizedString | null {
        const result = this.mDictionary[text];

        if (result === undefined) {
            return fallback();
        }

        return result;
    }

    public translateFormat(text: string, parameters: string[], fallback: () => ILocalizedString): ILocalizedString {
        const translation = this.translate(text, () => null);

        if (translation === null) {
            return fallback();
        }

        const formatedText = !parameters ? translation.value : this.formatString(translation, parameters);

        return {name: text, value: formatedText, resourceNotFound: translation.resourceNotFound};
    }

    private formatString(str: ILocalizedString, obj: string[]): string {
        return str.value.replace(/\{\s*([^}\s]+)\s*\}/g, (m, p1, offset, string) => obj[p1]);
    }
}

class LocalizationPluralizationDictionary extends BaseLocalizationDictionary<IPluralizedString> {
    constructor(dictionary: string) {
        super(dictionary);
    }

    public translatePluralization(text: string, number: number, fallback: () => ILocalizedString): ILocalizedString {
        const pluralizedString = this.mDictionary[text];

        if (pluralizedString === undefined || pluralizedString === null) {
            return fallback();
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

enum LocalizationErrorResolution {
    Null = 0,
    Key = 1,
}

interface ILocalizationConfiguration {
    errorResolution: LocalizationErrorResolution,
    siteUrl?: string,
    onError?: (localizationError: ILocalizationError) => void,
}

interface ILocalizationCookie {
    defaultCulture: string;
    currentCulture: string | null;
}

interface ILocalizationError {
    text: string;
    scope: string;
    message: string;
    errorType?: string;
    dictionary?: string;
    context?: object;
}

interface ILocalizationStatus {
    success: boolean;
    text: string;
    scope: string;
    message: string;
    errorType?: string;
    dictionary?: string;
    context?: object;
}

interface IDictionaryError {
    scope: string;
    context: object | null;
}

interface ILocalizationDictionaryResult<TDictionary> {
    result: TDictionary | null,
    status: IDictionaryError,
}

interface ILocalizationResult {
    result: ILocalizedString,
    status: ILocalizationStatus,

    toString(): string,
}

class LocalizationResult implements ILocalizationResult {
    public readonly result: ILocalizedString;
    public readonly status: ILocalizationStatus;

    constructor(result: ILocalizedString, status: ILocalizationStatus) {
        this.result = result;
        this.status = status;
    }

    public toString(): string {
        return this.result.value;
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
