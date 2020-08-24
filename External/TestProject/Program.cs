using Kostic017.Pigeon;
using System;

namespace TestProject
{
    class Program
    {
        static void Main(string[] args)
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

                SyntaxToken[] tokens = lexer.Lex(line);

                foreach (SyntaxToken token in tokens)
                {
                    Console.WriteLine($"{token.Type} {token.Value}");
                }

                foreach (CodeError error in lexer.Errors)
                {
                    Console.WriteLine(error.DetailedMessage());
                }

            }

        }
    }
}
