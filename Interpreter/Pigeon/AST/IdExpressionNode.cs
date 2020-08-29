namespace Kostic017.Pigeon.AST
{
    class IdExpressionNode : ExpressionNode
    {
        internal string Value { get; }

        internal IdExpressionNode(string value)
        {
            Value = value;
        }

        public override string AsString(int ident = 0)
        {
            return Value;
        }
    }
}
