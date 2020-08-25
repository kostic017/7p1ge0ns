using System.Collections.Generic;
using Xunit;

namespace Kostic017.Pigeon.Tests
{
    public class LexerTest
    {
        [Theory]
        [MemberData(nameof(GetTokenData))]
        public void Lex(SyntaxTokenType type, string text, object value)
        {
            Lexer lexer = new Lexer();
            SyntaxToken[] tokens = lexer.Lex(text);
            SyntaxToken token = Assert.Single(tokens);
            Assert.Equal(type, token.Type);
            Assert.Equal(value, token.Value);
        }

        public static IEnumerable<object[]> GetTokenData()
        {
            foreach (var (type, text) in TokensWithoutValue)
            {
                yield return new object[] { type, text, null };
            }
            foreach (var (type, text, value) in TokensWithValue)
            {
                yield return new object[] { type, text, value };
            }
        }

        static IEnumerable<(SyntaxTokenType type, string text)> TokensWithoutValue => new []
        {
            (SyntaxTokenType.If, "if"),
            (SyntaxTokenType.Else, "else"),
            (SyntaxTokenType.For, "for"),
            (SyntaxTokenType.To, "to"),
            (SyntaxTokenType.Step, "step"),
            (SyntaxTokenType.Do, "do"),
            (SyntaxTokenType.While, "while"),
            (SyntaxTokenType.Break, "break"),
            (SyntaxTokenType.Continue, "continue"),
            (SyntaxTokenType.Return, "return"),
            (SyntaxTokenType.Int, "int"),
            (SyntaxTokenType.Float, "float"),
            (SyntaxTokenType.Bool, "bool"),
            (SyntaxTokenType.String, "string"),
            (SyntaxTokenType.Void, "void"),
            (SyntaxTokenType.LPar, "("),
            (SyntaxTokenType.RPar, ")"),
            (SyntaxTokenType.LBrace, "{"),
            (SyntaxTokenType.RBrace, "}"),
            (SyntaxTokenType.Add, "+"),
            (SyntaxTokenType.AddEq, "+="),
            (SyntaxTokenType.Inc, "++"),
            (SyntaxTokenType.Sub, "-"),
            (SyntaxTokenType.SubEq, "-="),
            (SyntaxTokenType.Dec, "--"),
            (SyntaxTokenType.Mul, "*"),
            (SyntaxTokenType.MulEq, "*="),
            (SyntaxTokenType.Div, "/"),
            (SyntaxTokenType.DivEq, "/="),
            (SyntaxTokenType.Mod, "%"),
            (SyntaxTokenType.ModEq, "%="),
            (SyntaxTokenType.Not, "!"),
            (SyntaxTokenType.And, "&&"),
            (SyntaxTokenType.Or, "||"),
            (SyntaxTokenType.Lt, "<"),
            (SyntaxTokenType.Gt, ">"),
            (SyntaxTokenType.Leq, "<="),
            (SyntaxTokenType.Geq, ">="),
            (SyntaxTokenType.Eq, "=="),
            (SyntaxTokenType.Neq, "!="),
            (SyntaxTokenType.Assign, "="),
            (SyntaxTokenType.QMark, "?"),
            (SyntaxTokenType.Colon, ":"),
            (SyntaxTokenType.Comma, ","),
            (SyntaxTokenType.Semi, ";"),
            (SyntaxTokenType.Illegal, "$"),
            (SyntaxTokenType.Comment, "// this is a comment"),
            (SyntaxTokenType.BlockComment, "/* this is a comment */"),
        };

        static IEnumerable<(SyntaxTokenType type, string text, object value)> TokensWithValue =>
            new (SyntaxTokenType type, string text, object value)[]
        {
            (SyntaxTokenType.ID, "x", "x"),
            (SyntaxTokenType.IntLiteral, "17", 17),
            (SyntaxTokenType.FloatLiteral, "17.0", 17.0f),
            (SyntaxTokenType.StringLiteral, "\"Hello World\"", "Hello World"),
            (SyntaxTokenType.BoolLiteral, "true", true),
            (SyntaxTokenType.BoolLiteral, "false", false),
        };

    }
}
