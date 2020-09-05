using System.Collections.Generic;

namespace Kostic017.Pigeon.AST
{
    abstract class AstNode
    {
        internal abstract SyntaxNodeKind Kind { get; }
        internal abstract IEnumerable<AstNode> GetChildren();
    }
}
