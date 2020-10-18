using System.Collections.Generic;

namespace Kostic017.Pigeon
{
    public static class SyntaxFacts
    {
        public static readonly Dictionary<string, SyntaxTokenType> Keywords
            = new Dictionary<string, SyntaxTokenType>
        {
            { "if", SyntaxTokenType.If },
            { "else", SyntaxTokenType.Else },
            { "for", SyntaxTokenType.For },
            { "to", SyntaxTokenType.To },
            { "downto", SyntaxTokenType.Downto },
            { "do", SyntaxTokenType.Do },
            { "while", SyntaxTokenType.While },
            { "break", SyntaxTokenType.Break },
            { "continue", SyntaxTokenType.Continue },
            { "return", SyntaxTokenType.Return },
            { "let", SyntaxTokenType.Let },
            { "const", SyntaxTokenType.Const },
        };

        public static readonly Dictionary<string, SyntaxTokenType> Types
            = new Dictionary<string, SyntaxTokenType>
        {
            { "int", SyntaxTokenType.Int },
            { "float", SyntaxTokenType.Float },
            { "bool", SyntaxTokenType.Bool },
            { "string", SyntaxTokenType.String },
            { "void", SyntaxTokenType.Void }
        };

        internal static readonly HashSet<char> EscapeChars
            = new HashSet<char>
        {
            '\\',
            't',
            'n',
            '"',
        };

        internal static readonly SyntaxTokenType[] AssignmentOperators =
        {
            SyntaxTokenType.Eq,
            SyntaxTokenType.PlusEq,
            SyntaxTokenType.MinusEq,
            SyntaxTokenType.MulEq,
            SyntaxTokenType.DivEq,
            SyntaxTokenType.ModEq,
        };

        internal static readonly Dictionary<SyntaxTokenType, int> BinOpPrec
            = new Dictionary<SyntaxTokenType, int>
        {
            { SyntaxTokenType.And, 0 },
            { SyntaxTokenType.Or, 0 },
            { SyntaxTokenType.EqEq, 1 },
            { SyntaxTokenType.Neq, 1 },
            { SyntaxTokenType.Lt, 2 },
            { SyntaxTokenType.Gt, 2 },
            { SyntaxTokenType.Leq, 2 },
            { SyntaxTokenType.Geq, 2 },
            { SyntaxTokenType.Plus, 3 },
            { SyntaxTokenType.Minus, 3 },
            { SyntaxTokenType.Mul, 4 },
            { SyntaxTokenType.Div, 4 },
            { SyntaxTokenType.Mod, 4 },
        };
    }
}
