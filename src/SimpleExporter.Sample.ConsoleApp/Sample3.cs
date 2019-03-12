using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Reflection;
using SimpleExporter.Definition;
using SimpleExporter.Definition.Elements;
using SimpleExporter.Helpers;
using SimpleExporter.Sample.ConsoleApp.DataSource;
using SimpleExporter.Writer;
using SimpleExporter.Writer.XlsxReportWriter;

namespace SimpleExporter.Sample.ConsoleApp
{
    internal static class Sample3
    {
        public static void Run()
        {
            var query = TestRepository.GetAllPerson();

            var reportDataSource = query.ToReportDataSource("Persons");
            var reportDefinition = ReportDefinition.FromJson(
                EmbeddedResource.GetResourceTextFile("Sample3ReportDefinition.json"));

            var report = SimpleExporter.CreateReport(reportDefinition, reportDataSource);

            report.SetTextBlock("titleId", "Titulo de Prueba"); 
            report.SetTextBlock("subtitleId", "Subtitulo");
            var table = (Table)reportDefinition.Body[2];
            table.Columns = GetColumns(query);

            //CSV
            using (var fs = File.Create("Sample3.csv"))
            {
                var writer = new DelimitedTextReportWriter();
                report.WriteReport(fs, writer);
                Console.WriteLine("(CSV) Sample 3 created: {0}", fs.Name);

            }

            //Xlsx
            using (var fs = File.Create("Sample3.xlsx"))
            {
                var writer = new XlsxReportWriter();
                report.WriteReport(fs, writer);
                Console.WriteLine("(Xlsx) Sample 3 created: {0}", fs.Name);
            }
        }


        private static List<Column> GetColumns(IEnumerable dataEnumerable)
        {
            var columns = new List<Column>();
            var itemType = dataEnumerable.GetItemType();
            var properties = GetProperties(itemType);

            if (properties == null)
                return columns;

            foreach (var propInfo in properties)
                columns.Add(new Column
                {
                    Field = propInfo.Name,
                    FormatSpecifier = GetFormatSpecifier(propInfo),
                    Title = GetDisplayName(propInfo)
                });

            return columns;
        }

        private static string GetFormatSpecifier(PropertyInfo property)
        {
            var type = property.PropertyType;

            if (type == typeof(DateTime) || type == typeof(DateTime?))
            {
                return "dd/MM/yyyy";
            }
            if (type == typeof(Double) || type == typeof(Double?))
            {
                return "0.00";
            }

            return "";
        }

        private static string GetDisplayName(PropertyInfo property)
        {
            var attrName = GetAttributeDisplayName(property);
            if (!string.IsNullOrEmpty(attrName))
                return attrName;


            return property.Name;
        }


        private static string GetAttributeDisplayName(PropertyInfo property)
        {
            var atts = property.GetCustomAttributes(
                typeof(DisplayAttribute), true);
            if (atts.Length == 0)
                return null;
            return (atts[0] as DisplayAttribute).GetName();
        }


        private static IEnumerable<PropertyInfo> GetProperties(Type itemType)
        {
            if (itemType == null)
                return null;

            return itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }
    }
}