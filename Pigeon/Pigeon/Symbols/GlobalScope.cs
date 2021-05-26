﻿using System.Collections.Generic;

namespace Kostic017.Pigeon.Symbols
{
    class GlobalScope : Scope
    {
        private readonly Dictionary<string, Function> functions = new Dictionary<string, Function>();

        internal GlobalScope() : base(null)
        {
        }

        internal Function DeclareFunction(PigeonType returnType, string name, Variable[] parameters, object funcBody)
        {
            var function = new Function(returnType, name, parameters, funcBody);
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
