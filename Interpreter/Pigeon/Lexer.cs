using Kostic017.Pigeon.Errors;
using System.Collections.Generic;

namespace Kostic017.Pigeon
{
    class Lexer
    {
        int line;
        int column;
        int index;

        int tokenStartLine;
        int tokenStartColumn;
        int tokenStartIndex;

        readonly int tabSize;
        readonly string code;
        
        char PrevChar => index - 1 >= 0 ? code[index - 1] : '\0';
        char CurrentChar => index < code.Length ? code[index] : '\0';
        char NextChar => index + 1 < code.Length ? code[index + 1] : '\0';

        internal CodeErrorBag ErrorBag { get; }

        internal Lexer(string code, int tabSize)
        {
            line = 1;
            index = 0;
            column = 0;
            this.code = code;
            this.tabSize = tabSize;
            ErrorBag = new CodeErrorBag();
        }

        internal SyntaxToken[] Lex()
        {
            SyntaxToken tok;
            List<SyntaxToken> tokens = new List<SyntaxToken>();

            do
            {
                tok = NextToken();
                tokens.Add(tok);
            } while (tok.Type != SyntaxTokenType.EOF);

            return tokens.ToArray();
        }

        private SyntaxToken NextToken()
        {
            while (CurrentChar != '\0')
            {
                char ch = EatCurrentChar();

                if (char.IsWhiteSpace(ch))
                {
                    continue;
                }

                tokenStartLine = line;
                tokenStartColumn = column;
                tokenStartIndex = index;

                switch (ch)
                {
                    case '"':
                        return LexString();

                    case '0': case '1': case '2': case '3': case '4':
                    case '5': case '6': case '7': case '8': case '9':
                        return LexNumber();

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

                    case '.':
                        return Token(SyntaxTokenType.Dot);

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
                        if (CurrentChar == '/' || CurrentChar == '*')
                        {
                            return LexComment();
                        }
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
                            return Token(SyntaxTokenType.EqEq);
                        }
                        return Token(SyntaxTokenType.Eq);

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
                        if (ch == '_' || char.IsLetter(ch))
                        {
                            return LexWord();
                        }

                        ReportError(CodeErrorType.UNEXPECTED_CHARACTER, 1, $"{ch}");
                        return Token(SyntaxTokenType.Illegal);
                }
            }

            return Token(SyntaxTokenType.EOF);
        }

        private SyntaxToken LexString()
        {
            bool done = false;
            string value = "";

            while (CurrentChar != '"' && !done)
            {
                switch (CurrentChar)
                {
                    case '\0':
                    case '\n':
                        done = true;
                        ReportError(CodeErrorType.UNTERMINATED_STRING, 1);
                        break;
                    
                    case '\\':
                        if (SyntaxFacts.EscapeChars.Contains(NextChar))
                        {
                            value += NextChar;
                        }
                        else
                        {
                            ReportError(CodeErrorType.INVALID_ESCAPE_CHAR, 2, $"\\{NextChar}");
                        }
                        EatCurrentChar();
                        break;
                    
                    default:
                        value += CurrentChar;
                        break;
                }

                EatCurrentChar();
            }

            TryEatCurrentChar('"');

            return Token(SyntaxTokenType.StringLiteral, value);
        }

        private SyntaxToken LexComment()
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
                    ReportError(CodeErrorType.UNTERMINATED_COMMENT_BLOCK, 2);
                    break;
                }

                EatCurrentChar();
            }

            return Token(SyntaxTokenType.BlockComment);
        }

        private SyntaxToken LexNumber()
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

        private SyntaxToken LexWord()
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

            if (SyntaxFacts.Keywords.ContainsKey(value))
            {
                return Token(SyntaxFacts.Keywords[value]);
            }

            if (SyntaxFacts.Types.ContainsKey(value))
            {
                return Token(SyntaxFacts.Types[value]);
            }

            return Token(SyntaxTokenType.ID, value);
        }

        private char EatCurrentChar()
        {
            if (index >= code.Length)
            {
                return '\0';
            }

            char ch = CurrentChar;
            ++index;

            switch (ch)
            {
                case '\n':
                    column = 0;
                    ++line;
                    break;
                case '\t':
                    column += tabSize;
                    break;
                default:
                    ++column;
                    break;
            }

            return ch;
        }

        private bool TryEatCurrentChar(char ch)
        {
            if (CurrentChar == ch)
            {
                EatCurrentChar();
                return true;
            }
            return false;
        }

        private SyntaxToken Token(SyntaxTokenType type, string lexeme = null)
        {
            var textSpan = new TextSpan(tokenStartLine, tokenStartColumn, tokenStartIndex, index);
            return new SyntaxToken(type, textSpan, lexeme);
        }

        private void ReportError(CodeErrorType type, int length, params string[] data)
        {
            var textSpan = new TextSpan(tokenStartLine, tokenStartColumn, tokenStartIndex, tokenStartIndex + length);
            ErrorBag.Report(type, textSpan, data);
        }
    }
}