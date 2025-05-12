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
            //Initialize document
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(Destination));
            var doc = new Document(pdfDoc);

            doc.SetFontSize(11);
            var baseFont = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN);
            doc.SetFont(baseFont);

            foreach (var bodyElement in SimpleExporter.ReportDefinition.Body)
            {
                if (bodyElement is TextBlock textBlock)
                {
                    var paragraph = new Paragraph(textBlock.Text);
                    paragraph.SetFontSize(GetFontSize(textBlock.Size));

                    // Configurar fuente según estilo
                    PdfFont styleFont = GetStyleFont(baseFont, textBlock.Bold, textBlock.Italic); // Nuevo método
                    paragraph.SetFont(styleFont);

                    if (textBlock.UnderLine)
                        paragraph.SetUnderline(1f, 0f); // Cambio aquí con parámetros

                    doc.Add(paragraph);
                }

                if (bodyElement is Table table)
                {
                    var pdfTable = new iText.Layout.Element.Table(table.Columns.Count, true);

                    foreach (var tableColumn in table.Columns)
                    {
                        var headerParagraph = new Paragraph(tableColumn.Title).SetFont(
                            PdfFontFactory.CreateFont(StandardFonts.TIMES_BOLD)
                        ); // Fuente en negrita

                        var headerCell = new Cell(1, 1)
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetBackgroundColor(DeviceRgb.BLACK)
                            .SetFontColor(DeviceRgb.WHITE)
                            .Add(headerParagraph); // Cambio en estructura de texto

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

        // Nuevo método para manejar estilos tipográficos
        private PdfFont GetStyleFont(PdfFont baseFont, bool bold, bool italic)
        {
            if (bold && italic)
                return PdfFontFactory.CreateFont(StandardFonts.TIMES_BOLDITALIC);

            if (bold)
                return PdfFontFactory.CreateFont(StandardFonts.TIMES_BOLD);

            if (italic)
                return PdfFontFactory.CreateFont(StandardFonts.TIMES_ITALIC);

            return baseFont;
        }

        private float GetFontSize(TextSize textSize)
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
    }
}
