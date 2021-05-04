using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Kostic017.Pigeon;
using System;
using System.Text;

namespace TestProject
{
    class Program
    {
        static void Main()
        {
            PigeonBuiltins.RegisterFunction("int add(int, int)", Add);
            PigeonBuiltins.RegisterFunction("void print(int)", Print);

            while (true)
            {
                Console.Write("> ");
                var sb = new StringBuilder();

                string line = Console.ReadLine();

                if (line == "#cls")
                {
                    Console.Clear();
                    continue;
                }

                while (!string.IsNullOrWhiteSpace(line))
                {
                    sb.AppendLine(line);
                    Console.Write("| ");
                    line = Console.ReadLine();
                }

                Console.WriteLine();
                var inputStream = new AntlrInputStream(sb.ToString());
                var lexer = new PigeonLexer(inputStream);
                var tokenStream = new CommonTokenStream(lexer);
                var parser = new PigeonParser(tokenStream);
                parser.AddErrorListener(ConsoleErrorListener.Instance);
                parser.program().PrintTree(Console.Out, parser.RuleNames);
                Console.WriteLine();
            }
        }

        private static object Print(object[] arg)
        {
            Console.WriteLine(arg[0]);
            return null;
        }

        private static object Add(object[] arg)
        {
            return (int)arg[0] + (int)arg[1];
        }
    }
    
    // TODO this class should be part of the ANTLR4 runtime??
    class ConsoleErrorListener : BaseErrorListener
    {
        public static readonly ConsoleErrorListener Instance = new ConsoleErrorListener();
        public override void SyntaxError([NotNull] IRecognizer recognizer, [Nullable] IToken offendingSymbol, int line, int charPositionInLine, [NotNull] string msg, [Nullable] RecognitionException e)
        {
            Console.WriteLine("line " + line + ":" + charPositionInLine + " " + msg);
        }
    }
}
