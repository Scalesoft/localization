using System;
using System.Globalization;
using System.Web;

namespace Localization.Web
{
    public class LocalizationHttpModule : IHttpModule
    {
        private readonly string m_cookieName = "current-lang";

        public void Init(HttpApplication context)
        {
            context.BeginRequest += BeginRequest;
            context.EndRequest += EndRequest;
        }

        public void Dispose()
        {
        }

        private void BeginRequest(object sender, EventArgs e)
        {
            var application = (HttpApplication) sender;
            var context = application.Context;

            if (context.Request.Cookies[m_cookieName] != null)
            {
                var cultureInfoName = context.Request.Cookies[m_cookieName].Value;
                LocalizationManager.Instance.SetCultureInfo(new CultureInfo(cultureInfoName));
            }
        }

        private void EndRequest(object sender, EventArgs e)
        {
            var application = (HttpApplication) sender;
            var context = application.Context;
            var httpCookie = context.Response.Cookies[m_cookieName];
            if (httpCookie == null)
            {
                httpCookie = new HttpCookie(m_cookieName);
                context.Response.Cookies.Add(httpCookie);
            }
            httpCookie.Value = LocalizationManager.Instance.GetCultureInfo().Name;
            httpCookie.Expires = DateTime.UtcNow+TimeSpan.FromDays(30);
        }
    }
}