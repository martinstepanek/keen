namespace KeenActionParser.Expressions
{
    public class Literal : Expression
    {
        public string Value { get; set; }
        public DataType Type { get; set; }
        
    }
}