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

        public override string Print(string ident = "")
        {
            return "(" + Left.Print() + " " + Op.ToString() + " " + Right.Print() + ")";
        }
    }
}
