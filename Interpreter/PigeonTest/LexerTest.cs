using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Kostic017.Pigeon.Tests
{
    public class LexerTest
    {
        [Theory]
        [MemberData(nameof(GetData))]
        public void Lex(string code, SyntaxToken[] expected)
        {
            Interpreter interpreter = new Interpreter();
            List<CodeError> errors = new List<CodeError>();

            var actual = interpreter.Lex(code, errors);
            
            Assert.Equal(expected.Length, actual.Length);
            
            var pairs = expected.Zip(actual, (e, a) => new { Expected = e, Actual = a }); ;
            
            foreach (var pair in pairs)
            {
                Assert.Equal(pair.Expected.Type, pair.Actual.Type);
                Assert.Equal(pair.Expected.Value?.ToString(), pair.Actual.Value);
                //Assert.Equal(pair.Expected.StartLine, pair.Actual.StartLine);
                //Assert.Equal(pair.Expected.EndColumn, pair.Actual.EndColumn);
                //Assert.Equal(pair.Expected.StartIndex, pair.Actual.StartIndex);
                //Assert.Equal(pair.Expected.EndIndex, pair.Actual.EndIndex);
            }
        }

        public static IEnumerable<object[]> GetData()
        {
            foreach (var (_, code, json) in TestHelper.GetCases("lexer"))
            {
                var expected = JsonConvert.DeserializeObject<SyntaxToken[]>(json);
                yield return new object[] { code, expected };
            }
        }
    }
}
