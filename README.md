# SimpleExporter
Simple Exporter is an easy report library that generate reports (Excel, CSV, etc)  from any IEnumerable datasource.
Simple Exporter allows defining the structure of the reports from a simple JSON or a dotnet object.

**Note that this is a beta version, and is not yet ready for production.**

**This project started with the source from [DoddleReport](https://github.com/matthidinger/DoddleReport). It was a good project to start from.**

##A Quick Example
SimpleExporter is installed from NuGet.

```
Install-Package SimpleExporter
Install-Package SimpleExporter.Writer.XlsxReportWriter
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
        }
```


