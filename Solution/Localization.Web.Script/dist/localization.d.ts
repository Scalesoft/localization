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
    private getFallbackTranslation(text, scope, cultureName);
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
    translate(text: string): ILocalizedString;
    translateFormat(text: string, parameters: string[]): ILocalizedString;
    private formatString(str, obj);
}
interface ILocalizedString {
    name: string;
    resourceNotFound: boolean;
    value: string;
}
declare class LocalizationUtils {
    static getCookie(name: string): string;
}
