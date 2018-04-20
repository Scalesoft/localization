## Not implemented yet:
* Support for obtaining dictionary parts.
* Support for Database texts.


## Usage
### First the library must be initialized:
```
Localization.LibInit("path/to/libSettings.json");
```
*Default IDictionaryFactory is JsonDictionaryFactory to loading json resource files.*
*Default ILoggerFactory is NullLogerFactory which log nothing.*

### Using logger or custom resource file loader:
```
IDictionaryFactory customDictionaryFactory = ...;
ILoggerFactory loggerFactory = ...;
Localization.LibInit("path/to/libSettings.json", customDictionaryFactory, loggerFactory);
```
### Using library without config file:
```
LocalizationConfiguration.Configuration configuration = new LocalizationConfiguration.Configuration();
            configuration.BasePath = @"localization";
            configuration.DefaultCulture = @"cs";
            configuration.SupportedCultures = new List<string> { "en", "es" };
            configuration.DbSource = @"cosi://sql-source";
            configuration.DbUser = "SA";
            configuration.DbPassword = "SA";
            configuration.TranslationFallbackMode = TranslateFallbackMode.Key;
			
IConfiguration localizationConfiguration = new LocalizationConfiguration(configuration);
Localization.LibInit(localizationConfiguration);
```

## Structure of config json file:
```
{
	"BasePath":"path\\to\\localization",
	"SupportedCultures":["en","es"],
	"DefaultCulture":"cs",
	"TranslationFallbackMode":"Key",
	"DbSource":"cosi://sql-source",
	"DbUser":"SA",
	"DbPassword":"SA"
}
```
### Posibble values of TranslationFallbackMode:
* Key
* Exception
* EmptyString

## Structure of resource files (using JsonDictionaryFactory):
Every supported culture and default culture has to have resource file in every scope.
Scopes are defined by directory structure.

### For example consider:
```
basePath: /path/to/localization
DefaultCulture: "cs"
SupportedCultures: [cs, en, es]
```
### Directory structure is:
```
path/to/localization
			|__________info
			|__________statements
							|_________important
```						
### Resource files should be:
```
path/to/localization
		cs.json
		en.json
		es.json
			|__________info
			|		   info.cs.json
			|		   info.en.json
			|		   info.es.json
			|__________statements
					   statements.cs.json
					   statements.en.json
					   statements.es.json
							|_________important
									  statements.important.cs.json
									  statements.important.en.json
									  statements.important.es.json
```
### Json structure (cs.json):
```
Example of json resource file in global scope and cs culture:
{
	"culture":"cs",
	"scope":"global",
	"dictionary": {
		"text-1-odst" : "První odstavec v globálním slovníku",
		"text-2-odst" : "Druhý odstavec v globálním slovníku",
		"text-3-odst" : "Třetí odstavec v globálním slovníku",
		"text-4-odst" : "Čtvrtý odstavec v globálním slovníku",
		"text-5-odst" : "Pátý odstavec v globálním slovníku",
		"confirm-button" : "Potvrdit",
		"klíč-stringu" : "Dnes je {0}."
	},"constants": {
		"const-date": "MMMM dd, yyyy",
		"const-time": "hh:mm:ss.f"	
	}
}
```

**Notice support for parametrized strings {0}.**

**Notice support for constant strings. (To separate programming stuff from translators.)**

### Pluralized resource files:

Library also supports pluralization. Pluralized strings are stored in separate files.
Every dictionary in scope can have pluralized version. Name of file is e.g. scope.plural.cs.json 
(keyword plural between scope and culture name).

### Example of pluralized Json file (cs.plural.json):
```
{
	"culture":"cs",
	"scope":"global.plural",
	"dictionary": {
		"klíč-stringu": {
			"let": [ //<-This is default value
				[null, -5, "let"], 
				//the first value is start of the interval, the second value is end of interval (exclusive).
				//the third is value for defined interval.
				//null means possitive or negative infinity,
				//start of the interval is always less or equal than end of the interval.
				//No interval overlaps are allowed.
				[-4, -2, "roky"],
				[-1, -1, "rok"],
				[0, 0, "let"],
				[1, 1, "rok"],
				[2, 4, "roky"],
				[5, null, "let"]
			]
		}
	}	
}
```

## Translating

### Translate
```
LocalizedString ls = Translator.Translate("text-3-odst", new CultureInfo("cs"));
//ls.value == "Třetí odstavec v globálním slovníku";
```
### Translate scope
```
LocalizedString lsII = Translator.Translate("text-3-odst", new CultureInfo("en"), "info.important");
//lsII.value == "Text in important info scope";
```
## Translate constant
```
LocalizedString lsC = Translator.TranslateConstant("const-date", new CultureInfo("cs"));
//lsC.value == "MMMM dd, yyyy";
```
### Translate pluralized
```
LocalizedString lsP = Translator.TranslatePluralization("klíč-stringu", 1, new CultureInfo("cs"));
//lsP.value == "rok";
```
### Translate format
```
LocalizedString lsF = Translator.TranslateFormat("klíč-stringu", new[] {"pondělí"}, new CultureInfo("cs"));
//lsF.value == "Dnes je pondělí.";
```
## Getting dictionary
...

### Getting global dictionary
```
Dictionary<string, LocalizedString> dG = Translator.GetDictionary(new CultureInfo("cs"), "global")
```
### Getting dictionary part !!!Not supported NOW

TODO

### Geting constants dictionary
```
Dictionary<string, LocalizedString> dC = Translator.GetConstantsDictionary(new CultureInfo("cs"), "global")
```
### Getting pluralized dictionary
```
Dictionary<string, PluralizedString> dP = Translator.GetPluralizedDictionary(new CultureInfo("cs"), "global");
```

Konfigurace v ASP.NET:

Startup.cs
```
public void ConfigureServices(IServiceCollection services)
        {
            Localization.CoreLibrary.Localization.Init(
                @"C:\Pool\localization-ridics\Solution\Localization.Service\bin\Debug\netstandard1.3\localization.json.config",
                null,
                new JsonDictionaryFactory());

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddLocalizationService();
		}

public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
			
            Localization.CoreLibrary.Localization.AttachLogger(loggerFactory);   
		}
``` 



















