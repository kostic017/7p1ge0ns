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

                var interpreter = new Interpreter(line);
                var tokens = interpreter.Lex();
                var ast = interpreter.Parse();

                foreach (var token in tokens)
                {
                    Console.WriteLine($"{token.Type} {token.Value}");
                }

                Console.WriteLine();

                Console.WriteLine(ast.PrettyPrint());

                foreach (var error in interpreter.Errors)
                {
                    Console.WriteLine(error.DetailedMessage);
                }

                Console.WriteLine();

            }

        }
    }
}
