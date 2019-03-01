using Newtonsoft.Json;

namespace SimpleExporter.Definition.Elements
{
    /// <summary>
    ///     Controls the relative size of TextBlock elements
    /// </summary>
    [JsonConverter(typeof(IgnoreDefaultStringEnumConverter<TextSize>), true)]
    public enum TextSize
    {
        /// <summary>
        ///     Default text size
        /// </summary>
        Default = 0,

        /// <summary>
        ///     Smallest text size
        /// </summary>
        Small = 1,

        /// <summary>
        ///     Slightly larger than default
        /// </summary>
        Medium = 2,

        /// <summary>
        ///     Slightly larger then medium
        /// </summary>
        Large = 3,

        /// <summary>
        ///     The largest text size
        /// </summary>
        ExtraLarge = 4
    }
}