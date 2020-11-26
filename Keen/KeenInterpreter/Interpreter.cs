using System;
using System.Collections.Generic;
using System.Linq;
using KeenActionParser.Expressions;
using KeenInterpreter.Functions;
using KeenInterpreter.Functions.Static;

namespace KeenInterpreter
{
    public class Interpreter
    {
        private readonly List<BuiltInFunction> _builtInFunctions = new List<BuiltInFunction>();
        private readonly List<StaticBuiltInFunction> _staticBuiltInFunctions = new List<StaticBuiltInFunction>();
        private readonly Dictionary<string, StoredVariable> _variables = new Dictionary<string, StoredVariable>();

        public Interpreter()
        {
            _builtInFunctions.Add(new PrintFunction());
            _builtInFunctions.Add(new PlusFunction());
            _builtInFunctions.Add(new ConcatFunction());

            _staticBuiltInFunctions.Add(new ReadStringFunction());
            _staticBuiltInFunctions.Add(new ReadNumberFunction());
        }

        public void Run(IEnumerable<Expression> expressions)
        {
            foreach (var expression in expressions)
            {
                Run(expression);
            }
        }

        private ExpressionResult Run(Expression expression)
        {
            switch (expression)
            {
                case Literal literal:
                    return new ExpressionResult
                    {
                        Value = literal.Value,
                        Type = literal.Type,
                    };
                case Variable variable:
                {
                    var storedVariable = _variables[variable.Name];
                    return new ExpressionResult
                    {
                        Value = storedVariable.Value,
                        Type = storedVariable.Type,
                    };
                }
                case Assignment assignment:
                    return RunAssignment(assignment);
                case Function function:
                    return RunFunction(function);
                default:
                    throw new Exception("Unexpected expression: " + expression.GetType());
            }
        }

        private ExpressionResult RunAssignment(Assignment assignment)
        {
            var result = Run(assignment.Expression);
            var storedVariables = new StoredVariable
            {
                Value = result.Value,
                Type = result.Type,
            };
            _variables.Add(assignment.VariableName, storedVariables);
            return new ExpressionResult
            {
                Value = result.Value,
                Type = result.Type,
            };
        }

        private ExpressionResult RunFunction(Function function)
        {
            var builtInFunction = FindFunction(function);

            var functionParams = function.Params.Select(functionParam => Run(functionParam)).ToList();

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