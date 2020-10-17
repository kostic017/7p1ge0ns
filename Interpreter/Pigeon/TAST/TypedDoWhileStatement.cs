namespace Kostic017.Pigeon.TAST
{
    class TypedDoWhileStatement : TypedStatement
    {
        internal TypedStatement Body { get; }
        internal TypedExpression Condition { get; }

        internal TypedDoWhileStatement(TypedStatement body, TypedExpression condition)
        {
            Body = body;
            Condition = condition;
        }

        internal override NodeKind Kind => NodeKind.DoWhileStatement;
    }
}
