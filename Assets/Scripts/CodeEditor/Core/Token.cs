namespace CodeEditor.Core
{
    class Token
    {
        public string Value { get; set; }
        public TokenType Type { get; set; }
        
        public int StartLine { get; set; }
        public int StartColumn { get; set; }

        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
    }
}
