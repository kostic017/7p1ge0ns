using Kostic017.Pigeon;
using Kostic017.Pigeon.AST;
using System;
using System.Collections.Generic;

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

                List<CodeError> errors = new List<CodeError>();
                SyntaxToken[] tokens = interpreter.Lex(line, errors);
                AstNode ast = interpreter.Parse(tokens, errors);
                Console.WriteLine(ast.Print());

                foreach (CodeError error in errors)
                {
                    Console.WriteLine(error.DetailedMessage);
                }

                Console.WriteLine();

            }

        }
    }
}
