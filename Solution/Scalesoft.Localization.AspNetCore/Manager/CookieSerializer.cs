using Newtonsoft.Json;
using Scalesoft.Localization.AspNetCore.Models;

namespace Scalesoft.Localization.AspNetCore.Manager
{
    public class CookieSerializer
    {
        public static string Serialize(LocalizationCookie cookie)
        {
            return JsonConvert.SerializeObject(cookie);
        }

        public static LocalizationCookie Deserialize(string cookie)
        {
            if (string.IsNullOrEmpty(cookie))
            {
                return new LocalizationCookie();
            }

            try
            {
                return JsonConvert.DeserializeObject<LocalizationCookie>(cookie);
            }
            catch (JsonException)
            {
                // wrong value in cookie, so return object without set values:
                return new LocalizationCookie();
            }
        }
    }
}