using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleExporter.Writer
{
    public class DelimitedTextReportWriterSetting : IWriterSetting
    {
        public string Delimiter { get; set; }
    }
}
