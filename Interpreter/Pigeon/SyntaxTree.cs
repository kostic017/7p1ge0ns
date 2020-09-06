using Kostic017.Pigeon.AST;
using System.IO;
using System.Linq;

namespace Kostic017.Pigeon
{
    public class SyntaxTree
    {
        readonly Lexer lexer;
        readonly Parser parser;

        internal AstNode Ast { get; }
        internal CodeError[] LexerErrors => lexer.Errors.ToArray();
        internal CodeError[] ParserErrors => parser.Errors.ToArray();

        public SyntaxToken[] Tokens { get; }
        public CodeError[] Errors => lexer.Errors.Concat(parser.Errors).ToArray();

        SyntaxTree(string code, int tabSize = 4)
        {
            lexer = new Lexer(code, tabSize);
            Tokens = lexer.Lex();
            parser = new Parser(Tokens);
            Ast = parser.Parse();
        }

        public static SyntaxTree Parse(string code, int tabSize = 4)
        {
            return new SyntaxTree(code, tabSize);
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

        static void Print(AstNode root, TextWriter writer, string ident = "")
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
