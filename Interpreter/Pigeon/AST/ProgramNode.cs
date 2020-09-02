namespace Kostic017.Pigeon.AST
{
    class ProgramNode : AstNode
    {
        internal StatementBlockNode StatementBlock { get; }

        internal ProgramNode(StatementBlockNode statementBlock)
        {
            StatementBlock = statementBlock;
        }

        internal override AstNodeKind Kind => AstNodeKind.Program;
    }
}
