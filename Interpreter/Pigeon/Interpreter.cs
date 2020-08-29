using Kostic017.Pigeon.AST;
using System.Collections.Generic;
using System.Linq;

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
            return parser.Parse(RemoveComments(tokens), errors);
        }

        public void SetTabSize(int tabSize)
        {
            lexer.TabSize = tabSize;
        }

        SyntaxToken[] RemoveComments(SyntaxToken[] tokens)
        {
            return tokens.Where(token => token.Type != SyntaxTokenType.Comment && token.Type != SyntaxTokenType.BlockComment).ToArray();
        }
    }
}
