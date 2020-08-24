using System.Collections.Generic;

namespace Kostic017.Pigeon
{
    public class CodeError
    {
        public CodeErrorType Type { get; set; }
        public string Data { get; set; }
        public int Line { get; set; }
        public int Column { get; set; }

        public string Message()
        {
            return messages[Type].Replace("{d}", Data);
        }

        public string DetailedMessage()
        {
            return $"{Line}:{Column} " + Message();
        }

        private static readonly Dictionary<CodeErrorType, string> messages =
            new Dictionary<CodeErrorType, string>
            {
                { CodeErrorType.UNTERMINATED_COMMENT_BLOCK, "Unterminated comment block" },
                { CodeErrorType.INVALID_ESCAPE_CHAR, "Invalid escape char {d}" },
                { CodeErrorType.NEWLINE_IN_STRING, "Newline in string" },
                { CodeErrorType.UNTERMINATED_STRING, "Unterminated string" },
                { CodeErrorType.UNEXPECTED_CHARACTER, "Unexpected character {d}" },
                { CodeErrorType.ILLEGAL_NUMBER, "Illegal number {d}" },
            };
    }
}
