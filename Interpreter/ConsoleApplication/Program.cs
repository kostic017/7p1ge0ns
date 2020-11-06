using Kostic017.Pigeon;
using System;
using System.Linq;
using System.Text;

namespace TestProject
{
    class Program
    {
        static void Main()
        {
            BuiltinFunctions.Register("int add(int, int)", Add);
            BuiltinFunctions.Register("void print(int)", Print);

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
                var analysisResult = TypeChecker.Anaylize(syntaxTree);

                foreach (var error in syntaxTree.Errors.Concat(analysisResult.Errors))
                    Console.WriteLine(error);

                Evaluator.Evaluate(analysisResult);

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
}
