namespace Kostic017.Pigeon.TAST
{
    class TypedExpressionStatement : TypedStatement
    {
        internal TypedExpression Expression { get; }

        internal TypedExpressionStatement(TypedExpression expression)
        {
            Expression = expression;
        }

        internal override NodeKind Kind => NodeKind.ExpressionStatement;
    }
}
