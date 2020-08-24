namespace Kostic017.Pigeon
{
    public class SyntaxToken
    {
        public object Value { get; set; }
        public SyntaxTokenType Type { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public int ErrorIndex { get; set; }
    }
}