using System.Collections.Generic;
using System.Linq;
using KeenActionParser;

namespace KeenInterpreter.Functions
{
    public class PlusFunction : BuiltInFunction
    {
        public override string Name => "plus";

        protected override ExpressionResult Run(List<ExpressionResult> parameters)
        {
            double sum =
                parameters.Aggregate<ExpressionResult, double>(0,
                    (current, parameter) => current + int.Parse((parameter.Value as StoredScalarVariable).Value)); // TODO refactor

            return new ExpressionResult()
            {
                Value = new StoredScalarVariable
                {
                    Value = sum.ToString(),
                    Type = DataType.Number,
                }
            };
        }

        protected override List<DataType> ParameterTypes => new List<DataType> {DataType.Number};
    }
}