using Kostic017.Pigeon.AST;
using System.Linq;

namespace Kostic017.Pigeon
{
    public class SyntaxTree
    {
        
        public AstNode Ast { get; }
        public SyntaxToken[] Tokens { get; }
        public CodeError[] Errors { get; }

        public SyntaxTree(string code, int tabSize = 4)
        {
            var lexer = new Lexer(code, tabSize);
            Tokens = lexer.Lex();

            var parser = new Parser(Tokens);
            Ast = parser.Parse();

            Errors = lexer.Errors.Concat(parser.Errors).ToArray();
        }

    }
}
