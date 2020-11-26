using System;
using System.Collections.Generic;
using KeenAbstractSyntaxParser;
using KeenActionParser.Expressions;
using KeenTokenizer.Tokens;

namespace KeenActionParser
{
    public class ActionParser
    {
        private readonly List<Node> _nodes;
        private List<Expression> _expressions;

        public ActionParser(List<Node> nodes)
        {
            _nodes = nodes;
        }

        public IEnumerable<Expression> GetExpressions()
        {
            Parse();
            return _expressions;
        }

        private void Parse()
        {
            _expressions = new List<Expression>();
            foreach (var node in _nodes)
            {
                _expressions.Add(CreateExpression(node));
            }
        }

        private Expression CreateExpression(Node node)
        {
            if (node.Value is Word)
            {
                if (node.FirstChild != null)
                {
                    if (node.Value.Value == "is")
                    {
                        if (node.SecondChild == null)
                        {
                            // TODO: throw empty assigment exception
                        }

                        var assignment = new Assignment
                        {
                            VariableName = node.FirstChild.Value.Value,
                            Expression = CreateExpression(node.SecondChild)
                        };
                        return assignment;
                    }

                    var function = new Function {Name = node.Value.Value};

                    var firstChildExpression = CreateExpression(node.FirstChild);
                    if (firstChildExpression is StaticType type)
                    {
                        function.StaticType = type;
                    }
                    else
                    {
                        function.Params.Add(firstChildExpression);
                    }

                    if (node.SecondChild != null)
                    {
                        function.Params.Add(CreateExpression(node.SecondChild));
                    }

                    return function;
                }

                if (node.Value.Value == "string")
                {
                    var type = new StaticType {Type = DataType.String};
                    return type;
                }

                if (node.Value.Value == "number")
                {
                    var type = new StaticType {Type = DataType.Number};
                    return type;
                }

                var variable = new Variable {Name = node.Value.Value};
                return variable;
            }

            if (node.Value is Number)
            {
                var literal = new Literal {Value = node.Value.Value, Type = DataType.Number};
                return literal;
            }

            if (node.Value is QuotedContent)
            {
                var literal = new Literal {Value = node.Value.Value, Type = DataType.String};
                return literal;
            }

            throw new Exception("Unexpected token: " + node.Value.GetType());
        }
    }
}