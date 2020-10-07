using System.Collections.Generic;

namespace Kostic017.Pigeon.Error
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
        UNARY_OPERAND_INVALID_TYPE,
        BINARY_OPERAND_INVALID_TYPE,
        UNEXPECTED_TOKEN,
    }

    public class CodeError
    {
        public CodeErrorType Type { get; }
        public TextSpan TextSpan { get; }
        public string[] Data { get; }

        public CodeError(CodeErrorType type, TextSpan textSpan, params string[] data)
        {
            Type = type;
            Data = data;
            TextSpan = textSpan;
        }

        public string Message()
        {
            var message = messages[Type];
            for (int i = 0; i < Data.Length; ++i)
                message = message.Replace("{" + i + "}", Data[i]);
            return message;
        }

        public override string ToString() => $"{TextSpan.Line}:{TextSpan.Column} {Message()}";

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
                { CodeErrorType.LEFTOVER_TOKENS_FOUND, "Some tokens left unparsed" },
                { CodeErrorType.UNEXPECTED_TOKEN, "Token {0} was not expected here" },
                { CodeErrorType.UNARY_OPERAND_INVALID_TYPE, "Operator {0} is not defined for type {1}" },
                { CodeErrorType.BINARY_OPERAND_INVALID_TYPE, "Operator {0} is not defined for types {1} and {2}" },
            };
    }
}
