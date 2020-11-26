using System.Collections.Generic;

namespace Kostic017.Pigeon.AST
{
    class StatementBlock : Statement
    {
        internal Statement[] Statements { get; }

        internal StatementBlock(Statement[] statements)
        {
            Statements = statements;
        }

        internal override NodeKind Kind => NodeKind.StatementBlock;

        internal override IEnumerable<SyntaxNode> GetChildren()
        {
            foreach (var statement in Statements)
            {
                yield return statement;
            }
        }
    }
}