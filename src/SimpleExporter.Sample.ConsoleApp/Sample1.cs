using System;
using System.IO;
using SimpleExporter.Definition;
using SimpleExporter.Sample.ConsoleApp.DataSource;
using SimpleExporter.Writer;
using SimpleExporter.Writer.XlsxReportWriter;

namespace SimpleExporter.Sample.ConsoleApp
{
    internal static class Sample1
    {
        public static void Run()
        {
            var query = TestRepository.GetAllProducts();

            var reportDataSource = query.ToReportDataSource("Products");
            var reportDefinition = ReportDefinition.FromJson(
                EmbeddedResource.GetResourceTextFile("Sample1ReportDefinition.json"));

            var report = SimpleExporter.CreateReport(reportDefinition, reportDataSource);

            //CSV
            using (var fs = File.Create("Sample1.csv"))
            {
                var writer = new DelimitedTextReportWriter();
                report.WriteReport(fs, writer);
                Console.WriteLine("(CSV) Sample 1 created: {0}", fs.Name);

            }

            //Xlsx
            using (var fs = File.Create("Sample1.xlsx"))
            {
                var writer = new XlsxReportWriter();
                report.WriteReport(fs, writer);
                Console.WriteLine("(Xlsx) Sample 1 created: {0}", fs.Name);
            }
        }
    }
}