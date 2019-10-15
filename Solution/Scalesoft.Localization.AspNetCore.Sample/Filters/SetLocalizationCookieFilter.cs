using Microsoft.AspNetCore.Mvc.Filters;

namespace Scalesoft.Localization.AspNetCore.Sample.Filters
{
    public class SetLocalizationCookieFilter : IActionFilter {
        private readonly ILocalizationService m_localization;

        public SetLocalizationCookieFilter(ILocalizationService localization)
        {
            m_localization = localization;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            m_localization.SetDefaultCookie();
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }
    }
}