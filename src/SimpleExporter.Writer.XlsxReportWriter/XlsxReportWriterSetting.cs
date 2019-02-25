using System.Collections.Generic;

namespace SimpleExporter.Writer.XlsxReportWriter
{
    public class XlsxReportWriterSetting : IWriterSetting
    {
        public List<FormatTranslator> FormatTranslators { get; set; } = new List<FormatTranslator>();
    }
}
