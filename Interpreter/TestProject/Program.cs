using Kostic017.Pigeon;
using Kostic017.Pigeon.AST;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TestProject
{
    class Program
    {
        static void Main()
        {
            Interpreter interpreter = new Interpreter();

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

                var (tokens, lexErrors) = interpreter.Lex(line);
                var (ast, parseErrors) = interpreter.Parse(tokens);
                Console.WriteLine(ast.PrettyPrint());

                foreach (var error in lexErrors.Concat(parseErrors))
                {
                    Console.WriteLine(error.DetailedMessage);
                }

                Console.WriteLine();

            }

        }
    }
}
