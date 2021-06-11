﻿using System.Collections.Generic;

namespace Kostic017.Pigeon.Symbols
{
    public class Builtins
    {
        private readonly Dictionary<string, Variable> variables = new Dictionary<string, Variable>();
        private readonly Dictionary<string, Function> functions = new Dictionary<string, Function>();

        internal void Register(GlobalScope globalScope)
        {
            foreach (var v in variables.Values)
                globalScope.DeclareVariable(v.Type, v.Name, v.ReadOnly, v.Value);
            foreach (var f in functions.Values)
                globalScope.DeclareFunction(f.ReturnType, f.Name, f.Parameters, f.FuncBody);
        }

        public void RegisterVariable(PigeonType type, string name, bool readOnly, object value)
        {
            variables.Add(name, new Variable(type, name, readOnly) { Value = value });
        }

        public void RegisterFunction(PigeonType returnType, string name, Variable[] parameters, FuncPointer funcPointer)
        {
            if (returnType == PigeonType.Any)
            {
                functions.Add(name, new Function(PigeonType.String, name, parameters, funcPointer));
                functions.Add(name + "_i", new Function(PigeonType.Int, name + "_i", parameters, funcPointer));
                functions.Add(name + "_f", new Function(PigeonType.Float, name + "_f", parameters, funcPointer));
                functions.Add(name + "_b", new Function(PigeonType.Bool, name + "_b", parameters, funcPointer));
            }
            else
            {
                functions.Add(name, new Function(returnType, name, parameters, funcPointer));
            }
        }
    }
}
