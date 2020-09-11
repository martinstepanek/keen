using System;
using System.Collections.Generic;
using KeenAbstractSyntaxParser;
using KeenActionParser.Expressions;
using KeenTokenizer.Tokens;

namespace KeenActionParser
{
    public class ActionParser
    {
        public List<Node> _nodes;
        public List<Expression> _expressions;

        public ActionParser(List<Node> nodes)
        {
            _nodes = nodes;
        }

        public List<Expression> GetExpressions()
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
                if (node.FirstChild != null || node.SecondChild != null)
                {
                    if (node.Value.Value == "is")
                    {
                        var assignment = new Assignment();
                        assignment.VariableName = node.FirstChild.Value.Value;
                        assignment.Expression = CreateExpression(node.SecondChild);
                        return assignment;
                    }

                    var function = new Function();
                    function.Name = node.Value.Value;
                    function.Params.Add(CreateExpression(node.FirstChild));
                    if (node.SecondChild != null)
                    {
                        function.Params.Add(CreateExpression(node.SecondChild));
                    }

                    return function;
                }

                var variable = new Variable();
                variable.Name = node.Value.Value;
                return variable;
            }

            if (node.Value is Number)
            {
                var literal = new Literal();
                literal.Value = node.Value.Value;
                literal.Type = DataType.Number;
                return literal;
            }
            if (node.Value is QuotedContent)
            {
                var literal = new Literal();
                literal.Value = node.Value.Value;
                literal.Type = DataType.String;
                return literal;
            }

            throw new Exception("Unexpected token: " + node.Value.GetType());
        }
    }
}