using System.Collections.Generic;

namespace Kostic017.Pigeon.AST
{
    class ExpressionStatement : Statement
    {
        internal Expression Expression { get; }

        internal ExpressionStatement(Expression expression)
        {
            Expression = expression;
        }

        internal override NodeKind Kind => NodeKind.ExpressionStatement;

        internal override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Expression;
        }
    }
}
