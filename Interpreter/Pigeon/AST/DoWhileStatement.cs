using System.Collections.Generic;

namespace Kostic017.Pigeon.AST
{
    internal class DoWhileStatement : Statement
    {
        internal StatementBlock Body { get; }
        internal Expression Condition { get; }

        public DoWhileStatement(StatementBlock body, Expression condition)
        {
            Body = body;
            Condition = condition;
        }

        internal override NodeKind Kind => NodeKind.DoWhileStatement;

        internal override IEnumerable<AstNode> GetChildren()
        {
            yield return Body;
            yield return Condition;
        }
    }
}
