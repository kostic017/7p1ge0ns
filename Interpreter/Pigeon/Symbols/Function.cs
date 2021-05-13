namespace Kostic017.Pigeon.Symbols
{
    public class Function
    {
        internal string Name { get; }
        internal PigeonType ReturnType { get; }
        internal Variable[] Parameters { get; }

        internal Function(PigeonType returnType, string name, Variable[] parameters)
        {
            Name = name;
            ReturnType = returnType;
            Parameters = parameters;
        }
    }
}
