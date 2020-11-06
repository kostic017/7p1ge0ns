namespace Kostic017.Pigeon.Symbols
{
    class FunctionSymbol : Symbol
    {
        internal TypeSymbol ReturnType { get; }
        internal VariableSymbol[] Parameters { get; }

        internal FunctionSymbol(TypeSymbol returnType, string name, VariableSymbol[] parameters) : base(name)
        {
            ReturnType = returnType;
            Parameters = parameters;
        }

        internal override SymbolKind Kind => SymbolKind.Function;
    }
}
