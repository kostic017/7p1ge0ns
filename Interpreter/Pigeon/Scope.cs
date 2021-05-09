using Kostic017.Pigeon.Symbols;
using System.Collections.Generic;

namespace Kostic017.Pigeon
{
    class Scope
    {
        internal Scope Parent { get; }
        
        readonly Dictionary<string, Variable> variables = new Dictionary<string, Variable>();

        internal Scope(Scope parent)
        {
            Parent = parent;
        }

        internal void Declare(PigeonType type, string name, bool readOnly)
        {
            variables.Add(name, new Variable(type, name, readOnly));
        }

        internal bool TryLookup(string name, out Variable variable)
        {
            variable = null;
            var scope = this;
            while (scope != null)
            {
                if (scope.variables.TryGetValue(name, out variable))
                    return true;
                scope = scope.Parent;
            }
            return false;
        }
    }
}
