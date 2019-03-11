using System.Collections.Generic;
using System.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Table;
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

            ws.Cells.Style.Font.Size = 11; //Default font size for whole sheet
            ws.Cells.Style.Font.Name = "Calibri"; //Default Font name for whole sheet

            var rowPosition = 1;
            var tableId = 1;
            foreach (var bodyElement in SimpleExporter.ReportDefinition.Body)
            {
                if (bodyElement.GetType() == typeof(TextBlock))
                {
                    var textBlock = (TextBlock)bodyElement;
                    ws.Cells[rowPosition, 1].Value = textBlock.Text;
                    ws.Cells[rowPosition, 1].Style.Font.Size = GetFontSize(textBlock.Size);
                    ws.Cells[rowPosition, 1].Style.Font.Bold = textBlock.Bold;
                    ws.Cells[rowPosition, 1].Style.Font.Italic = textBlock.Italic;
                    ws.Cells[rowPosition, 1].Style.Font.UnderLine = textBlock.UnderLine;
                    rowPosition++;
                }

                if (bodyElement.GetType() == typeof(Table))
                {
                    rowPosition += 1;

                    var table = (Table) bodyElement;

                    var datatable = SimpleExporter.ReportDataSource.ToDataTable(table);
                    ws.Cells[rowPosition, 1].LoadFromDataTable(datatable, table.IncludeHeader, TableStyles.Medium1);

                    for (var i = 0; i < table.Columns.Count; i++)
                    {
                        var tableColumn = table.Columns[i];

                        ws.Cells[rowPosition, i + 1].Value = tableColumn.Title;

                        if (!string.IsNullOrWhiteSpace(tableColumn.FormatSpecifier))
                            ws.Cells[rowPosition + 1, i + 1, rowPosition + datatable.Rows.Count, i + 1].Style.Numberformat.Format
                                = GetExcelFormat(tableColumn.FormatSpecifier);
                    }

                    rowPosition += datatable.Rows.Count + 1;
                }
            }

            ws.Cells.AutoFitColumns();
            pck.SaveAs(Destination);
        }

        private float GetFontSize(TextSize textSize)
        {
            switch (textSize)
            {
                case TextSize.ExtraLarge:
                    return 24;
                case TextSize.Large:
                    return 18;
                case TextSize.Medium:
                    return 14;
                case TextSize.Default:
                    return 11;
                case TextSize.Small:
                    return 9;
            }

            return 11;
        }


        private string GetExcelFormat(string formatSpecifier)
        {
            var translate = Setting.FormatTranslators.SingleOrDefault(f => f.From.Equals(formatSpecifier));
            if (translate != null) return translate.To;

            return formatSpecifier;
        }
    }
}