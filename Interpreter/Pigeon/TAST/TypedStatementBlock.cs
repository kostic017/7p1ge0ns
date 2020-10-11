namespace Kostic017.Pigeon.TAST
{
    class TypedStatementBlock : TypedStatement
    {
        internal TypedStatement[] Statements { get; }

        internal TypedStatementBlock(TypedStatement[] statements)
        {
            Statements = statements;
        }

        internal override NodeKind Kind => NodeKind.StatementBlock;
    }
}
