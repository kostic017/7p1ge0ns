using Kostic017.Pigeon.Symbols;
using System.Collections.Generic;
using System.Linq;

namespace Kostic017.Pigeon.Errors
{
    internal class CodeErrorBag
    {
        private readonly List<CodeError> errors;

        internal CodeError[] Errors => errors.ToArray();

        internal CodeErrorBag()
        {
            errors = new List<CodeError>();
        }

        private void Report(CodeErrorType errorType, TextSpan textSpan, params string[] data)
        {
            errors.Add(new CodeError(errorType, textSpan, data));
        }

        internal void ReportUnparseableNumber(TextSpan textSpan, string number)
        {
            Report(CodeErrorType.UNPARSEABLE_NUMBER, textSpan, number);
        }

        internal void ReportUnterminatedString(TextSpan textSpan)
        {
            Report(CodeErrorType.UNTERMINATED_STRING, textSpan);
        }

        internal void ReportInvalidEscapeChar(TextSpan textSpan, string character)
        {
            Report(CodeErrorType.INVALID_ESCAPE_CHAR, textSpan, character);
        }

        internal void ReportUnexpectedCharacter(TextSpan textSpan, string character)
        {
            Report(CodeErrorType.UNEXPECTED_CHARACTER, textSpan, character);
        }

        internal void ReportMissingExpectedToken(TextSpan textSpan, SyntaxTokenType[] tokenTypes)
        {
            Report(CodeErrorType.MISSING_EXPECTED_TOKEN, textSpan, string.Join(", ", tokenTypes.Select(t => $"'{t.GetDescription()}'")));
        }

        internal void ReportInvalidExpressionTerm(TextSpan textSpan, SyntaxTokenType tokenType)
        {
            Report(CodeErrorType.INVALID_EXPRESSION_TERM, textSpan, tokenType.GetDescription());
        }

        internal void ReportUnterminatedCommentBlock(TextSpan textSpan)
        {
            Report(CodeErrorType.UNTERMINATED_COMMENT_BLOCK, textSpan);
        }

        internal void ReportUnterminatedStatementBlock(TextSpan textSpan)
        {
            Report(CodeErrorType.UNTERMINATED_STATEMENT_BLOCK, textSpan);
        }

        internal void ReportLeftoverTokensFound(TextSpan textSpan)
        {
            Report(CodeErrorType.LEFTOVER_TOKENS_FOUND, textSpan);
        }

        internal void ReportUnexpectedToken(TextSpan textSpan, SyntaxTokenType tokenType)
        {
            Report(CodeErrorType.UNEXPECTED_TOKEN, textSpan, tokenType.GetDescription());
        }

        internal void ReportVariableNotDefined(TextSpan textSpan, string name)
        {
            Report(CodeErrorType.VARIABLE_NOT_DEFINED, textSpan, name);
        }

        internal void ReportVariableAlreadyDefined(TextSpan textSpan, string name)
        {
            Report(CodeErrorType.VARIABLE_ALREADY_DEFINED, textSpan, name);
        }

        internal void ReportFunctionNotDefined(TextSpan textSpan, string name)
        {
            Report(CodeErrorType.FUNCTION_NOT_DEFINED, textSpan, name);
        }

        internal void ReportInvalidTypeAssignment(TextSpan textSpan, string variableName, TypeSymbol variableType, TypeSymbol valueType)
        {
            Report(CodeErrorType.INVALID_TYPE_ASSIGNMENT, textSpan, variableName, variableType.ToString(), valueType.ToString());
        }

        internal void ReportInvalidTypeUnaryOperand(TextSpan textSpan, SyntaxTokenType op, TypeSymbol type)
        {
            Report(CodeErrorType.INVALID_TYPE_UNARY_OPERAND, textSpan, op.GetDescription(), type.ToString());
        }

        internal void ReportInvalidTypeBinaryOperand(TextSpan textSpan, SyntaxTokenType op, TypeSymbol type1, TypeSymbol type2)
        {
            Report(CodeErrorType.INVALID_TYPE_BINARY_OPERAND, textSpan, op.GetDescription(), type1.ToString(), type2.ToString());
        }

        internal void ReportModifyingReadOnlyVariable(TextSpan textSpan, string name)
        {
            Report(CodeErrorType.MODIFYING_READ_ONLY_VARIABLE, textSpan, name);
        }

        internal void ReportUnexpectedType(TextSpan textSpan, TypeSymbol expectedType, TypeSymbol actualType)
        {
            Report(CodeErrorType.UNEXPECTED_TYPE, textSpan, expectedType.ToString(), actualType.ToString());
        }

        internal void ReportInvalidNumberOfParameters(TextSpan textSpan, int numOfParm)
        {
            Report(CodeErrorType.INVALID_NUMBER_OF_PARAMETERS, textSpan, $"{numOfParm}");
        }

        internal void ReportInvalidParameterType(TextSpan textSpan, int argCnt, TypeSymbol expectedType)
        {
            Report(CodeErrorType.INVALID_PARAMETER_TYPE, textSpan, $"{argCnt}", expectedType.Name);
        }
    }
}
