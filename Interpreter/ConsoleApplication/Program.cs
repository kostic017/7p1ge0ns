using Kostic017.Pigeon;
using System;
using System.Text;

namespace TestProject
{
    class Program
    {
        static void Main()
        {
            while (true)
            {
                Console.Write("> ");
                var sb = new StringBuilder();

                string line = Console.ReadLine();

                switch (line)
                {
                    case "#quit":
                        return;
                    case "#cls":
                        Console.Clear();
                        continue;
                }

                while (!string.IsNullOrWhiteSpace(line))
                {
                    sb.AppendLine(line);
                    Console.Write("| ");
                    line = Console.ReadLine();
                }

                var syntaxTree = SyntaxTree.Parse(sb.ToString());
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
