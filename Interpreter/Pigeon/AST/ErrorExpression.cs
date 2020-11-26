using System.Collections.Generic;
using System.Linq;

namespace Kostic017.Pigeon.AST
{
    class ErrorExpression : Expression
    {
        internal override NodeKind Kind => NodeKind.ErrorExpression;
        internal override IEnumerable<SyntaxNode> GetChildren() => Enumerable.Empty<SyntaxNode>();
    }
}
