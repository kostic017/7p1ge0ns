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

                var syntaxTree = SyntaxTree.Parse(line);
                syntaxTree.PrintTree(Console.Out);

                foreach (var error in syntaxTree.Errors)
                {
                    Console.WriteLine(error.DetailedMessage);
                }

                Console.WriteLine();

            }

        }
    }
}
