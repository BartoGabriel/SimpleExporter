using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using SimpleExporter.Helpers;

namespace SimpleExporter.Source
{
    /// <summary>
    ///     Generate a Report from any IEnumerable
    /// </summary>
    public class EnumerableDataSourceBuilder : IDataSourceBuilder<IEnumerable>
    {
        public DataSource BuildDataSource(IEnumerable source, string id)
        {
            return new DataSource
            {
                Data = source,
                Id = id,
                Fields = GetFields(source)
            };
        }

        private DataSourceFieldCollection GetFields(IEnumerable source)
        {
            var fields = new DataSourceFieldCollection();
            var itemType = source.GetItemType();
            var properties = GetProperties(itemType);

            if (properties == null)
                return fields;

            foreach (var propInfo in properties) fields.Add(new DataSourceField(propInfo.Name, propInfo.PropertyType));

            return fields;
        }

        private static IEnumerable<PropertyInfo> GetProperties(Type itemType)
        {
            if (itemType == null)
                return null;

            return itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }
    }
}