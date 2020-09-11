using System;
using System.Collections.Generic;
using KeenActionParser;

namespace KeenInterpreter.Functions
{
    public class PrintFunction : BuiltInFunction
    {
        public override string Name => "print";

        public override ExpressionResult Run(List<ExpressionResult> parameters)
        {
            foreach (var parameter in parameters)
            {
                Console.WriteLine(parameter.Value);
            }

            return new ExpressionResult()
            {
                Value = "1",
                Type = DataType.Number,
            };
        }
    }
}