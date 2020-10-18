namespace Kostic017.Pigeon.TAST
{
    class TypedIfStatement : TypedStatement
    {
        internal TypedExpression Condition { get; }
        internal TypedStatementBlock ThenBlock { get; }
        internal TypedStatementBlock ElseBlock { get; }

        internal TypedIfStatement(TypedExpression condition, TypedStatementBlock thenBlock, TypedStatementBlock elseBlock)
        {
            Condition = condition;
            ThenBlock = thenBlock;
            ElseBlock = elseBlock;
        }

        internal override NodeKind Kind => NodeKind.IfStatement;
    }
}