using System.Collections.Generic;

namespace Kostic017.Pigeon.Symbols
{
    class Scope
    {
        internal Scope Parent { get; }
        
        readonly Dictionary<string, Variable> variables = new Dictionary<string, Variable>();
        readonly Dictionary<string, Function> functions = new Dictionary<string, Function>();

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

        internal Function DeclareFunction(PigeonType returnType, string name, Variable[] parameters)
        {
            var function = new Function(returnType, name, parameters);
            functions.Add(name, function);
            return function;
        }

        internal bool IsVariableDeclaredHere(string name)
        {
            return variables.ContainsKey(name);
        }

        internal bool TryLookupVariable(string name, out Variable variable)
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

        internal bool TryLookupFunction(string name, out Function function)
        {
            function = null;
            var scope = this;
            while (scope != null)
            {
                if (scope.functions.TryGetValue(name, out function))
                    return true;
                scope = scope.Parent;
            }
            return false;
        }
    }
}
