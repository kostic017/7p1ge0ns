using Kostic017.Pigeon;
using System;

namespace TestProject
{
    class Program
    {
        static void Main()
        {
            

            while (true)
            {
                Console.Write("> ");
                string line = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(line))
                    break;

                if (line == "#cls")
                {
                    Console.Clear();
                    break;
                }

                var syntaxTree = new SyntaxTree(line);
                var tokens = syntaxTree.Lex();
                var ast = syntaxTree.Parse();

                foreach (var token in tokens)
                {
                    Console.WriteLine($"{token.Type} {token.Value}");
                }

                Console.WriteLine(ast);

                Console.WriteLine();

                foreach (var error in syntaxTree.Errors)
                {
                    Console.WriteLine(error.DetailedMessage);
                }

                Console.WriteLine();

            }

        }
    }
}
