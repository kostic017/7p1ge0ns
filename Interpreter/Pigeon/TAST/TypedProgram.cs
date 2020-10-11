namespace Kostic017.Pigeon.TAST
{
    class TypedProgram : TypedAstNode
    {
        internal TypedStatementBlock StatementBlock { get; }

        internal TypedProgram(TypedStatementBlock statementBlock)
        {
            StatementBlock = statementBlock;
        }
        
        internal override NodeKind Kind => NodeKind.Program;
    }
}
