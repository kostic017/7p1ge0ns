using System.Collections.Generic;
using System.IO;

namespace Kostic017.Pigeon.Tests
{
    class TestHelper
    {
        const string CASES_DIR = "../../../Cases";

        internal static IEnumerable<(string name, string code, string exp)> GetCases(string kind)
        {
            var resultFiles = Directory.GetFiles(CASES_DIR, $"*-{kind}.json");
            foreach (var resultFile in resultFiles)
            {
                var exp = File.ReadAllText(resultFile);
                var name = Path.GetFileNameWithoutExtension(resultFile).Split('-')[0];
                var code = File.ReadAllText($"{CASES_DIR}/{name}.pig");
                yield return (name, code, exp);
            }
        }
    }
}
