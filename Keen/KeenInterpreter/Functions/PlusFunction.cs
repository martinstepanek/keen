using System.Collections.Generic;

namespace KeenInterpreter.Functions
{
    public class PlusFunction : BuiltInFunction
    {
        public override string Name => "plus";

        public override string Run(List<string> parameters)
        {
            double sum = 0;

            foreach (var parameter in parameters)
            {
                sum += int.Parse(parameter);
            }

            return sum.ToString();
        }
    }
}