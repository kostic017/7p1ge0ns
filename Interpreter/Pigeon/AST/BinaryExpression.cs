using System.Collections.Generic;

namespace Kostic017.Pigeon.AST
{
    class BinaryExpression : Expression
    {
        internal Expression Left { get; }
        internal SyntaxToken Op { get; }
        internal Expression Right { get; }

        internal BinaryExpression(Expression left, SyntaxToken op, Expression right)
        {
            Left = left;
            Op = op;
            Right = right;
        }

        internal override NodeKind Kind => NodeKind.BinaryExpression;

        internal override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Left;
            yield return new SyntaxTokenWrap(Op);
            yield return Right;
        }
    }
}
