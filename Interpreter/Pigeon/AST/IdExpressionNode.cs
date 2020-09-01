namespace Kostic017.Pigeon.AST
{
    class IdExpressionNode : ExpressionNode
    {
        internal string Value { get; }

        internal IdExpressionNode(string value)
        {
            Value = value;
        }

        internal override AstNodeKind Kind() => AstNodeKind.IdExpression;
    }
}
