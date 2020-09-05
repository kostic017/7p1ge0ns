using Kostic017.Pigeon.AST;
using System.Collections.Generic;
using Xunit;

namespace Kostic017.Pigeon.Tests
{
    public class ParserTest
    {
        [Fact]
        public void ParseVaraibleDeclaration()
        {
            var text = "let a = 4";
            var ast = SyntaxTree.Parse(text).Ast;
            using var e = new AssertingEnumerator(ast);
            e.AssertNode(SyntaxNodeKind.Program);
            e.AssertNode(SyntaxNodeKind.StatementBlock);
            e.AssertNode(SyntaxNodeKind.VariableDeclaration);
                e.AssertToken(SyntaxTokenType.Let);
                    e.AssertToken(SyntaxTokenType.ID, "a");
                    e.AssertNode(SyntaxNodeKind.LiteralExpression);
                        e.AssertToken(SyntaxTokenType.IntLiteral, "4");
        }
        
        [Theory]
        [MemberData(nameof(GetOperatorPairs))]
        public void ParseBinaryExpressions(SyntaxTokenType op1, SyntaxTokenType op2)
        {
            var p1 = SyntaxFacts.BinOpPrec[op1];
            var p2 = SyntaxFacts.BinOpPrec[op2];
            var a1 = SyntaxFacts.BinOpAssoc(op1);

            var text = $"1 {op1.PrettyPrint()} 2.3 {op2.PrettyPrint()} x";

            var ast = SyntaxTree.Parse(text).Ast;
            using var e = new AssertingEnumerator(ast);

            e.AssertNode(SyntaxNodeKind.Program);
            e.AssertNode(SyntaxNodeKind.StatementBlock);
            e.AssertNode(SyntaxNodeKind.ExpressionStatement);

            if (p1 > p2 || (p1 == p2 && a1 == Associativity.Left))
            {
                e.AssertNode(SyntaxNodeKind.BinaryExpression);
                    e.AssertNode(SyntaxNodeKind.BinaryExpression);
                        e.AssertNode(SyntaxNodeKind.LiteralExpression);
                            e.AssertToken(SyntaxTokenType.IntLiteral, "1");
                        e.AssertToken(op1);
                        e.AssertNode(SyntaxNodeKind.LiteralExpression);
                            e.AssertToken(SyntaxTokenType.FloatLiteral, "2.3");
                    e.AssertToken(op2);
                    e.AssertNode(SyntaxNodeKind.IdentifierExpression);
                        e.AssertToken(SyntaxTokenType.ID, "x");
            }
            else
            {
                e.AssertNode(SyntaxNodeKind.BinaryExpression);
                    e.AssertNode(SyntaxNodeKind.LiteralExpression);
                        e.AssertToken(SyntaxTokenType.IntLiteral, "1");
                    e.AssertToken(op1);
                    e.AssertNode(SyntaxNodeKind.BinaryExpression);
                        e.AssertNode(SyntaxNodeKind.LiteralExpression);
                            e.AssertToken(SyntaxTokenType.FloatLiteral, "2.3");
                        e.AssertToken(op2);
                        e.AssertNode(SyntaxNodeKind.IdentifierExpression);
                            e.AssertToken(SyntaxTokenType.ID, "x");
            }

        }

        public static IEnumerable<object[]> GetOperatorPairs()
        {
            foreach (var op1 in SyntaxFacts.BinOpPrec.Keys)
            {
                foreach (var op2 in SyntaxFacts.BinOpPrec.Keys)
                {
                    yield return new object[] { op1, op2 };
                }
            }
        }
    }
}
