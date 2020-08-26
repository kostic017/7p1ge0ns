using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using Xunit;

namespace Kostic017.Pigeon.Tests
{
    public class LexerTest
    {
        [Theory]
        [MemberData(nameof(GetData))]
        public void Lex(string code, SyntaxToken[] expected)
        {
            Lexer lexer = new Lexer();
            var actual = lexer.Lex(code);
            
            Assert.Equal(expected.Length, actual.Length);
            
            var pairs = expected.Zip(actual, (e, a) => new { Expected = e, Actual = a }); ;
            
            foreach (var pair in pairs)
            {
                Assert.Equal(pair.Expected.Type, pair.Actual.Type);

                var actualValue = pair.Actual.Value != null
                    ? string.Format(CultureInfo.InvariantCulture, "{0}", pair.Actual.Value)
                    : null;

                Assert.Equal(pair.Expected.Value?.ToString(), actualValue);

                if (pair.Expected.StartIndex > -1)
                {
                    Assert.Equal(pair.Expected.StartIndex, pair.Actual.StartIndex);
                }

                if (pair.Expected.EndIndex > -1)
                {
                    Assert.Equal(pair.Expected.EndIndex, pair.Actual.EndIndex);
                }
            }
        }

        public static IEnumerable<object[]> GetData()
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new SyntaxTokenTypeConverter());
            foreach (var (caseName, code, exp) in TestHelper.GetCases("l"))
            {
                var expected = JsonSerializer.Deserialize<SyntaxToken[]>(exp, options);
                yield return new object[] { code, expected };
            }
        }
    }
}
