namespace Kostic017.Pigeon.Symbols
{
    class TypeSymbol : Symbol
    {
        internal static readonly TypeSymbol Error = new TypeSymbol("?");
        internal static readonly TypeSymbol Bool = new TypeSymbol("bool");
        internal static readonly TypeSymbol Int = new TypeSymbol("int");
        internal static readonly TypeSymbol Float = new TypeSymbol("float");
        internal static readonly TypeSymbol String = new TypeSymbol("string");
        internal static readonly TypeSymbol Void = new TypeSymbol("void");

        internal static TypeSymbol FromName(string name)
        {
            switch (name)
            {
                case "bool": return Bool;
                case "int": return Int;
                case "float": return Float;
                case "string": return String;
                case "void": return Void;
                default: return Error;
            }
        }

        private TypeSymbol(string name) : base(name)
        {
        }

        internal override SymbolKind Kind => SymbolKind.Type;
    }
}
