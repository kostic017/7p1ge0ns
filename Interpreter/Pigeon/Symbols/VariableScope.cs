using System.Collections.Generic;

namespace Kostic017.Pigeon.Symbols
{
    class VariableScope
    {
        readonly Dictionary<string, VariableSymbol> variables = new Dictionary<string, VariableSymbol>();

        internal VariableScope Parent { get; }

        internal VariableScope(VariableScope parent)
        {
            Parent = parent;
        }

        internal bool TryDeclare(VariableSymbol variable)
        {
            if (variables.ContainsKey(variable.Name))
                return false;
            variables.Add(variable.Name, variable);
            return true;
        }

        internal bool TryLookup(string name, out VariableSymbol variable)
        {
            if (variables.TryGetValue(name, out variable))
                return true;

            if (Parent == null)
                return false;

            return Parent.TryLookup(name, out variable);
        }
    }
}
