namespace Kostic017.Pigeon.AST
{
    class LiteralExpressionNode : ExpressionNode
    {
        internal object Value { get; }

        internal LiteralExpressionNode(object value)
        {
            Value = value;
        }

        public override string Print(string ident = "")
        {
            return Value.ToString();
        }
    }
}
