namespace Kostic017.Pigeon.TAST
{
    /// <summary>
    /// One might put type and related info directly in AST classes as fields that can be
    /// modified appropriately, but I made all AST classes immutable to increase thread safety.
    /// If this was a compiler and we wanted to do some optimisations before code generation,
    /// we would modify this tree, while leaving alone the tree that represents original input.
    /// </summary>
    abstract class TypedAstNode
    {
        internal abstract NodeKind Kind { get; }
    }
}
