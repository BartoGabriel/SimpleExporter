using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SimpleExporter.Definition.Elements
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    [JsonConverter(typeof(TypedElementConverter))]
    public abstract class TypedElement
    {
        /// <summary>
        ///     The type name of the element
        /// </summary>
        [JsonProperty(Order = -10, Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Include)]
        [XmlIgnore]
        public abstract string Type { get; set; }

        /// <summary>
        ///     A unique ID associated with the element. For Inputs the ID will be used as the key for Action.Submit response
        /// </summary>
        [JsonProperty(Order = -9, DefaultValueHandling = DefaultValueHandling.Ignore)]
        [XmlAttribute]
        [DefaultValue(null)]
        public string Id { get; set; }

        /// <summary>
        ///     Additional properties not found on the default schema
        /// </summary>
        [JsonExtensionData]
        [XmlIgnore]
        public IDictionary<string, object> AdditionalProperties { get; set; } =
            new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
    }
}