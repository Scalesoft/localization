using Scalesoft.Localization.AspNetCore.Models;

namespace Scalesoft.Localization.AspNetCore.Manager
{
    public class DeserializationResult
    {
        public bool Success { get; set; }

        public LocalizationCookie Value { get; set; }
    }
}