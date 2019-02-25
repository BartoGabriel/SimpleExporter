namespace SimpleExporter.Source
{
    public interface IDataSourceBuilder<TSource>
    {
        DataSource BuildDataSource(TSource source, string id);
    }
}