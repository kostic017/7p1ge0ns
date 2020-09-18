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

        [Fact]
        public void ParseForStatement()
        {
            var text = @"
                for i = 1 to 10
                    i
            ";
            var syntaxTree = SyntaxTree.Parse(text);
            Assert.Empty(syntaxTree.ParserErrors);

            using var e = new AssertingEnumerator(syntaxTree.Ast);

            e.AssertNode(SyntaxNodeKind.Program);
                e.AssertNode(SyntaxNodeKind.StatementBlock);
                    e.AssertNode(SyntaxNodeKind.ForStatement);
                        e.AssertToken(SyntaxTokenType.ID, "i");
                        e.AssertNode(SyntaxNodeKind.LiteralExpression);
                            e.AssertToken(SyntaxTokenType.IntLiteral, "1");
                        e.AssertToken(SyntaxTokenType.To);
                        e.AssertNode(SyntaxNodeKind.LiteralExpression);
                            e.AssertToken(SyntaxTokenType.IntLiteral, "10");
                        e.AssertNode(SyntaxNodeKind.StatementBlock);
                            e.AssertNode(SyntaxNodeKind.ExpressionStatement);
                                e.AssertNode(SyntaxNodeKind.IdentifierExpression);
                                    e.AssertToken(SyntaxTokenType.ID, "i");
        }

        [Fact]
        public void ParseForStatement_DowntoWithStep()
        {
            var text = @"
                for i = 1 downto 10 step 2
                    i
            ";
                        var syntaxTree = SyntaxTree.Parse(text);
            Assert.Empty(syntaxTree.ParserErrors);

            using var e = new AssertingEnumerator(syntaxTree.Ast);

            e.AssertNode(SyntaxNodeKind.Program);
                e.AssertNode(SyntaxNodeKind.StatementBlock);
                    e.AssertNode(SyntaxNodeKind.ForStatement);
                        e.AssertToken(SyntaxTokenType.ID, "i");
                        e.AssertNode(SyntaxNodeKind.LiteralExpression);
                            e.AssertToken(SyntaxTokenType.IntLiteral, "1");
                        e.AssertToken(SyntaxTokenType.Downto);
                        e.AssertNode(SyntaxNodeKind.LiteralExpression);
                            e.AssertToken(SyntaxTokenType.IntLiteral, "10");
                        e.AssertNode(SyntaxNodeKind.LiteralExpression);
                            e.AssertToken(SyntaxTokenType.IntLiteral, "2");
                        e.AssertNode(SyntaxNodeKind.StatementBlock);
                            e.AssertNode(SyntaxNodeKind.ExpressionStatement);
                                e.AssertNode(SyntaxNodeKind.IdentifierExpression);
                                    e.AssertToken(SyntaxTokenType.ID, "i");
        }

        [Fact]
        public void ParseWhileStatement()
        {
            var text = @"
                while a > 4
                    a
            ";
            var syntaxTree = SyntaxTree.Parse(text);
            Assert.Empty(syntaxTree.ParserErrors);

            using var e = new AssertingEnumerator(syntaxTree.Ast);

            e.AssertNode(SyntaxNodeKind.Program);
                e.AssertNode(SyntaxNodeKind.StatementBlock);
                    e.AssertNode(SyntaxNodeKind.WhileStatement);
                        e.AssertNode(SyntaxNodeKind.BinaryExpression);
                            e.AssertNode(SyntaxNodeKind.IdentifierExpression);
                                e.AssertToken(SyntaxTokenType.ID, "a");
                            e.AssertToken(SyntaxTokenType.Gt);
                            e.AssertNode(SyntaxNodeKind.LiteralExpression);
                                e.AssertToken(SyntaxTokenType.IntLiteral, "4");
                        e.AssertNode(SyntaxNodeKind.StatementBlock);
                            e.AssertNode(SyntaxNodeKind.ExpressionStatement);
                                e.AssertNode(SyntaxNodeKind.IdentifierExpression);
                                    e.AssertToken(SyntaxTokenType.ID, "a");
        }

        [Fact]
        public void ParseIfStatement()
        {
            var text = @"
                if (a > b)
                    a
                else if a < b
                    b
                else
                {
                    a + b
                }
            ";

            var syntaxTree = SyntaxTree.Parse(text);
            Assert.Empty(syntaxTree.ParserErrors);

            using var e = new AssertingEnumerator(syntaxTree.Ast);

            e.AssertNode(SyntaxNodeKind.Program);
                e.AssertNode(SyntaxNodeKind.StatementBlock);
                    e.AssertNode(SyntaxNodeKind.IfStatement); // if 1
                        e.AssertNode(SyntaxNodeKind.ParenthesizedExpression);
                            e.AssertNode(SyntaxNodeKind.BinaryExpression);
                                e.AssertNode(SyntaxNodeKind.IdentifierExpression);
                                    e.AssertToken(SyntaxTokenType.ID, "a");
                                e.AssertToken(SyntaxTokenType.Gt);
                                e.AssertNode(SyntaxNodeKind.IdentifierExpression);
                                    e.AssertToken(SyntaxTokenType.ID, "b");
                        e.AssertNode(SyntaxNodeKind.StatementBlock); // then 1
                            e.AssertNode(SyntaxNodeKind.ExpressionStatement);
                                e.AssertNode(SyntaxNodeKind.IdentifierExpression);
                                    e.AssertToken(SyntaxTokenType.ID, "a");
                        e.AssertNode(SyntaxNodeKind.StatementBlock); // else 1
                            e.AssertNode(SyntaxNodeKind.IfStatement); // if 2
                                e.AssertNode(SyntaxNodeKind.BinaryExpression);
                                    e.AssertNode(SyntaxNodeKind.IdentifierExpression);
                                        e.AssertToken(SyntaxTokenType.ID, "a");
                                    e.AssertToken(SyntaxTokenType.Lt);
                                    e.AssertNode(SyntaxNodeKind.IdentifierExpression);
                                        e.AssertToken(SyntaxTokenType.ID, "b");
                                e.AssertNode(SyntaxNodeKind.StatementBlock); // then 2
                                    e.AssertNode(SyntaxNodeKind.ExpressionStatement);
                                        e.AssertNode(SyntaxNodeKind.IdentifierExpression);
                                            e.AssertToken(SyntaxTokenType.ID, "b");
                                e.AssertNode(SyntaxNodeKind.StatementBlock); // else 2
                                    e.AssertNode(SyntaxNodeKind.ExpressionStatement);
                                        e.AssertNode(SyntaxNodeKind.BinaryExpression);
                                            e.AssertNode(SyntaxNodeKind.IdentifierExpression);
                                                e.AssertToken(SyntaxTokenType.ID, "a");
                                            e.AssertToken(SyntaxTokenType.Plus);
                                            e.AssertNode(SyntaxNodeKind.IdentifierExpression);
                                                e.AssertToken(SyntaxTokenType.ID, "b");
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
        public void ParseBinaryExpression(string text, string expected)
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
