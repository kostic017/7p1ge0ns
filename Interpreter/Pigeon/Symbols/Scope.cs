using System.Collections.Generic;

namespace Kostic017.Pigeon.Symbols
{
    internal class Scope
    {
        private readonly Dictionary<string, Variable> variables = new Dictionary<string, Variable>();

        internal Scope Parent { get; }

        internal Scope(Scope parent)
        {
            Parent = parent;
        }

        public Variable DeclareVariable(PigeonType type, string name, bool readOnly)
        {
            var variable = new Variable(type, name, readOnly);
            variables.Add(variable.Name, variable);
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
