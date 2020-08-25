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
            foreach (var (type, text, value) in TokenData)
            {
                yield return new object[] { type, text, value };
            }
        }

        static IEnumerable<(SyntaxTokenType type, string text, object value)> TokenData =>
            new (SyntaxTokenType type, string text, object value)[]
            {
                (SyntaxTokenType.If, "if", null),
                (SyntaxTokenType.Else, "else", null),
                (SyntaxTokenType.For, "for", null),
                (SyntaxTokenType.To, "to", null),
                (SyntaxTokenType.Step, "step", null),
                (SyntaxTokenType.Do, "do", null),
                (SyntaxTokenType.While, "while", null),
                (SyntaxTokenType.Break, "break", null),
                (SyntaxTokenType.Continue, "continue", null),
                (SyntaxTokenType.Return, "return", null),
                (SyntaxTokenType.Int, "int", null),
                (SyntaxTokenType.Float, "float", null),
                (SyntaxTokenType.Bool, "bool", null),
                (SyntaxTokenType.String, "string", null),
                (SyntaxTokenType.Void, "void", null),
                (SyntaxTokenType.LPar, "(", null),
                (SyntaxTokenType.RPar, ")", null),
                (SyntaxTokenType.LBrace, "{", null),
                (SyntaxTokenType.RBrace, "}", null),
                (SyntaxTokenType.Add, "+", null),
                (SyntaxTokenType.AddEq, "+=", null),
                (SyntaxTokenType.Inc, "++", null),
                (SyntaxTokenType.Sub, "-", null),
                (SyntaxTokenType.SubEq, "-=", null),
                (SyntaxTokenType.Dec, "--", null),
                (SyntaxTokenType.Mul, "*", null),
                (SyntaxTokenType.MulEq, "*=", null),
                (SyntaxTokenType.Div, "/", null),
                (SyntaxTokenType.DivEq, "/=", null),
                (SyntaxTokenType.Mod, "%", null),
                (SyntaxTokenType.ModEq, "%=", null),
                (SyntaxTokenType.Not, "!", null),
                (SyntaxTokenType.And, "&&", null),
                (SyntaxTokenType.Or, "||", null),
                (SyntaxTokenType.Lt, "<", null),
                (SyntaxTokenType.Gt, ">", null),
                (SyntaxTokenType.Leq, "<=", null),
                (SyntaxTokenType.Geq, ">=", null),
                (SyntaxTokenType.Eq, "==", null),
                (SyntaxTokenType.Neq, "!=", null),
                (SyntaxTokenType.Assign, "=", null),
                (SyntaxTokenType.QMark, "?", null),
                (SyntaxTokenType.Colon, ":", null),
                (SyntaxTokenType.Comma, ",", null),
                (SyntaxTokenType.Semi, ";", null),

                (SyntaxTokenType.BoolLiteral, "true", true),
                (SyntaxTokenType.BoolLiteral, "false", false),

                (SyntaxTokenType.ID, "x", "x"),
                (SyntaxTokenType.IntLiteral, "17", 17),
                (SyntaxTokenType.FloatLiteral, "17.0", 17.0f),
                (SyntaxTokenType.StringLiteral, "\"Hello World\"", "Hello World"),

                (SyntaxTokenType.Error, "$", null),
                (SyntaxTokenType.Comment, "// this is a comment", null),
                (SyntaxTokenType.BlockComment, "/* this is a comment */", null),
            };
    }
}
