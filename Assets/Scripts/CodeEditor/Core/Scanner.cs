using System.Collections.Generic;

public class Scanner
{
    int line;
    int column;
    int index;

    int tokenStartLine;
    int tokenStartColumn;
    int tokenStartIndex;

    string code;

    readonly int tabSize;

    public List<ScanError> Errors { get; private set; }

    public static readonly Dictionary<string, TokenType> keywords =
        new Dictionary<string, TokenType> {
            { "if", TokenType.If },
            { "else", TokenType.Else },
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
            { "void", TokenType.VoidType }
        };

    public Scanner(int tabSize)
    {
        this.tabSize = tabSize;
    }

    public Token[] Scan(string str)
    {
        line = 1;
        column = 0;
        index = 0;
        code = str;
        Errors = new List<ScanError>();

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
                if (ch == '\t')
                {
                    column += tabSize - 1;
                }
                continue;
            }

            tokenStartLine = line;
            tokenStartColumn = column;
            tokenStartIndex = index;

            if (ch == '"')
            {
                return LexString();
            }

            if (ch == '/' && (PeekNextChar() == '/' || PeekNextChar() == '*'))
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
                    return Token(TokenType.LPar);

                case ')':
                    return Token(TokenType.RPar);

                case '{':
                    return Token(TokenType.LBrace);

                case '}':
                    return Token(TokenType.RBrace);

                case '?':
                    return Token(TokenType.Quest);

                case ':':
                    return Token(TokenType.Colon);

                case ';':
                    return Token(TokenType.Semicolon);

                case '+':
                    if (TryNextChar('='))
                    {
                        return Token(TokenType.AddEq);
                    }
                    if (TryNextChar('+'))
                    {
                        return Token(TokenType.Inc);
                    }
                    return Token(TokenType.Add);

                case '-':
                    if (TryNextChar('='))
                    {
                        return Token(TokenType.SubEq);
                    }
                    if (TryNextChar('-'))
                    {
                        return Token(TokenType.Dec);
                    }
                    return Token(TokenType.Sub);

                case '*':
                    if (TryNextChar('='))
                    {
                        return Token(TokenType.MulEq);
                    }
                    return Token(TokenType.Mul);

                case '/':
                    if (TryNextChar('='))
                    {
                        return Token(TokenType.DivEq);
                    }
                    return Token(TokenType.Div);

                case '%':
                    if (TryNextChar('='))
                    {
                        NextChar();
                        return Token(TokenType.ModEq);
                    }
                    return Token(TokenType.Mod);

                case '<':
                    if (TryNextChar('='))
                    {
                        return Token(TokenType.Leq);
                    }
                    return Token(TokenType.Lt);

                case '>':
                    if (TryNextChar('='))
                    {
                        return Token(TokenType.Geq);
                    }
                    return Token(TokenType.Gt);

                case '!':
                    if (TryNextChar('='))
                    {
                        return Token(TokenType.Neq);
                    }
                    return Token(TokenType.Not);

                case '=':
                    if (TryNextChar('='))
                    {
                        return Token(TokenType.Eq);
                    }
                    return Token(TokenType.Assign);

                case '&':
                    if (TryNextChar('&'))
                    {
                        return Token(TokenType.And);
                    }
                    goto default;

                case '|':
                    if (TryNextChar('|'))
                    {
                        return Token(TokenType.Or);
                    }
                    goto default;

                default:
                    Errors.Add(Error(ScanErrorType.UNEXPECTED_CHARACTER, $"{ch}"));
                    return Token(TokenType.Error);
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
                Errors.Add(Error(ScanErrorType.UNTERMINATED_STRING));
                break;
            }

            if (ch == '\n')
            {
                Errors.Add(Error(ScanErrorType.NEWLINE_IN_STRING));
                break;
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

        return Token(TokenType.StringConst, value);
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
            case '"':
                return '\"';
            default:
                Errors.Add(Error(ScanErrorType.INVALID_ESCAPE_CHAR, $"\\{ch}"));
                return '\0';
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
            } while (ch != '\n' && ch != '\0');
            return Token(TokenType.Comment);
        }

        // ch == '*'

        while (true)
        {
            ch = NextChar();

            if (ch == '\0')
            {
                Errors.Add(Error(ScanErrorType.UNTERMINATED_COMMENT_BLOCK));
                break;
            }

            if (ch == '*' && PeekNextChar() == '/')
            {
                NextChar();
                break;
            }
        }

        return Token(TokenType.BlockComment);

    }

    private Token LexNumber()
    {
        bool isReal = false;
        string value = PrevChar().ToString();

        char c = PeekNextChar();
        while (char.IsDigit(c) || (!isReal && c == '.' && char.IsDigit(Lookahead(1))))
        {
            if (c == '.')
            {
                isReal = true;
            }
            value += NextChar();
            c = PeekNextChar();
        }

        return Token(isReal ? TokenType.FloatConst : TokenType.IntConst, value);
    }

    private Token LexWord()
    {
        string value = PrevChar().ToString();

        char c = PeekNextChar();
        while (c == '_' || char.IsLetterOrDigit(c))
        {
            value += NextChar();
            c = PeekNextChar();
        }

        if (value == "true" || value == "false")
        {
            return Token(TokenType.BoolConst, value);
        }

        if (keywords.ContainsKey(value))
        {
            return Token(keywords[value]);
        }

        if (types.ContainsKey(value))
        {
            return Token(types[value]);
        }

        return Token(TokenType.Identifier, value);
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
        if (PeekNextChar() == ch)
        {
            NextChar();
            return true;
        }
        return false;
    }

    private char PeekNextChar()
    {
        return Lookahead(0);
    }

    private char PrevChar()
    {
        return Lookahead(-1);
    }

    private char Lookahead(int i)
    {
        if (index + i >= code.Length)
        {
            return '\0';
        }
        return code[index + i];
    }

    private Token Token(TokenType tokenType, string value = "")
    {
        return new Token
        {
            Value = value,
            Type = tokenType,
            StartIndex = tokenStartIndex,
            EndIndex = index
        };
    }

    private ScanError Error(ScanErrorType type, string details = "")
    {
        return new ScanError
        {
            Type = type,
            Details = details,
            Line = tokenStartLine,
            Column = tokenStartColumn,
            StartIndex = tokenStartIndex,
            EndIndex = index
        };
    }
}
