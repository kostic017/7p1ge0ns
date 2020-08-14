public class Token
{
    public string Value { get; set; }
    public TokenType Type { get; set; }
    public int StartIndex { get; set; }
    public int EndIndex { get; set; }
}
