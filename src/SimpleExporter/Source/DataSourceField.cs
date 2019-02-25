using System;

namespace SimpleExporter.Source
{
    public class DataSourceField
    {
        public DataSourceField(string name, Type dataType)
        {
            Name = name;
            DataType = dataType;
        }

        /// <summary>
        ///     Gets the name of the field
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     The type of data contained within this field
        /// </summary>
        public Type DataType { get; set; }
    }
}