var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var LocalizationStatusSuccess = function (text, scope) { return ({
    success: true,
    message: "Success",
    text: text,
    scope: scope,
}); };
var LocalizationDictionaryStatusSuccess = function (scope) { return ({
    scope: scope,
    context: null,
}); };
var Localization = /** @class */ (function () {
    function Localization(localizationConfiguration) {
        this.mGlobalScope = "global";
        this.mCultureCookieName = "Localization.Culture";
        this.mDictionary = {};
        this.mDictionaryQueue = {};
        this.mPluralizedDictionary = {};
        this.mPluralizedDictionaryQueue = {};
        this.mErrorHandlerCalled = false;
        this.mLocalizationConfiguration = localizationConfiguration === undefined || localizationConfiguration === null
            ? {
                errorResolution: LocalizationErrorResolution.Key,
            } : localizationConfiguration;
    }
    Localization.prototype.callErrorHandler = function (errorStatus) {
        if (!this.mErrorHandlerCalled) {
            this.mErrorHandlerCalled = true;
            if (this.mLocalizationConfiguration.onError !== undefined) {
                this.mLocalizationConfiguration.onError(errorStatus);
            }
        }
    };
    Localization.prototype.getTranslationOnError = function (text, scope) {
        switch (this.mLocalizationConfiguration.errorResolution) {
            case LocalizationErrorResolution.Null:
                return null;
            case LocalizationErrorResolution.Key:
                return {
                    name: text,
                    resourceNotFound: true,
                    value: text,
                };
        }
    };
    /**
     * @deprecated Use translateAsync or getDictionaryAsync() => translate
     */
    Localization.prototype.translate = function (text, scope, cultureName) {
        var dictionary = this.getDictionary(scope, cultureName);
        return dictionary.translate(text, function () { return dictionary.getFallbackTranslation(text, scope, cultureName); });
    };
    Localization.prototype.translateAsync = function (text, scope, cultureName) {
        var _this = this;
        return new Promise(function (resolve, reject) {
            _this.getDictionaryAsync(scope, cultureName)
                .then(function (dictionaryResponse) {
                var result = dictionaryResponse.result.translate(text, function () { return dictionaryResponse.result.getFallbackTranslation(text, scope, cultureName); });
                resolve(new LocalizationResult(result, LocalizationStatusSuccess(text, scope)));
            }, function (dictionaryResponse) {
                var errorStatus = {
                    success: false,
                    message: "Unable to load required dictionary",
                    errorType: 'loadDictionary',
                    text: text,
                    scope: dictionaryResponse.status.scope,
                    context: dictionaryResponse.status.context,
                };
                _this.callErrorHandler(errorStatus);
                reject(new LocalizationResult(_this.getTranslationOnError(text, scope), LocalizationStatusSuccess(text, scope)));
            });
        });
    };
    /**
     *@deprecated Use translateFormatAsync or getDictionaryAsync() => translateFormat
     */
    Localization.prototype.translateFormat = function (text, parameters, scope, cultureName) {
        var dictionary = this.getDictionary(scope, cultureName);
        return dictionary.translateFormat(text, parameters, function () { return dictionary.getFallbackTranslation(text, scope, cultureName); });
    };
    Localization.prototype.translateFormatAsync = function (text, parameters, scope, cultureName) {
        var _this = this;
        return new Promise(function (resolve, reject) {
            _this.getDictionaryAsync(scope, cultureName)
                .then(function (dictionaryResponse) {
                var result = dictionaryResponse.result.translateFormat(text, parameters, function () { return dictionaryResponse.result.getFallbackTranslation(text, scope, cultureName); });
                resolve(new LocalizationResult(result, LocalizationStatusSuccess(text, scope)));
            }, function (dictionaryResponse) {
                var errorStatus = {
                    success: false,
                    message: "Unable to load required dictionary",
                    errorType: 'loadDictionary',
                    text: text,
                    scope: dictionaryResponse.status.scope,
                    context: dictionaryResponse.status.context,
                };
                _this.callErrorHandler(errorStatus);
                reject(new LocalizationResult(_this.getTranslationOnError(text, scope), LocalizationStatusSuccess(text, scope)));
            });
        });
    };
    /**
     *@deprecated Use translatePluralizationAsync or getPluralizationDictionaryAsync() => translatePluralization
     */
    Localization.prototype.translatePluralization = function (text, number, scope, cultureName) {
        var dictionary = this.getPluralizationDictionary(scope, cultureName);
        try {
            return dictionary.translatePluralization(text, number, function () { return dictionary.getFallbackTranslation(text, scope, cultureName); });
        }
        catch (exception) {
            return this.handleError(exception, text);
        }
    };
    Localization.prototype.translatePluralizationAsync = function (text, number, scope, cultureName) {
        var _this = this;
        return new Promise(function (resolve, reject) {
            _this.getPluralizationDictionaryAsync(scope, cultureName)
                .then(function (dictionaryResponse) {
                try {
                    var result = dictionaryResponse.result.translatePluralization(text, number, function () { return dictionaryResponse.result.getFallbackTranslation(text, scope, cultureName); });
                    resolve(new LocalizationResult(result, LocalizationStatusSuccess(text, scope)));
                }
                catch (exception) {
                    reject(new LocalizationResult(_this.handleError(exception, text), {
                        success: false,
                        message: exception.message,
                        errorType: 'exception',
                        text: text,
                        scope: scope,
                    }));
                }
            }, function (dictionaryResponse) {
                var errorStatus = {
                    success: false,
                    message: "Unable to load required pluralization dictionary",
                    errorType: 'loadDictionary',
                    text: text,
                    scope: dictionaryResponse.status.scope,
                    context: dictionaryResponse.status.context,
                };
                _this.callErrorHandler(errorStatus);
                reject(new LocalizationResult(_this.getTranslationOnError(text, scope), LocalizationStatusSuccess(text, scope)));
            });
        });
    };
    Localization.prototype.handleError = function (exception, text) {
        console.error(exception.message);
        return { name: text, value: "X{error}", resourceNotFound: true };
    };
    Localization.prototype.configureSiteUrl = function (siteUrl) {
        this.mLocalizationConfiguration.siteUrl = siteUrl;
    };
    /**
     *@deprecated Use getDictionaryAsync
     */
    Localization.prototype.getDictionary = function (scope, cultureName) {
        scope = this.checkScope(scope);
        cultureName = this.checkCultureName(cultureName);
        return this.getLocalizationDictionary(scope, cultureName);
    };
    Localization.prototype.getDictionaryAsync = function (scope, cultureName) {
        var _this = this;
        scope = this.checkScope(scope);
        cultureName = this.checkCultureName(cultureName);
        return new Promise(function (resolve, reject) {
            _this.getLocalizationDictionaryAsync(scope, cultureName, function (dictionary) {
                resolve({
                    result: dictionary,
                    status: LocalizationDictionaryStatusSuccess(scope),
                });
            }, function (status) {
                reject({
                    result: null,
                    status: status,
                });
            });
        });
    };
    /**
     *@deprecated Use getPluralizationDictionaryAsync
     */
    Localization.prototype.getPluralizationDictionary = function (scope, cultureName) {
        scope = this.checkScope(scope);
        cultureName = this.checkCultureName(cultureName);
        return this.getPluralizationLocalizationDictionary(scope, cultureName);
    };
    Localization.prototype.getPluralizationDictionaryAsync = function (scope, cultureName) {
        var _this = this;
        scope = this.checkScope(scope);
        cultureName = this.checkCultureName(cultureName);
        return new Promise(function (resolve, reject) {
            _this.getPluralizationLocalizationDictionaryAsync(scope, cultureName, function (dictionary) {
                resolve({
                    result: dictionary,
                    status: LocalizationDictionaryStatusSuccess(scope),
                });
            }, function (status) {
                reject({
                    result: null,
                    status: status,
                });
            });
        });
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
    /**
     *@deprecated Use getLocalizationDictionaryAsync
     */
    Localization.prototype.getLocalizationDictionary = function (scope, cultureName) {
        var dictionaryKey = this.dictionaryKey(scope, cultureName);
        var dictionary = this.mDictionary[dictionaryKey];
        if (typeof dictionary === "undefined") {
            this.downloadDictionary(scope, cultureName);
            return this.mDictionary[dictionaryKey];
        }
        return dictionary;
    };
    Localization.prototype.getLocalizationDictionaryAsync = function (scope, cultureName, onSuccess, onFailed) {
        var dictionaryKey = this.dictionaryKey(scope, cultureName);
        var dictionary = this.mDictionary[dictionaryKey];
        if (dictionary === undefined) {
            this.downloadDictionaryAsync(scope, cultureName, onSuccess, onFailed);
        }
        else {
            onSuccess(dictionary);
        }
    };
    /**
     *@deprecated Use getPluralizationLocalizationDictionaryAsync
     */
    Localization.prototype.getPluralizationLocalizationDictionary = function (scope, cultureName) {
        var dictionaryKey = this.dictionaryKey(scope, cultureName);
        var dictionary = this.mPluralizedDictionary[dictionaryKey];
        if (typeof dictionary === "undefined") {
            this.downloadPluralizedDictionary(scope, cultureName);
            return this.mPluralizedDictionary[dictionaryKey];
        }
        return dictionary;
    };
    Localization.prototype.getPluralizationLocalizationDictionaryAsync = function (scope, cultureName, onSuccess, onFailed) {
        var dictionaryKey = this.dictionaryKey(scope, cultureName);
        var dictionary = this.mPluralizedDictionary[dictionaryKey];
        if (dictionary === undefined) {
            this.downloadPluralizedDictionaryAsync(scope, cultureName, onSuccess, onFailed);
        }
        else {
            onSuccess(dictionary);
        }
    };
    Localization.prototype.dictionaryKey = function (scope, cultureName) {
        return scope.concat("|", cultureName);
    };
    /**
     *@deprecated Use downloadDictionaryAsync
     */
    Localization.prototype.downloadDictionary = function (scope, cultureName) {
        var _this = this;
        var dictionaryKey = this.dictionaryKey(scope, cultureName);
        if (this.mDictionaryQueue[scope] === undefined) {
            this.mDictionaryQueue[scope] = [];
        }
        var xmlHttpRequest = new XMLHttpRequest();
        xmlHttpRequest.onreadystatechange = function () {
            if (xmlHttpRequest.readyState === XMLHttpRequest.DONE
                && xmlHttpRequest.status === 200) {
                var response = xmlHttpRequest.responseText;
                if (_this.mDictionary[dictionaryKey] === undefined) {
                    _this.mDictionary[dictionaryKey] = new LocalizationDictionary(response);
                }
                _this.processDictionaryQueue(scope, dictionaryKey);
            }
        };
        xmlHttpRequest.open("GET", this.getBaseUrl() + "/Localization/Dictionary?scope=" + scope, false);
        xmlHttpRequest.send();
    };
    Localization.prototype.downloadDictionaryAsync = function (scope, cultureName, onSuccess, onFailed) {
        /*
        this.getDownloadPromise(
            (response) => new LocalizationDictionary(response),
            this.mDictionary,
            "Dictionary",
            scope,
            cultureName
        );
        */
        var _this = this;
        var dictionaryKey = this.dictionaryKey(scope, cultureName);
        if (this.mDictionaryQueue[scope] === undefined) {
            this.mDictionaryQueue[scope] = [];
            var xmlHttpRequest_1 = new XMLHttpRequest();
            xmlHttpRequest_1.onreadystatechange = function () {
                if (xmlHttpRequest_1.readyState !== XMLHttpRequest.DONE) {
                    return;
                }
                if (xmlHttpRequest_1.status === 200) {
                    var response = xmlHttpRequest_1.responseText;
                    if (_this.mDictionary[dictionaryKey] === undefined) {
                        _this.mDictionary[dictionaryKey] = new LocalizationDictionary(response);
                    }
                    _this.processDictionaryQueue(scope, dictionaryKey);
                }
                else {
                    onFailed({
                        scope: scope,
                        context: xmlHttpRequest_1,
                    });
                }
            };
            xmlHttpRequest_1.open("GET", this.getBaseUrl() + "/Localization/Dictionary?scope=" + scope, true);
            xmlHttpRequest_1.send();
        }
        this.mDictionaryQueue[scope].push(onSuccess);
        this.processDictionaryQueue(scope, dictionaryKey);
    };
    Localization.prototype.processDictionaryQueue = function (scope, dictionaryKey) {
        if (this.mDictionary[dictionaryKey] !== undefined) {
            var onSuccessQueue = this.mDictionaryQueue[scope].shift();
            while (onSuccessQueue !== undefined) {
                onSuccessQueue(this.mDictionary[dictionaryKey]);
                onSuccessQueue = this.mDictionaryQueue[scope].shift();
            }
        }
    };
    /**
     * @deprecated Use downloadPluralizedDictionaryAsync
     */
    Localization.prototype.downloadPluralizedDictionary = function (scope, cultureName) {
        var _this = this;
        var dictionaryKey = this.dictionaryKey(scope, cultureName);
        if (this.mPluralizedDictionaryQueue[scope] === undefined) {
            this.mPluralizedDictionaryQueue[scope] = [];
        }
        var xmlHttpRequest = new XMLHttpRequest();
        xmlHttpRequest.onreadystatechange = function () {
            if (xmlHttpRequest.readyState === XMLHttpRequest.DONE
                && xmlHttpRequest.status === 200) {
                var response = xmlHttpRequest.responseText;
                if (_this.mPluralizedDictionary[dictionaryKey] === undefined) {
                    _this.mPluralizedDictionary[dictionaryKey] = new LocalizationPluralizationDictionary(response);
                }
                _this.processPluralizedDictionaryQueue(scope, dictionaryKey);
            }
        };
        xmlHttpRequest.open("GET", this.getBaseUrl() + "/Localization/PluralizedDictionary?scope=" + scope, false);
        xmlHttpRequest.send();
    };
    Localization.prototype.downloadPluralizedDictionaryAsync = function (scope, cultureName, onSuccess, onFailed) {
        /*
        this.getDownloadPromise(
            (response) => new LocalizationPluralizationDictionary(response),
            this.mPluralizedDictionary,
            "PluralizedDictionary",
            scope,
            cultureName
        );
        */
        var _this = this;
        var dictionaryKey = this.dictionaryKey(scope, cultureName);
        if (this.mPluralizedDictionaryQueue[scope] === undefined) {
            this.mPluralizedDictionaryQueue[scope] = [];
            var xmlHttpRequest_2 = new XMLHttpRequest();
            xmlHttpRequest_2.onreadystatechange = function () {
                if (xmlHttpRequest_2.readyState !== XMLHttpRequest.DONE) {
                    return;
                }
                if (xmlHttpRequest_2.status === 200) {
                    var response = xmlHttpRequest_2.responseText;
                    if (_this.mPluralizedDictionary[dictionaryKey] === undefined) {
                        _this.mPluralizedDictionary[dictionaryKey] = new LocalizationPluralizationDictionary(response);
                    }
                    _this.processPluralizedDictionaryQueue(scope, dictionaryKey);
                }
                else {
                    onFailed({
                        scope: scope,
                        context: xmlHttpRequest_2,
                    });
                }
            };
            xmlHttpRequest_2.open("GET", this.getBaseUrl() + "/Localization/PluralizedDictionary?scope=" + scope, true);
            xmlHttpRequest_2.send();
        }
        this.mPluralizedDictionaryQueue[scope].push(onSuccess);
        this.processPluralizedDictionaryQueue(scope, dictionaryKey);
    };
    Localization.prototype.processPluralizedDictionaryQueue = function (scope, dictionaryKey) {
        if (this.mPluralizedDictionary[dictionaryKey] !== undefined) {
            var onSuccessQueue = this.mPluralizedDictionaryQueue[scope].shift();
            while (onSuccessQueue !== undefined) {
                onSuccessQueue(this.mPluralizedDictionary[dictionaryKey]);
                onSuccessQueue = this.mPluralizedDictionaryQueue[scope].shift();
            }
        }
    };
    /**
     * TODO Use in downloadPluralizedDictionaryAsync and downloadDictionaryAsync when synchronous methods are removed
     */
    Localization.prototype.getDownloadPromise = function (dictionaryFactory, dictionaryCache, path, scope, cultureName) {
        var _this = this;
        var dictionaryKey = this.dictionaryKey(scope, cultureName);
        return new Promise(function (resolve, reject) {
            var xmlHttpRequest = new XMLHttpRequest();
            xmlHttpRequest.open("GET", _this.getBaseUrl() + "/Localization/" + path + "?scope=" + scope, true);
            xmlHttpRequest.send();
            xmlHttpRequest.onreadystatechange = function () {
                if (xmlHttpRequest.readyState !== XMLHttpRequest.DONE) {
                    return;
                }
                if (xmlHttpRequest.status >= 200 && xmlHttpRequest.status < 300) {
                    var response = xmlHttpRequest.responseText;
                    if (dictionaryCache[dictionaryKey] === undefined) {
                        dictionaryCache[dictionaryKey] = dictionaryFactory(response);
                    }
                    resolve(dictionaryCache[dictionaryKey]);
                }
                else {
                    reject({
                        scope: scope,
                        status: xmlHttpRequest.status,
                        statusText: xmlHttpRequest.statusText,
                        context: xmlHttpRequest,
                    });
                }
            };
        });
    };
    Localization.prototype.getBaseUrl = function () {
        var baseUrl = this.mLocalizationConfiguration.siteUrl;
        if (baseUrl && baseUrl.charAt(baseUrl.length - 1) === "/") {
            baseUrl = baseUrl.substring(0, baseUrl.length - 1);
        }
        return baseUrl;
    };
    Localization.prototype.getCurrentCulture = function () {
        if (this.mCurrentCulture === undefined) {
            var parsedCookieValue = this.getParsedCultureCookie();
            var currentCulture = parsedCookieValue.currentCulture === null
                ? parsedCookieValue.defaultCulture
                : parsedCookieValue.currentCulture;
            this.setCurrentCulture(currentCulture);
        }
        return this.mCurrentCulture;
    };
    Localization.prototype.setCurrentCulture = function (culture) {
        this.mCurrentCulture = culture;
    };
    Localization.prototype.getParsedCultureCookie = function () {
        var currentCultureCookieValue = this.getCurrentCultureCookie();
        var parsedCookieValue = JSON.parse(currentCultureCookieValue);
        if (parsedCookieValue.defaultCulture === undefined
            || parsedCookieValue.currentCulture === undefined) {
            console.error("Unexpected value of the cookie " + this.mCultureCookieName + ". Expected object with properties 'defaultCulture', and 'currentCulture'.", parsedCookieValue);
        }
        return parsedCookieValue;
    };
    Localization.prototype.getCurrentCultureCookie = function () {
        return LocalizationUtils.getCookie(this.mCultureCookieName);
    };
    return Localization;
}());
var BaseLocalizationDictionary = /** @class */ (function () {
    function BaseLocalizationDictionary(dictionary) {
        this.mDictionary = JSON.parse(dictionary);
    }
    BaseLocalizationDictionary.prototype.getFallbackTranslation = function (text, scope, cultureName) {
        console.log("Localized string with key=" + text + " was not found in dictionary=" + scope + " with culture=" + cultureName);
        return { name: text, value: "X{undefined}", resourceNotFound: true };
    };
    return BaseLocalizationDictionary;
}());
var LocalizationDictionary = /** @class */ (function (_super) {
    __extends(LocalizationDictionary, _super);
    function LocalizationDictionary(dictionary) {
        return _super.call(this, dictionary) || this;
    }
    LocalizationDictionary.prototype.translate = function (text, fallback) {
        var result = this.mDictionary[text];
        if (result === undefined) {
            return fallback();
        }
        return result;
    };
    LocalizationDictionary.prototype.translateFormat = function (text, parameters, fallback) {
        var translation = this.translate(text, function () { return null; });
        if (translation === null) {
            return fallback();
        }
        var formatedText = !parameters ? translation.value : this.formatString(translation, parameters);
        return { name: text, value: formatedText, resourceNotFound: translation.resourceNotFound };
    };
    LocalizationDictionary.prototype.formatString = function (str, obj) {
        return str.value.replace(/\{\s*([^}\s]+)\s*\}/g, function (m, p1, offset, string) { return obj[p1]; });
    };
    return LocalizationDictionary;
}(BaseLocalizationDictionary));
var LocalizationPluralizationDictionary = /** @class */ (function (_super) {
    __extends(LocalizationPluralizationDictionary, _super);
    function LocalizationPluralizationDictionary(dictionary) {
        return _super.call(this, dictionary) || this;
    }
    LocalizationPluralizationDictionary.prototype.translatePluralization = function (text, number, fallback) {
        var pluralizedString = this.mDictionary[text];
        if (pluralizedString === undefined || pluralizedString === null) {
            return fallback();
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
}(BaseLocalizationDictionary));
var LocalizationErrorResolution;
(function (LocalizationErrorResolution) {
    LocalizationErrorResolution[LocalizationErrorResolution["Null"] = 0] = "Null";
    LocalizationErrorResolution[LocalizationErrorResolution["Key"] = 1] = "Key";
})(LocalizationErrorResolution || (LocalizationErrorResolution = {}));
var LocalizationResult = /** @class */ (function () {
    function LocalizationResult(result, status) {
        this.result = result;
        this.status = status;
    }
    LocalizationResult.prototype.toString = function () {
        return this.result.value;
    };
    return LocalizationResult;
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
