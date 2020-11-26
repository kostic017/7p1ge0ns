using System.Collections.Generic;

namespace Kostic017.Pigeon.AST
{
    class IfStatement : Statement
    {
        internal Expression Condition { get; }
        internal StatementBlock ThenBlock { get; }
        internal StatementBlock ElseBlock { get; }

        internal IfStatement(Expression condition, StatementBlock thenBlock, StatementBlock elseBlock = null)
        {
            Condition = condition;
            ThenBlock = thenBlock;
            ElseBlock = elseBlock;
        }

        internal override NodeKind Kind => NodeKind.IfStatement;

        internal override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Condition;
            yield return ThenBlock;
            if (ElseBlock != null)
                yield return ElseBlock;
        }
    }
}
