namespace Kostic017.Pigeon.TAST
{
    class TypedDoWhileStatement : TypedStatement
    {
        internal TypedStatementBlock Body { get; }
        internal TypedExpression Condition { get; }

        internal TypedDoWhileStatement(TypedStatementBlock body, TypedExpression condition)
        {
            Body = body;
            Condition = condition;
        }

        internal override NodeKind Kind => NodeKind.DoWhileStatement;
    }
}
