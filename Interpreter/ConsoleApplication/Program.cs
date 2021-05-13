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

        private readonly BuiltinSymbols builtinSymbols = new BuiltinSymbols();

        private Program()
        {
            builtinSymbols.RegisterFunction(PigeonType.Void, "prompt", new Variable[] {
                new Variable(PigeonType.String),
                new Variable(PigeonType.Any),
            }, Print);

            builtinSymbols.RegisterFunction(PigeonType.Void, "print", new Variable[]
            {
                new Variable(PigeonType.Any)
            }, Prompt);
        }
        
        private void ExecuteCode(string code)
        {
            Console.WriteLine();
            var interpreter = new Interpreter(code, builtinSymbols);
            interpreter.PrintTree(Console.Out);
            interpreter.PrintErrors(Console.Out);
        }

        private void ExecuteFile(string file)
        {
            var name = Path.GetFileNameWithoutExtension(file);
            Console.WriteLine($"### {name} ###");
            ExecuteCode(File.ReadAllText(file));
        }

        private bool HandleCommand(string line)
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

        static void Main()
        {
            var program = new Program();

            while (true)
            {
                Console.Write("> ");
                var sb = new StringBuilder();
                var line = Console.ReadLine();

                if (program.HandleCommand(line))
                    continue;
                
                while (!string.IsNullOrWhiteSpace(line))
                {
                    sb.AppendLine(line);
                    Console.Write("| ");
                    line = Console.ReadLine();
                }

                program.ExecuteCode(sb.ToString());
            }
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
