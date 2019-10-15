using Newtonsoft.Json;
using Scalesoft.Localization.AspNetCore.Models;

namespace Scalesoft.Localization.AspNetCore.Manager
{
    public class SerializationManager
    {
        public static string SerializeCookie(LocalizationCookie cookie)
        {
            return JsonConvert.SerializeObject(cookie);
        }

        public static LocalizationCookie DeserializeCookie(string cookie)
        {
            return JsonConvert.DeserializeObject<LocalizationCookie>(cookie);
        }
    }
}