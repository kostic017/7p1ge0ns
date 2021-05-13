using System.Collections.Generic;

namespace Kostic017.Pigeon.Symbols
{
    internal class GlobalScope : Scope
    {
        private readonly Dictionary<string, Function> functions = new Dictionary<string, Function>();

        public GlobalScope() : base(null)
        {
        }

        public Function DeclareFunction(PigeonType returnType, string name, Variable[] parameters)
        {
            var function = new Function(returnType, name, parameters);
            functions.Add(function.Name, function);
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
