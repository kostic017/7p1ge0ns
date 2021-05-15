﻿using System.Collections.Generic;

namespace Kostic017.Pigeon
{
    public static class SyntaxFacts
    {
        public static readonly HashSet<string> Keywords = new HashSet<string>
        {
            "if",
            "else",
            "for",
            "to",
            "downto",
            "do",
            "while",
            "break",
            "continue",
            "return",
            "let",
            "const",
        };

        public static readonly HashSet<string> Types = new HashSet<string>
        {
            "int",
            "float",
            "bool",
            "string",
            "void"
        };

        internal static readonly HashSet<char> EscapeChars = new HashSet<char>
        {
            '\\',
            't',
            'n',
            '"',
        };
    }
}
