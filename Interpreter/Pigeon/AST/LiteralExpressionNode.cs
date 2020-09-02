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

        internal override AstNodeKind Kind => AstNodeKind.LiteralExpression;
    }
}
