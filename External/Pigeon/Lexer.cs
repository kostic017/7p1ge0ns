using System.Collections.Generic;

namespace Kostic017.Pigeon
{
    public class Lexer
    {
        public static readonly Dictionary<string, SyntaxTokenType> keywords =
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

        public static readonly Dictionary<string, SyntaxTokenType> types =
            new Dictionary<string, SyntaxTokenType>
            {
                { "int", SyntaxTokenType.IntType },
                { "float", SyntaxTokenType.FloatType },
                { "bool", SyntaxTokenType.BoolType },
                { "string", SyntaxTokenType.StringType },
                { "void", SyntaxTokenType.VoidType }
            };

        static readonly HashSet<char> escapes =
            new HashSet<char>
            {
                '\\',
                't',
                'n',
                '"',
            };

        int line;
        int column;
        int index;

        int tokenStartLine;
        int tokenStartColumn;
        int tokenStartIndex;

        string code;

        char PrevChar => index - 1 > 0 ? code[index - 1] : '\0';
        char CurrentChar => index < code.Length ? code[index] : '\0';
        char NextChar => index + 1 < code.Length ? code[index + 1] : '\0';

        public int TabSize { get; set; } = 4;
        public List<CodeError> Errors { get; private set; } = new List<CodeError>();

        public SyntaxToken[] Lex(string str)
        {
            line = 1;
            index = 0;
            code = str;
            column = 0;
            Errors = new List<CodeError>();

            SyntaxToken tok;
            List<SyntaxToken> tokens = new List<SyntaxToken>();

            while ((tok = NextToken()).Type != SyntaxTokenType.EOF)
            {
                tokens.Add(tok);
            }

            return tokens.ToArray();
        }

        SyntaxToken NextToken()
        {
            char ch;
            while ((ch = EatChar()) != '\0')
            {
                if (char.IsWhiteSpace(ch))
                {
                    if (ch == '\t')
                    {
                        column += TabSize - 1;
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

                if (ch == '/' && (CurrentChar == '/' || CurrentChar == '*'))
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
                        return Token(SyntaxTokenType.LPar);

                    case ')':
                        return Token(SyntaxTokenType.RPar);

                    case '{':
                        return Token(SyntaxTokenType.LBrace);

                    case '}':
                        return Token(SyntaxTokenType.RBrace);

                    case '?':
                        return Token(SyntaxTokenType.QMark);

                    case ':':
                        return Token(SyntaxTokenType.Colon);

                    case ';':
                        return Token(SyntaxTokenType.Semi);

                    case ',':
                        return Token(SyntaxTokenType.Comma);

                    case '+':
                        if (TryEatChar('='))
                        {
                            return Token(SyntaxTokenType.AddEq);
                        }
                        if (TryEatChar('+'))
                        {
                            return Token(SyntaxTokenType.Inc);
                        }
                        return Token(SyntaxTokenType.Add);

                    case '-':
                        if (TryEatChar('='))
                        {
                            return Token(SyntaxTokenType.SubEq);
                        }
                        if (TryEatChar('-'))
                        {
                            return Token(SyntaxTokenType.Dec);
                        }
                        return Token(SyntaxTokenType.Sub);

                    case '*':
                        if (TryEatChar('='))
                        {
                            return Token(SyntaxTokenType.MulEq);
                        }
                        return Token(SyntaxTokenType.Mul);

                    case '/':
                        if (TryEatChar('='))
                        {
                            return Token(SyntaxTokenType.DivEq);
                        }
                        return Token(SyntaxTokenType.Div);

                    case '%':
                        if (TryEatChar('='))
                        {
                            EatChar();
                            return Token(SyntaxTokenType.ModEq);
                        }
                        return Token(SyntaxTokenType.Mod);

                    case '<':
                        if (TryEatChar('='))
                        {
                            return Token(SyntaxTokenType.Leq);
                        }
                        return Token(SyntaxTokenType.Lt);

                    case '>':
                        if (TryEatChar('='))
                        {
                            return Token(SyntaxTokenType.Geq);
                        }
                        return Token(SyntaxTokenType.Gt);

                    case '!':
                        if (TryEatChar('='))
                        {
                            return Token(SyntaxTokenType.Neq);
                        }
                        return Token(SyntaxTokenType.Not);

                    case '=':
                        if (TryEatChar('='))
                        {
                            return Token(SyntaxTokenType.Eq);
                        }
                        return Token(SyntaxTokenType.Assign);

                    case '&':
                        if (TryEatChar('&'))
                        {
                            return Token(SyntaxTokenType.And);
                        }
                        goto default;

                    case '|':
                        if (TryEatChar('|'))
                        {
                            return Token(SyntaxTokenType.Or);
                        }
                        goto default;

                    default:
                        SyntaxToken token = Token(SyntaxTokenType.Error);
                        token.ErrorIndex = Error(CodeErrorType.UNEXPECTED_CHARACTER, $"{ch}");
                        return token;
                }
            }

            return Token(SyntaxTokenType.EOF);
        }

        SyntaxToken LexString()
        {
            char ch;
            int err = -1;
            string value = "";

            while ((ch = EatChar()) != '"')
            {
                if (ch == '\0')
                {
                    err = Error(CodeErrorType.UNTERMINATED_STRING);
                    break;
                }

                if (ch == '\n')
                {
                    err = Error(CodeErrorType.NEWLINE_IN_STRING);
                    break;
                }

                if (ch == '\\')
                {
                    ch = EatChar();
                    if (escapes.Contains(ch))
                    {
                        value += ch;
                    }
                    else if (err == -1)
                    {
                        err = Error(CodeErrorType.INVALID_ESCAPE_CHAR, $"\\{ch}");
                    }
                }
                else
                {
                    value += ch;
                }
            }

            SyntaxToken token = Token(SyntaxTokenType.StringConst, value);
            token.ErrorIndex = err;
            return token;
        }

        SyntaxToken LexComment()
        {
            char ch = EatChar();

            if (ch == '/')
            {
                do
                {
                    ch = EatChar();
                } while (ch != '\n' && ch != '\0');
                return Token(SyntaxTokenType.Comment);
            }

            // ch == '*'

            int err = -1;

            while (true)
            {
                ch = EatChar();

                if (ch == '\0')
                {
                    err = Error(CodeErrorType.UNTERMINATED_COMMENT_BLOCK);
                    break;
                }

                if (ch == '*' && CurrentChar == '/')
                {
                    EatChar();
                    break;
                }
            }

            SyntaxToken token = Token(SyntaxTokenType.BlockComment);
            token.ErrorIndex = err;
            return token;
        }

        SyntaxToken LexNumber()
        {
            bool isReal = false;
            string value = PrevChar.ToString();

            char c = CurrentChar;
            while (char.IsDigit(c) || (!isReal && c == '.' && char.IsDigit(NextChar)))
            {
                if (c == '.')
                {
                    isReal = true;
                }
                value += EatChar();
                c = CurrentChar;
            }

            SyntaxToken tok;

            if (isReal)
            {
                tok = Token(SyntaxTokenType.FloatConst);
                if (float.TryParse(value, out float f))
                {
                    tok.Value = f;
                }
                else
                {
                    tok.ErrorIndex = Error(CodeErrorType.ILLEGAL_NUMBER, value);
                }
            }
            else
            {
                tok = Token(SyntaxTokenType.IntConst);
                if (int.TryParse(value, out int i))
                {
                    tok.Value = i;
                }
                else
                {
                    tok.ErrorIndex = Error(CodeErrorType.ILLEGAL_NUMBER, value);
                }
            }

            return tok;
        }

        SyntaxToken LexWord()
        {
            string value = PrevChar.ToString();

            char c = CurrentChar;
            while (c == '_' || char.IsLetterOrDigit(c))
            {
                value += EatChar();
                c = CurrentChar;
            }

            if (value == "true" || value == "false")
            {
                bool.TryParse(value, out bool b);
                return Token(SyntaxTokenType.BoolConst, b);
            }

            if (keywords.ContainsKey(value))
            {
                return Token(keywords[value]);
            }

            if (types.ContainsKey(value))
            {
                return Token(types[value]);
            }

            return Token(SyntaxTokenType.ID, value);
        }

        char EatChar()
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

        bool TryEatChar(char ch)
        {
            if (CurrentChar == ch)
            {
                EatChar();
                return true;
            }
            return false;
        }

        SyntaxToken Token(SyntaxTokenType tokenType, object value = null)
        {
            return new SyntaxToken
            {
                Type = tokenType,
                Value = value,
                StartIndex = tokenStartIndex,
                EndIndex = index,
                ErrorIndex = -1,
            };
        }

        int Error(CodeErrorType type, string data = "")
        {
            Errors.Add
            (
                new CodeError
                {
                    Type = type,
                    Data = data,
                    Line = tokenStartLine,
                    Column = tokenStartColumn
                }
            );
            return Errors.Count - 1;
        }
    }
}