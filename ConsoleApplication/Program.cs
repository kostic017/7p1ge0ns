using Kostic017.Pigeon;
using Kostic017.Pigeon.Symbols;
using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace TestProject
{
    class Program
    {
        private static readonly string SAMPLES_FOLDER = "..\\..\\..\\..\\Samples";

        private bool printTree = false;
        private readonly Builtins builtins = new Builtins();

        private Program()
        {
            builtins.RegisterFunction(PigeonType.String, "prompt", new Variable[] { new Variable(PigeonType.String) }, Prompt);
            builtins.RegisterFunction(PigeonType.Int, "prompt_i", new Variable[] { new Variable(PigeonType.String) }, PromptI);
            builtins.RegisterFunction(PigeonType.Float, "prompt_f", new Variable[] { new Variable(PigeonType.String) }, PromptF);
            builtins.RegisterFunction(PigeonType.Bool, "prompt_b", new Variable[] { new Variable(PigeonType.String) }, PromptB);
            builtins.RegisterFunction(PigeonType.Void, "print", new Variable[] { new Variable(PigeonType.Any) }, Print);
        }
        
        private void ExecuteCode(string code)
        {
            var interpreter = new Interpreter(code, builtins);
            if (printTree)
                interpreter.PrintTree(Console.Out);
            interpreter.PrintErr(Console.Out);
            if (interpreter.NoErrors())
                interpreter.Evaluate();
        }

        private void ExecuteFile(string file)
        {
            Console.WriteLine();
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
                    Console.WriteLine();
                    break;
                case "#cls:":
                    Console.Clear();
                    break;
                case "#tree":
                    printTree = !printTree;
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
            return Console.ReadLine();
        }

        private static object PromptI(object[] arg)
        {
            Console.Write(arg[0]);
            return int.Parse(Console.ReadLine());
        }

        private static object PromptF(object[] arg)
        {
            Console.Write(arg[0]);
            return float.Parse(Console.ReadLine(), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
        }

        private static object PromptB(object[] arg)
        {
            Console.Write(arg[0]);
            return bool.Parse(Console.ReadLine());
        }
    }
}
