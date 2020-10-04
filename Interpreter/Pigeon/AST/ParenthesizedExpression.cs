using System.Collections.Generic;

namespace Kostic017.Pigeon.AST
{
    class ParenthesizedExpression : Expression
    {
        internal Expression Expression { get; }

        internal ParenthesizedExpression(Expression expression)
        {
            Expression = expression;
        }

        internal override NodeKind Kind => NodeKind.ParenthesizedExpression;

        internal override IEnumerable<AstNode> GetChildren()
        {
            yield return Expression;
        }
    }
}
