export declare const LocalizationStatusSuccess: (text: string, scope: string) => ILocalizationStatus;
export declare const LocalizationDictionaryStatusSuccess: (scope: string) => IDictionaryError;
export declare class Localization {
    private mGlobalScope;
    private mCultureCookieName;
    private mCurrentCulture;
    private readonly mDictionary;
    private readonly mDictionaryQueue;
    private mPluralizedDictionary;
    private readonly mPluralizedDictionaryQueue;
    private readonly mLocalizationConfiguration;
    private mErrorHandlerCalled;
    constructor(localizationConfiguration?: ILocalizationConfiguration);
    private callErrorHandler;
    private getTranslationOnError;
    /**
     * @deprecated Use translateAsync or getDictionaryAsync() => translate
     */
    translate(text: string, scope?: string, cultureName?: string): ILocalizedString;
    translateAsync(text: string, scope?: string, cultureName?: string): Promise<ILocalizationResult>;
    /**
     *@deprecated Use translateFormatAsync or getDictionaryAsync() => translateFormat
     */
    translateFormat(text: string, parameters: string[], scope?: string, cultureName?: string): ILocalizedString;
    translateFormatAsync(text: string, parameters: string[], scope?: string, cultureName?: string): Promise<ILocalizationResult>;
    /**
     *@deprecated Use translatePluralizationAsync or getPluralizationDictionaryAsync() => translatePluralization
     */
    translatePluralization(text: string, number: number, scope?: string, cultureName?: string): ILocalizedString;
    translatePluralizationAsync(text: string, number: number, scope?: string, cultureName?: string): Promise<ILocalizationResult>;
    private handleError;
    configureSiteUrl(siteUrl: string): void;
    /**
     *@deprecated Use getDictionaryAsync
     */
    private getDictionary;
    getDictionaryAsync(scope?: string, cultureName?: string): Promise<ILocalizationDictionaryResult<LocalizationDictionary>>;
    /**
     *@deprecated Use getPluralizationDictionaryAsync
     */
    private getPluralizationDictionary;
    getPluralizationDictionaryAsync(scope?: string, cultureName?: string): Promise<ILocalizationDictionaryResult<LocalizationPluralizationDictionary>>;
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
    /**
     * TODO Use in downloadPluralizedDictionaryAsync and downloadDictionaryAsync when synchronous methods are removed
     */
    private getDownloadPromise;
    private getBaseUrl;
    /**
     * @deprecated Use getCurrentCultureAsync
     */
    getCurrentCulture(): string;
    getCurrentCultureAsync(): Promise<string>;
    private setCurrentCulture;
    private getParsedCultureCookie;
    private getCurrentCultureCookie;
}
export declare abstract class BaseLocalizationDictionary<TResponse> {
    protected readonly mDictionary: {
        [key: string]: TResponse;
    };
    private readonly mScope;
    protected constructor(dictionary: string, scope: string);
    getFallbackTranslation(text: string, scope: string, cultureName: string): ILocalizedString;
    get scope(): string;
}
export declare class LocalizationDictionary extends BaseLocalizationDictionary<ILocalizedString> {
    constructor(dictionary: string, scope: string);
    translate(text: string, fallback: () => ILocalizedString): ILocalizedString | null;
    translateFormat(text: string, parameters: string[], fallback: () => ILocalizedString): ILocalizedString;
    private formatString;
}
export declare class LocalizationPluralizationDictionary extends BaseLocalizationDictionary<IPluralizedString> {
    constructor(dictionary: string, scope: string);
    translatePluralization(text: string, number: number, fallback: () => ILocalizedString): ILocalizedString;
}
export declare enum LocalizationErrorResolution {
    Null = 0,
    Key = 1
}
export interface ILocalizationConfiguration {
    errorResolution: LocalizationErrorResolution;
    siteUrl?: string;
    onError?: (localizationError: ILocalizationError) => void;
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
    result: TDictionary | null;
    status: IDictionaryError;
}
export interface ILocalizationResult {
    result: ILocalizedString;
    status: ILocalizationStatus;
    toString(): string;
}
export declare class LocalizationResult implements ILocalizationResult {
    readonly result: ILocalizedString;
    readonly status: ILocalizationStatus;
    constructor(result: ILocalizedString, status: ILocalizationStatus);
    toString(): string;
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
export declare class PluralizationInterval {
    readonly start: number;
    readonly end: number;
    constructor(start: number, end: number);
}
export declare class LocalizationUtils {
    static getCookie(name: string): string;
    static isInInterval(value: number, interval: PluralizationInterval): boolean;
}
