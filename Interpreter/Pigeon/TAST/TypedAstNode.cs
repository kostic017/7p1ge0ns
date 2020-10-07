namespace Kostic017.Pigeon.TAST
{
    /// <summary>
    /// One might put type info directly in AST classes as a field that can be changed
    /// appropriately, but I made all classes immutable to increase thread safety, so
    /// we kinda need to duplicate the structure while adding more data.
    /// </summary>
    abstract class TypedAstNode
    {
        internal abstract NodeKind Kind { get; }
    }
}
