namespace Kostic017.Pigeon.Symbols
{
    class FunctionSymbol : Symbol
    {
        internal PigeonType ReturnType { get; }
        internal Variable[] Parameters { get; }

        internal FunctionSymbol(PigeonType returnType, string name, Variable[] parameters) : base(name)
        {
            ReturnType = returnType;
            Parameters = parameters;
        }

        internal override SymbolKind Kind => SymbolKind.Function;
    }
}
