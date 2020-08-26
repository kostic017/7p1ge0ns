namespace Kostic017.Pigeon
{
    public class SyntaxToken
    {
        public SyntaxTokenType Type { get; set; }
        public object Value { get; set; }

        public int StartIndex { get; set; } = -1;
        public int EndIndex { get; set; } = -1;
        public int ErrorIndex { get; set; } = -1;

        public SyntaxToken()
        {
        }

        public SyntaxToken(SyntaxTokenType type)
        {
            Type = type;
        }

        public SyntaxToken(SyntaxTokenType type, object value)
        {
            Type = type;
            Value = value;
        }
    }
}