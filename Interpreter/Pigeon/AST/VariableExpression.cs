using System.Collections.Generic;

namespace Kostic017.Pigeon.AST
{
    class VariableExpression : Expression
    {
        internal SyntaxToken IdentifierToken { get; }

        internal VariableExpression(SyntaxToken idToken)
        {
            IdentifierToken = idToken;
        }

        internal override NodeKind Kind => NodeKind.VariableExpression;

        internal override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return new SyntaxTokenWrap(IdentifierToken);
        }
    }
}
