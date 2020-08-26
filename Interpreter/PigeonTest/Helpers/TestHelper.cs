using System.Collections.Generic;
using System.IO;

namespace Kostic017.Pigeon.Tests
{
    class TestHelper
    {
        const string CASES_DIR = "../../../Cases";

        internal static IEnumerable<(string caseName, string code, string exp)> GetCases(string kind)
        {
            var resultFiles = Directory.GetFiles(CASES_DIR, $"*-{kind}.json");
            foreach (var resultFile in resultFiles)
            {
                var exp = File.ReadAllText(resultFile);
                var caseName = Path.GetFileNameWithoutExtension(resultFile).Split('-')[0];
                var code = File.ReadAllText($"{CASES_DIR}/{caseName}.pig");
                yield return (caseName, code, exp);
            }
        }
    }
}
