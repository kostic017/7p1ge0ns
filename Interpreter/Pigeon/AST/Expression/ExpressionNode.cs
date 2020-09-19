using System.IO;

namespace Kostic017.Pigeon.AST
{
    internal abstract class ExpressionNode : AstNode
    {
        public override string ToString()
        {
            var writer = new StringWriter();
            Print(this, writer);
            return writer.ToString();
        }

        private static void Print(AstNode node, TextWriter writer)
        {
            switch (node.Kind)
            {
                case SyntaxNodeKind.AssignmentExpression:
                    PrintAssignmentExpression((AssignmentExpressionNode)node, writer);
                    break;
                case SyntaxNodeKind.IdentifierExpression:
                    PrintIdentiferExpression((IdentifierExpressionNode)node, writer);
                    break;
                case SyntaxNodeKind.BinaryExpression:
                    PrintBinaryExpression((BinaryExpressionNode)node, writer);
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

        private static void PrintBinaryExpression(BinaryExpressionNode node, TextWriter writer)
        {
            writer.Write("(");
            Print(node.Left, writer);
            writer.Write($" {node.Op.Type.GetDescription()} ");
            Print(node.Right, writer);
            writer.Write(")");
        }

        private static void PrintAssignmentExpression(AssignmentExpressionNode node, TextWriter writer)
        {
            writer.Write("(");
            writer.Write(node.IdentifierToken.Value);
            writer.Write(" = ");
            Print(node.Value, writer);
            writer.Write(")");
        }

        private static void PrintIdentiferExpression(IdentifierExpressionNode node, TextWriter writer)
        {
            writer.Write(node.IdentifierToken.Value);
        }

        private static void PrintLiteralExpression(LiteralExpressionNode node, TextWriter writer)
        {
            writer.Write(node.LiteralToken.Value);
        }

        private static void PrintParenthesizedExpression(ParenthesizedExpressionNode node, TextWriter writer)
        {
            Print(node.Expression, writer);
        }

        private static void PrintUnaryExpression(UnaryExpressionNode node, TextWriter writer)
        {
            writer.Write("(");
            writer.Write(node.Op.Type.GetDescription());
            Print(node.Value, writer);
            writer.Write(")");
        }
    }
}
