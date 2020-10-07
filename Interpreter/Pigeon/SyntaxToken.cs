namespace Kostic017.Pigeon
{
    public class SyntaxToken
    {
        public SyntaxTokenType Type { get; }
        public string Value { get; }
        public TextSpan TextSpan { get; }

        internal SyntaxToken(SyntaxTokenType type, TextSpan textSpan, string value = null)
        {
            Type = type;
            TextSpan = textSpan;
            Value = value;
        }
    }
}