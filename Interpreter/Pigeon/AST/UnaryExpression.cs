using System.Collections.Generic;

namespace Kostic017.Pigeon.AST
{
    class UnaryExpression : Expression
    {
        internal SyntaxToken Op { get; }
        internal Expression Value { get; }

        internal UnaryExpression(SyntaxToken op, Expression value)
        {
            Op = op;
            Value = value;
        }

        internal override NodeKind Kind => NodeKind.UnaryExpression;

        internal override IEnumerable<AstNode> GetChildren()
        {
            yield return new SyntaxTokenWrap(Op);
            yield return Value;
        }
    }
}
