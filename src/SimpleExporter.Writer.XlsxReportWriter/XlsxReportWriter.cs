using System.Linq;
using OfficeOpenXml;
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

            var pck = new ExcelPackage();

            var ws = pck.Workbook.Worksheets.Add("Report");

            var rowPosition = 1;
            foreach (var bodyElement in SimpleExporter.ReportDefinition.Body)
            {
                if (bodyElement.GetType() == typeof(TextBlock))
                {
                    var textBlock = (TextBlock) bodyElement;
                    ws.Cells[rowPosition, 1].Value = textBlock.Text;
                    rowPosition++;
                }

                if (bodyElement.GetType() == typeof(Table))
                {
                    var table = (Table) bodyElement;

                    var datatable = SimpleExporter.ReportDataSource.ToDataTable(table);
                    ws.Cells[rowPosition, 1].LoadFromDataTable(datatable, table.IncludeHeader);

                    for (var i = 0; i < table.Columns.Count; i++)
                    {
                        var tableColumn = table.Columns[i];
                        if (!string.IsNullOrWhiteSpace(tableColumn.FormatSpecifier))
                            ws.Cells[rowPosition + 1, i + 1, datatable.Rows.Count + 1, i + 1].Style.Numberformat.Format
                                = GetExcelFormat(tableColumn.FormatSpecifier);
                    }

                    rowPosition += datatable.Rows.Count + 1;
                }
            }

            ws.Cells.AutoFitColumns();
            pck.SaveAs(Destination);
        }

        private string GetExcelFormat(string formatSpecifier)
        {
            var translate = Setting.FormatTranslators.SingleOrDefault(f => f.From.Equals(formatSpecifier));
            if (translate != null)
            {
                return translate.To;
            }

            return formatSpecifier;
        }
    }
}