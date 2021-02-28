using System;
using System.Collections.Generic;
using KeenActionParser;

namespace KeenInterpreter.Functions
{
    public class PrintFunction : BuiltInFunction
    {
        public override string Name => "print";

        protected override ExpressionResult Run(List<ExpressionResult> parameters)
        {
            foreach (var parameter in parameters)
            {
                if (parameter.Value is StoredScalarVariable scalarVariable)
                {
                    Console.WriteLine(scalarVariable.Value);
                }
                if (parameter.Value is StoredCollectionVariable collectionVariable) // TODO refactor
                {
                    Console.WriteLine(collectionVariable);
                }
            }

            return new ExpressionResult()
            {
                Value = new StoredScalarVariable
                {
                    Value = "1",
                    Type = DataType.Number,
                }
            };
        }

        protected override List<DataType> ParameterTypes => new List<DataType> {DataType.String};
    }
}