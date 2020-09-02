using System.IO;

namespace Kostic017.Pigeon.AST
{
    public abstract class AstNode
    {
        internal abstract AstNodeKind Kind { get; }

        public override string ToString()
        {
            var writer = new StringWriter();
            Print(this, writer);
            return writer.ToString();
        }

        static void Print(AstNode node, TextWriter writer, string ident = "")
        {
            switch (node.Kind)
            {
                case AstNodeKind.Program:
                    PrintProgram((ProgramNode)node, writer);
                    break;
                case AstNodeKind.StatementBlock:
                    PrintStatementBlock((StatementBlockNode)node, writer, ident);
                    break;
                case AstNodeKind.VariableDeclaration:
                    PrintVariableDeclaration((VariableDeclarationNode)node, writer, ident);
                    break;
                case AstNodeKind.ExpressionStatement:
                    PrintExpressionStatement((ExpressionStatementNode)node, writer, ident);
                    break;
                case AstNodeKind.BinaryExpression:
                    PrintBinaryExpression((BinaryExpressionNode)node, writer);
                    break;
                case AstNodeKind.IdExpression:
                    PrintIdExpression((IdExpressionNode)node, writer);
                    break;
                case AstNodeKind.LiteralExpression:
                    PrintLiteralExpression((LiteralExpressionNode)node, writer);
                    break;
                case AstNodeKind.ParenthesizedExpression:
                    PrintParenthesizedExpression((ParenthesizedExpressionNode)node, writer);
                    break;
                case AstNodeKind.UnaryExpression:
                    PrintUnaryExpression((UnaryExpressionNode)node, writer);
                    break;
                default:
                    node.GetType().ToString();
                    break;
            }
        }

        private static void PrintProgram(ProgramNode node, TextWriter writer)
        {
            Print(node.StatementBlock, writer);
        }

        private static void PrintStatementBlock(StatementBlockNode node, TextWriter writer, string ident)
        {
            foreach (var stmt in node.Statements)
            {
                Print(stmt, writer, ident + "    ");
            }
        }

        private static void PrintVariableDeclaration(VariableDeclarationNode node, TextWriter writer, string ident)
        {
            writer.Write(ident);
            writer.Write($"{node.Keyword.Type} {node.Name.Value}");
            
            if (node.Value != null)
            {
                writer.Write(" = ");
                Print(node.Value, writer);
            }
        }

        private static void PrintExpressionStatement(ExpressionStatementNode node, TextWriter writer, string ident)
        {
            writer.Write(ident);
            Print(node.expression, writer);
        }

        private static void PrintBinaryExpression(BinaryExpressionNode node, TextWriter writer)
        {
            writer.Write("(");
            Print(node.Left, writer);
            writer.Write($" {node.Op.PrettyPrint()} ");
            Print(node.Right, writer);
            writer.Write(")");
        }

        private static void PrintIdExpression(IdExpressionNode node, TextWriter writer)
        {
            writer.Write(node.Value);
        }

        private static void PrintLiteralExpression(LiteralExpressionNode node, TextWriter writer)
        {
            writer.Write(node.Value);
        }

        private static void PrintParenthesizedExpression(ParenthesizedExpressionNode node, TextWriter writer)
        {
            writer.Write("[");
            Print(node.Expression, writer);
            writer.Write("]");
        }

        private static void PrintUnaryExpression(UnaryExpressionNode node, TextWriter writer)
        {
            writer.Write("(");
            writer.Write(node.Op.PrettyPrint());
            Print(node.Value, writer);
            writer.Write(")");
        }
    }
}
