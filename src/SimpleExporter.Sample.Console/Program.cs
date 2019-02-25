using System.IO;
using SimpleExporter.Definition;
using SimpleExporter.Sample.Console.DataSource;
using SimpleExporter.Writer;
using SimpleExporter.Writer.XlsxReportWriter;

namespace SimpleExporter.Sample.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            System.Console.WriteLine("Sample Started");

            var query = ProductRepository.GetAll();

            var reportDataSource = query.ToReportDataSource("Products");
            var reportDefinition = ReportDefinition.FromJson(GetResourceTextFile("reportDefinition.json"));
            
            var report = SimpleExporter.CreateReport(reportDefinition, reportDataSource);

            using (var stream = File.Create("Report.txt"))
            {
                var writer = new DelimitedTextReportWriter();
                report.WriteReport(stream, writer);
            }
            //XlsxReportWriter
            using (var stream = File.Create("Report.xlsx"))
            {
                var writer = new XlsxReportWriter();
                report.WriteReport(stream, writer);
            }

            System.Console.WriteLine("Sample Finish");
            System.Console.ReadLine();
        }

        public static string GetResourceTextFile(string filename)
        {
            string result;

            using (var stream =
                typeof(Program).Assembly.GetManifestResourceStream("SimpleExporter.Sample.Console." + filename))
            {
                using (var sr = new StreamReader(stream))
                {
                    result = sr.ReadToEnd();
                }
            }

            return result;
        }
    }
}