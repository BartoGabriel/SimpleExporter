using System.Collections.Generic;

namespace SimpleExporter.Writer.PdfReportWriter
{
    public class PdfReportWriterSetting : IWriterSetting
    {
        public List<FormatTranslator> FormatTranslators { get; set; } = new List<FormatTranslator>();
    }
}