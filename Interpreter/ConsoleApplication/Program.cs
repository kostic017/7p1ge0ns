using Kostic017.Pigeon;
using System;
using System.IO;
using System.Text;

namespace TestProject
{
    class Program
    {
        static void Main()
        {
            foreach (var sample in Directory.GetFiles("..\\..\\..\\..\\Samples", "*.pig"))
            {
                var name = Path.GetFileNameWithoutExtension(sample);
                var code = File.ReadAllText(sample);
                var interpreter = new Interpreter(code);
                interpreter.PrintTree(Console.Out);
                interpreter.PrintErrors(Console.Out);
            }
            Console.ReadLine();
        }

        //static void Main()
        //{
        //    PigeonBuiltins.RegisterFunction("int add(int, int)", Add);
        //    PigeonBuiltins.RegisterFunction("void print(int)", Print);

        //    while (true)
        //    {
        //        Console.Write("> ");
        //        var sb = new StringBuilder();

        //        string line = Console.ReadLine();

        //        if (line == "#cls")
        //        {
        //            Console.Clear();
        //            continue;
        //        }

        //        while (!string.IsNullOrWhiteSpace(line))
        //        {
        //            sb.AppendLine(line);
        //            Console.Write("| ");
        //            line = Console.ReadLine();
        //        }

        //        Console.WriteLine();
        //        var interpreter = new Interpreter(sb.ToString());
        //        interpreter.PrintTree(Console.Out);
        //        interpreter.PrintErrors(Console.Out);
        //        Console.WriteLine();
        //    }
        //}

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
