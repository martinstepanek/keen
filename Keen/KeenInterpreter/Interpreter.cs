using System;
using System.Collections.Generic;
using KeenActionParser.Expressions;
using KeenInterpreter.Functions;
using KeenInterpreter.Functions.Static;

namespace KeenInterpreter
{
    public class Interpreter
    {
        private List<BuiltInFunction> _builtInFunctions = new List<BuiltInFunction>();
        private List<StaticBuiltInFunction> _staticBuiltInFunctions = new List<StaticBuiltInFunction>();
        private Dictionary<string, StoredVariable> _variables = new Dictionary<string, StoredVariable>();

        public Interpreter()
        {
            _builtInFunctions.Add(new PrintFunction());
            _builtInFunctions.Add(new PlusFunction());
            _builtInFunctions.Add(new ConcatFunction());

            _staticBuiltInFunctions.Add(new ReadStringFunction());
            _staticBuiltInFunctions.Add(new ReadNumberFunction());
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

            if (expression is Function function)
            {
                return RunFunction(function);
            }

            throw new Exception("Unexpected expression: " + expression.GetType());
        }

        private ExpressionResult RunFunction(Function function)
        {
            var builtInFunction = FindFunction(function);

            var functionParams = new List<ExpressionResult>();
            foreach (var functionParam in function.Params)
            {
                functionParams.Add(Run(functionParam));
            }

            return builtInFunction.Execute(functionParams);
        }

        private BuiltInFunction FindFunction(Function function)
        {
            BuiltInFunction builtInFunction;
            if (function.IsStatic)
            {
                builtInFunction =
                    _staticBuiltInFunctions.Find(
                        _builtInFunction =>
                            _builtInFunction.Name == function.Name &&
                            _builtInFunction.StaticType == function.StaticType.Type
                    );
            }
            else
            {
                builtInFunction =
                    _builtInFunctions.Find(_builtInFunction => _builtInFunction.Name == function.Name);
            }

            if (builtInFunction == null)
            {
                throw new Exception("Unknown function: " + function.Name);
            }

            return builtInFunction;
        }
    }
}