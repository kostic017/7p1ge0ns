using System.Collections.Generic;

namespace Kostic017.Pigeon
{
    public class CodeError
    {
        public CodeErrorType Type { get; }
        public string Data { get; }

        public int Line { get; }
        public int Column { get; }
        
        public string Message { get; }
        public string DetailedMessage { get; }

        public CodeError(CodeErrorType type, int line, int column) : this(type, line, column, null)
        {
        }

        public CodeError(CodeErrorType type, int line, int column, string data)
        {
            Type = type;
            Data = data;
            Line = line;
            Column = column;

            Message = messages[Type];
            
            if (!string.IsNullOrWhiteSpace(data))
            {
                Message = Message.Replace("{d}", Data);
            }

            DetailedMessage = $"{Line}:{Column} " + Message;
        }

        private static readonly Dictionary<CodeErrorType, string> messages =
            new Dictionary<CodeErrorType, string>
            {
                { CodeErrorType.UNTERMINATED_COMMENT_BLOCK, "Unterminated comment block" },
                { CodeErrorType.INVALID_ESCAPE_CHAR, "Invalid escape char {d}" },
                { CodeErrorType.NEWLINE_IN_STRING, "Newline in string" },
                { CodeErrorType.UNTERMINATED_STRING, "Unterminated string" },
                { CodeErrorType.ILLEGAL_CHARACTER, "Illegal character {d}" },
                { CodeErrorType.ILLEGAL_NUMBER, "Illegal number {d}" },
            };
    }
}
