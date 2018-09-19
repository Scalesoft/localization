declare class Localization {
    private mGlobalScope;
    private mCultureCookieName;
    private mCurrentCulture;
    private mDownloading;
    private mDownloadingCulture;
    private mDownloadingScope;
    private mDictionary;
    private mSiteUrl;
    translate(text: string, scope?: string, cultureName?: string): string;
    translateFormat(text: string, parameters: string[], scope?: string, cultureName?: string): string;
    configureSiteUrl(siteUrl: string): void;
    private getDictionary(scope?, cultureName?);
    private checkCultureName(cultureName?);
    private checkScope(scope?);
    private getLocalizationDictionary(scope, cultureName);
    private dictionaryKey(scope, cultureName);
    private downloadDictionary(scope, cultureName);
    private getCurrentCulture();
    private setCurrentCulture(culture);
    private getCurrentCultureCookie();
    private markDownloading(downloading, scope, cultureName);
}
declare class LocalizationDictionary {
    private mDictionary;
    constructor(dictionary: string);
    translate(text: string): string;
    translateFormat(text: string, parameters: string[]): string;
    private formatString(str, obj);
}
declare class LocalizationUtils {
    static getCookie(name: string): string;
}
