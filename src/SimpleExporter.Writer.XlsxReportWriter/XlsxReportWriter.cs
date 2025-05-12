using ClosedXML.Excel;
using SimpleExporter.Definition.Elements;
using SimpleExporter.Helpers;

namespace SimpleExporter.Writer.XlsxReportWriter
{
    public class XlsxReportWriter : ReportWriterBase
    {
        private XlsxReportWriterSetting Setting { get; set; }

        protected override void WriteReport()
        {
            Setting = GetSetting<XlsxReportWriterSetting>();

            using (var workbook = new XLWorkbook())
            {
                var ws = workbook.AddWorksheet("Report");

                ws.Style.Font.FontSize = 11; // Default font size
                ws.Style.Font.FontName = "Calibri"; // Default font name

                var rowPosition = 1;
                foreach (var bodyElement in SimpleExporter.ReportDefinition.Body)
                {
                    if (bodyElement is TextBlock textBlock)
                    {
                        var cell = ws.Cell(rowPosition, 1);
                        cell.Value = textBlock.Text;
                        cell.Style.Font.FontSize = GetFontSize(textBlock.Size);
                        cell.Style.Font.Bold = textBlock.Bold;
                        cell.Style.Font.Italic = textBlock.Italic;
                        cell.Style.Font.Underline = XLFontUnderlineValues.Single;
                        rowPosition++;
                    }

                    if (bodyElement is Table table)
                    {
                        rowPosition++;

                        var dataTable = SimpleExporter.ReportDataSource.ToDataTable(table);
                        ws.Cell(rowPosition, 1).InsertTable(dataTable, table.IncludeHeader);

                        for (var i = 0; i < table.Columns.Count; i++)
                        {
                            var tableColumn = table.Columns[i];
                            ws.Cell(rowPosition, i + 1).Value = tableColumn.Title;

                            if (!string.IsNullOrWhiteSpace(tableColumn.FormatSpecifier))
                            {
                                ws.Range(
                                    rowPosition + 1,
                                    i + 1,
                                    rowPosition + dataTable.Rows.Count,
                                    i + 1
                                ).Style.NumberFormat.Format = GetExcelFormat(
                                    tableColumn.FormatSpecifier
                                );
                            }
                        }

                        rowPosition += dataTable.Rows.Count + 1;
                    }
                }

                ws.Columns().AdjustToContents();
                workbook.SaveAs(Destination);
            }
        }

        private double GetFontSize(TextSize textSize)
        {
            return textSize switch
            {
                TextSize.ExtraLarge => 24,
                TextSize.Large => 18,
                TextSize.Medium => 14,
                TextSize.Default => 11,
                TextSize.Small => 9,
                _ => 11
            };
        }

        private string GetExcelFormat(string formatSpecifier)
        {
            var translate = Setting.FormatTranslators.SingleOrDefault(f =>
                f.From.Equals(formatSpecifier)
            );
            return translate != null ? translate.To : formatSpecifier;
        }
    }
}
