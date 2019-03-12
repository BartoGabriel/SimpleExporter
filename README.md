# SimpleExporter
Simple Exporter is an easy report library that generate reports (Excel, CSV, PDF, etc) from any IEnumerable datasource.
Simple Exporter allows defining the structure of the reports from a simple JSON or a dotnet object.


**This project started with the source from [DoddleReport](https://github.com/matthidinger/DoddleReport). It was a good project to start from.**

## A Quick Example
SimpleExporter is installed from NuGet.

```
Install-Package SimpleExporter
Install-Package SimpleExporter.Writer.XlsxReportWriter
Install-Package SimpleExporter.Writer.PdfReportWriter
```

Report Definition (JSON):
``` javascript
{
  "cultureName": "es-AR",
  "body": [
    {
      "type": "TextBlock",
      "text": "Title"
    },
    {
      "type": "Table",
      "dataSourceId": "datasource",
      "columns": [
        {
          "field": "Name",
          "title": "Nombre"
        },
        {
          "field": "Description",
          "title": "Descripción"
        },
        {
          "field": "Price",
          "title": "Precio",
          "FormatSpecifier": "C"
        }
      ]
    }
  ],
  "writersSettings": {
    "delimitedTextReportWriter": {
      "delimiter": ";"
    },
    "xlsxReportWriter": {
      "formatTranslators": [
        {
          "from": "C",
          "to": "$###,###,##0.00"
        }
      ]
    }
  }
}
```

Export to txt and xlsx:
``` c#
       public static void Export()
        {
            var query = ProductRepository.GetAll();

            var reportDataSource = query.ToReportDataSource("Products");
            var reportDefinition = ReportDefinition.FromJson(GetResourceTextFile("reportDefinition.json"));
            
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

            //PDF
            using (var fs = File.Create("Sample1.pdf"))
            {
                var writer = new PdfReportWriter();
                report.WriteReport(fs, writer);
                Console.WriteLine("(PDF) Sample 1 created: {0}", fs.Name);
            }
        }
```


