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
        UNEXPECTED_TOKEN,
        NAME_NOT_DEFINED,
        NAME_ALREADY_DEFINED,
        INVALID_TYPE_ASSIGNMENT,
        INVALID_TYPE_UNARY_OPERAND,
        INVALID_TYPE_BINARY_OPERAND,
        MODIFYING_READ_ONLY_VARIABLE,
        UNEXPECTED_TYPE,
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
                { CodeErrorType.UNEXPECTED_TOKEN, "Token '{0}' was not expected here" },
                { CodeErrorType.NAME_NOT_DEFINED, "The name '{0}' does not exist in the current contex" },
                { CodeErrorType.NAME_ALREADY_DEFINED, "The name '{0}' is already defined in the current scope" },
                { CodeErrorType.INVALID_TYPE_UNARY_OPERAND, "Operator {0} is not defined for type {1}" },
                { CodeErrorType.INVALID_TYPE_BINARY_OPERAND, "Operator {0} is not defined for types {1} and {2}" },
                { CodeErrorType.INVALID_TYPE_ASSIGNMENT, "Variable '{0}' of type {1} cannot have value of type {2}" },
                { CodeErrorType.MODIFYING_READ_ONLY_VARIABLE, "Variable '{0}' is read-only" },
                { CodeErrorType.UNEXPECTED_TYPE, "Expected value of type {0}, not value of type {1}"}
            };
    }
}
