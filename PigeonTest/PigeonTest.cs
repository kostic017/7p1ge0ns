﻿using Kostic017.Pigeon.Symbols;
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
        
        private readonly Builtins b = new Builtins();

        public PigeonTest()
        {
            b.RegisterFunction(PigeonType.String, "prompt", Prompt, PigeonType.String);
            b.RegisterFunction(PigeonType.Int, "prompt_i", PromptI, PigeonType.String);
            b.RegisterFunction(PigeonType.Float, "prompt_f", PromptF, PigeonType.String);
            b.RegisterFunction(PigeonType.Bool, "prompt_b", PromptB, PigeonType.String);
            b.RegisterFunction(PigeonType.Void, "print", Print, PigeonType.Any);
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public void Test(string sample)
        {
            var inFile = Path.Combine(SAMPLES_FOLDER, sample + ".in");
            var code = Normalize(File.ReadAllText(Path.Combine(SAMPLES_FOLDER, sample + ".pig")));
            var outputs = ReadCases(Path.Combine(SAMPLES_FOLDER, sample + ".out"));

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

        public static IEnumerable<object[]> TestCases()
        {
            foreach (var sample in Directory.GetFiles(SAMPLES_FOLDER, "*.out"))
                yield return new string[] { Path.GetFileNameWithoutExtension(sample) };
        }

        private string Output()
        {
            return Normalize(outputStream.ToString());
        }

        private string[] ReadCases(string file)
        {
            return Normalize(File.ReadAllText(file)).Split("---").Select(v => v.Trim()).ToArray();
        }

        private string Normalize(string str)
        {
            return str.Replace("\r\n", "\n").Trim();
        }

        private void Execute(string code)
        {
            var interpreter = new Interpreter(code, b);
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
