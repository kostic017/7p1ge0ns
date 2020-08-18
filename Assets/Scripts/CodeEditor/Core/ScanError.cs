using System.Collections.Generic;
using System.Linq;

public class ScanError
{
    public ScanErrorType Type { get; set; }
    public string Details { get; set; }

    public int Line { get; set; }
    public int Column { get; set; }

    public int StartIndex { get; set; }
    public int EndIndex { get; set; }

    public string Message()
    {
        return messages[Type]
            .Replace("{d}", Details)
            .Replace("{l}", "" + Line)
            .Replace("{c}", "" + Column);
    }

    // leaving a tiny bit of space for translation support
    private static readonly Dictionary<ScanErrorType, string> messages =
        new Dictionary<ScanErrorType, string>
        {
            { ScanErrorType.UNTERMINATED_COMMENT_BLOCK, "Unterminated comment block at {l}:{c}" },
            { ScanErrorType.INVALID_ESCAPE_CHAR, "Invalid escape char {d} at {l}:{c}" },
            { ScanErrorType.NEWLINE_IN_STRING, "Newline in string at {l}:{c}" },
            { ScanErrorType.UNTERMINATED_STRING, "Unterminated string at {l}:{c}" },
            { ScanErrorType.UNEXPECTED_CHARACTER, "Unexpected character {d} at {l}:{c}" },
        };
}
