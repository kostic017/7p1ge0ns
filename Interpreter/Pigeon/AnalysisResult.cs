using Kostic017.Pigeon.AST;
using Kostic017.Pigeon.Errors;
using Kostic017.Pigeon.TAST;
using System;
using System.IO;

namespace Kostic017.Pigeon
{
    public class AnalysisResult
    {
        internal SyntaxToken[] Tokens { get; }
        internal AstRoot AstRoot { get; }
        internal TypedAstRoot TypedAstRoot { get; }

        public SyntaxErrorBag Errors { get; }

        internal AnalysisResult(SyntaxToken[] tokens, AstRoot astRoot, TypedAstRoot typedAstRoot, SyntaxErrorBag errorBag)
        {
            Tokens = tokens;
            AstRoot = astRoot;
            TypedAstRoot = typedAstRoot;
            Errors = errorBag;
        }

        public void PrintTree(TextWriter writer)
        {
            var isConsole = writer == Console.Out;

            if (isConsole)
                Console.ForegroundColor = ConsoleColor.Cyan;
            
            writer.WriteLine(AstRoot.Kind.ToString());
            foreach (var node in AstRoot.GetChildren())
                PrintTree(node, writer, "  ");

            if (isConsole)
                Console.ResetColor();
        }

        private void PrintTree(SyntaxNode root, TextWriter writer, string ident)
        {
            var isConsole = writer == Console.Out;

            if (root is SyntaxTokenWrap tokenWrap)
            {
                if (isConsole)
                    Console.ForegroundColor = ConsoleColor.Blue;
                writer.WriteLine(ident + tokenWrap.Token.Type + " " + tokenWrap.Token.Value);
            }
            else
            {
                if (isConsole)
                    Console.ForegroundColor = ConsoleColor.Cyan;
            
                writer.WriteLine(ident + root.Kind.ToString());
                foreach (var node in root.GetChildren())
                    PrintTree(node, writer, ident + "  ");
            }

            if (isConsole)
                Console.ResetColor();
        }
    }
}
