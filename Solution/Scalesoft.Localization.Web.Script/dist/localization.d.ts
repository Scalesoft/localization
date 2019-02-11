declare class Localization {
    private mGlobalScope;
    private mCultureCookieName;
    private mCurrentCulture;
    private mDictionary;
    private mPluralizedDictionary;
    private mSiteUrl;
    translate(text: string, scope?: string, cultureName?: string): ILocalizedString;
    translateFormat(text: string, parameters: string[], scope?: string, cultureName?: string): ILocalizedString;
    translatePluralization(text: string, number: number, scope?: string, cultureName?: string): ILocalizedString;
    private getFallbackTranslation;
    private handleError;
    configureSiteUrl(siteUrl: string): void;
    private getDictionary;
    private getPluralizationDictionary;
    private checkCultureName;
    private checkScope;
    private getLocalizationDictionary;
    private getPluralizationLocalizationDictionary;
    private dictionaryKey;
    private downloadDictionary;
    private downloadPluralizedDictionary;
    getCurrentCulture(): string;
    private setCurrentCulture;
    private getCurrentCultureCookie;
}
declare class LocalizationDictionary {
    private mDictionary;
    constructor(dictionary: string);
    translate(text: string): ILocalizedString;
    translateFormat(text: string, parameters: string[]): ILocalizedString;
    private formatString;
}
declare class LocalizationPluralizationDictionary {
    private mDictionary;
    constructor(dictionary: string);
    translatePluralization(text: string, number: number): ILocalizedString;
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
declare class PluralizationInterval {
    readonly start: number;
    readonly end: number;
    constructor(start: number, end: number);
    isOverlaping(obj: PluralizationInterval): boolean;
    isInInterval(obj: PluralizationInterval): boolean;
}
declare class LocalizationUtils {
    static getCookie(name: string): string;
}
