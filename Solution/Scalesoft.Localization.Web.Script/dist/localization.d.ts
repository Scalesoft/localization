declare const LocalizationStatusSuccess: (text: string, scope: string) => ILocalizationStatus;
declare class Localization {
    private mGlobalScope;
    private mCultureCookieName;
    private mCurrentCulture;
    private readonly mDictionary;
    private readonly mDictionaryQueue;
    private mPluralizedDictionary;
    private readonly mPluralizedDictionaryQueue;
    private mSiteUrl;
    private readonly mLocalizationConfiguration;
    private mErrorHandlerCalled;
    constructor(localizationConfiguration?: ILocalizationConfiguration);
    private callErrorHandler;
    private getTranslationOnError;
    /**
     * @deprecated Use translateAsync
     */
    translate(text: string, scope?: string, cultureName?: string): ILocalizedString;
    translateAsync(text: string, scope?: string, cultureName?: string): Promise<ILocalizationResult>;
    /**
     *@deprecated Use translateFormatAsync
     */
    translateFormat(text: string, parameters: string[], scope?: string, cultureName?: string): ILocalizedString;
    translateFormatAsync(text: string, parameters: string[], scope?: string, cultureName?: string): Promise<ILocalizationResult>;
    /**
     *@deprecated Use translatePluralizationAsync
     */
    translatePluralization(text: string, number: number, scope?: string, cultureName?: string): ILocalizedString;
    translatePluralizationAsync(text: string, number: number, scope?: string, cultureName?: string): Promise<ILocalizationResult>;
    private getFallbackTranslation;
    private handleError;
    configureSiteUrl(siteUrl: string): void;
    /**
     *@deprecated Use getDictionaryAsync
     */
    private getDictionary;
    private getDictionaryAsync;
    /**
     *@deprecated Use getPluralizationDictionaryAsync
     */
    private getPluralizationDictionary;
    private getPluralizationDictionaryAsync;
    private checkCultureName;
    private checkScope;
    /**
     *@deprecated Use getLocalizationDictionaryAsync
     */
    private getLocalizationDictionary;
    private getLocalizationDictionaryAsync;
    /**
     *@deprecated Use getPluralizationLocalizationDictionaryAsync
     */
    private getPluralizationLocalizationDictionary;
    private getPluralizationLocalizationDictionaryAsync;
    private dictionaryKey;
    /**
     *@deprecated Use downloadDictionaryAsync
     */
    private downloadDictionary;
    private downloadDictionaryAsync;
    private processDictionaryQueue;
    /**
     * @deprecated Use downloadPluralizedDictionaryAsync
     */
    private downloadPluralizedDictionary;
    private downloadPluralizedDictionaryAsync;
    private processPluralizedDictionaryQueue;
    private getDownloadPromise;
    private getBaseUrl;
    getCurrentCulture(): string;
    private setCurrentCulture;
    private getParsedCultureCookie;
    private getCurrentCultureCookie;
}
declare class LocalizationDictionary {
    private readonly mDictionary;
    constructor(dictionary: string);
    translate(text: string): ILocalizedString;
    translateFormat(text: string, parameters: string[]): ILocalizedString;
    private formatString;
}
declare class LocalizationPluralizationDictionary {
    private readonly mDictionary;
    constructor(dictionary: string);
    translatePluralization(text: string, number: number): ILocalizedString;
}
declare enum LocalizationErrorResolution {
    Null = 0,
    Key = 1
}
interface ILocalizationConfiguration {
    errorResolution: LocalizationErrorResolution;
    onError?: (localizationError: ILocalizationError) => void;
}
interface ILocalizationCookie {
    DefaultCulture: string;
    CurrentCulture: string | null;
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
    context: object;
}
interface ILocalizationResult {
    result: ILocalizedString;
    status: ILocalizationStatus;
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
declare class PluralizationInterval {
    readonly start: number;
    readonly end: number;
    constructor(start: number, end: number);
}
declare class LocalizationUtils {
    static getCookie(name: string): string;
    static isInInterval(value: number, interval: PluralizationInterval): boolean;
}
