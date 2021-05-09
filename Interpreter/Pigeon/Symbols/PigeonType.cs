namespace Kostic017.Pigeon.Symbols
{
    class PigeonType
    {
        internal static readonly PigeonType Error = new PigeonType("?");
        internal static readonly PigeonType Bool = new PigeonType("bool");
        internal static readonly PigeonType Int = new PigeonType("int");
        internal static readonly PigeonType Float = new PigeonType("float");
        internal static readonly PigeonType String = new PigeonType("string");
        internal static readonly PigeonType Void = new PigeonType("void");

        internal static PigeonType FromName(string name)
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

        internal string Name { get; }

        internal PigeonType(string name)
        {
            Name = name;
        }
    }
}
