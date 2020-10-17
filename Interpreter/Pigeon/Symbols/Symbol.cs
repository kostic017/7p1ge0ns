namespace Kostic017.Pigeon.Symbols
{
    abstract class Symbol
    {
        internal string Name { get; }

        internal Symbol(string name)
        {
            Name = name;
        }

        internal abstract SymbolKind Kind { get; }
        public override string ToString() => Name;
    }
}
