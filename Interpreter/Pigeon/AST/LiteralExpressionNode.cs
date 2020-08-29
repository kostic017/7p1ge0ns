namespace Kostic017.Pigeon.AST
{
    class LiteralExpressionNode : ExpressionNode
    {
        internal SyntaxTokenType Type { get; }
        internal object Value { get; }

        internal LiteralExpressionNode(SyntaxTokenType type, object value)
        {
            Type = type;
            Value = value;
        }

        public override string AsString(int ident = 0)
        {
            return Value.ToString();
        }
    }
}
