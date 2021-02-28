using System.Collections.Generic;

namespace KeenInterpreter
{
    public class StoredCollectionVariable : StoredVariable
    {
        public List<StoredVariable> StoredVariables { get; set; }
    }
}