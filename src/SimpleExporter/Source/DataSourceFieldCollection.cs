using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SimpleExporter.Source
{
    /// <summary>
    ///     Represents the fields of a data source, they will contain all the properties name and the data types of them
    /// </summary>
    public class DataSourceFieldCollection : IEnumerable<DataSourceField>
    {
        private readonly Dictionary<string, DataSourceField> _internalFields;

        public DataSourceFieldCollection()
        {
            _internalFields = new Dictionary<string, DataSourceField>();
        }

        public DataSourceField this[string name]
        {
            get
            {
                if (!_internalFields.ContainsKey(name)) return null;

                return _internalFields[name];
            }
        }

        public DataSourceField this[int index] => _internalFields.Values.ElementAt(index);

        public int Count => _internalFields.Count;


        public IEnumerator<DataSourceField> GetEnumerator()
        {
            return _internalFields.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _internalFields.Values.GetEnumerator();
        }

        public void Add(string fieldName, Type dataType)
        {
            Add(new DataSourceField(fieldName, dataType));
        }

        public void Add(DataSourceField field)
        {
            _internalFields.Add(field.Name, field);
        }

        public bool Contains(string name)
        {
            return _internalFields.ContainsKey(name);
        }
    }
}