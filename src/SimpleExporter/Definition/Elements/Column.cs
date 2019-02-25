namespace SimpleExporter.Definition.Elements
{
    public class Column
    {
        public string Title { get; set; }
        public string Field { get; set; }

        /// <summary>
        ///     A format string to customize how the data is displayed. For example, use "c" for currency.
        ///     Examples for numbers: https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings
        /// </summary>
        public string FormatSpecifier { get; set; }
    }
}