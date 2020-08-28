using System.Collections.Generic;

namespace Kostic017.Pigeon
{
    public class ReservedWords
    {
        public static readonly Dictionary<string, SyntaxTokenType> Keywords =
            new Dictionary<string, SyntaxTokenType>
            {
                { "if", SyntaxTokenType.If },
                { "else", SyntaxTokenType.Else },
                { "for", SyntaxTokenType.For },
                { "to", SyntaxTokenType.To },
                { "step", SyntaxTokenType.Step },
                { "do", SyntaxTokenType.Do },
                { "while", SyntaxTokenType.While },
                { "break", SyntaxTokenType.Break },
                { "continue", SyntaxTokenType.Continue },
                { "return", SyntaxTokenType.Return },
            };

        public static readonly Dictionary<string, SyntaxTokenType> Types =
            new Dictionary<string, SyntaxTokenType>
            {
                { "int", SyntaxTokenType.Int },
                { "float", SyntaxTokenType.Float },
                { "bool", SyntaxTokenType.Bool },
                { "string", SyntaxTokenType.String },
                { "void", SyntaxTokenType.Void }
            };
    }
}
