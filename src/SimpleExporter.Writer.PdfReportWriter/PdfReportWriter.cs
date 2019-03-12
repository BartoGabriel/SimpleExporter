using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using SimpleExporter.Definition.Elements;
using Table = SimpleExporter.Definition.Elements.Table;

namespace SimpleExporter.Writer.PdfReportWriter
{
    public class PdfReportWriter : ReportWriterBase
    {
        private PdfReportWriterSetting Setting { get; set; }

        protected override void WriteReport()
        {
            Setting = GetSetting<PdfReportWriterSetting>();

            var writer = new PdfWriter(Destination);

            //Initialize document
            var pdfDoc = new PdfDocument(writer);
            var doc = new Document(pdfDoc);

            doc.SetFontSize(11);
            var font = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN);
            doc.SetFont(font);

            foreach (var bodyElement in SimpleExporter.ReportDefinition.Body)
            {
                if (bodyElement.GetType() == typeof(TextBlock))
                {
                    var textBlock = (TextBlock) bodyElement;
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

                if (bodyElement.GetType() == typeof(Table))
                {
                    var table = (Table) bodyElement;
                    var pdfTable = new iText.Layout.Element.Table(table.Columns.Count);

                    // render header cells
                    foreach (var tableColumn in table.Columns)
                    {
                        var headerCell = new Cell(1, 1)
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetBackgroundColor(new DeviceRgb(0, 0, 0))
                            .SetFontColor(new DeviceRgb(255, 255, 255))
                            .Add(new Paragraph(tableColumn.Title));

                        pdfTable.AddCell(headerCell);
                    }

                    foreach (var data in SimpleExporter.ReportDataSource.Data)
                    foreach (var tableColumn in table.Columns)
                    {
                        var rowCell = new Cell(1, 1)
                            .SetTextAlignment(TextAlignment.CENTER)
                            .Add(new Paragraph(GetRowDataFormatted(data, tableColumn)));

                        pdfTable.AddCell(rowCell);
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
    }
}