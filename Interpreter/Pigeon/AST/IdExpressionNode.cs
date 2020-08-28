namespace Kostic017.Pigeon.AST
{
    class IdExpressionNode : ExpressionNode
    {
        internal string Value { get; }

        internal IdExpressionNode(string value)
        {
            Value = value;
        }

        public override string Print(string ident = "")
        {
            return Value;
        }
    }
}
