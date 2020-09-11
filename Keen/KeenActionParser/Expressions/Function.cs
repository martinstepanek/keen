using System.Collections.Generic;

namespace KeenActionParser.Expressions
{
    public class Function : Expression
    {
        public List<Expression> Params = new List<Expression>();
        public string Name { get; set; }
    }
}