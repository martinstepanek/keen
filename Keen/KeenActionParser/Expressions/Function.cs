using System.Collections.Generic;

namespace KeenActionParser.Expressions
{
    public class Function : Expression
    {
        public List<Expression> Params = new List<Expression>();
        public string Name { get; set; }

        public bool IsStatic => StaticType != null;

        public StaticType StaticType { get; set; }
    }
}