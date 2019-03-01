using System.Globalization;
using System.IO;
using SimpleExporter.Definition.Elements;

namespace SimpleExporter.Writer
{
    public abstract class ReportWriterBase
    {
        protected CultureInfo CultureInfo { get; set; } = CultureInfo.CurrentCulture;
        protected SimpleExporter SimpleExporter { get; set; }
        protected Stream Destination { get; set; }

        public void WriteReport(SimpleExporter simpleExporter, Stream destination)
        {
            SimpleExporter = simpleExporter;
            Destination = destination;

            if (!string.IsNullOrWhiteSpace(simpleExporter.ReportDefinition.CultureName))
                CultureInfo = new CultureInfo(simpleExporter.ReportDefinition.CultureName);

            WriteReport();
        }

        protected abstract void WriteReport();

        protected TSetting GetSetting<TSetting>() where TSetting : IWriterSetting, new()
        {
            var name = GetType().Name;
            name = char.ToLowerInvariant(name[0]) + name.Substring(1);
            return SimpleExporter.ReportDefinition.GetWriterSetting<TSetting>(name);
        }

        protected string GetRowDataFormatted(object data, Column tableColumn)
        {
            var internalValue = data.GetType().GetProperty(tableColumn.Field).GetValue(data, null);

            if (string.IsNullOrWhiteSpace(tableColumn.FormatSpecifier)) return internalValue.ToString();
            var format = $"{{0:{tableColumn.FormatSpecifier}}}";
            return string.Format(CultureInfo, format, internalValue);
        }
    }
}