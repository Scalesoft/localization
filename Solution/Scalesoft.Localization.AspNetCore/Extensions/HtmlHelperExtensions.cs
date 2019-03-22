using System;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Scalesoft.Localization.AspNetCore.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static IHtmlContent EncodedString(this IHtmlHelper htmlHelper, string value)
        {
            return new StringHtmlContent(value);
        }

        public static IHtmlContent EncodedString(this IHtmlHelper htmlHelper, object value, IFormatProvider formatProvider = null)
        {
            var formattedString = Convert.ToString(value, formatProvider);
            return new StringHtmlContent(formattedString);
        }
    }
}
