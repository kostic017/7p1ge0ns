using System.Collections.Generic;
using System.Globalization;

namespace Kostic017.Pigeon
{
    public class Lexer
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

        char PrevChar => index - 1 >= 0 ? code[index - 1] : '\0';
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
            while (CurrentChar != '\0')
            {
                char ch = EatCurrentChar();
                
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
                        if (TryEatCurrentChar('='))
                        {
                            return Token(SyntaxTokenType.AddEq);
                        }
                        if (TryEatCurrentChar('+'))
                        {
                            return Token(SyntaxTokenType.Inc);
                        }
                        return Token(SyntaxTokenType.Add);

                    case '-':
                        if (TryEatCurrentChar('='))
                        {
                            return Token(SyntaxTokenType.SubEq);
                        }
                        if (TryEatCurrentChar('-'))
                        {
                            return Token(SyntaxTokenType.Dec);
                        }
                        return Token(SyntaxTokenType.Sub);

                    case '*':
                        if (TryEatCurrentChar('='))
                        {
                            return Token(SyntaxTokenType.MulEq);
                        }
                        return Token(SyntaxTokenType.Mul);

                    case '/':
                        if (TryEatCurrentChar('='))
                        {
                            return Token(SyntaxTokenType.DivEq);
                        }
                        return Token(SyntaxTokenType.Div);

                    case '%':
                        if (TryEatCurrentChar('='))
                        {
                            EatCurrentChar();
                            return Token(SyntaxTokenType.ModEq);
                        }
                        return Token(SyntaxTokenType.Mod);

                    case '<':
                        if (TryEatCurrentChar('='))
                        {
                            return Token(SyntaxTokenType.Leq);
                        }
                        return Token(SyntaxTokenType.Lt);

                    case '>':
                        if (TryEatCurrentChar('='))
                        {
                            return Token(SyntaxTokenType.Geq);
                        }
                        return Token(SyntaxTokenType.Gt);

                    case '!':
                        if (TryEatCurrentChar('='))
                        {
                            return Token(SyntaxTokenType.Neq);
                        }
                        return Token(SyntaxTokenType.Not);

                    case '=':
                        if (TryEatCurrentChar('='))
                        {
                            return Token(SyntaxTokenType.Eq);
                        }
                        return Token(SyntaxTokenType.Assign);

                    case '&':
                        if (TryEatCurrentChar('&'))
                        {
                            return Token(SyntaxTokenType.And);
                        }
                        goto default;

                    case '|':
                        if (TryEatCurrentChar('|'))
                        {
                            return Token(SyntaxTokenType.Or);
                        }
                        goto default;

                    default:
                        SyntaxToken token = Token(SyntaxTokenType.Illegal);
                        token.ErrorIndex = Error(CodeErrorType.ILLEGAL_CHARACTER, $"{ch}");
                        return token;
                }
            }

            return Token(SyntaxTokenType.EOF);
        }

        SyntaxToken LexString()
        {
            int err = -1;
            string value = "";

            while (CurrentChar != '"')
            {
                if (CurrentChar == '\0')
                {
                    err = Error(CodeErrorType.UNTERMINATED_STRING);
                    break;
                }

                if (CurrentChar == '\n')
                {
                    err = Error(CodeErrorType.NEWLINE_IN_STRING);
                    break;
                }

                if (CurrentChar == '\\')
                {
                    if (escapes.Contains(NextChar))
                    {
                        value += NextChar;
                    }
                    else if (err == -1)
                    {
                        err = Error(CodeErrorType.INVALID_ESCAPE_CHAR, $"\\{NextChar}");
                    }
                    EatCurrentChar();
                }
                else
                {
                    value += CurrentChar;
                }

                EatCurrentChar();
            }

            TryEatCurrentChar('"');

            SyntaxToken token = Token(SyntaxTokenType.StringLiteral, value);
            token.ErrorIndex = err;
            return token;
        }

        SyntaxToken LexComment()
        {
            if (CurrentChar == '/')
            {
                while (CurrentChar != '\n' && CurrentChar != '\0')
                {
                    EatCurrentChar();
                }
                return Token(SyntaxTokenType.Comment);
            }
             
            /* lorem ipsum */

            int err = -1;
            TryEatCurrentChar('*');

            while (true)
            {
                if (CurrentChar == '*' && NextChar == '/')
                {
                    EatCurrentChar();
                    EatCurrentChar();
                    break;
                }

                if (CurrentChar == '\0')
                {
                    err = Error(CodeErrorType.UNTERMINATED_COMMENT_BLOCK);
                    break;
                }

                EatCurrentChar();
            }

            SyntaxToken token = Token(SyntaxTokenType.BlockComment);
            token.ErrorIndex = err;
            return token;
        }

        SyntaxToken LexNumber()
        {
            bool isReal = false;
            string value = PrevChar.ToString();

            while (char.IsDigit(CurrentChar) || (!isReal && CurrentChar == '.' && char.IsDigit(NextChar)))
            {
                if (CurrentChar == '.')
                {
                    isReal = true;
                }
                value += CurrentChar;
                EatCurrentChar();
            }

            SyntaxToken tok;

            if (isReal)
            {
                tok = Token(SyntaxTokenType.FloatLiteral);
                if (float.TryParse(value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out float f))
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
                tok = Token(SyntaxTokenType.IntLiteral);
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

            while (CurrentChar == '_' || char.IsLetterOrDigit(CurrentChar))
            {
                value += CurrentChar;
                EatCurrentChar();
            }

            if (value == "true" || value == "false")
            {
                bool.TryParse(value, out bool b);
                return Token(SyntaxTokenType.BoolLiteral, b);
            }

            if (Keywords.ContainsKey(value))
            {
                return Token(Keywords[value]);
            }

            if (Types.ContainsKey(value))
            {
                return Token(Types[value]);
            }

            return Token(SyntaxTokenType.ID, value);
        }

        char EatCurrentChar()
        {
            if (index >= code.Length)
            {
                return '\0';
            }

            char ch = CurrentChar;
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

        bool TryEatCurrentChar(char ch)
        {
            if (CurrentChar == ch)
            {
                EatCurrentChar();
                return true;
            }
            return false;
        }

        SyntaxToken Token(SyntaxTokenType type, object value = null)
        {
            return new SyntaxToken(type, value)
            {
                StartIndex = tokenStartIndex,
                EndIndex = index,
                ErrorIndex = -1,
            };
        }

        int Error(CodeErrorType type, string data = "")
        {
            Errors.Add(new CodeError(type, tokenStartLine, tokenStartColumn, data));
            return Errors.Count - 1;
        }
    }
}