using System;
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
            {
                var dataType = source.Fields[tableDefinitionColumn.Field].DataType;
                if (dataType.IsGenericType && dataType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    dataType = Nullable.GetUnderlyingType(dataType);
                    tb.Columns.Add(tableDefinitionColumn.Field, dataType).AllowDBNull = true;
                }
                else
                {
                    tb.Columns.Add(tableDefinitionColumn.Field, dataType);
                }
            }


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