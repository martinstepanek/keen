using System;
using System.Collections.Generic;
using KeenActionParser;

namespace KeenInterpreter.Functions.Static
{
    public class ReadStringFunction : StaticBuiltInFunction
    {
        public override string Name => "read";

        public override DataType StaticType => DataType.String;

        protected override ExpressionResult Run(List<ExpressionResult> parameters)
        {
            string value = Console.ReadLine();

            return new ExpressionResult()
            {
                Value = value,
                Type = DataType.String,
            };
        }

        protected override List<DataType> ParameterTypes => new List<DataType> {};
    }
}