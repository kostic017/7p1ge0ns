using System.Collections.Generic;

namespace Kostic017.Pigeon.AST
{
    class BinaryExpressionNode : ExpressionNode
    {
        internal ExpressionNode Left { get; }
        internal SyntaxToken Op { get; }
        internal ExpressionNode Right { get; }

        internal BinaryExpressionNode(ExpressionNode left, SyntaxToken op, ExpressionNode right)
        {
            Left = left;
            Op = op;
            Right = right;
        }

        internal override SyntaxNodeKind Kind => SyntaxNodeKind.BinaryExpression;

        internal override IEnumerable<AstNode> GetChildren()
        {
            yield return Left;
            yield return new SyntaxTokenWrap(Op);
            yield return Right;
        }
    }
}
