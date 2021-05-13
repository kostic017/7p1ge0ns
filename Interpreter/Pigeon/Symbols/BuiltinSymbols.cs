using System.Collections.Generic;

namespace Kostic017.Pigeon.Symbols
{
    public class BuiltinSymbols
    {
        private readonly List<Variable> variables = new List<Variable>();
        private readonly List<Function> functions = new List<Function>();

        internal void Register(GlobalScope globalScope)
        {
            foreach (var v in variables)
                globalScope.DeclareVariable(v.Type, v.Name, v.ReadOnly);
            foreach (var f in functions)
                globalScope.DeclareFunction(f.ReturnType, f.Name, f.Parameters, f.Func);
        }

        public void RegisterVariable(PigeonType type, string name, bool readOnly)
        {
            variables.Add(new Variable(type, name, readOnly));
        }

        public void RegisterFunction(PigeonType returnType, string name, Variable[] parameters, FuncPointer func)
        {
            functions.Add(new Function(returnType, name, parameters, func));
        }
    }
}
