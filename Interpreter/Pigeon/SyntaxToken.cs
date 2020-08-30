namespace Kostic017.Pigeon
{
    public class SyntaxToken
    {
        public SyntaxTokenType Type { get; }

        public string Value { get; }

        public int StartIndex { get; }
        public int EndIndex { get; }

        public int StartLine { get; }
        public int StartColumn { get; }

        public SyntaxToken(SyntaxTokenType type, int startIndex, int endIndex, int startLine, int startColumn, string value = null)
        {
            Type = type;
            StartIndex = startIndex;
            EndIndex = endIndex;
            StartLine = startLine;
            StartColumn = startColumn;
            Value = value;
        }
    }
}