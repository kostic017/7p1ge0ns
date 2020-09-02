using System.IO;

namespace Kostic017.Pigeon.AST
{
    public abstract class AstNode
    {
        internal abstract AstNodeKind Kind();

        public override string ToString()
        {
            var writer = new StringWriter();
            PrettyPrint(this, writer);
            return writer.ToString();
        }

        static void PrettyPrint(AstNode node, TextWriter writer, string ident = "")
        {
            switch (node.Kind())
            {
                case AstNodeKind.Program:
                    PrettyPrintProgram((ProgramNode)node, writer, ident);
                    break;
                case AstNodeKind.BinaryExpression:
                    PrettyPrintBinaryExpression((BinaryExpressionNode)node, writer);
                    break;
                case AstNodeKind.IdExpression:
                    PrettyPrintIdExpression((IdExpressionNode)node, writer);
                    break;
                case AstNodeKind.LiteralExpression:
                    PrettyPrintLiteralExpression((LiteralExpressionNode)node, writer);
                    break;
                case AstNodeKind.ParenthesizedExpression:
                    PrettyPrintParenthesizedExpression((ParenthesizedExpressionNode)node, writer);
                    break;
                case AstNodeKind.UnaryExpression:
                    PrettyPrintUnaryExpression((UnaryExpressionNode)node, writer);
                    break;
                default:
                    node.GetType().ToString();
                    break;
            }
        }

        static void PrettyPrintProgram(ProgramNode node, TextWriter writer, string ident)
        {
            
            foreach (StatementNode stmt in node.Statements)
            {
                PrettyPrint(stmt, writer, ident + "    ");
            }
        }

        static void PrettyPrintBinaryExpression(BinaryExpressionNode node, TextWriter writer)
        {
            writer.Write("(");
            PrettyPrint(node.Left, writer);
            writer.Write($" {node.Op.PrettyPrint()} ");
            PrettyPrint(node.Right, writer);
            writer.Write(")");
        }

        static void PrettyPrintIdExpression(IdExpressionNode node, TextWriter writer)
        {
            writer.Write(node.Value);
        }

        static void PrettyPrintLiteralExpression(LiteralExpressionNode node, TextWriter writer)
        {
            writer.Write(node.Value);
        }

        static void PrettyPrintParenthesizedExpression(ParenthesizedExpressionNode node, TextWriter writer)
        {
            writer.Write("[");
            PrettyPrint(node.Expression, writer);
            writer.Write("]");
        }

        static void PrettyPrintUnaryExpression(UnaryExpressionNode node, TextWriter writer)
        {
            writer.Write("(");
            writer.Write(node.Op.PrettyPrint());
            PrettyPrint(node.Value, writer);
            writer.Write(")");
        }
    }
}
