using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Kostic017.Pigeon.Tests
{
    public class SyntaxTokenTypeParser : JsonConverter<SyntaxTokenType>
    {
        public override SyntaxTokenType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            => Enum.Parse<SyntaxTokenType>(reader.GetString());

        public override void Write(Utf8JsonWriter writer, SyntaxTokenType value, JsonSerializerOptions options)
            => value.ToString();
    }
}
