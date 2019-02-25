using System.Collections.Generic;

namespace SimpleExporter.Definition.Elements
{
    public class Table : BaseElement
    {
        public const string TypeName = "Table";
        public override string Type { get; set; } = TypeName;

        public string DataSourceId { get; set; }

        public bool IncludeHeader { get; set; } = true;

        public List<Column> Columns { get; set; } = new List<Column>();
    }
}