using System;
using Newtonsoft.Json;

namespace Localization.CoreLibrary.Manager
{
    public class FallbackModeConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            TranslateFallbackMode translateFallbackMode = (TranslateFallbackMode) value;

            switch (translateFallbackMode)
            {
                case TranslateFallbackMode.Null:
                    writer.WriteValue("Null");
                    break;
                case TranslateFallbackMode.Key:
                    writer.WriteValue("Key");
                    break;
                case TranslateFallbackMode.Exception:
                    writer.WriteValue("Exception");
                    break;
                case TranslateFallbackMode.EmptyString:
                    writer.WriteValue("EmptyString");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var enumString = (string) reader.Value;

            return Enum.Parse(typeof(TranslateFallbackMode), enumString, true);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }
    }
}