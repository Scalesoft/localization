declare class Localization {
    private mGlobalScope;
    private mCultureCookieName;
    private mCurrentCulture;
    private mDownloading;
    private mDownloadingCulture;
    private mDownloadingScope;
    private mDictionary;
    private mSiteUrl;
    translate(text: string, scope?: string, cultureName?: string): ILocalizedString;
    translateFormat(text: string, parameters: string[], scope?: string, cultureName?: string): ILocalizedString;
    private getFallbackTranslation;
    configureSiteUrl(siteUrl: string): void;
    private getDictionary;
    private checkCultureName;
    private checkScope;
    private getLocalizationDictionary;
    private dictionaryKey;
    private downloadDictionary;
    getCurrentCulture(): string;
    private setCurrentCulture;
    private getCurrentCultureCookie;
    private markDownloading;
}
declare class LocalizationDictionary {
    private mDictionary;
    constructor(dictionary: string);
    translate(text: string): ILocalizedString;
    translateFormat(text: string, parameters: string[]): ILocalizedString;
    private formatString;
}
interface ILocalizedString {
    name: string;
    resourceNotFound: boolean;
    value: string;
}
declare class LocalizationUtils {
    static getCookie(name: string): string;
}
