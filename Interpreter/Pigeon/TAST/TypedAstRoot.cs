namespace Kostic017.Pigeon.TAST
{
    class TypedAstRoot : TypedAstNode
    {
        internal TypedStatementBlock StatementBlock { get; }

        internal TypedAstRoot(TypedStatementBlock statementBlock)
        {
            StatementBlock = statementBlock;
        }
        
        internal override NodeKind Kind => NodeKind.AstRoot;
    }
}
