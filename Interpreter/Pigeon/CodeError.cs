using System.Collections.Generic;

namespace Kostic017.Pigeon
{
    public enum CodeErrorType
    {
        ILLEGAL_NUMBER,
        UNTERMINATED_STRING,
        INVALID_ESCAPE_CHAR,
        UNEXPECTED_CHARACTER,
        MISSING_EXPECTED_TOKEN,
        INVALID_EXPRESSION_TERM,
        UNTERMINATED_COMMENT_BLOCK,
        UNTERMINATED_STATEMENT_BLOCK,
        LEFTOVER_TOKENS_FOUND,
        INVALID_OPERAND_TYPE_UNARY,
        INVALID_OPERAND_TYPE_BINARY,
    }

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
                Message = Message.Replace("{" + i + "}", Data[i]);

            DetailedMessage = $"{Line}:{Column} " + Message;
        }

        private static readonly Dictionary<CodeErrorType, string> messages =
            new Dictionary<CodeErrorType, string>
            {
                { CodeErrorType.ILLEGAL_NUMBER, "Illegal number {0}" },
                { CodeErrorType.UNTERMINATED_STRING, "Unterminated string" },
                { CodeErrorType.INVALID_ESCAPE_CHAR, "Invalid escape char {0}" },
                { CodeErrorType.UNEXPECTED_CHARACTER, "Unexpected character {0}" },
                { CodeErrorType.MISSING_EXPECTED_TOKEN, "Expected token(s): {0}" },
                { CodeErrorType.INVALID_EXPRESSION_TERM, "Invalid expression term {0}" },
                { CodeErrorType.UNTERMINATED_COMMENT_BLOCK, "Unterminated comment block" },
                { CodeErrorType.UNTERMINATED_STATEMENT_BLOCK, "Unterminated statement block" },
                { CodeErrorType.LEFTOVER_TOKENS_FOUND, "Could not parse some tokens" },
                { CodeErrorType.INVALID_OPERAND_TYPE_UNARY, "Operator {0} is not defined for type {1}" },
                { CodeErrorType.INVALID_OPERAND_TYPE_BINARY, "Operator {0} is not defined for types {1} and {2}" },
            };
    }
}
