using KeenActionParser;

namespace KeenInterpreter.Functions.Static
{
    public abstract class StaticBuiltInFunction : BuiltInFunction
    {
        public virtual DataType StaticType { get; }
        
    }
}