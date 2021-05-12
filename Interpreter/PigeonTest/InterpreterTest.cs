using System.IO;
using Xunit;

namespace Kostic017.Pigeon.Tests
{
    public class InterpreterTest
    {
        [Fact]
        public void Test()
        {
            foreach (var sample in Directory.GetFiles("..\\..\\..\\..\\Samples", "*.pig"))
            {
                var name = Path.GetFileNameWithoutExtension(sample);
                var code = File.ReadAllText(sample);
                var interpreter = new Interpreter(code);
                var writer = new StringWriter();
                interpreter.PrintTree(writer);
            }
        }
    }
}
