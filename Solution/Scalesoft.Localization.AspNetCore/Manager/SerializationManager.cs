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

        public static DeserializationResult DeserializeCookie(string cookie)
        {
            LocalizationCookie value = null;
            var success = false;

            try
            {
                value = JsonConvert.DeserializeObject<LocalizationCookie>(cookie);
                success = true;
            }
            catch (JsonException)
            {
                // keep fail values
            }

            return new DeserializationResult {Success = success, Value = value};
        }
    }
}