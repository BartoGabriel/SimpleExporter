using System.Collections;

namespace SimpleExporter.Source
{
    public class DataSource
    {
        public string Id { get; set; }
        public IEnumerable Data { get; set; }
        public DataSourceFieldCollection Fields { get; set; }

        //internal string GetFormattedValue(object data, DataSourceField field)
        //{
        //    var internalValue = data.GetType().GetProperty(field.Name).GetValue(data, null);
        //    if (string.IsNullOrWhiteSpace(field.DataFormatString))
        //    {
        //        return internalValue.ToString();
        //    }
        //    else
        //    {
        //        return string.Format(field.DataFormatString, internalValue);
        //    }
        //}
    }
}