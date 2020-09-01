using Kostic017.Pigeon.AST;
using System.Collections.Generic;
using System.Linq;

namespace Kostic017.Pigeon
{
    public class SyntaxTree
    {
        readonly int tabSize;
        readonly string code;

        public List<CodeError> Errors { get; }

        public SyntaxTree(string code, int tabSize = 4)
        {
            this.code = code;
            this.tabSize = tabSize;
            Errors = new List<CodeError>();
        }

        public SyntaxToken[] Lex()
        {
            var lexer = new Lexer(code, tabSize);
            Errors.Concat(lexer.Errors);
            return lexer.Lex();
        }

        public AstNode Parse()
        {
            var parser = new Parser(RemoveComments(Lex()));
            Errors.Concat(parser.Errors);
            return parser.Parse();
        }
        
        SyntaxToken[] RemoveComments(SyntaxToken[] tokens)
        {
            return tokens.Where(token => token.Type != SyntaxTokenType.Comment && token.Type != SyntaxTokenType.BlockComment).ToArray();
        }
    }
}
