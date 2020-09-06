using System.Collections.Generic;

namespace Kostic017.Pigeon.AST
{
    class IdentifierExpressionNode : ExpressionNode
    {
        internal SyntaxToken IdentifierToken { get; }

        internal IdentifierExpressionNode(SyntaxToken idToken)
        {
            IdentifierToken = idToken;
        }

        internal override SyntaxNodeKind Kind => SyntaxNodeKind.IdentifierExpression;

        internal override IEnumerable<AstNode> GetChildren()
        {
            yield return new SyntaxTokenWrap(IdentifierToken);
        }
    }
}
