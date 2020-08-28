using Kostic017.Pigeon.AST;
using System.Collections.Generic;

namespace Kostic017.Pigeon
{
    public class Interpreter
    {
        readonly Lexer lexer;
        readonly Parser parser;

        public Interpreter()
        {
            lexer = new Lexer();
            parser = new Parser();
        }

        public SyntaxToken[] Lex(string code, List<CodeError> errors)
        {
            return lexer.Lex(code, errors);
        }

        public AstNode Parse(SyntaxToken[] tokens, List<CodeError> errors)
        {
            return parser.Parse(tokens, errors);
        }

        public void SetTabSize(int tabSize)
        {
            lexer.TabSize = tabSize;
        }
    }
}
