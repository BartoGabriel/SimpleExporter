using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SimpleExporter.Definition.Elements;
using SimpleExporter.Writer;

namespace SimpleExporter.Definition
{
    public class ReportDefinition
    {
        public string Title { get; set; }
        public string CultureName { get; set; }
        public ReportPage Page { get; set; }
        public List<BaseElement> Body { get; set; }

        [XmlIgnore]
        public Dictionary<string, IWriterSetting> WritersSettings { get; set; } = new Dictionary<string, IWriterSetting>();

        [JsonProperty("writersSettings")] protected JObject WritersSettingsJson { get; private set; } = null;

        public T GetWriterSetting<T>(string name) where T : IWriterSetting, new()
        {
            if (WritersSettingsJson != null)
            {
                JsonSerializer serializer = new JsonSerializer();
                return (T)serializer.Deserialize(new JTokenReader(WritersSettingsJson[name]), typeof(T));
            }
            else
            {
                if (WritersSettings.ContainsKey(name))
                {
                    return (T)WritersSettings[name];
                }
                else
                {
                    return new T();
                }
            }
        }

        /// <summary>
        ///     Parse an Report Definition from a JSON string
        /// </summary>
        /// <param name="json">A JSON-serialized Report Definition</param>
        /// <returns></returns>
        public static ReportDefinition FromJson(string json)
        {
            return JsonConvert.DeserializeObject<ReportDefinition>(json);
        }
    }
}