using System.Collections.Generic;

namespace KeenInterpreter.Functions
{
    public abstract class BuiltInFunction
    {

        public abstract string Name { get; }
        public abstract ExpressionResult Run(List<ExpressionResult> parameters);

    }
}