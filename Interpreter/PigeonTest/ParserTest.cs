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

            e.AssertNode(NodeKind.Program);
                e.AssertNode(NodeKind.StatementBlock);
                    e.AssertNode(NodeKind.VariableDeclaration);
                        e.AssertToken(SyntaxTokenType.Let);
                            e.AssertToken(SyntaxTokenType.ID, "a");
                            e.AssertNode(NodeKind.LiteralExpression);
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

            e.AssertNode(NodeKind.Program);
                e.AssertNode(NodeKind.StatementBlock);
                    e.AssertNode(NodeKind.ForStatement);
                        e.AssertToken(SyntaxTokenType.ID, "i");
                        e.AssertNode(NodeKind.LiteralExpression);
                            e.AssertToken(SyntaxTokenType.IntLiteral, "1");
                        e.AssertToken(SyntaxTokenType.To);
                        e.AssertNode(NodeKind.LiteralExpression);
                            e.AssertToken(SyntaxTokenType.IntLiteral, "10");
                        e.AssertNode(NodeKind.StatementBlock);
                            e.AssertNode(NodeKind.ExpressionStatement);
                                e.AssertNode(NodeKind.IdentifierExpression);
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

            e.AssertNode(NodeKind.Program);
                e.AssertNode(NodeKind.StatementBlock);
                    e.AssertNode(NodeKind.ForStatement);
                        e.AssertToken(SyntaxTokenType.ID, "i");
                        e.AssertNode(NodeKind.LiteralExpression);
                            e.AssertToken(SyntaxTokenType.IntLiteral, "1");
                        e.AssertToken(SyntaxTokenType.Downto);
                        e.AssertNode(NodeKind.LiteralExpression);
                            e.AssertToken(SyntaxTokenType.IntLiteral, "10");
                        e.AssertNode(NodeKind.LiteralExpression);
                            e.AssertToken(SyntaxTokenType.IntLiteral, "2");
                        e.AssertNode(NodeKind.StatementBlock);
                            e.AssertNode(NodeKind.ExpressionStatement);
                                e.AssertNode(NodeKind.IdentifierExpression);
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

            e.AssertNode(NodeKind.Program);
                e.AssertNode(NodeKind.StatementBlock);
                    e.AssertNode(NodeKind.WhileStatement);
                        e.AssertNode(NodeKind.BinaryExpression);
                            e.AssertNode(NodeKind.IdentifierExpression);
                                e.AssertToken(SyntaxTokenType.ID, "a");
                            e.AssertToken(SyntaxTokenType.Gt);
                            e.AssertNode(NodeKind.LiteralExpression);
                                e.AssertToken(SyntaxTokenType.IntLiteral, "4");
                        e.AssertNode(NodeKind.StatementBlock);
                            e.AssertNode(NodeKind.ExpressionStatement);
                                e.AssertNode(NodeKind.IdentifierExpression);
                                    e.AssertToken(SyntaxTokenType.ID, "a");
        }

        [Fact]
        public void ParseDoWhileStatement()
        {
            var text = @"
                do
                    a
                while a > 4
            ";
            var syntaxTree = SyntaxTree.Parse(text);
            Assert.Empty(syntaxTree.ParserErrors);

            using var e = new AssertingEnumerator(syntaxTree.Ast);

            e.AssertNode(NodeKind.Program);
                e.AssertNode(NodeKind.StatementBlock);
                    e.AssertNode(NodeKind.DoWhileStatement);
                        e.AssertNode(NodeKind.StatementBlock);
                            e.AssertNode(NodeKind.ExpressionStatement);
                                e.AssertNode(NodeKind.IdentifierExpression);
                                    e.AssertToken(SyntaxTokenType.ID, "a");
                        e.AssertNode(NodeKind.BinaryExpression);
                            e.AssertNode(NodeKind.IdentifierExpression);
                                e.AssertToken(SyntaxTokenType.ID, "a");
                            e.AssertToken(SyntaxTokenType.Gt);
                            e.AssertNode(NodeKind.LiteralExpression);
                                e.AssertToken(SyntaxTokenType.IntLiteral, "4");

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

            e.AssertNode(NodeKind.Program);
                e.AssertNode(NodeKind.StatementBlock);
                    e.AssertNode(NodeKind.IfStatement); // if 1
                        e.AssertNode(NodeKind.ParenthesizedExpression);
                            e.AssertNode(NodeKind.BinaryExpression);
                                e.AssertNode(NodeKind.IdentifierExpression);
                                    e.AssertToken(SyntaxTokenType.ID, "a");
                                e.AssertToken(SyntaxTokenType.Gt);
                                e.AssertNode(NodeKind.IdentifierExpression);
                                    e.AssertToken(SyntaxTokenType.ID, "b");
                        e.AssertNode(NodeKind.StatementBlock); // then 1
                            e.AssertNode(NodeKind.ExpressionStatement);
                                e.AssertNode(NodeKind.IdentifierExpression);
                                    e.AssertToken(SyntaxTokenType.ID, "a");
                        e.AssertNode(NodeKind.StatementBlock); // else 1
                            e.AssertNode(NodeKind.IfStatement); // if 2
                                e.AssertNode(NodeKind.BinaryExpression);
                                    e.AssertNode(NodeKind.IdentifierExpression);
                                        e.AssertToken(SyntaxTokenType.ID, "a");
                                    e.AssertToken(SyntaxTokenType.Lt);
                                    e.AssertNode(NodeKind.IdentifierExpression);
                                        e.AssertToken(SyntaxTokenType.ID, "b");
                                e.AssertNode(NodeKind.StatementBlock); // then 2
                                    e.AssertNode(NodeKind.ExpressionStatement);
                                        e.AssertNode(NodeKind.IdentifierExpression);
                                            e.AssertToken(SyntaxTokenType.ID, "b");
                                e.AssertNode(NodeKind.StatementBlock); // else 2
                                    e.AssertNode(NodeKind.ExpressionStatement);
                                        e.AssertNode(NodeKind.BinaryExpression);
                                            e.AssertNode(NodeKind.IdentifierExpression);
                                                e.AssertToken(SyntaxTokenType.ID, "a");
                                            e.AssertToken(SyntaxTokenType.Plus);
                                            e.AssertNode(NodeKind.IdentifierExpression);
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
        [InlineData("a = 4", "(a = 4)")]
        [InlineData("a = b + 4", "(a = (b + 4))")]
        [InlineData("4 + a = 4", "(4 + (a = 4))")]
        [InlineData("a = b = c = 4", "(a = (b = (c = 4)))")]
        public void ParseExpression(string text, string expected)
        {
            var syntaxTree = SyntaxTree.Parse(text);
            Assert.Empty(syntaxTree.Errors);

            using var e = new AssertingEnumerator(syntaxTree.Ast, false);

            e.AssertNode(NodeKind.Program);
            e.AssertNode(NodeKind.StatementBlock);
            e.AssertNode(NodeKind.ExpressionStatement);

            var expression = Assert.IsAssignableFrom<Expression>(e.GetNext());
            Assert.Equal(expected, expression.ToString());
        }
    }
}
