namespace Kostic017.Pigeon.Symbols
{
    public class Function
    {
        internal string Name { get; }
        internal PigeonType ReturnType { get; }
        internal Variable[] Parameters { get; }
        internal FuncPointer Func { get; }

        internal Function(PigeonType returnType, string name, Variable[] parameters, FuncPointer func = default)
        {
            Name = name;
            ReturnType = returnType;
            Parameters = parameters;
            Func = func;
        }
    }
}
