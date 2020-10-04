using System.IO;

namespace Kostic017.Pigeon.AST
{
    internal abstract class Expression : AstNode
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
                case NodeKind.AssignmentExpression:
                    PrintAssignmentExpression((AssignmentExpression)node, writer);
                    break;
                case NodeKind.IdentifierExpression:
                    PrintIdentiferExpression((IdentifierExpression)node, writer);
                    break;
                case NodeKind.BinaryExpression:
                    PrintBinaryExpression((BinaryExpression)node, writer);
                    break;
                case NodeKind.LiteralExpression:
                    PrintLiteralExpression((LiteralExpression)node, writer);
                    break;
                case NodeKind.ParenthesizedExpression:
                    PrintParenthesizedExpression((ParenthesizedExpression)node, writer);
                    break;
                case NodeKind.UnaryExpression:
                    PrintUnaryExpression((UnaryExpression)node, writer);
                    break;
                default:
                    node.GetType().ToString();
                    break;
            }
        }

        private static void PrintBinaryExpression(BinaryExpression node, TextWriter writer)
        {
            writer.Write("(");
            Print(node.Left, writer);
            writer.Write($" {node.Op.Type.GetDescription()} ");
            Print(node.Right, writer);
            writer.Write(")");
        }

        private static void PrintAssignmentExpression(AssignmentExpression node, TextWriter writer)
        {
            writer.Write("(");
            writer.Write(node.IdentifierToken.Value);
            writer.Write(" = ");
            Print(node.Value, writer);
            writer.Write(")");
        }

        private static void PrintIdentiferExpression(IdentifierExpression node, TextWriter writer)
        {
            writer.Write(node.IdentifierToken.Value);
        }

        private static void PrintLiteralExpression(LiteralExpression node, TextWriter writer)
        {
            writer.Write(node.LiteralToken.Value);
        }

        private static void PrintParenthesizedExpression(ParenthesizedExpression node, TextWriter writer)
        {
            Print(node.Expression, writer);
        }

        private static void PrintUnaryExpression(UnaryExpression node, TextWriter writer)
        {
            writer.Write("(");
            writer.Write(node.Op.Type.GetDescription());
            Print(node.Value, writer);
            writer.Write(")");
        }
    }
}
