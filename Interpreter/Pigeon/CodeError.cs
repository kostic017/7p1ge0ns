using System.Collections.Generic;

namespace Kostic017.Pigeon
{
    public class CodeError
    {
        public CodeErrorType Type { get; }
        public string[] Data { get; }

        public int Line { get; }
        public int Column { get; }
        
        public string Message { get; }
        public string DetailedMessage { get; }

        public CodeError(CodeErrorType type, int line, int column, params string[] data)
        {
            Type = type;
            Data = data;
            Line = line;
            Column = column;

            Message = messages[Type];
            
            for (int i = 0; i < data.Length; ++i)
            {
                Message = Message.Replace("{" + i + "}", Data[i]);
            }

            DetailedMessage = $"{Line}:{Column} " + Message;
        }

        private static readonly Dictionary<CodeErrorType, string> messages =
            new Dictionary<CodeErrorType, string>
            {
                { CodeErrorType.ILLEGAL_NUMBER, "Illegal number {0}" },
                { CodeErrorType.EXPECTED_TOKEN, "Expected {0} token" },
                { CodeErrorType.UNTERMINATED_STRING, "Unterminated string" },
                { CodeErrorType.INVALID_ESCAPE_CHAR, "Invalid escape char {0}" },
                { CodeErrorType.UNEXPECTED_CHARACTER, "Unexpected character {0}" },
                { CodeErrorType.INVALID_EXPRESSION_TERM, "Invalid expression term {0}" },
                { CodeErrorType.UNTERMINATED_COMMENT_BLOCK, "Unterminated comment block" },
                { CodeErrorType.UNTERMINATED_STATEMENT_BLOCK, "Unterminated statement block" },
            };
    }
}
