﻿using Kostic017.Pigeon.Symbols;
using System.Collections.Generic;

namespace Kostic017.Pigeon.Errors
{
    internal class CodeErrorBag
    {
        readonly List<CodeError> errors;

        internal CodeError[] Errors => errors.ToArray();

        internal CodeErrorBag()
        {
            errors = new List<CodeError>();
        }

        void Report(string message, TextSpan textSpan)
        {
            errors.Add(new CodeError(message, textSpan));
        }

        internal void ReportUndeclaredVariable(TextSpan textSpan, string name)
        {
            Report($"The variable '{name}' does not exist in the current contex", textSpan);
        }

        internal void ReportUndeclaredFunction(TextSpan textSpan, string name)
        {
            Report($"The function '{name}' does not exist in the current contex", textSpan);
        }

        internal void ReportVariableRedeclaration(TextSpan textSpan, string name)
        {
            Report($"The variable '{name}' is already defined in the current scope", textSpan);
        }

        internal void ReportInvalidTypeAssignment(TextSpan textSpan, string variableName, PigeonType variableType, PigeonType valueType)
        {
            Report($"Variable '{variableName}' of type {variableType.Name} cannot have value of type {valueType.Name}", textSpan);
        }

        internal void ReportInvalidTypeUnaryOperator(TextSpan textSpan, string op, PigeonType type)
        {
            Report($"Operator {op} is not defined for type {type.Name}", textSpan);
        }

        internal void ReportInvalidTypeBinaryOperator(TextSpan textSpan, string op, PigeonType type1, PigeonType type2)
        {
            Report($"Operator {op} is not defined for types {type1.Name} and {type2.Name}", textSpan);
        }

        internal void ReportInvalidTypeTernaryOperator(TextSpan textSpan, PigeonType type1, PigeonType type2)
        {
            Report($"Types {type1.Name} and {type2.Name} are not compatible", textSpan);
        }

        internal void ReportRedefiningReadOnlyVariable(TextSpan textSpan, string name)
        {
            Report($"Variable '{name}' is read-only", textSpan);
        }

        internal void ReportUnexpectedType(TextSpan textSpan, PigeonType actualType, PigeonType expectedType)
        {
            Report($"Got value of type {actualType.Name} instead of {expectedType.Name}", textSpan);
        }

        internal void ReportInvalidNumberOfArguments(TextSpan textSpan, string functionName, int numOfArgs)
        {
            Report($"Function {functionName} expects {numOfArgs}", textSpan);
        }

        internal void ReportInvalidArgumentType(TextSpan textSpan, int argCnt, PigeonType expectedType)
        {
            Report($"Argument {argCnt} should be of type {expectedType.Name}", textSpan);
        }

        internal void ReportStatementNotInLoop(TextSpan textSpan, string statement)
        {
            Report($"{statement} should be in a loop", textSpan);
        }
    }
}
