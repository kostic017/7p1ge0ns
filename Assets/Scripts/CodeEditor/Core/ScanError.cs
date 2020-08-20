using System.Collections.Generic;

public class ScanError
{
    public ScanErrorType Type { get; set; }
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
    
    private static readonly Dictionary<ScanErrorType, string> messages =
        new Dictionary<ScanErrorType, string>
        {
            { ScanErrorType.UNTERMINATED_COMMENT_BLOCK, "Unterminated comment block" },
            { ScanErrorType.INVALID_ESCAPE_CHAR, "Invalid escape char {d}" },
            { ScanErrorType.NEWLINE_IN_STRING, "Newline in string" },
            { ScanErrorType.UNTERMINATED_STRING, "Unterminated string" },
            { ScanErrorType.UNEXPECTED_CHARACTER, "Unexpected character {d}" },
        };
}
