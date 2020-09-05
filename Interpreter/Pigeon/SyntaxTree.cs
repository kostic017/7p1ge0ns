using Kostic017.Pigeon.AST;
using System.IO;
using System.Linq;

namespace Kostic017.Pigeon
{
    public class SyntaxTree
    {

        internal AstNode Ast { get; }
        internal CodeError[] LexerErrors { get; }
        internal CodeError[] ParserErrors { get; }

        public SyntaxToken[] Tokens { get; }
        public CodeError[] Errors { get; }

        SyntaxTree(string code, int tabSize = 4)
        {
            var lexer = new Lexer(code, tabSize);
            Tokens = lexer.Lex();
            
            var parser = new Parser(Tokens);
            Ast = parser.Parse();

            LexerErrors = lexer.Errors.ToArray();
            ParserErrors = parser.Errors.ToArray();
            Errors = lexer.Errors.Concat(parser.Errors).ToArray();
        }

        public string PrintTree()
        {
            return PrintTree(new StringWriter());
        }

        public string PrintTree(TextWriter writer)
        {
            Print(Ast, writer);
            return writer.ToString();
        }

        public static SyntaxTree Parse(string code, int tabSize = 4)
        {
            return new SyntaxTree(code, tabSize);
        }

        internal static void Print(AstNode root, TextWriter writer, string ident = "")
        {
            if (root is SyntaxTokenWrap tokenWrap)
            {
                writer.WriteLine(ident + tokenWrap.Token.Type + " " + tokenWrap.Token.Value);
            }
            else
            {
                writer.WriteLine(ident + root.Kind.ToString());
                foreach (var node in root.GetChildren())
                {
                    Print(node, writer, ident + "--");
                }
            }
        }

    }
}
