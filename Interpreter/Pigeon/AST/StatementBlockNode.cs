namespace Kostic017.Pigeon.AST
{
    internal class StatementBlockNode : StatementNode
    {
        internal StatementNode[] Statements { get; }

        public StatementBlockNode(StatementNode[] statements)
        {
            Statements = statements;
        }

        internal override AstNodeKind Kind => AstNodeKind.StatementBlock;
    }
}