using System.Collections.Generic;

class Scanner
{
    private int line;
    private int column;
    private int index;

    private string code;

    private int tokenStartLine;
    private int tokenStartColumn;
    private int tokenStartIndex;

    public static readonly Dictionary<string, TokenType> keywords =
        new Dictionary<string, TokenType> {
            { "if", TokenType.If },
            { "for", TokenType.For },
            { "do", TokenType.Do },
            { "while", TokenType.While },
            { "break", TokenType.Break },
            { "continue", TokenType.Continue },
            { "return", TokenType.Return },
        };

    public static readonly Dictionary<string, TokenType> types =
        new Dictionary<string, TokenType> {
            { "int", TokenType.IntType },
            { "float", TokenType.FloatType },
            { "bool", TokenType.BoolType },
            { "string", TokenType.StringType },
        };

    public Token[] Scan(string code)
    {
        line = 1;
        column = 0;
        index = 0;
        this.code = code;

        List<Token> tokens = new List<Token>();

        Token tok;
        while ((tok = NextToken()) != null)
        {
            tokens.Add(tok);
        }

        return tokens.ToArray();
    }

    private Token NextToken()
    {
        char ch;
        while ((ch = NextChar()) != '\0')
        {
            if (char.IsWhiteSpace(ch))
            {
                continue;
            }

            tokenStartLine = line;
            tokenStartColumn = column;
            tokenStartIndex = index;

            if (ch == '"')
            {
                return LexString();
            }

            if (ch == '/' && (Lookahead() != '/' && Lookahead() != '*'))
            {
                return LexComment();
            }

            if (char.IsDigit(ch))
            {
                return LexNumber();
            }

            if (ch == '_' || char.IsLetter(ch))
            {
                return LexWord();
            }

            switch (ch)
            {
                case '(':
                    return NewToken(TokenType.LPar);

                case ')':
                    return NewToken(TokenType.RPar);

                case '{':
                    return NewToken(TokenType.LBrace);

                case '}':
                    return NewToken(TokenType.RBrace);

                case '?':
                    return NewToken(TokenType.Quest);

                case ':':
                    return NewToken(TokenType.Colon);

                case ';':
                    return NewToken(TokenType.Semicolon);

                case '+':
                    if (TryNextChar('='))
                    {
                        return NewToken(TokenType.AddEq);
                    }
                    if (TryNextChar('+'))
                    {
                        return NewToken(TokenType.Inc);
                    }
                    return NewToken(TokenType.Add);

                case '-':
                    if (TryNextChar('='))
                    {
                        return NewToken(TokenType.SubEq);
                    }
                    if (TryNextChar('-'))
                    {
                        return NewToken(TokenType.Dec);
                    }
                    return NewToken(TokenType.Sub);

                case '*':
                    if (TryNextChar('='))
                    {
                        return NewToken(TokenType.MulEq);
                    }
                    return NewToken(TokenType.Mul);

                case '/':
                    if (TryNextChar('='))
                    {
                        return NewToken(TokenType.DivEq);
                    }
                    return NewToken(TokenType.Div);

                case '%':
                    if (TryNextChar('='))
                    {
                        NextChar();
                        return NewToken(TokenType.ModEq);
                    }
                    return NewToken(TokenType.Mod);

                case '<':
                    if (TryNextChar('='))
                    {
                        return NewToken(TokenType.Leq);
                    }
                    return NewToken(TokenType.Lt);

                case '>':
                    if (TryNextChar('='))
                    {
                        return NewToken(TokenType.Geq);
                    }
                    return NewToken(TokenType.Gt);

                case '!':
                    if (TryNextChar('='))
                    {
                        return NewToken(TokenType.Neq);
                    }
                    return NewToken(TokenType.Not);

                case '=':
                    if (TryNextChar('='))
                    {
                        return NewToken(TokenType.Eq);
                    }
                    goto default;

                case '&':
                    if (TryNextChar('&'))
                    {
                        return NewToken(TokenType.And);
                    }
                    goto default;

                case '|':
                    if (TryNextChar('|'))
                    {
                        return NewToken(TokenType.Or);
                    }
                    goto default;

                default:
                    throw NewException($"Unexpected character {ch}");
            }
        }

        return null;
    }

    private Token LexString()
    {
        char ch;
        string value = "";

        while ((ch = NextChar()) != '"')
        {
            if (ch == '\0')
            {
                throw NewException("Unterminated string");
            }

            if (ch == '\n')
            {
                throw NewException("Newline in string");
            }

            if (ch == '\\')
            {
                value += LexEscape();
            }
            else
            {
                value += ch;
            }
        }

        return NewToken(TokenType.StringConst, value);
    }

    private char LexEscape()
    {
        char ch;
        switch (ch = NextChar())
        {
            case '\\':
                return '\\';
            case 't':
                return '\t';
            case 'n':
                return '\n';
            default:
                throw NewException($"Invalid escape sequence \\{ch}");
        }
    }

    private Token LexComment()
    {
        char ch = NextChar();

        if (ch == '/')
        {
            do
            {
                ch = NextChar();
            } while (ch != '\n' || ch != '\0');
            return NewToken(TokenType.Comment);
        }

        // ch == '*'

        do
        {
            ch = NextChar();
            if (ch == '\0')
            {
                throw NewException("Unterminated comment block");
            }
        } while (ch != '*' && Lookahead() != '/');
        NextChar(); // eat /

        return NewToken(TokenType.BlockComment);
    }

    private Token LexNumber()
    {
        bool isReal = false;
        char nextCh = Lookahead();
        string value = Lookahead(-1).ToString();

        while (char.IsDigit(nextCh) || (!isReal && nextCh == '.'))
        {
            if (nextCh == '.')
            {
                isReal = true;
            }
            value += NextChar();
        }
        return NewToken(isReal ? TokenType.FloatConst : TokenType.IntConst, value);
    }

    private Token LexWord()
    {
        string value = Lookahead(-1).ToString();

        char c = Lookahead(0);
        while (c == '_' || char.IsLetterOrDigit(c))
        {
            value += NextChar();
            c = Lookahead(0);
        }

        if (value == "true" || value == "false")
        {
            return NewToken(TokenType.BoolConst, value);
        }

        if (keywords.ContainsKey(value))
        {
            return NewToken(keywords[value]);
        }

        if (types.ContainsKey(value))
        {
            return NewToken(types[value]);
        }

        return NewToken(TokenType.Identifier, value);
    }

    private char NextChar()
    {
        if (index >= code.Length)
        {
            return '\0';
        }

        char ch = code[index];
        ++index;

        if (ch == '\n')
        {
            column = 0;
            ++line;
        }
        else
        {
            ++column;
        }

        return ch;
    }

    private bool TryNextChar(char ch)
    {
        if (Lookahead() == ch)
        {
            NextChar();
            return true;
        }
        return false;
    }

    private char Lookahead(int i = 1)
    {
        if (index + i >= code.Length)
        {
            return '\0';
        }
        return code[index + i];
    }

    private ScannerException NewException(string message)
    {
        return new ScannerException(message, tokenStartColumn, tokenStartLine);
    }

    private Token NewToken(TokenType tokenType, string value = "")
    {
        return new Token
        {
            Value = value,
            Type = tokenType,
            StartIndex = tokenStartIndex,
            EndIndex = index
        };
    }
}

