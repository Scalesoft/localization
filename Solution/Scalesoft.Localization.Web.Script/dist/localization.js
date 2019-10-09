var Localization = /** @class */ (function () {
    function Localization() {
        this.mGlobalScope = "global";
        this.mCultureCookieName = "Localization.Culture";
        this.mDictionary = {};
        this.mPluralizedDictionary = {};
    }
    Localization.prototype.translate = function (text, scope, cultureName) {
        var dictionary = this.getDictionary(scope, cultureName);
        var result = dictionary.translate(text);
        if (result == null) {
            return this.getFallbackTranslation(text, scope, cultureName);
        }
        return result;
    };
    Localization.prototype.translateAsync = function (onSuccess, text, scope, cultureName) {
        var _this = this;
        this.getDictionaryAsync(function (dictionary) {
            var result = dictionary.translate(text);
            if (result == null) {
                return _this.getFallbackTranslation(text, scope, cultureName);
            }
            onSuccess(result);
        }, scope, cultureName);
    };
    Localization.prototype.translateFormat = function (text, parameters, scope, cultureName) {
        var dictionary = this.getDictionary(scope, cultureName);
        var result = dictionary.translateFormat(text, parameters);
        if (result == null) {
            return this.getFallbackTranslation(text, scope, cultureName);
        }
        return result;
    };
    Localization.prototype.translateFormatAsync = function (onSuccess, text, parameters, scope, cultureName) {
        var _this = this;
        this.getDictionaryAsync(function (dictionary) {
            var result = dictionary.translateFormat(text, parameters);
            if (result == null) {
                return _this.getFallbackTranslation(text, scope, cultureName);
            }
            onSuccess(result);
        }, scope, cultureName);
    };
    Localization.prototype.translatePluralization = function (text, number, scope, cultureName) {
        var dictionary = this.getPluralizationDictionary(scope, cultureName);
        try {
            var result = dictionary.translatePluralization(text, number);
            if (result == null) {
                return this.getFallbackTranslation(text, scope, cultureName);
            }
            return result;
        }
        catch (exception) {
            return this.handleError(exception, text);
        }
    };
    Localization.prototype.translatePluralizationAsync = function (onSuccess, text, number, scope, cultureName) {
        var _this = this;
        this.getPluralizationDictionaryAsync(function (dictionary) {
            try {
                var result = dictionary.translatePluralization(text, number);
                if (result == null) {
                    return _this.getFallbackTranslation(text, scope, cultureName);
                }
                onSuccess(result);
            }
            catch (exception) {
                onSuccess(_this.handleError(exception, text));
            }
        }, scope, cultureName);
    };
    Localization.prototype.getFallbackTranslation = function (text, scope, cultureName) {
        console.log("Localized string with key=" + text + " was not found in dictionary=" + scope + " with culture=" + cultureName);
        return { name: text, value: "X{undefined}", resourceNotFound: true };
    };
    Localization.prototype.handleError = function (exception, text) {
        console.error(exception.message);
        return { name: text, value: "X{error}", resourceNotFound: true };
    };
    Localization.prototype.configureSiteUrl = function (siteUrl) {
        this.mSiteUrl = siteUrl;
    };
    Localization.prototype.getDictionary = function (scope, cultureName) {
        scope = this.checkScope(scope);
        cultureName = this.checkCultureName(cultureName);
        return this.getLocalizationDictionary(scope, cultureName);
    };
    Localization.prototype.getDictionaryAsync = function (onSuccess, scope, cultureName) {
        scope = this.checkScope(scope);
        cultureName = this.checkCultureName(cultureName);
        return this.getLocalizationDictionaryAsync(scope, cultureName, onSuccess);
    };
    Localization.prototype.getPluralizationDictionary = function (scope, cultureName) {
        scope = this.checkScope(scope);
        cultureName = this.checkCultureName(cultureName);
        return this.getPluralizationLocalizationDictionary(scope, cultureName);
    };
    Localization.prototype.getPluralizationDictionaryAsync = function (onSuccess, scope, cultureName) {
        scope = this.checkScope(scope);
        cultureName = this.checkCultureName(cultureName);
        return this.getPluralizationLocalizationDictionaryAsync(scope, cultureName, onSuccess);
    };
    Localization.prototype.checkCultureName = function (cultureName) {
        if (cultureName) {
            return cultureName;
        }
        return this.getCurrentCulture();
    };
    Localization.prototype.checkScope = function (scope) {
        if (scope) {
            return scope;
        }
        return this.mGlobalScope;
    };
    Localization.prototype.getLocalizationDictionary = function (scope, cultureName) {
        var dictionaryKey = this.dictionaryKey(scope, cultureName);
        var dictionary = this.mDictionary[dictionaryKey];
        if (typeof dictionary === "undefined") {
            this.downloadDictionary(scope, cultureName);
            return this.mDictionary[dictionaryKey];
        }
        return dictionary;
    };
    Localization.prototype.getLocalizationDictionaryAsync = function (scope, cultureName, onSuccess) {
        var dictionaryKey = this.dictionaryKey(scope, cultureName);
        var dictionary = this.mDictionary[dictionaryKey];
        if (typeof dictionary === "undefined") {
            this.downloadDictionary(scope, cultureName, onSuccess);
        }
        onSuccess(dictionary);
    };
    Localization.prototype.getPluralizationLocalizationDictionary = function (scope, cultureName) {
        var dictionaryKey = this.dictionaryKey(scope, cultureName);
        var dictionary = this.mPluralizedDictionary[dictionaryKey];
        if (typeof dictionary === "undefined") {
            this.downloadPluralizedDictionary(scope, cultureName);
            return this.mPluralizedDictionary[dictionaryKey];
        }
        return dictionary;
    };
    Localization.prototype.getPluralizationLocalizationDictionaryAsync = function (scope, cultureName, onSuccess) {
        var dictionaryKey = this.dictionaryKey(scope, cultureName);
        var dictionary = this.mPluralizedDictionary[dictionaryKey];
        if (typeof dictionary === "undefined") {
            this.downloadPluralizedDictionary(scope, cultureName, onSuccess);
        }
        onSuccess(dictionary);
    };
    Localization.prototype.dictionaryKey = function (scope, cultureName) {
        return scope.concat("|", cultureName);
    };
    Localization.prototype.downloadDictionary = function (scope, cultureName, onSuccess) {
        var _this = this;
        var xmlHttpRequest = new XMLHttpRequest();
        xmlHttpRequest.onreadystatechange = function () {
            if (xmlHttpRequest.readyState === XMLHttpRequest.DONE) {
                if (xmlHttpRequest.status === 200) {
                    var response = xmlHttpRequest.responseText;
                    var dictionaryKey = _this.dictionaryKey(scope, cultureName);
                    if (_this.mDictionary[dictionaryKey] === undefined) {
                        _this.mDictionary[dictionaryKey] = new LocalizationDictionary(response);
                    }
                    if (onSuccess !== undefined) {
                        onSuccess(_this.mDictionary[dictionaryKey]);
                    }
                }
            }
        };
        var baseUrl = this.mSiteUrl;
        if (baseUrl && baseUrl.charAt(baseUrl.length - 1) === "/") {
            baseUrl = baseUrl.substring(0, baseUrl.length - 1);
        }
        xmlHttpRequest.open("GET", baseUrl + "/Localization/Dictionary?scope=" + scope, onSuccess !== undefined);
        xmlHttpRequest.send();
    };
    Localization.prototype.downloadPluralizedDictionary = function (scope, cultureName, onSuccess) {
        var _this = this;
        var xmlHttpRequest = new XMLHttpRequest();
        xmlHttpRequest.onreadystatechange = function () {
            if (xmlHttpRequest.readyState === XMLHttpRequest.DONE) {
                if (xmlHttpRequest.status === 200) {
                    var response = xmlHttpRequest.responseText;
                    var dictionaryKey = _this.dictionaryKey(scope, cultureName);
                    if (_this.mPluralizedDictionary[dictionaryKey] === undefined) {
                        _this.mPluralizedDictionary[dictionaryKey] = new LocalizationPluralizationDictionary(response);
                    }
                    if (onSuccess !== undefined) {
                        onSuccess(_this.mPluralizedDictionary[dictionaryKey]);
                    }
                }
            }
        };
        var baseUrl = this.mSiteUrl;
        if (baseUrl && baseUrl.charAt(baseUrl.length - 1) === "/") {
            baseUrl = baseUrl.substring(0, baseUrl.length - 1);
        }
        xmlHttpRequest.open("GET", baseUrl + "/Localization/PluralizedDictionary?scope=" + scope, onSuccess !== undefined);
        xmlHttpRequest.send();
    };
    Localization.prototype.getCurrentCulture = function () {
        if (typeof this.mCurrentCulture === "undefined") {
            var currentCulture = this.getCurrentCultureCookie();
            this.setCurrentCulture(currentCulture);
        }
        return this.mCurrentCulture;
    };
    Localization.prototype.setCurrentCulture = function (culture) {
        this.mCurrentCulture = culture;
    };
    Localization.prototype.getCurrentCultureCookie = function () {
        return LocalizationUtils.getCookie(this.mCultureCookieName);
    };
    return Localization;
}());
var LocalizationDictionary = /** @class */ (function () {
    function LocalizationDictionary(dictionary) {
        this.mDictionary = JSON.parse(dictionary);
    }
    LocalizationDictionary.prototype.translate = function (text) {
        var result = this.mDictionary[text];
        if (typeof result === "undefined") {
            return null;
        }
        return result;
    };
    LocalizationDictionary.prototype.translateFormat = function (text, parameters) {
        var translation = this.translate(text);
        var formatedText = !parameters ? translation.value : this.formatString(translation, parameters);
        return { name: text, value: formatedText, resourceNotFound: translation.resourceNotFound };
    };
    LocalizationDictionary.prototype.formatString = function (str, obj) {
        return str.value.replace(/\{\s*([^}\s]+)\s*\}/g, function (m, p1, offset, string) { return obj[p1]; });
    };
    return LocalizationDictionary;
}());
var LocalizationPluralizationDictionary = /** @class */ (function () {
    function LocalizationPluralizationDictionary(dictionary) {
        this.mDictionary = JSON.parse(dictionary);
    }
    LocalizationPluralizationDictionary.prototype.translatePluralization = function (text, number) {
        var pluralizedString = this.mDictionary[text];
        if (typeof pluralizedString === "undefined" || pluralizedString === null) {
            return null;
        }
        for (var _i = 0, _a = pluralizedString.intervals; _i < _a.length; _i++) {
            var interval = _a[_i];
            var translationInterval = interval.interval;
            if (LocalizationUtils.isInInterval(number, translationInterval)) {
                return interval.localizedString;
            }
        }
        return pluralizedString.defaultLocalizedString;
    };
    return LocalizationPluralizationDictionary;
}());
var PluralizationInterval = /** @class */ (function () {
    function PluralizationInterval(start, end) {
        if (start > end) {
            var intervalErrorMsg = "The start value should be less or equal than end.";
            throw new Error(intervalErrorMsg);
        }
        this.start = start;
        this.end = end;
    }
    return PluralizationInterval;
}());
var LocalizationUtils = /** @class */ (function () {
    function LocalizationUtils() {
    }
    LocalizationUtils.getCookie = function (name) {
        name = name + "=";
        return document.cookie
            .split(";")
            .map(function (c) { return c.trim(); })
            .filter(function (cookie) { return cookie.indexOf(name) === 0; })
            .map(function (cookie) { return decodeURIComponent(cookie.substring(name.length)); })[0]
            || null;
    };
    /*
     * Returns true when value is in the pluralization interval
     */
    LocalizationUtils.isInInterval = function (value, interval) {
        if (!interval) {
            return false;
        }
        return interval.start <= value && value <= interval.end;
    };
    return LocalizationUtils;
}());

//# sourceMappingURL=localization.js.map
