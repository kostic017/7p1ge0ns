using System.Collections.Generic;

namespace Kostic017.Pigeon.AST
{
    class LiteralExpressionNode : ExpressionNode
    {
        internal SyntaxToken LiteralToken { get; }
        internal object ParsedValue { get; }

        internal LiteralExpressionNode(SyntaxToken literalToken, object parsedValue)
        {
            LiteralToken = literalToken;
            ParsedValue = parsedValue;
        }

        internal override AstNodeKind Kind => AstNodeKind.LiteralExpression;

        internal override IEnumerable<AstNode> GetChildren()
        {
            yield return new SyntaxTokenWrap(LiteralToken);
        }
    }
}
