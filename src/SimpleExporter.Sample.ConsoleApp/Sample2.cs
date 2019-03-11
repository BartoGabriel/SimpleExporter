using System;
using System.IO;
using SimpleExporter.Definition;
using SimpleExporter.Sample.ConsoleApp.DataSource;
using SimpleExporter.Writer;
using SimpleExporter.Writer.XlsxReportWriter;

namespace SimpleExporter.Sample.ConsoleApp
{
    internal static class Sample2
    {
        public static void Run()
        { 
            var query = TestRepository.GetAllProducts();

            var reportDataSource = query.ToReportDataSource("Products");
            var reportDefinition = ReportDefinition.FromJson(
                EmbeddedResource.GetResourceTextFile("Sample2ReportDefinition.json"));

            var report = SimpleExporter.CreateReport(reportDefinition, reportDataSource);

            report.SetTextBlock("titleId", "Titulo de Prueba"); 
            report.SetTextBlock("subtitleId", "Other text for the sample"); 

            //CSV
            using (var fs = File.Create("Sample2.csv"))
            {
                var writer = new DelimitedTextReportWriter();
                report.WriteReport(fs, writer);
                Console.WriteLine("(CSV) Sample 2 created: {0}", fs.Name);

            }

            //Xlsx
            using (var fs = File.Create("Sample2.xlsx"))
            {
                var writer = new XlsxReportWriter();
                report.WriteReport(fs, writer);
                Console.WriteLine("(Xlsx) Sample 2 created: {0}", fs.Name);
            }
        }
    }
}