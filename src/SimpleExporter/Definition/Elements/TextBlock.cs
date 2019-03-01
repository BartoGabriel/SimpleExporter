using System.ComponentModel;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace SimpleExporter.Definition.Elements
{
    public class TextBlock : BaseElement
    {
        public const string TypeName = "TextBlock";
        public override string Type { get; set; } = TypeName;

        public string Text { get; set; }

        /// <summary>
        ///     The size of the text
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        [XmlAttribute]
        [DefaultValue(typeof(TextSize), "default")]
        public TextSize Size { get; set; }
        
        /// <summary>
        ///     Font Bold
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        [XmlAttribute]
        [DefaultValue(typeof(bool), "false")]
        public bool Bold { get; set; }

        /// <summary>
        ///     Font Italic
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        [XmlAttribute]
        [DefaultValue(typeof(bool), "false")]
        public bool Italic { get; set; }
        
        /// <summary>
        ///     Font UnderLine
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        [XmlAttribute]
        [DefaultValue(typeof(bool), "false")]
        public bool UnderLine { get; set; }

    }
}