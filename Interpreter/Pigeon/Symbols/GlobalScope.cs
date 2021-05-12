using System.Collections.Generic;

namespace Kostic017.Pigeon.Symbols
{
    class GlobalScope : Scope
    {
        readonly Dictionary<string, Function> functions = new Dictionary<string, Function>();

        internal GlobalScope() : base(null)
        {
        }

        internal Function DeclareFunction(PigeonType returnType, string name, Variable[] parameters)
        {
            var function = new Function(returnType, name, parameters);
            functions.Add(name, function);
            return function;
        }

        internal bool TryGetFunction(string name, out Function function)
        {
            if (functions.TryGetValue(name, out function))
                return true;
            return false;
        }
    }
}
