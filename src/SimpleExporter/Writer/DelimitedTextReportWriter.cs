using System;
using System.IO;
using System.Text;
using SimpleExporter.Definition.Elements;

namespace SimpleExporter.Writer
{
    public class DelimitedTextReportWriter : ReportWriterBase
    {
        private DelimitedTextReportWriterSetting Setting { get; set; }

        protected override void WriteReport()
        {
            Setting = GetSetting<DelimitedTextReportWriterSetting>();

            var builder = new StringBuilder();

            foreach (var bodyElement in SimpleExporter.ReportDefinition.Body)
            {
                if (bodyElement.GetType() == typeof(TextBlock))
                {
                    var textBlock = (TextBlock)bodyElement;
                    builder.Append(textBlock.Text);
                }

                if (bodyElement.GetType() == typeof(Table))
                {
                    var table = (Table)bodyElement;

                    if (table.IncludeHeader)
                    {
                        foreach (var tableColumn in table.Columns)
                            builder.AppendFormat("{0}{1}",
                                tableColumn.Title, Setting.Delimiter);
                        builder.Remove(builder.Length - 1, 1);
                        builder.Append(Environment.NewLine);
                    }

                    foreach (var data in SimpleExporter.ReportDataSource.Data)
                    {
                        foreach (var tableColumn in table.Columns)
                            builder.AppendFormat("{0}{1}",
                                GetRowDataFormatted(data, tableColumn), Setting.Delimiter);
                        builder.Remove(builder.Length - 1, 1);
                        builder.Append(Environment.NewLine);
                    }
                }

                builder.Append(Environment.NewLine);
            }

            var sw = new StreamWriter(Destination);

            sw.Write(builder);
            sw.Flush();
        }
    }
}