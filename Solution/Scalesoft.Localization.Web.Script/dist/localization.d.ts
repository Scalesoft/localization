declare class Localization {
    private mGlobalScope;
    private mCultureCookieName;
    private mCurrentCulture;
    private readonly mDictionary;
    private readonly mDictionaryQueue;
    private mPluralizedDictionary;
    private readonly mPluralizedDictionaryQueue;
    private mSiteUrl;
    /**
     * @deprecated Use translateAsync
     */
    translate(text: string, scope?: string, cultureName?: string): ILocalizedString;
    translateAsync(onSuccess: (translation: ILocalizedString) => void, text: string, scope?: string, cultureName?: string): void;
    /**
     *@deprecated Use translateFormatAsync
     */
    translateFormat(text: string, parameters: string[], scope?: string, cultureName?: string): ILocalizedString;
    translateFormatAsync(onSuccess: (translation: ILocalizedString) => void, text: string, parameters: string[], scope?: string, cultureName?: string): void;
    /**
     *@deprecated Use translatePluralizationAsync
     */
    translatePluralization(text: string, number: number, scope?: string, cultureName?: string): ILocalizedString;
    translatePluralizationAsync(onSuccess: (translation: ILocalizedString) => void, text: string, number: number, scope?: string, cultureName?: string): void;
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
    private getBaseUrl;
    private checkAndGetCurrentCulture;
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
interface ILocalizedString {
    name: string;
    resourceNotFound: boolean;
    value: string;
}
interface ILocalizationCookie {
    defaultCulture: string;
    currentCulture: string;
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
