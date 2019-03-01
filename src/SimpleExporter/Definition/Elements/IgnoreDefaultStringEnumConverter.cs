using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SimpleExporter.Definition.Elements
{
    internal class IgnoreDefaultStringEnumConverter<TEnum> : StringEnumConverter
    {
        private readonly string defaultValue;

        public IgnoreDefaultStringEnumConverter()
        {
            defaultValue = Enum.Parse(typeof(TEnum), "0").ToString();
        }

        public IgnoreDefaultStringEnumConverter(bool camelCaseText) : base(camelCaseText)
        {
            defaultValue = Enum.Parse(typeof(TEnum), "0").ToString();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            try
            {
                // Try to read regularly
                return base.ReadJson(reader, objectType, existingValue, serializer);
            }
            catch
            {
                // Catch invalid values and replace them with default value
                // Add warning stating behavior
                return Enum.Parse(typeof(TEnum), "0");
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value?.ToString() == defaultValue)
                value = null;
            base.WriteJson(writer, value, serializer);
        }

        // TODO: temporary warning code for invalid value. Remove when common set of error codes created and integrated.
        private enum WarningStatusCode
        {
            UnknownElementType = 0
        }
    }
}