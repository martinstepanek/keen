using System.Collections.Generic;
using KeenActionParser;

namespace KeenInterpreter.Functions
{
    public abstract class BuiltInFunction
    {
        public abstract string Name { get; }

        public ExpressionResult Execute(List<ExpressionResult> parameters)
        {
            BeforeRun(parameters);
            return Run(parameters);
        }

        protected void BeforeRun(List<ExpressionResult> parameters)
        {
            if (!CheckParameterTypes(parameters))
            {
                // TODO: throw invalid type exception
            }
        }

        private bool CheckParameterTypes(List<ExpressionResult> parameters)
        {
            /*for (int i = 0; i < parameters.Count; i++)
            {
                DataType type = i < ParameterTypes.Count ? ParameterTypes[i] : ParameterTypes[^1];
                if (type != parameters[i].Type)
                {
                    return false;
                }
            }*/

            return true;
        }

        protected abstract ExpressionResult Run(List<ExpressionResult> parameters);

        protected abstract List<DataType> ParameterTypes { get; }
    }
}