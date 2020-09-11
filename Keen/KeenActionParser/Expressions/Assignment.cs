namespace KeenActionParser.Expressions
{
    public class Assignment : Expression
    {
        public Expression Expression { get; set; }
        public string VariableName { get; set; }
    }
}