using System.Collections.Generic;
using System.Globalization;

namespace Kostic017.Pigeon
{
    class Lexer
    {
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

        List<CodeError> errors;

        char PrevChar => index - 1 >= 0 ? code[index - 1] : '\0';
        char CurrentChar => index < code.Length ? code[index] : '\0';
        char NextChar => index + 1 < code.Length ? code[index + 1] : '\0';

        public int TabSize { get; set; } = 4;
        
        internal SyntaxToken[] Lex(string str, List<CodeError> errors)
        {
            line = 1;
            index = 0;
            code = str;
            column = 0;
            this.errors = errors;

            SyntaxToken tok;
            List<SyntaxToken> tokens = new List<SyntaxToken>();

            do
            {
                tok = NextToken();
                tokens.Add(tok);
            } while (tok.Type != SyntaxTokenType.EOF);

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
                        return Token(SyntaxTokenType.Semicolon);

                    case ',':
                        return Token(SyntaxTokenType.Comma);

                    case '+':
                        if (TryEatCurrentChar('='))
                        {
                            return Token(SyntaxTokenType.PlusEq);
                        }
                        if (TryEatCurrentChar('+'))
                        {
                            return Token(SyntaxTokenType.Inc);
                        }
                        return Token(SyntaxTokenType.Plus);

                    case '-':
                        if (TryEatCurrentChar('='))
                        {
                            return Token(SyntaxTokenType.MinusEq);
                        }
                        if (TryEatCurrentChar('-'))
                        {
                            return Token(SyntaxTokenType.Dec);
                        }
                        return Token(SyntaxTokenType.Minus);

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

                    case '^':
                        if (TryEatCurrentChar('='))
                        {
                            EatCurrentChar();
                            return Token(SyntaxTokenType.PowerEq);
                        }
                        return Token(SyntaxTokenType.Power);

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
                        ReportError(CodeErrorType.UNEXPECTED_CHARACTER, $"{ch}");
                        return Token(SyntaxTokenType.Illegal);
                }
            }

            return Token(SyntaxTokenType.EOF);
        }

        SyntaxToken LexString()
        {
            string value = "";

            while (CurrentChar != '"')
            {
                if (CurrentChar == '\0')
                {
                    ReportError(CodeErrorType.UNTERMINATED_STRING);
                    break;
                }

                if (CurrentChar == '\n')
                {
                    ReportError(CodeErrorType.NEWLINE_IN_STRING);
                    break;
                }

                if (CurrentChar == '\\')
                {
                    if (escapes.Contains(NextChar))
                    {
                        value += NextChar;
                    }
                    else
                    {
                        ReportError(CodeErrorType.INVALID_ESCAPE_CHAR, $"\\{NextChar}");
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

            return Token(SyntaxTokenType.StringLiteral, value);
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
                    ReportError(CodeErrorType.UNTERMINATED_COMMENT_BLOCK);
                    break;
                }

                EatCurrentChar();
            }

            return Token(SyntaxTokenType.BlockComment);
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

            return Token(isReal ? SyntaxTokenType.FloatLiteral : SyntaxTokenType.IntLiteral, value);
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
                return Token(SyntaxTokenType.BoolLiteral, value);
            }

            if (ReservedWords.Keywords.ContainsKey(value))
            {
                return Token(ReservedWords.Keywords[value]);
            }

            if (ReservedWords.Types.ContainsKey(value))
            {
                return Token(ReservedWords.Types[value]);
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

        SyntaxToken Token(SyntaxTokenType type, string lexeme = null)
        {
            return new SyntaxToken(type, tokenStartIndex, index, tokenStartLine, tokenStartColumn, lexeme);
        }

        void ReportError(CodeErrorType type, params string[] data)
        {
            errors.Add(new CodeError(type, tokenStartLine, tokenStartColumn, data));
        }
    }
}