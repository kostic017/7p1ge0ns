namespace Kostic017.Pigeon.AST
{
    class BinaryExpressionNode : ExpressionNode
    {
        internal ExpressionNode Left { get; }
        internal SyntaxTokenType Op { get; }
        internal ExpressionNode Right { get; }

        internal BinaryExpressionNode(ExpressionNode left, SyntaxTokenType op, ExpressionNode right)
        {
            Left = left;
            Op = op;
            Right = right;
        }

        public override string AsString(int ident = 0)
        {
            return "(" + Left.AsString() + " " + Op.PrettyPrint() + " " + Right.AsString() + ")";
        }
    }
}
