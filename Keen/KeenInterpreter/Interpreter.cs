using System;
using System.Collections.Generic;
using KeenActionParser.Expressions;
using KeenInterpreter.Functions;

namespace KeenInterpreter
{
    public class Interpreter
    {
        private List<BuiltInFunction> _builtInFunctions = new List<BuiltInFunction>();
        private Dictionary<string, StoredVariable> _variables = new Dictionary<string, StoredVariable>();

        public Interpreter()
        {
            _builtInFunctions.Add(new PrintFunction());
            _builtInFunctions.Add(new PlusFunction());
            _builtInFunctions.Add(new ConcatFunction());
        }

        public void Run(List<Expression> expressions)
        {
            foreach (var expression in expressions)
            {
                Run(expression);
            }
        }

        public ExpressionResult Run(Expression expression)
        {
            if (expression is Literal)
            {
                return new ExpressionResult()
                {
                    Value = (expression as Literal).Value,
                    Type = (expression as Literal).Type,
                };
            }

            if (expression is Variable)
            {
                var variable = _variables[(expression as Variable).Name];
                return new ExpressionResult()
                {
                    Value = variable.Value,
                    Type = variable.Type,
                };
            }

            if (expression is Assignment)
            {
                var assignment = (expression as Assignment);
                var result = Run(assignment.Expression);
                var storedVariables = new StoredVariable()
                {
                    Value = result.Value,
                    Type = result.Type,
                };
                _variables.Add(assignment.VariableName, storedVariables);
                return new ExpressionResult()
                {
                    Value = result.Value,
                    Type = result.Type,
                };
            }

            if (expression is Function)
            {
                var function = (expression as Function);
                var builtInFunction =
                    _builtInFunctions.Find(_builtInFunction => _builtInFunction.Name == function.Name);
                if (builtInFunction == null)
                {
                    throw new Exception("Unknown function: " + function.Name);
                }

                var functionParams = new List<ExpressionResult>();
                foreach (var functionParam in function.Params)
                {
                    functionParams.Add(Run(functionParam));
                }

                return builtInFunction.Run(functionParams);
            }

            throw new Exception("Unexpected expression: " + expression.GetType());
        }
    }
}