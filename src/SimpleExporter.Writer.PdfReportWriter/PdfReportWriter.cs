using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using SimpleExporter.Definition.Elements;
using SimpleExporter.Helpers;
using System.Data;
using System.Linq;

namespace SimpleExporter.Writer.PdfReportWriter
{
    public class PdfReportWriter : ReportWriterBase
    {
        private PdfReportWriterSetting Setting { get; set; }

        protected override void WriteReport()
        {
            Setting = GetSetting<PdfReportWriterSetting>();

            PdfWriter writer = new PdfWriter(Destination);

            //Initialize document
            PdfDocument pdfDoc = new PdfDocument(writer);
            Document doc = new Document(pdfDoc);

            doc.SetFontSize(11);
            PdfFont font = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN);
            doc.SetFont(font);

            foreach (var bodyElement in SimpleExporter.ReportDefinition.Body)
            {
                if (bodyElement.GetType() == typeof(TextBlock))
                {
                    var textBlock = (TextBlock)bodyElement;
                    var paragraph = new Paragraph(textBlock.Text);
                    paragraph.SetFontSize(GetFontSize(textBlock.Size));

                    if (textBlock.Bold)
                        paragraph.SetBold();

                    if (textBlock.Italic)
                        paragraph.SetItalic();

                    if (textBlock.UnderLine)
                        paragraph.SetUnderline();

                    //Add paragraph to the document
                    doc.Add(paragraph);
                }

                if (bodyElement.GetType() == typeof(Definition.Elements.Table))
                {
                    var table = (Definition.Elements.Table)bodyElement;
                    var datatable = SimpleExporter.ReportDataSource.ToDataTable(table);
                    Cell cell = null;
                    iText.Layout.Element.Table pdfTable = new iText.Layout.Element.Table(datatable.Columns.Count);

                    // render header cells
                    foreach (var dc in datatable.Columns.OfType<DataColumn>())
                    {
                        cell = new Cell(1, 1)
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetBackgroundColor(new DeviceRgb(0, 0, 0))
                                .SetFontColor(new DeviceRgb(255, 255, 255))
                                .Add(new Paragraph(dc.ColumnName.ToString()));

                        pdfTable.AddCell(cell);
                    }

                    // render data rows
                    foreach (var dr in datatable.Rows.OfType<DataRow>())
                    {
                        var colIdx = 0;
                        foreach (var field in dr.ItemArray)
                        {
                            string value = "";
                            if (!string.IsNullOrWhiteSpace(table.Columns[colIdx].FormatSpecifier))
                            {
                                string formatPattern = GetFormat(table.Columns[colIdx].FormatSpecifier);
                                value = string.Format("{0:" + formatPattern + "}", field);
                            }
                            else
                            {
                                value = field.ToString();
                            }

                            cell = new Cell(1, 1)
                                .SetTextAlignment(TextAlignment.CENTER)
                                .Add(new Paragraph(value));

                            pdfTable.AddCell(cell);

                            colIdx++;
                        }
                    }

                    doc.Add(pdfTable);
                }
            }

            //Close document
            doc.Close();
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

        private string GetFormat(string formatSpecifier)
        {
            var translate = Setting.FormatTranslators.SingleOrDefault(f => f.From.Equals(formatSpecifier));
            if (translate != null) return translate.To;

            return formatSpecifier;
        }
    }
}