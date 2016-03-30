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
