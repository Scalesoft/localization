using System;
using Newtonsoft.Json;

namespace Localization.CoreLibrary.Util
{
    internal class FallbackModeConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var translateFallbackMode = (LocTranslateFallbackMode) value;

            switch (translateFallbackMode)
            {
                case LocTranslateFallbackMode.Null:
                    writer.WriteValue("Null");
                    break;
                case LocTranslateFallbackMode.Key:
                    writer.WriteValue("Key");
                    break;
                case LocTranslateFallbackMode.Exception:
                    writer.WriteValue("Exception");
                    break;
                case LocTranslateFallbackMode.EmptyString:
                    writer.WriteValue("EmptyString");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var enumString = (string) reader.Value;

            return Enum.Parse(typeof(LocTranslateFallbackMode), enumString, true);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }
    }
}