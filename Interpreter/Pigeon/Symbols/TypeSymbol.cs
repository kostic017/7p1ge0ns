namespace Kostic017.Pigeon.Symbols
{
    class TypeSymbol : Symbol
    {
        public static readonly TypeSymbol Bool = new TypeSymbol("bool");
        public static readonly TypeSymbol Int = new TypeSymbol("int");
        public static readonly TypeSymbol Float = new TypeSymbol("float");
        public static readonly TypeSymbol String = new TypeSymbol("string");

        private TypeSymbol(string name) : base(name)
        {
        }

        internal override SymbolKind Kind => SymbolKind.Type;
    }
}
