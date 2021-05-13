using Kostic017.Pigeon;
using Kostic017.Pigeon.Symbols;
using System;
using System.IO;
using System.Text;

namespace TestProject
{
    class Program
    {
        private static readonly string SAMPLES_FOLDER = "..\\..\\..\\..\\Samples";

        static void Main()
        {
            var globalScope = new GlobalScope();

            globalScope.DeclareFunction(PigeonType.Void, "prompt", new Variable[] {
                new Variable(PigeonType.String),
                new Variable(PigeonType.Any),
            }, Print);

            globalScope.DeclareFunction(PigeonType.Void, "print", new Variable[]
            {
                new Variable(PigeonType.Any)
            }, Prompt);

            while (true)
            {
                var sb = new StringBuilder();
                var line = Console.ReadLine();

                if (HandleCommand(line))
                    continue;
                
                while (!string.IsNullOrWhiteSpace(line))
                {
                    sb.AppendLine(line);
                    Console.Write("| ");
                    line = Console.ReadLine();
                }

                ExecuteCode(sb.ToString(), globalScope);

                Console.Write("> ");
            }
        }

        private static bool HandleCommand(string line)
        {
            if (!line.StartsWith("#"))
                return false;
            var tokens = line.Split(' ');

            switch (tokens[0])
            {
                case "#list":
                    foreach (var file in Directory.GetFiles(SAMPLES_FOLDER, "*.pig"))
                        Console.WriteLine(Path.GetFileNameWithoutExtension(file));
                    break;
                case "#exec":
                    if (tokens.Length > 1)
                        ExecuteFile(Path.Combine(SAMPLES_FOLDER, tokens[1] + ".pig"));
                    else
                        foreach (var file in Directory.GetFiles(SAMPLES_FOLDER, "*.pig"))
                            ExecuteFile(file);
                    break;
                case "#cls:":
                    Console.Clear();
                    break;
                default:
                    Console.WriteLine($"Valid commands: #list, #exec [file], #cls");
                    break;
            }

            return true;
        }

        private static void ExecuteCode(string code, GlobalScope globalScope)
        {
            Console.WriteLine();
            var interpreter = new Interpreter(code, globalScope);
            interpreter.PrintTree(Console.Out);
            interpreter.PrintErrors(Console.Out);
        }

        private static void ExecuteFile(string file)
        {
            var name = Path.GetFileNameWithoutExtension(file);
            Console.WriteLine($"### {name} ###");
            ExecuteCode(File.ReadAllText(file));
        }

        private static object Print(object[] arg)
        {
            Console.WriteLine(arg[0]);
            return null;
        }

        private static object Prompt(object[] arg)
        {
            Console.Write(arg[0]);
            return null;
        }
    }
}
