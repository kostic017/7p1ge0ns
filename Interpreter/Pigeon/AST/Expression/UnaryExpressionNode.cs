using System.Collections.Generic;

namespace Kostic017.Pigeon.AST
{
    class UnaryExpressionNode : ExpressionNode
    {
        internal SyntaxToken Op { get; }
        internal ExpressionNode Value { get; }

        internal UnaryExpressionNode(SyntaxToken op, ExpressionNode value)
        {
            Op = op;
            Value = value;
        }

        internal override SyntaxNodeKind Kind => SyntaxNodeKind.UnaryExpression;

        internal override IEnumerable<AstNode> GetChildren()
        {
            yield return new SyntaxTokenWrap(Op);
            yield return Value;
        }
    }
}
