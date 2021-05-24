using Kostic017.Pigeon.Symbols;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Xunit;

namespace Kostic017.Pigeon.Tests
{
    public class PigeonTest
    {
        private static readonly string SAMPLES_FOLDER = "..\\..\\..\\..\\Samples";

        private TextWriter outputStream;
        private Queue<string> inputStream;
        
        private readonly Builtins builtins = new Builtins();

        public PigeonTest()
        {
            builtins.RegisterFunction(PigeonType.String, "prompt", new Variable[] { new Variable(PigeonType.String) }, Prompt);
            builtins.RegisterFunction(PigeonType.Int, "prompt_i", new Variable[] { new Variable(PigeonType.String) }, PromptI);
            builtins.RegisterFunction(PigeonType.Float, "prompt_f", new Variable[] { new Variable(PigeonType.String) }, PromptF);
            builtins.RegisterFunction(PigeonType.Bool, "prompt_b", new Variable[] { new Variable(PigeonType.String) }, PromptB);
            builtins.RegisterFunction(PigeonType.Void, "print", new Variable[] { new Variable(PigeonType.Any) }, Print);
        }

        [Fact]
        public void Test()
        {
            foreach (var outFile in Directory.GetFiles(SAMPLES_FOLDER, "*.out"))
            {
                var sample = Path.GetFileNameWithoutExtension(outFile);
                var inFile = Path.Combine(SAMPLES_FOLDER, sample + ".in");
                var code = File.ReadAllText(Path.Combine(SAMPLES_FOLDER, sample + ".pig"));
                var outputs = ReadCases(outFile);

                if (File.Exists(inFile))
                {
                    var inputs = ReadCases(inFile);
                    for (var i = 0; i < inputs.Length; ++i)
                    {
                        
                        inputStream = new Queue<string>();
                        outputStream = new StringWriter();
                        
                        foreach (var input in inputs[i].Split('\n'))
                            inputStream.Enqueue(input);

                        Execute(code);

                        Assert.Equal(outputs[i], Output());
                    
                    }
                }
                else
                {
                    outputStream = new StringWriter();
                    Execute(code);
                    Assert.Equal(outputs[0], Output());
                }

            }
        }

        private string Output()
        {
            return outputStream.ToString().Replace("\r\n", "\n").Trim();
        }

        private string[] ReadCases(string file)
        {
            return File.ReadAllText(file).Split("---").Select(v => v.Trim()).ToArray();
        }

        private void Execute(string code)
        {
            var interpreter = new Interpreter(code, builtins);
            interpreter.PrintErr(outputStream);
            interpreter.Evaluate();
        }

        private object Print(object[] arg)
        {
            outputStream.WriteLine(arg[0]);
            return null;
        }

        private object Prompt(object[] arg)
        {
            return inputStream.Dequeue();
        }

        private object PromptI(object[] arg)
        {
            return int.Parse(inputStream.Dequeue());
        }

        private object PromptF(object[] arg)
        {
            return float.Parse(inputStream.Dequeue(), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
        }

        private object PromptB(object[] arg)
        {
            return bool.Parse(inputStream.Dequeue());
        }
    }
}
