using System;
using System.Collections.Generic;
using KeenActionParser;

namespace KeenInterpreter.Functions.Static
{
    public class ReadNumberFunction : StaticBuiltInFunction
    {
        public override string Name => "read";

        public override DataType StaticType => DataType.Number;

        protected override ExpressionResult Run(List<ExpressionResult> parameters)
        {
            var value = Console.ReadLine();

            if (!int.TryParse(value, out _))
            {
                // TODO: throw invalid input parameter, expected type: number
            }

            return new ExpressionResult
            {
                Value = new StoredScalarVariable
                {
                    Value = value,
                    Type = DataType.Number,
                }
            };
        }

        protected override List<DataType> ParameterTypes => new List<DataType>();
    }
}