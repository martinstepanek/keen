using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using KeenActionParser;

namespace KeenInterpreter.Functions
{
    public class ConcatFunction : BuiltInFunction
    {
        public override string Name => "concat";

        protected override ExpressionResult Run(List<ExpressionResult> parameters)
        {
            string result = "";
            foreach (var parameter in parameters)
            {
                result += Concat(parameter.Value);
            }

            return new ExpressionResult
            {
                Value = new StoredScalarVariable
                {
                    Value = result,
                    Type = DataType.String,
                }
            };
        }

        private string Concat(StoredVariable storedVariable)
        {
            if (storedVariable is StoredScalarVariable storedScalarVariable)
            {
                return storedScalarVariable.Value;
            }

            string result = "";
            if (storedVariable is StoredCollectionVariable storedCollectionVariable)
            {
                foreach (var variable in storedCollectionVariable.StoredVariables)
                {
                    result += Concat(variable);
                }
            }

            return result;
        }

        protected override List<DataType> ParameterTypes => new List<DataType> {DataType.String};
    }
}