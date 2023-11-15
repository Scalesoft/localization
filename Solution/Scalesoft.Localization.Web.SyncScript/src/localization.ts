export const LocalizationStatusSuccess = (text: string, scope: string): ILocalizationStatus => ({
    success: true,
    message: "Success",
    text,
    scope,
});

export const LocalizationDictionaryStatusSuccess = (scope: string): IDictionaryError => ({
    scope,
    context: null,
});

export class Localization {
    private mGlobalScope = "global";

    private mCultureCookieName: string = "Localization.Culture";
    private mCurrentCulture: string;

    private mDictionary: ILocalizationDictionary | null = null;

    private readonly mLocalizationConfiguration: ILocalizationConfiguration;

    private mErrorHandlerCalled = false;

    public constructor(localizationConfiguration?: ILocalizationConfiguration) {
        this.mLocalizationConfiguration = localizationConfiguration === undefined || localizationConfiguration === null
            ? {
                errorResolution: LocalizationErrorResolution.Key,
            } : localizationConfiguration;
    }

    public setDictionaries(dictionaries: ILocalizationDictionary): void {
        this.mDictionary = dictionaries;
    }

    public translate(
        text: string,
        scope?: string,
        cultureName?: string,
    ): ILocalizedString {

        const usedCulture = this.checkCultureName(cultureName);
        const usedScope = this.checkScope(scope);
        
        const cultureDictionary = this.mDictionary[usedCulture];
        const scopeDictionary = cultureDictionary[usedScope];
        const messagesDictionary = scopeDictionary.messages;
        
        
        return messagesDictionary[text] ?? this.getFallbackTranslation(text, usedScope, usedCulture);
    }


    public translateFormat(
        text: string,
        parameters: string[],
        scope?: string,
        cultureName?: string,
    ): ILocalizedString {

        return this.mDictionary.translateFormat(
            text,
            parameters,
            () => this.mDictionary.getFallbackTranslation(text, scope, cultureName),
        );
    }

    public translatePluralization(
        text: string,
        number: number,
        scope?: string,
        cultureName?: string,
    ): ILocalizedString {

        try {
            return this.mDictionary.translatePluralization(
                text,
                number,
                () => this.mDictionary.getFallbackTranslation(text, scope, cultureName),
            );
        } catch (exception) {
            return this.handleError(exception, text);
        }
    }

    private handleError(exception: Error, text: string) {
        console.error(exception.message);

        return {name: text, value: "X{error}", resourceNotFound: true};
    }

    public configureSiteUrl(siteUrl: string) {
        this.mLocalizationConfiguration.siteUrl = siteUrl;
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
            if (!parsedCookieValue) {
                const xmlHttpRequest = new XMLHttpRequest();

                xmlHttpRequest.onreadystatechange = () => {
                    if (
                        xmlHttpRequest.readyState === XMLHttpRequest.DONE
                        && xmlHttpRequest.status === 200
                    ) {
                        let response = xmlHttpRequest.responseText;

                        this.setCurrentCulture(response);
                    }
                };

                xmlHttpRequest.open(
                    "GET",
                    `${this.getBaseUrl()}/Localization/CurrentCulture`,
                    false,
                );
                xmlHttpRequest.send();
            } else {
                this.setCurrentCulture(parsedCookieValue.currentCulture);
            }
        }

        return this.mCurrentCulture;
    }

    private setCurrentCulture(culture: string) {
        this.mCurrentCulture = culture;
    }

    private getParsedCultureCookie(): ILocalizationCookie {
        const currentCultureCookieValue = this.getCurrentCultureCookie();
        if (!currentCultureCookieValue) {
            return null;
        }

        const parsedCookieValue = JSON.parse(currentCultureCookieValue) as ILocalizationCookie;

        if (
            parsedCookieValue.currentCulture === undefined
        ) {
            console.error(
                `Unexpected value of the cookie ${this.mCultureCookieName}. Expected object with property 'currentCulture'.`,
                parsedCookieValue,
            );
        }

        return parsedCookieValue;
    }

    private getCurrentCultureCookie(): string {
        return LocalizationUtils.getCookie(this.mCultureCookieName);
    }

    public getFallbackTranslation(text: string, scope: string, cultureName: string): ILocalizedString {
        console.log(
            `Localized string with key=${text} was not found in dictionary=${scope} with culture=${cultureName}`,
        );

        return {name: text, value: "X{undefined}", resourceNotFound: true};
    }

}

declare type  ILocalizationDictionary = Record<string, ILocaleDictionary>;
declare type  ILocaleDictionary = Record<string, IScopeDictionary>;
export interface IScopeDictionary {
    messages: Record<string, ILocalizedString>;    
    pluralization: Record<string, IPluralizedString>;    
}

export abstract class BaseLocalizationDictionary<TResponse> {
    protected readonly mDictionary: ILocalizationDictionary;

    protected constructor(dictionary: string, scope: string) {
        this.mDictionary = JSON.parse(dictionary);
    }

    public getFallbackTranslation(text: string, scope: string, cultureName: string): ILocalizedString {
        console.log(
            `Localized string with key=${text} was not found in dictionary=${scope} with culture=${cultureName}`,
        );

        return {name: text, value: "X{undefined}", resourceNotFound: true};
    }
    
}

export class LocalizationDictionary extends BaseLocalizationDictionary<ILocalizedString> {
    constructor(dictionary: string, scope: string) {
        super(dictionary, scope);
    }

    public translate(text: string, scope: string, cultureName: string, fallback: () => ILocalizedString): ILocalizedString | null {
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

export enum LocalizationErrorResolution {
    Null = 0,
    Key = 1,
}

export interface ILocalizationConfiguration {
    errorResolution: LocalizationErrorResolution,
    siteUrl?: string,
    onError?: (localizationError: ILocalizationError) => void,
}

export interface ILocalizationCookie {
    currentCulture: string;
}

export interface ILocalizationError {
    text: string;
    scope: string;
    message: string;
    errorType?: string;
    dictionary?: string;
    context?: object;
}

export interface ILocalizationStatus {
    success: boolean;
    text: string;
    scope: string;
    message: string;
    errorType?: string;
    dictionary?: string;
    context?: object;
}

export interface IDictionaryError {
    scope: string;
    context: object | null;
}

export interface ILocalizationDictionaryResult<TDictionary> {
    result: TDictionary | null,
    status: IDictionaryError,
}

export interface ILocalizationResult {
    result: ILocalizedString,
    status: ILocalizationStatus,

    toString(): string,
}

export class LocalizationResult implements ILocalizationResult {
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

export interface ILocalizedString {
    name: string;
    resourceNotFound: boolean;
    value: string;
}

export interface IPluralizedString {
    intervals: IIntervalWithTranslation[];
    defaultLocalizedString: ILocalizedString;
}

export interface IIntervalWithTranslation {
    interval: PluralizationInterval;
    localizedString: ILocalizedString;
}

export class PluralizationInterval {
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

export class LocalizationUtils {

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
