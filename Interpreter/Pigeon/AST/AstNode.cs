namespace Kostic017.Pigeon.AST
{
    public abstract class AstNode
    {
        internal abstract AstNodeKind Kind();

        public override string ToString()
        {
            return PrettyPrint(this);
        }

        static string PrettyPrint(AstNode node, string ident = "")
        {
            switch (node.Kind())
            {
                case AstNodeKind.BinaryExpression:
                    return PrettyPrintBinaryExpression((BinaryExpressionNode)node);
                case AstNodeKind.IdExpression:
                    return PrettyPrintIdExpression((IdExpressionNode)node);
                case AstNodeKind.LiteralExpression:
                    return PrettyPrintLiteralExpression((LiteralExpressionNode)node);
                case AstNodeKind.ParenthesizedExpression:
                    return PrettyPrintParenthesizedExpression((ParenthesizedExpressionNode)node);
                case AstNodeKind.UnaryExpression:
                    return PrettyPrintUnaryExpression((UnaryExpressionNode)node);
                default:
                    return node.GetType().ToString();
            }
        }

        static string PrettyPrintBinaryExpression(BinaryExpressionNode node)
        {
            return $"({PrettyPrint(node.Left)} {node.Op.PrettyPrint()} {PrettyPrint(node.Right)})";
        }

        static string PrettyPrintIdExpression(IdExpressionNode node)
        {
            return node.Value;
        }

        static string PrettyPrintLiteralExpression(LiteralExpressionNode node)
        {
            return node.Value.ToString();
        }

        static string PrettyPrintParenthesizedExpression(ParenthesizedExpressionNode node)
        {
            return $"[{PrettyPrint(node.Expression)}]";
        }

        static string PrettyPrintUnaryExpression(UnaryExpressionNode node)
        {
            return $"({node.Op.PrettyPrint()}{PrettyPrint(node.Value)})";
        }
    }
}
