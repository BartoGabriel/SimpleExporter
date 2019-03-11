using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SimpleExporter.Definition.Elements
{
    /// <summary>
    ///     This handles using type field to instantiate strongly typed object on deserialization
    /// </summary>
    public class TypedElementConverter : JsonConverter
    {
        /// <summary>
        ///     Default types to support, register any new types to this list
        /// </summary>
        private static readonly Lazy<Dictionary<string, Type>> TypedElementTypes = new Lazy<Dictionary<string, Type>>(
            () =>
            {
                // TODO: Should this be a static? It makes it impossible to have diff renderers support different elements
                var types = new Dictionary<string, Type>
                {
                    [TextBlock.TypeName] = typeof(TextBlock),
                    [Table.TypeName] = typeof(Table)
                };
                return types;
            });

        private static HashSet<string> Ids { get; } = new HashSet<string>();

        public override bool CanRead => true;

        public override bool CanWrite => false;

        public static void ClearIds()
        {
            Ids.Clear();
        }

        public static void RegisterTypedElement<T>(string typeName = null)
            where T : TypedElement
        {
            if (typeName == null)
                typeName = ((TypedElement) Activator.CreateInstance(typeof(T))).Type;

            TypedElementTypes.Value[typeName] = typeof(T);
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(TypedElement).GetTypeInfo().IsAssignableFrom(objectType.GetTypeInfo());
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);

            var typeName = jObject["type"]?.Value<string>() ?? jObject["@type"]?.Value<string>();
            if (typeName == null)
            {
                // Get value of this objectType's "Type" JsonProperty(Required)
                var typeJsonPropertyRequiredValue = objectType.GetRuntimeProperty("Type")
                    .CustomAttributes.Where(a => a.AttributeType == typeof(JsonPropertyAttribute)).FirstOrDefault()?
                    .NamedArguments.Where(a => a.TypedValue.ArgumentType == typeof(Required)).FirstOrDefault()
                    .TypedValue.Value.ToString();

                // If this objectType does not require "Type" attribute, use the objectType's XML "TypeName" attribute
                if (typeJsonPropertyRequiredValue == "0")
                    typeName = objectType
                        .GetRuntimeFields().Where(x => x.Name == "TypeName").FirstOrDefault()?
                        .GetValue("TypeName").ToString();
                else
                    throw new Exception("Required property 'type' not found on element");
            }

            if (TypedElementTypes.Value.TryGetValue(typeName, out var type))
            {
                if (jObject.Value<string>("id") != null)
                {
                    var objectId = jObject.Value<string>("id");
                    if (Ids.Contains(objectId))
                        throw new Exception($"Duplicate 'id' found: '{objectId}'");
                    Ids.Add(objectId);
                }

                var result = (TypedElement) Activator.CreateInstance(type);
                try
                {
                    serializer.Populate(jObject.CreateReader(), result);
                }
                catch (JsonSerializationException)
                {
                    return result;
                }

                HandleAdditionalProperties(result);
                return result;
            }

            return null;
        }

        private void HandleAdditionalProperties(TypedElement te)
        {
            // https://stackoverflow.com/questions/34995406/nullvaluehandling-ignore-influences-deserialization-into-extensiondata-despite

            // The default behavior of JsonExtensionData is to include properties if the VALUE could not be set, including abstract properties or default values
            // We don't want to deserialize any properties that exist on the type into AdditionalProperties, so this function removes them

            // Create a list of known property names
            var knownPropertyNames = new List<string>();
            var runtimeProperties = te.GetType().GetRuntimeProperties();
            foreach (var runtimeProperty in runtimeProperties)
            {
                // Check if the property has a JsonPropertyAttribute with the value set
                string jsonPropertyName = null;
                foreach (var attribute in runtimeProperty.CustomAttributes)
                    if (attribute.AttributeType == typeof(JsonPropertyAttribute) &&
                        attribute.ConstructorArguments.Count == 1)
                    {
                        jsonPropertyName = attribute.ConstructorArguments[0].Value as string;
                        break;
                    }

                // Add the json property name if present, otherwise use the runtime property name
                knownPropertyNames.Add(jsonPropertyName != null ? jsonPropertyName : runtimeProperty.Name);
            }

            te.AdditionalProperties
                .Select(prop => knownPropertyNames
                    .SingleOrDefault(p => p.Equals(prop.Key, StringComparison.OrdinalIgnoreCase)))
                .Where(p => p != null)
                .ToList()
                .ForEach(p => te.AdditionalProperties.Remove(p));
        }

        public static T CreateElement<T>(string typeName = null)
            where T : TypedElement
        {
            if (typeName == null)
                typeName = ((T) Activator.CreateInstance(typeof(T))).Type;

            if (TypedElementTypes.Value.TryGetValue(typeName, out var type)) return (T) Activator.CreateInstance(type);
            return null;
        }
    }
}