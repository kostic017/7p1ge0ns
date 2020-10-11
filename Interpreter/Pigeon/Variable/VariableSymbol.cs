using System;

namespace Kostic017.Pigeon.Variable
{
    class VariableSymbol
    {
        internal string Name { get; }
        internal Type Type { get; }
        internal bool ReadOnly { get; }

        internal VariableSymbol(string name, Type type, bool readOnly)
        {
            Name = name;
            Type = type;
            ReadOnly = readOnly;
        }
    }
}
