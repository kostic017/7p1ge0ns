using System.Collections.Generic;

namespace Kostic017.Pigeon.AST
{
    class ParenthesizedExpression : Expression
    {
        internal SyntaxToken LeftParen { get; }
        internal Expression Expression { get; }
        internal SyntaxToken RightParen { get; }

        internal ParenthesizedExpression(SyntaxToken leftParen, Expression expression, SyntaxToken rightParen)
        {
            LeftParen = leftParen;
            Expression = expression;
            RightParen = rightParen;
        }

        internal override NodeKind Kind => NodeKind.ParenthesizedExpression;

        internal override IEnumerable<AstNode> GetChildren()
        {
            yield return new SyntaxTokenWrap(LeftParen);
            yield return Expression;
            yield return new SyntaxTokenWrap(RightParen);
        }
    }
}
