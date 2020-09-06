using Kostic017.Pigeon.AST;
using Xunit;

namespace Kostic017.Pigeon.Tests
{
    public class ParserTest
    {
        [Fact]
        public void ParseVariableDeclaration()
        {
            var text = "let a = 4";

            var syntaxTree = SyntaxTree.Parse(text);
            Assert.Empty(syntaxTree.ParserErrors);

            using var e = new AssertingEnumerator(syntaxTree.Ast);

            e.AssertNode(SyntaxNodeKind.Program);
            e.AssertNode(SyntaxNodeKind.StatementBlock);
            e.AssertNode(SyntaxNodeKind.VariableDeclaration);
                e.AssertToken(SyntaxTokenType.Let);
                    e.AssertToken(SyntaxTokenType.ID, "a");
                    e.AssertNode(SyntaxNodeKind.LiteralExpression);
                        e.AssertToken(SyntaxTokenType.IntLiteral, "4");
        }
        
        [Theory]
        [InlineData("-a * b", "((-a) * b)")]
        [InlineData("!-a", "(!(-a))")]
        [InlineData("a + b + c", "((a + b) + c)")]
        [InlineData("a + b - c", "((a + b) - c)")]
        [InlineData("a * b * c", "((a * b) * c)")]
        [InlineData("a * b / c", "((a * b) / c)")]
        [InlineData("a + b / c", "(a + (b / c))")]
        [InlineData("a + b * c + d / e - f", "(((a + (b * c)) + (d / e)) - f)")]
        [InlineData("5 > 4 == 3 < 4", "((5 > 4) == (3 < 4))")]
        [InlineData("5 < 4 != 3 > 4", "((5 < 4) != (3 > 4))")]
        [InlineData("3 + 4 * 5 == 3 * 1 + 4 * 5", "((3 + (4 * 5)) == ((3 * 1) + (4 * 5)))")]
        [InlineData("3 > 5 == false", "((3 > 5) == false)")]
        [InlineData("3 < 5 == true", "((3 < 5) == true)")]
        [InlineData("1 + (2 + 3) + 4", "((1 + (2 + 3)) + 4)")]
        [InlineData("(5 + 5) * 2", "((5 + 5) * 2)")]
        [InlineData("2 / (5 + 5)", "(2 / (5 + 5))")]
        [InlineData("-(5 + 5)", "(-(5 + 5))")]
        [InlineData("!(true == true)", "(!(true == true))")]
        public void ParseBinaryExpressions(string text, string expected)
        {
            var syntaxTree = SyntaxTree.Parse(text);
            Assert.Empty(syntaxTree.Errors);

            using var e = new AssertingEnumerator(syntaxTree.Ast, false);

            e.AssertNode(SyntaxNodeKind.Program);
            e.AssertNode(SyntaxNodeKind.StatementBlock);
            e.AssertNode(SyntaxNodeKind.ExpressionStatement);

            var expression = Assert.IsAssignableFrom<ExpressionNode>(e.GetNext());
            Assert.Equal(expected, expression.ToString());
        }
    }
}
