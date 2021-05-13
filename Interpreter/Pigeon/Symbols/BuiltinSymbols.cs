using System.Collections.Generic;

namespace Kostic017.Pigeon.Symbols
{
    class BuiltinFunction
    {
        internal Function Function { get; }
        internal FuncPointer Pointer { get; }

        internal BuiltinFunction(Function function, FuncPointer pointer)
        {
            Function = function;
            Pointer = pointer;
        }
    }

    class BuiltinVariable
    {
        internal Variable Variable { get; }
        internal object Value { get; }

        internal BuiltinVariable(Variable variable, object value)
        {
            Variable = variable;
            Value = value;
        }
    }

    public class BuiltinSymbols
    {
        private readonly Dictionary<string, BuiltinVariable> variables = new Dictionary<string, BuiltinVariable>();
        private readonly Dictionary<string, BuiltinFunction> functions = new Dictionary<string, BuiltinFunction>();

        internal void Register(GlobalScope globalScope)
        {
            foreach (var bv in variables.Values)
                globalScope.DeclareVariable(bv.Variable.Type, bv.Variable.Name, bv.Variable.ReadOnly);
            foreach (var bf in functions.Values)
                globalScope.DeclareFunction(bf.Function.ReturnType, bf.Function.Name, bf.Function.Parameters);
        }

        public void RegisterVariable(PigeonType type, string name, bool readOnly, object value)
        {
            variables.Add(name, new BuiltinVariable(new Variable(type, name, readOnly), value));
        }

        public void RegisterFunction(PigeonType returnType, string name, Variable[] parameters, FuncPointer func)
        {
            functions.Add(name, new BuiltinFunction(new Function(returnType, name, parameters), func));
        }
    }
}
