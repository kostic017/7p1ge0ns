using System.Collections.Generic;

namespace Kostic017.Pigeon.Symbols
{
    class Scope
    {
        internal Scope Parent { get; }
        
        readonly Dictionary<string, Variable> variables = new Dictionary<string, Variable>();

        internal Scope(Scope parent)
        {
            Parent = parent;
        }

        internal Variable DeclareVariable(PigeonType type, string name, bool readOnly)
        {
            var variable = new Variable(type, name, readOnly);
            variables.Add(name, variable);
            return variable;
        }

        internal bool IsVariableDeclaredHere(string name)
        {
            return variables.ContainsKey(name);
        }

        internal bool TryGetVariable(string name, out Variable variable)
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
