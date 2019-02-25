using System.Data;
using SimpleExporter.Definition.Elements;
using SimpleExporter.Source;

namespace SimpleExporter.Helpers
{
    public static class DataSourceExtensions
    {
        public static DataTable ToDataTable(this DataSource source, Table tableDefinition)
        {
            var tb = new DataTable(source.Id);

            foreach (var tableDefinitionColumn in tableDefinition.Columns)
                tb.Columns.Add(tableDefinitionColumn.Field, source.Fields[tableDefinitionColumn.Field].DataType);

            foreach (var item in source.Data)
            {
                var values = new object[tableDefinition.Columns.Count];
                for (var i = 0; i < tableDefinition.Columns.Count; i++)
                    values[i] = item.GetType().GetProperty(tableDefinition.Columns[i].Field).GetValue(item, null);

                tb.Rows.Add(values);
            }

            return tb;
        }
    }
}