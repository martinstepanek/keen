using System;
using System.Collections.Generic;

namespace KeenInterpreter.Functions
{
    public class PrintFunction : BuiltInFunction
    {
        public override string Name => "print";
        
        public override string Run(List<string> parameters)
        {
            foreach (var parameter in parameters)
            {
                Console.WriteLine(parameter);
            }
            return "1";
        }
    }
}