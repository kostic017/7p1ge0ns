using Kostic017.Pigeon.AST;
using Kostic017.Pigeon.Error;
using System;
using System.IO;
using System.Linq;

namespace Kostic017.Pigeon
{
    public class SyntaxTree
    {
        readonly Lexer lexer;
        readonly Parser parser;

        internal Program Root { get; }
        internal CodeError[] LexerErrors => lexer.ErrorBag.Errors;
        internal CodeError[] ParserErrors => parser.ErrorBag.Errors;

        public SyntaxToken[] Tokens { get; }
        public CodeError[] Errors => lexer.ErrorBag.Errors.Concat(parser.ErrorBag.Errors).ToArray();

        private SyntaxTree(string code, int tabSize = 4)
        {
            lexer = new Lexer(code, tabSize);
            Tokens = lexer.Lex();
            parser = new Parser(Tokens);
            Root = parser.Parse();
        }

        public static SyntaxTree Parse(string code, int tabSize = 4)
        {
            return new SyntaxTree(code, tabSize);
        }

        public void PrintTree(TextWriter writer)
        {
            Print(Root, writer);
        }

        private static void Print(AstNode root, TextWriter writer, string ident = "")
        {
            var isConsole = writer == Console.Out;
            if (root is SyntaxTokenWrap tokenWrap)
            {
                if (isConsole)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                }
                writer.WriteLine(ident + tokenWrap.Token.Type + " " + tokenWrap.Token.Value);
            }
            else
            {
                if (isConsole)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                }
                writer.WriteLine(ident + root.Kind.ToString());
                foreach (var node in root.GetChildren())
                {
                    Print(node, writer, ident + "  ");
                }
            }
            if (isConsole)
            {
                Console.ResetColor();
            }
        }

    }
}
