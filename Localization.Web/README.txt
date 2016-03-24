1. Register HttpModule in Web.config like this:

  <system.webServer>
    <modules>
      <add name="Localization.Web" type="Localization.Web.LocalizationHttpModule"/>
    </modules>
  </system.webServer>

2. Initialize LocationManager before first request can occur. Use Global.asax for this and code below with usage of container:

    var container = Container.Current; //Create container on startup
    container.Resolve<LocalizationManager>();	//Initialize instance of LocationManager
