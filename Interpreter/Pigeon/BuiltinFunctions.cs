using Kostic017.Pigeon.Errors;
using Kostic017.Pigeon.Symbols;
using System.Collections.Generic;

namespace Kostic017.Pigeon
{
    public delegate object BuiltinFunc(params object[] arguments);

    class BuiltinFunctionDeclaration
    {
        internal FunctionSymbol Symbol { get; }
        internal BuiltinFunc Action { get; }

        internal BuiltinFunctionDeclaration(FunctionSymbol function, BuiltinFunc action)
        {
            Symbol = function;
            Action = action;
        }
    }

    public static class BuiltinFunctions
    {
        private static readonly Dictionary<string, BuiltinFunctionDeclaration> functions
            = new Dictionary<string, BuiltinFunctionDeclaration>();

        internal static bool TryLookup(string name, out BuiltinFunctionDeclaration builtinFunction)
        {
            if (functions.TryGetValue(name, out builtinFunction))
                return true;
            return false;
        }

        public static void Register(string prototype, BuiltinFunc action)
        {
            var parts = prototype.TrimEnd(')').Split('(');
            var leftPart = parts[0].Split(' ');

            var returnTypeName = leftPart[0].Trim();
            var functionName = leftPart[1].Trim();

            var returnType = TypeSymbol.FromName(returnTypeName);
            if (returnType == TypeSymbol.Error)
                throw new IllegalUsageException($"Unsupported return type {returnTypeName}");

            var parameters = new List<VariableSymbol>();
            foreach (var parameter in parts[1].Split(','))
            {
                var parameterTypeName = parameter.Trim();
                var parameterType = TypeSymbol.FromName(parameterTypeName);
                if (parameterType == TypeSymbol.Error)
                    throw new IllegalUsageException($"Unsupported parameter type {parameterTypeName}");
                parameters.Add(new VariableSymbol("", parameterType, true));
            }

            var functionSymbol = new FunctionSymbol(returnType, functionName, parameters.ToArray());
            functions.Add(functionName, new BuiltinFunctionDeclaration(functionSymbol, action));
        }
    }
}
