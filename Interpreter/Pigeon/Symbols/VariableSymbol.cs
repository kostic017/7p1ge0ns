namespace Kostic017.Pigeon.Symbols
{
    class VariableSymbol : Symbol
    {
        internal TypeSymbol Type { get; }
        internal bool ReadOnly { get; }

        public VariableSymbol(string name, TypeSymbol type, bool readOnly) : base(name)
        {
            Type = type;
            ReadOnly = readOnly;
        }

        internal override SymbolKind Kind => SymbolKind.Variable;
    }
}
