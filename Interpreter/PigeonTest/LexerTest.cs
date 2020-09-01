using System.Collections.Generic;
using Xunit;

namespace Kostic017.Pigeon.Tests
{
    public class LexerTest
    {
        [Theory]
        [MemberData(nameof(GetTestData))]
        public void Lex(string text, Expected expected)
        {
            var syntaxTree = new SyntaxTree(text);

            var tokens = syntaxTree.Lex();

            Assert.True(tokens.Length > 0 && tokens.Length <= 2);

            if (tokens.Length == 2)
            {
                Assert.Equal(SyntaxTokenType.EOF, tokens[1].Type);
            }

            Assert.Equal(tokens[0].Type, expected.TokenType);
            Assert.Equal(tokens[0].Value, expected.Value);

            if (expected.Line > -1)
            {
                Assert.Equal(expected.Line, tokens[0].StartLine);
            }

            if (expected.Column > -1)
            {
                Assert.Equal(expected.Column, tokens[0].StartColumn);
            }
        }

        public static IEnumerable<object[]> GetTestData()
        {
            foreach (var (text, expected) in TestData)
            {
                yield return new object[] { text, expected };
            }
        }

        static IEnumerable<(string, Expected)> TestData => new[]
        {
            ("", E(SyntaxTokenType.EOF)),
            ("$", E(SyntaxTokenType.Illegal)),
            ("// this is a comment", E(SyntaxTokenType.Comment)),
            ("/* this is a comment */", E(SyntaxTokenType.BlockComment)),
            ("if", E(SyntaxTokenType.If)),
            ("else", E(SyntaxTokenType.Else)),
            ("for", E(SyntaxTokenType.For)),
            ("to", E(SyntaxTokenType.To)),
            ("step", E(SyntaxTokenType.Step)),
            ("do", E(SyntaxTokenType.Do)),
            ("while", E(SyntaxTokenType.While)),
            ("break", E(SyntaxTokenType.Break)),
            ("continue", E(SyntaxTokenType.Continue)),
            ("return", E(SyntaxTokenType.Return)),
            ("int", E(SyntaxTokenType.Int)),
            ("float", E(SyntaxTokenType.Float)),
            ("bool", E(SyntaxTokenType.Bool)),
            ("string", E(SyntaxTokenType.String)),
            ("void", E(SyntaxTokenType.Void)),
            ("x", E(SyntaxTokenType.ID, "x")),
            ("17", E(SyntaxTokenType.IntLiteral, "17")),
            ("17.0", E(SyntaxTokenType.FloatLiteral, "17.0")),
            ("\"Hello World\"", E(SyntaxTokenType.StringLiteral, "Hello World")),
            ("true", E(SyntaxTokenType.BoolLiteral, "true")),
            ("false", E(SyntaxTokenType.BoolLiteral, "false")),
            ("(", E(SyntaxTokenType.LPar)),
            (")", E(SyntaxTokenType.RPar)),
            ("{", E(SyntaxTokenType.LBrace)),
            ("}", E(SyntaxTokenType.RBrace)),
            ("+", E(SyntaxTokenType.Plus)),
            ("+=", E(SyntaxTokenType.PlusEq)),
            ("++", E(SyntaxTokenType.Inc)),
            ("-", E(SyntaxTokenType.Minus)),
            ("-=", E(SyntaxTokenType.MinusEq)),
            ("--", E(SyntaxTokenType.Dec)),
            ("*", E(SyntaxTokenType.Mul)),
            ("*=", E(SyntaxTokenType.MulEq)),
            ("/", E(SyntaxTokenType.Div)),
            ("/=", E(SyntaxTokenType.DivEq)),
            ("%", E(SyntaxTokenType.Mod)),
            ("%=", E(SyntaxTokenType.ModEq)),
            ("!", E(SyntaxTokenType.Not)),
            ("&&", E(SyntaxTokenType.And)),
            ("||", E(SyntaxTokenType.Or)),
            ("<", E(SyntaxTokenType.Lt)),
            (">", E(SyntaxTokenType.Gt)),
            ("<=", E(SyntaxTokenType.Leq)),
            (">=", E(SyntaxTokenType.Geq)),
            ("==", E(SyntaxTokenType.Eq)),
            ("!=", E(SyntaxTokenType.Neq)),
            ("=", E(SyntaxTokenType.Assign)),
            ("?", E(SyntaxTokenType.QMark)),
            (":", E(SyntaxTokenType.Colon)),
            (",", E(SyntaxTokenType.Comma)),
            (";", E(SyntaxTokenType.Semicolon)),
            ("\tif", E(SyntaxTokenType.If, l:1, c:5)),
            ("\nif", E(SyntaxTokenType.If, l:2, c:1)),
            ("\n\t if", E(SyntaxTokenType.If, l:2, c:6)),
        };

        static Expected E(SyntaxTokenType t, string v = null, int l = -1, int c = -1)
        {
            return new Expected(t, v, l, c);
        }

        public struct Expected
        {
            internal SyntaxTokenType TokenType { get; }
            internal string Value { get; }
            internal int Line { get; }
            internal int Column { get; }

            public Expected(SyntaxTokenType tokenType, string value, int line, int column)
            {
                TokenType = tokenType;
                Value = value;
                Line = line;
                Column = column;
            }
        }

    }
}
