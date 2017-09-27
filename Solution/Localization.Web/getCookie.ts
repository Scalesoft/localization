

function getCookie(name: string): string {
    const cultureCookieNameLenghtPlus = this.mCultureCookieName.length + 1;
    return document.cookie
        .split(";")
        .map(c => c.trim())
        .filter(cookie => {
            return cookie.substring(0, cultureCookieNameLenghtPlus) === `${name}`;
        })
        .map(cookie => {
            return decodeURIComponent(cookie.substring(cultureCookieNameLenghtPlus));
        })[0] ||
        null;
}