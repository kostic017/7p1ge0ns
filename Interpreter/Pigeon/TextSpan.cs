namespace Kostic017.Pigeon
{
    public class TextSpan
    {
        public int Line { get; }
        public int Column { get; }
        public int StartIndex { get; }
        public int EndIndex { get; }
 
        internal TextSpan(int startLine, int startColumn, int startIndex, int endIndex)
        {
            Line = startLine;
            Column = startColumn;
            StartIndex = startIndex;
            EndIndex = endIndex;
        }
    }
}
