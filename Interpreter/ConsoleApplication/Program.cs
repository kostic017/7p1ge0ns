using Antlr4.Runtime;
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

            bool showTree = false;

            while (true)
            {
                Console.Write("> ");
                var sb = new StringBuilder();

                string line = Console.ReadLine();

                switch (line)
                {
                    case "#quit":
                        return;
                    case "#showTree":
                        showTree = !showTree;
                        break;
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
                
                var inputStream = new AntlrInputStream(sb.ToString());
                var lexer = new PigeonLexer(inputStream);
                var commonTokenStream = new CommonTokenStream(lexer);
                var parser = new PigeonParser(commonTokenStream);
                var tree = parser.program();
                Console.WriteLine(tree.ToStringTree(parser));
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
