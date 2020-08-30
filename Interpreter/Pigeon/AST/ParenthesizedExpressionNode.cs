namespace Kostic017.Pigeon.AST
{
    class ParenthesizedExpressionNode : ExpressionNode
    {
        internal ExpressionNode Expression { get; }

        public ParenthesizedExpressionNode(ExpressionNode expression)
        {
            Expression = expression;
        }
    }
}
