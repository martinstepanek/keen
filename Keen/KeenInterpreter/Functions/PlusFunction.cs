using System.Collections.Generic;
using KeenActionParser;

namespace KeenInterpreter.Functions
{
    public class PlusFunction : BuiltInFunction
    {
        public override string Name => "plus";

        protected override ExpressionResult Run(List<ExpressionResult> parameters)
        {
            double sum = 0;

            foreach (var parameter in parameters)
            {
                sum += int.Parse(parameter.Value);
            }

            return new ExpressionResult()
            {
                Value = sum.ToString(),
                Type = DataType.Number,
            };
        }
        
        protected override List<DataType> ParameterTypes => new List<DataType> {DataType.Number};
    }
}