using System.Collections.Generic;

namespace Kostic017.Pigeon.AST
{
    class ParenthesizedExpressionNode : ExpressionNode
    {
        internal ExpressionNode Expression { get; }

        internal ParenthesizedExpressionNode(ExpressionNode expression)
        {
            Expression = expression;
        }

        internal override AstNodeKind Kind => AstNodeKind.ParenthesizedExpression;

        internal override IEnumerable<AstNode> GetChildren()
        {
            yield return Expression;
        }
    }
}
