namespace Kostic017.Pigeon.TAST
{
    /// <summary>
    /// One might put type and related info directly in AST classes as fields that can be
    /// modified appropriately, but I made all AST classes immutable to increase thread safety.
    /// </summary>
    abstract class TypedAstNode
    {
        internal abstract NodeKind Kind { get; }
    }
}
