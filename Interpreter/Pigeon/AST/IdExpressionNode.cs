using System.Collections.Generic;

namespace Kostic017.Pigeon.AST
{
    class IdExpressionNode : ExpressionNode
    {
        internal SyntaxToken IdToken { get; }

        internal IdExpressionNode(SyntaxToken idToken)
        {
            IdToken = idToken;
        }

        internal override AstNodeKind Kind => AstNodeKind.IdExpression;

        internal override IEnumerable<AstNode> GetChildren()
        {
            yield return new SyntaxTokenWrap(IdToken);
        }
    }
}
