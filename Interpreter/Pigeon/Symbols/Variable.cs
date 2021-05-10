namespace Kostic017.Pigeon.Symbols
{
    class Variable
    {
        internal PigeonType Type { get; }
        internal string Name { get; }
        internal bool ReadOnly { get; }

        internal bool IsArray { get; set; }
        internal object Value { get; set; }

        internal Variable(PigeonType type, string name, bool readOnly)
        {
            Type = type;
            Name = name;
            ReadOnly = readOnly;
        }
    }
}
