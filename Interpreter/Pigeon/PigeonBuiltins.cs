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

    public static class PigeonBuiltins
    {
        private static readonly Dictionary<string, BuiltinFunctionDeclaration> functions
            = new Dictionary<string, BuiltinFunctionDeclaration>();

        internal static bool TryLookup(string name, out BuiltinFunctionDeclaration builtinFunction)
        {
            if (functions.TryGetValue(name, out builtinFunction))
                return true;
            return false;
        }

        public static void RegisterFunction(string prototype, BuiltinFunc action)
        {
            /*
            "          int         add          (               int         ,       int                )            "
            
            "int         add          "
            "               int         ,       int                "
            */

            var parts = prototype.Trim().TrimEnd(')').Split('(');
            var leftPart = parts[0].Split(' ');

            var returnTypeName = leftPart[0].Trim();
            var functionName = leftPart[1].Trim();

            var returnType = PigeonType.FromName(returnTypeName);
            if (returnType == PigeonType.Error)
                throw new IllegalUsageException($"Unsupported return type {returnTypeName}");

            var parameters = new List<Variable>();
            foreach (var parameter in parts[1].Split(','))
            {
                var parameterTypeName = parameter.Trim();
                var parameterType = PigeonType.FromName(parameterTypeName);
                if (parameterType == PigeonType.Error)
                    throw new IllegalUsageException($"Unsupported parameter type {parameterTypeName}");
                parameters.Add(new Variable("", parameterType, true));
            }

            var functionSymbol = new FunctionSymbol(returnType, functionName, parameters.ToArray());
            functions.Add(functionName, new BuiltinFunctionDeclaration(functionSymbol, action));
        }
    }
}
