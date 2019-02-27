using System.IO;
using SimpleExporter.Definition;
using SimpleExporter.Source;
using SimpleExporter.Writer;

namespace SimpleExporter
{
    public class SimpleExporter
    {
        public ReportDefinition ReportDefinition { get; set; }
        public DataSource ReportDataSource { get; set; }

        public static SimpleExporter CreateReport(ReportDefinition reportDefinition)
        {
            return new SimpleExporter
            {
                ReportDefinition = reportDefinition,
                ReportDataSource = null
            };
        }

        public static SimpleExporter CreateReport(ReportDefinition reportDefinition, DataSource reportDataSource)
        {
            return new SimpleExporter
            {
                ReportDefinition = reportDefinition,
                ReportDataSource = reportDataSource
            };
        }

        public void WriteReport(Stream destination, ReportWriterBase writer)
        {
            writer.WriteReport(this, destination);
        }
    }
}