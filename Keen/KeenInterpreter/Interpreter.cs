using System.Collections.Generic;
using KeenActionParser.Expressions;
using KeenInterpreter.Functions;

namespace KeenInterpreter
{
    public class Interpreter
    {
        private List<BuiltInFunction> _builtInFunctions = new List<BuiltInFunction>();
        private Dictionary<string, string> _variables = new Dictionary<string, string>();

        public Interpreter()
        {
            _builtInFunctions.Add(new PrintFunction());
            _builtInFunctions.Add(new PlusFunction());
        }

        public void Run(List<Expression> expressions)
        {
            foreach (var expression in expressions)
            {
                Run(expression);
            }
        }

        public string Run(Expression expression)
        {
            if (expression is Literal)
            {
                return (expression as Literal).Value;
            }

            if (expression is Variable)
            {
                return _variables[(expression as Variable).Name];
            }

            if (expression is Assignment)
            {
                var assignment = (expression as Assignment);
                _variables.Add(assignment.VariableName, Run(assignment.Expression));
            }

            if (expression is Function)
            {
                var function = (expression as Function);
                var builtInFunction = _builtInFunctions.Find(_builtInFunction => _builtInFunction.Name == function.Name);

                var functionParams = new List<string>();
                foreach (var functionParam in function.Params)
                {    
                    functionParams.Add(Run(functionParam));
                }
                return builtInFunction.Run(functionParams);
            }

            return "";
        }
    }
}