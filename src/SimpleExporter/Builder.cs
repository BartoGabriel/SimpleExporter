using System.Collections;
using SimpleExporter.Source;

namespace SimpleExporter
{
    public static class Builder
    {
        public static DataSource ToReportDataSource(this IEnumerable source, string id)
        {
            var builder = new EnumerableDataSourceBuilder();
            return builder.BuildDataSource(source, id);
        }
    }
}