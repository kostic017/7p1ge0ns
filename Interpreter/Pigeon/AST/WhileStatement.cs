using System.Collections.Generic;

namespace Kostic017.Pigeon.AST
{
    internal class WhileStatement : Statement
    {
        internal Expression Condition { get; }
        internal StatementBlock Body { get; }

        public WhileStatement(Expression condition, StatementBlock body)
        {
            Condition = condition;
            Body = body;
        }

        internal override NodeKind Kind => NodeKind.WhileStatement;

        internal override IEnumerable<AstNode> GetChildren()
        {
            yield return Condition;
            yield return Body;
        }
    }
}
