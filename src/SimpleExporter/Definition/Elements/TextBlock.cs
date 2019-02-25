namespace SimpleExporter.Definition.Elements
{
    public class TextBlock : BaseElement
    {
        public const string TypeName = "TextBlock";
        public override string Type { get; set; } = TypeName;

        public string Text { get; set; }
    }
}