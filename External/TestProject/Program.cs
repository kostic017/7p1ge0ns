using Kostic017.Pigeon;
using System;

namespace TestProject
{
    class Program
    {
        static void Main()
        {
            Lexer lexer = new Lexer();

            while (true)
            {
                Console.Write("> ");
                string line = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(line))
                {
                    break;
                }

                if (line == "#cls")
                {
                    Console.Clear();
                    break;
                }

                SyntaxToken[] tokens = lexer.Lex(line);

                foreach (SyntaxToken token in tokens)
                {
                    Console.WriteLine($"{token.Type} {token.Value}");
                }

                foreach (CodeError error in lexer.Errors)
                {
                    Console.WriteLine(error.DetailedMessage());
                }

                Console.WriteLine();

            }

        }
    }
}
