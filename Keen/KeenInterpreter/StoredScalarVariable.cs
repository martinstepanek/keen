using KeenActionParser;

namespace KeenInterpreter
{
    public class StoredScalarVariable : StoredVariable
    {
        public string Value { get; set; }
        public DataType Type { get; set; }
    }
}