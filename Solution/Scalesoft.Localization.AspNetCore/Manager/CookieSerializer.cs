using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Scalesoft.Localization.AspNetCore.Models;

namespace Scalesoft.Localization.AspNetCore.Manager
{
    public class CookieSerializer
    {
        private static JsonSerializerSettings JsonSerializerSettings => new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
        };

        public static string Serialize(LocalizationCookie cookie)
        {
            return JsonConvert.SerializeObject(cookie, JsonSerializerSettings);
        }

        public static LocalizationCookie Deserialize(string cookie)
        {
            if (string.IsNullOrEmpty(cookie))
            {
                return new LocalizationCookie();
            }

            try
            {
                return JsonConvert.DeserializeObject<LocalizationCookie>(cookie, JsonSerializerSettings);
            }
            catch (JsonException)
            {
                // wrong value in cookie, so return object without set values:
                return new LocalizationCookie();
            }
        }
    }
}
