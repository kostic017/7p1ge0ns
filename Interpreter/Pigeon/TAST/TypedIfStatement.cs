namespace Kostic017.Pigeon.TAST
{
    class TypedIfStatement : TypedStatement
    {
        internal TypedExpression Condition { get; }
        internal TypedStatement ThenBlock { get; }
        internal TypedStatement ElseBlock { get; }

        internal TypedIfStatement(TypedExpression condition, TypedStatement thenBlock, TypedStatement elseBlock)
        {
            Condition = condition;
            ThenBlock = thenBlock;
            ElseBlock = elseBlock;
        }

        internal override NodeKind Kind => NodeKind.IfStatement;
    }
}