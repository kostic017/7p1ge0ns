namespace Kostic017.Pigeon.TAST
{
    /// <summary>
    /// One might put type info directly in AST classes as a field that he can change
    /// appropriately, but I made all classes immutable to increase thread safety.
    /// </summary>
    abstract class TypedAstNode
    {
        internal abstract NodeKind Kind { get; }
    }
}
