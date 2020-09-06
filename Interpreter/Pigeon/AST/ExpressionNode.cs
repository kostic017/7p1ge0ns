using System.IO;

namespace Kostic017.Pigeon.AST
{
    abstract class ExpressionNode : AstNode
    {
        public override string ToString()
        {
            var writer = new StringWriter();
            Print(this, writer);
            return writer.ToString();
        }

        static void Print(AstNode node, TextWriter writer)
        {
            switch (node.Kind)
            {
                case SyntaxNodeKind.BinaryExpression:
                    PrintBinaryExpression((BinaryExpressionNode)node, writer);
                    break;
                case SyntaxNodeKind.IdentifierExpression:
                    PrintIdentiferExpression((IdentifierExpressionNode)node, writer);
                    break;
                case SyntaxNodeKind.LiteralExpression:
                    PrintLiteralExpression((LiteralExpressionNode)node, writer);
                    break;
                case SyntaxNodeKind.ParenthesizedExpression:
                    PrintParenthesizedExpression((ParenthesizedExpressionNode)node, writer);
                    break;
                case SyntaxNodeKind.UnaryExpression:
                    PrintUnaryExpression((UnaryExpressionNode)node, writer);
                    break;
                default:
                    node.GetType().ToString();
                    break;
            }
        }

        static void PrintBinaryExpression(BinaryExpressionNode node, TextWriter writer)
        {
            writer.Write("(");
            Print(node.Left, writer);
            writer.Write($" {node.Op.Type.GetDescription()} ");
            Print(node.Right, writer);
            writer.Write(")");
        }

        static void PrintIdentiferExpression(IdentifierExpressionNode node, TextWriter writer)
        {
            writer.Write(node.IdentifierToken.Value);
        }

        static void PrintLiteralExpression(LiteralExpressionNode node, TextWriter writer)
        {
            writer.Write(node.LiteralToken.Value);
        }

        static void PrintParenthesizedExpression(ParenthesizedExpressionNode node, TextWriter writer)
        {
            Print(node.Expression, writer);
        }

        static void PrintUnaryExpression(UnaryExpressionNode node, TextWriter writer)
        {
            writer.Write("(");
            writer.Write(node.Op.Type.GetDescription());
            Print(node.Value, writer);
            writer.Write(")");
        }
    }
}
