namespace Kostic017.Pigeon
{
    public class TextSpan
    {
        internal int Line { get; }
        internal int Column { get; }
        internal int StartIndex { get; }
        internal int EndIndex { get; }
 
        public TextSpan(int startLine, int startColumn, int startIndex, int endIndex)
        {
            Line = startLine;
            Column = startColumn;
            StartIndex = startIndex;
            EndIndex = endIndex;
        }
    }
}
