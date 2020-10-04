using System.Collections.Generic;

namespace Kostic017.Pigeon.AST
{
    class IdentifierExpression : Expression
    {
        internal SyntaxToken IdentifierToken { get; }

        internal IdentifierExpression(SyntaxToken idToken)
        {
            IdentifierToken = idToken;
        }

        internal override NodeKind Kind => NodeKind.IdentifierExpression;

        internal override IEnumerable<AstNode> GetChildren()
        {
            yield return new SyntaxTokenWrap(IdentifierToken);
        }
    }
}
