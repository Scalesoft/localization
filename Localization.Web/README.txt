1. Register HttpModule in Web.config like this:

  <system.webServer>
    <modules>
      <add name="Localization.Web" type="Localization.Web.LocalizationHttpModule"/>
    </modules>
  </system.webServer>

