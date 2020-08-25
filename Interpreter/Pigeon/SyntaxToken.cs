namespace Kostic017.Pigeon
{
    public class SyntaxToken
    {
        public SyntaxTokenType Type { get; set; }
        public object Value { get; set; }

        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public int ErrorIndex { get; set; }

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