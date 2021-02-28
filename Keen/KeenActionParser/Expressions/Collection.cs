using System.Collections.Generic;

namespace KeenActionParser.Expressions
{
    public class Collection : Expression
    {
        public List<Expression> Expressions { get; set; }
        public DataType Type { get; set; }
        
    }
}