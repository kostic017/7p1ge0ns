using Kostic017.Pigeon.AST;
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

        public (SyntaxToken[], CodeError[]) Lex(string code)
        {
            return lexer.Lex(code);
        }

        public (AstNode, CodeError[]) Parse(SyntaxToken[] tokens)
        {
            return parser.Parse(RemoveComments(tokens));
        }

        public (AstNode, CodeError[]) Parse(string code)
        {
            var (tokens, _) = Lex(code);
            return Parse(tokens);
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
