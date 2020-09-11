using System.Collections.Generic;
using KeenActionParser;

namespace KeenInterpreter.Functions
{
    public class PlusFunction : BuiltInFunction
    {
        public override string Name => "plus";

        public override ExpressionResult Run(List<ExpressionResult> parameters)
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
    }
}