class Localization {
    private mCultureCookieName = "Localization.Culture";
    private mCurrentCulture = "";

    private mDownloading: boolean;
    private mDownloadingCulture = "";
    private mDownloadingScope = "";

    public getDictionary(scope: string, cultureName: string){
        this.markDownloading(true, scope, cultureName);

        let xmlHttpRequest = new XMLHttpRequest();
       // xmlHttpRequest.setRequestHeader("","");

        xmlHttpRequest.onreadystatechange = () => {
            if (xmlHttpRequest.readyState === XMLHttpRequest.DONE) {
                if (xmlHttpRequest.status === 200) {
                    let response = xmlHttpRequest.responseText;

                    //                   
                }

                this.markDownloading(false, scope, cultureName);
            }
        }

        xmlHttpRequest.open("GET", `$`, false);
        xmlHttpRequest.send();
    }

    public getCurrentCulture(): string {
        if (this.mCurrentCulture === "") {
            const currentCulture = this.getCurrentCultureCookie();
            this.setCurrentCulture(currentCulture);
        }

        return this.mCurrentCulture;
    }

    public setCurrentCulture(culture: string) {
        this.mCurrentCulture = culture;
    }

    private getCurrentCultureCookie(): string {
        return getCookie(this.mCultureCookieName);
    }

    private markDownloading(downloading: boolean, scope: string, cultureName: string) {       
        if (downloading) {
            this.mDownloadingScope = scope;
            this.mDownloadingCulture = cultureName;
        } else {
            this.mDownloadingScope = "";
            this.mDownloadingCulture = "";
        }

        this.mDownloading = downloading;
    }
}

class LocalizationDictionary {
    private mDictionary;

    constructor(dictionary: string) {
        this.mDictionary = JSON.parse(dictionary);
    }

    public translate(text: string): string {
        let result = this.mDictionary[text];
        if (typeof result === "undefined") {
            return null;
        }

        return result;
    }
}