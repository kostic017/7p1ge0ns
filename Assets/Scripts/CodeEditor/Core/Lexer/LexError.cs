using System.Collections.Generic;

public class LexError
{
    public LexErrorType Type { get; set; }
    public string Data { get; set; }

    public int Line { get; set; }
    public int Column { get; set; }

    public string Message()
    {
        return messages[Type].Replace("{d}", Data);
    }

    public string DetailedMessage()
    {
        return messages[Type].Replace("{d}", Data) + $" at {Line}:{Column}";
    }
    
    private static readonly Dictionary<LexErrorType, string> messages =
        new Dictionary<LexErrorType, string>
        {
            { LexErrorType.UNTERMINATED_COMMENT_BLOCK, "Unterminated comment block" },
            { LexErrorType.INVALID_ESCAPE_CHAR, "Invalid escape char {d}" },
            { LexErrorType.NEWLINE_IN_STRING, "Newline in string" },
            { LexErrorType.UNTERMINATED_STRING, "Unterminated string" },
            { LexErrorType.UNEXPECTED_CHARACTER, "Unexpected character {d}" },
        };
}
