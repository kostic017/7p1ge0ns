namespace Kostic017.Pigeon.AST
{
    class ParenthesizedExpressionNode : ExpressionNode
    {
        internal ExpressionNode expression;

        public ParenthesizedExpressionNode(ExpressionNode expression)
        {
            this.expression = expression;
        }

        public override string Print(string ident = "")
        {
            return "[" + expression.Print() + "]";
        }
    }
}
