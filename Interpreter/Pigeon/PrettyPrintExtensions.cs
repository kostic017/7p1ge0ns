using Kostic017.Pigeon.AST;
using System;
using System.ComponentModel;
using System.Reflection;

namespace Kostic017.Pigeon
{
    public static class PrettyPrintExtensions
    {
        public static string PrettyPrint(this SyntaxTokenType tokenType)
        {
            FieldInfo field = tokenType.GetType().GetField(tokenType.ToString());
            if (!(Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attr))
            {
                return tokenType.ToString();
            }
            return attr.Description;
        }

        public static string PrettyPrint(this AstNode node, int level = 0)
        {
            if (node is BinaryExpressionNode binary)
            {
                return $"({PrettyPrint(binary.Left)} {binary.Op.PrettyPrint()} {PrettyPrint(binary.Right)})";
            }

            if (node is IdExpressionNode id)
            {
                return id.Value;
            }

            if (node is LiteralExpressionNode literal)
            {
                return literal.Value.ToString();
            }

            if (node is ParenthesizedExpressionNode parenthesized)
            {
                return $"[{PrettyPrint(parenthesized.Expression)}]";
            }

            if (node is UnaryExpressionNode unary)
            {
                return $"({unary.Op.PrettyPrint()}{PrettyPrint(unary.Value)})";
            }

            return node.GetType().ToString();
        }
    }
}
