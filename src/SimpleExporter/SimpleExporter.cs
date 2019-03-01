using System;
using System.IO;
using System.Linq;
using SimpleExporter.Definition;
using SimpleExporter.Definition.Elements;
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

        /// <summary>
        ///     Set text property in a TextBlock by id
        /// </summary>
        /// <param name="id">TextBlock Id</param>
        /// <param name="text">Text to be set</param>
        public void SetTextBlock(string id, string text)
        {
            var textBlock =
                ReportDefinition.Body.SingleOrDefault(e =>
                    e.GetType() == typeof(TextBlock) && e.Id != null && e.Id == id);

            if (textBlock == null) throw new Exception("The element id don't found");

            ((TextBlock) textBlock).Text = text;
        }
    }
}