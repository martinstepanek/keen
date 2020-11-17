using System.Collections.Generic;
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
                result += parameter.Value;
            }

            return new ExpressionResult()
            {
                Value = result,
                Type = DataType.String,
            };
        }
        
        protected override List<DataType> ParameterTypes => new List<DataType> {DataType.String};
    }
}