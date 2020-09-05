using System.Collections.Generic;

namespace Kostic017.Pigeon.AST
{
    internal class StatementBlockNode : StatementNode
    {
        internal StatementNode[] Statements { get; }

        internal StatementBlockNode(StatementNode[] statements)
        {
            Statements = statements;
        }

        internal override SyntaxNodeKind Kind => SyntaxNodeKind.StatementBlock;

        internal override IEnumerable<AstNode> GetChildren()
        {
            foreach (var statement in Statements)
            {
                yield return statement;
            }
        }
    }
}