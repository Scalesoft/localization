1. Register HttpModule in Web.config like this:

  <system.webServer>
    <modules>
      <add name="Localization.Web" type="Localization.Web.LocalizationHttpModule"/>
    </modules>
  </system.webServer>

2. Reference javascript file in Views/Shared/_Layout.cshtml:

	<script src="/Scripts/Localize.js"></script>

3. In javascript use as this:

	translate("JSHello");

4. In Razor view use as this:

	<h2>@Translator.Translate("Hello")</h2>

or this in script part:

	<script>
		$(document).ready(() => {
			$("#razorViewJsLocalization").html('@Translator.Translate("JSHello")');
		});
	</script>

5. For formatted text with parameters can used translateFormat method

	In Razor:

	<h2>@Translator.TranslateFormat("Hello with date", new[]{DateTime.Now.Date.ToString()})</h2>

	In javascript:

	translateFormat("Hello with date", [Date.now()]);

6. For strings categorization can be used user defined scope. Scope string in resx should looks as follows:
	{scope}-{stringKey} and translation for each scope can differ.

	examples of resx keys scoping :

			with scope 'global'		|	with scope 'form'	|	without scope
			-------------------------------------------------------------------
			global-Confirm			|	form-Confirm		|	Confirm
			global-WarningMessage	|	form-WarningMessage	|	WarningMessage

	All translation methods have overload for define scope:

	translateFormat("Hello with date and scope", [Date.now(), "global"], "global");
	translate("Hello", "greetings");
	<h2>@Translator.TranslateFormat("Hello with date and scope", new[]{DateTime.Now.Date.ToString(), "global"}, "global")</h2>
	<h2>@Translator.Translate("Hello", "greetings")</h2>