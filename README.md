## Usage
### First the library must be initialized:
```c#
using Localization.AspNetCore.Service.IoC;

public IServiceProvider ConfigureServices(IServiceCollection services)
    var localizationConfiguration = m_configuration.GetSection("Localization").Get<LocalizationConfiguration>();
    IDatabaseConfiguration databaseConfiguration = null;
    //IDatabaseConfiguration databaseConfiguration = new NHibernateDatabaseConfiguration();
    
    services.AddLocalizationService(
        localizationConfiguration,
        databaseConfiguration
    );
}
```

The cookies are enabled by default but this behavior can be changed by adding the class which implements `ICookieConfigResolver` (useful for GDPR cookie consent):
```c#
services.AddSingleton<ICookieConfigResolver, CustomCookieConfigResolver>();
```

### Using library without config file:
```c#
public IServiceProvider ConfigureServices(IServiceCollection services)
    services.TryAddSingleton<LocalizationConfiguration>(
        cfg => new LocalizationConfiguration
        {
            BasePath = "Localization",
            DefaultScope = "global",
            SupportedCultures = new List<CultureInfo>(),
            DefaultCulture = new CultureInfo("en-US"),
            TranslateFallbackMode = LocTranslateFallbackMode.Key,
            AutoLoadResources = true,
            FirstAutoTranslateResource = LocLocalizationResource.File,
        }
    );
}
```

### Using library with database:

While using database texts, register entities mappings to NHibernate. Mappings can be obtained by this method:
```c#
NHibernateDatabaseConfiguration.GetMappings();
```

## Structure of config json file:
```json
{
	"BasePath":"path\\to\\localization",
	"DefaultScope":"global",
	"SupportedCultures":["en","es"],
	"DefaultCulture":"cs",
	"TranslateFallbackMode":"Key",
	"AutoLoadResources":true,
	"FirstAutoTranslateResource":"File"
}
```
### Posibble values of TranslateFallbackMode:
* Key
* Exception
* EmptyString

## Structure of resource files (using JsonDictionaryFactory):
Every supported culture and default culture has to have resource file in every scope.
Scopes are defined by directory structure.

### For example consider:
```json
{
  "BasePath": "/path/to/localization",
  "DefaultCulture": "cs",
  "SupportedCultures": ["cs", "en", "es"]
}
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
Example of json resource file in global scope and cs culture:
```json
{
	"culture":"cs",
	"scope":"global",
	"scopeAlias": [
	  "scopeAlias1",
	  "scopeAlias2"
	],
	"parentScope":null,
	"dictionary": {
		"text-1-odst" : "První odstavec v globálním slovníku",
		"text-2-odst" : "Druhý odstavec v globálním slovníku",
		"text-3-odst" : "Třetí odstavec v globálním slovníku",
		"text-4-odst" : "Čtvrtý odstavec v globálním slovníku",
		"text-5-odst" : "Pátý odstavec v globálním slovníku",
		"confirm-button" : "Potvrdit",
		"klíč-stringu" : "Dnes je {0}."
	},
	"constants": {
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
```json5
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
```c#
LocalizedString ls = Translator.Translate("text-3-odst", new CultureInfo("cs"));
//ls.value == "Třetí odstavec v globálním slovníku";
```
### Translate scope
```c#
LocalizedString lsII = Translator.Translate("text-3-odst", new CultureInfo("en"), "info.important");
//lsII.value == "Text in important info scope";
```
## Translate constant
```c#
LocalizedString lsC = Translator.TranslateConstant("const-date", new CultureInfo("cs"));
//lsC.value == "MMMM dd, yyyy";
```
### Translate pluralized
```c#
LocalizedString lsP = Translator.TranslatePluralization("klíč-stringu", 1, new CultureInfo("cs"));
//lsP.value == "rok";
```
### Translate format
```c#
LocalizedString lsF = Translator.TranslateFormat("klíč-stringu", new[] {"pondělí"}, new CultureInfo("cs"));
//lsF.value == "Dnes je pondělí.";
```
## Getting dictionary
...

### Getting global dictionary
```c#
Dictionary<string, LocalizedString> dG = Translator.GetDictionary(new CultureInfo("cs"), "global")
```
### Getting dictionary part !!!Not supported NOW

TODO

### Geting constants dictionary
```c#
Dictionary<string, LocalizedString> dC = Translator.GetConstantsDictionary(new CultureInfo("cs"), "global")
```
### Getting pluralized dictionary
```c#
Dictionary<string, PluralizedString> dP = Translator.GetPluralizedDictionary(new CultureInfo("cs"), "global");
```

## Translate Data annotations

To translate data annotations you have to add json resource file in scope, which name is same as model class. 
Values of data annotation attributes are used as keys in dictionaries.

For example: 
In attribute `[Display(Name = "UserName")]` UserName is a key for translation in json resource file.
For validation attributes it works the same. `UserNameNotEmpty` could be a key for translation in json resource file. 
```c#
public class LoginViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "UserNameNotEmpty")]
        [DataType(DataType.Text)]
        [Display(Name = "UserName")]
        public string UserName { get; set; }
...
```
LoginViewModel.cs-CZ.json
```json
{
  "culture": "cs-CZ",
  "scope": "LoginViewModel",
  "dictionary": {
    "RememberMe": "Zapamatovat si me",
    "UserName": "Uživatelské jméno",
    "Password": "Heslo"
  }
}
```

Configuration in ASP.NET:

Startup.cs
```c#
public void ConfigureServices(IServiceCollection services)
{
    var localizationConfiguration = m_configuration.GetSection("Localization").Get<LocalizationConfiguration>();
    IDatabaseConfiguration databaseConfiguration = null;
    //IDatabaseConfiguration databaseConfiguration = new NHibernateDatabaseConfiguration();
    
    services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    services.AddLocalizationService(
        localizationConfiguration,
        databaseConfiguration
    );
}

public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
{  
}
``` 

## Database structure 
Init database schema script for SQL Server is in Sample project named `CreateDBSchema.sql`. Run it in SQL Server Management studio

## Sample project for testing

1. Run `YarnInstall.ps1` script before first launch.
2. Execute `default` task in Task Runner Exlorer in Visual Studio if you are modifiying localization web script (of force rebuild the sample project).

> If gulp throws exception from task `link-to-external-project`, try shutdown IIS Express and Visual Studio.
