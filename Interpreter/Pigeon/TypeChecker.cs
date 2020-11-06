using Kostic017.Pigeon.AST;
using Kostic017.Pigeon.Errors;
using Kostic017.Pigeon.TAST;
using Kostic017.Pigeon.Symbols;
using System.Linq;
using System;
using System.Collections.Generic;

namespace Kostic017.Pigeon
{
    public class TypeChecker
    {
        private TypedScope scope;
        private readonly CodeErrorBag errorBag;

        internal TypeChecker()
        {
            scope = new TypedScope(null);
            errorBag = new CodeErrorBag();
        }

        public static AnalysisResult Anaylize(SyntaxTree syntaxTree)
        {
            var analyzer = new TypeChecker();
            var program = analyzer.AnalyzeProgram(syntaxTree.Root);
            return new AnalysisResult(program, analyzer.errorBag.Errors);
        }

        private TypedProgram AnalyzeProgram(Program program)
        {
            var statementBlock = (TypedStatementBlock) AnalyzeStatement(program.StatementBlock);
            return new TypedProgram(statementBlock);
        }

        private TypedStatement AnalyzeStatement(Statement node)
        {
            switch (node.Kind)
            {
                case NodeKind.StatementBlock:
                    return AnalyzeStatementBlock((StatementBlock) node);
                case NodeKind.IfStatement:
                    return AnalyzeIfStatement((IfStatement) node);
                case NodeKind.ForStatement:
                    return AnalyzeForStatement((ForStatement) node);
                case NodeKind.WhileStatement:
                    return AnalyzeWhileStatement((WhileStatement) node);
                case NodeKind.DoWhileStatement:
                    return AnalyzeDoWhileStatement((DoWhileStatement) node);
                case NodeKind.VariableDeclaration:
                    return AnalyzeVariableDeclaration((VariableDeclaration) node);
                case NodeKind.VariableAssignment:
                    return AnalyzeVariableAssignment((VariableAssignment) node);
                case NodeKind.ExpressionStatement:
                    return AnalyzeExpressionStatement((ExpressionStatement) node);
                default:
                    throw new InternalErrorException($"Unsupported statement '{node.Kind}'");
            }
        }

        private TypedStatement AnalyzeExpressionStatement(ExpressionStatement node)
        {
            var expression = AnalyzeExpression(node.Expression);
            return new TypedExpressionStatement(expression);
        }

        private TypedStatement AnalyzeStatementBlock(StatementBlock node, TypedScope predefinedScope = null)
        {
            scope = predefinedScope ?? new TypedScope(scope);
            var statements = node.Statements.Select(statement => AnalyzeStatement(statement)).ToArray();
            scope = scope.Parent;
            return new TypedStatementBlock(statements);
        }

        private TypedStatement AnalyzeIfStatement(IfStatement node)
        {
            var condition = AnalyzeExpression(node.Condition, TypeSymbol.Bool);
            var thenBlock = (TypedStatementBlock) AnalyzeStatementBlock(node.ThenBlock);
            var elseBlock = (TypedStatementBlock) (node.ElseBlock != null ? AnalyzeStatementBlock(node.ElseBlock) : null);
            return new TypedIfStatement(condition, thenBlock, elseBlock);
        }

        private TypedStatement AnalyzeForStatement(ForStatement node)
        {            
            var variableScope = new TypedScope(scope);
            var counterVariable = new VariableSymbol(node.IdentifierToken.Value, TypeSymbol.Int, false);
            variableScope.TryDeclare(counterVariable);

            var startValue = AnalyzeExpression(node.StartValue, TypeSymbol.Int);
            var targetValue = AnalyzeExpression(node.TargetValue, TypeSymbol.Int);
            var stepValue = node.StepValue != null ? AnalyzeExpression(node.StepValue, TypeSymbol.Int) : null;
            var direction = node.DirectionToken.Type == SyntaxTokenType.To ? LoopDirection.To : LoopDirection.Downto;
            var body = (TypedStatementBlock) AnalyzeStatementBlock(node.Body, variableScope);

            return new TypedForStatement(counterVariable, startValue, targetValue, stepValue, direction, body);
        }

        private TypedStatement AnalyzeWhileStatement(WhileStatement node)
        {
            var condition = AnalyzeExpression(node.Condition, TypeSymbol.Bool);
            var body = (TypedStatementBlock) AnalyzeStatementBlock(node.Body);
            return new TypedWhileStatement(condition, body);
        }

        private TypedStatement AnalyzeDoWhileStatement(DoWhileStatement node)
        {
            var body = (TypedStatementBlock) AnalyzeStatementBlock(node.Body);
            var condition = AnalyzeExpression(node.Condition, TypeSymbol.Bool);
            return new TypedDoWhileStatement(body, condition);
        }

        private TypedStatement AnalyzeVariableDeclaration(VariableDeclaration node)
        {
            var value = AnalyzeExpression(node.Value);
            var variable = new VariableSymbol(node.IdentifierToken.Value, value.Type, node.Keyword.Type == SyntaxTokenType.Const);
            
            if (!scope.TryDeclare(variable))
            {
                errorBag.ReportVariableAlreadyDefined(node.IdentifierToken.TextSpan, variable.Name);
                return new TypedErrorStatement();
            }
          
            return new TypedVariableDeclaration(variable, value);
        }

        private TypedStatement AnalyzeVariableAssignment(VariableAssignment node)
        {
            if (!scope.TryLookup(node.IdentifierToken.Value, out var variable))
            {
                errorBag.ReportVariableNotDefined(node.IdentifierToken.TextSpan, node.IdentifierToken.Value);
                return new TypedErrorStatement();
            }
            
            if (variable.ReadOnly)
            {
                errorBag.ReportModifyingReadOnlyVariable(node.IdentifierToken.TextSpan, node.IdentifierToken.Value);
                return new TypedErrorStatement();
            }
            
            var value = AnalyzeExpression(node.Value);

            if (!TypedAssignmentOperator.TryBind(node.Op.Type, variable.Type, value.Type, out var typedOperator))
            {
                errorBag.ReportInvalidTypeAssignment(node.Op.TextSpan, variable.Name, variable.Type, value.Type);
                return new TypedErrorStatement();
            }
       
            return new TypedVariableAssignment(variable, typedOperator, value);
        }

        private TypedExpression AnalyzeExpression(Expression node, TypeSymbol expectedType)
        {
            var expression = AnalyzeExpression(node);
            if (expression.Type != expectedType)
            {
                errorBag.ReportUnexpectedType(node.TextSpan, expectedType, expression.Type);
                return new TypedErrorExpression();
            }
            return expression;
        }

        private TypedExpression AnalyzeExpression(Expression node)
        {
            switch (node.Kind)
            {
                case NodeKind.VariableExpression:
                    return AnalyzeVariableExpression((VariableExpression) node);
                case NodeKind.LiteralExpression:
                    return AnalyzeLiteralExpression((LiteralExpression) node);
                case NodeKind.UnaryExpression:
                    return AnalyzeUnaryExpression((UnaryExpression) node);
                case NodeKind.BinaryExpression:
                    return AnalyzeBinaryExpression((BinaryExpression) node);
                case NodeKind.ParenthesizedExpression:
                    return AnalyzeParenthesizedExpression((ParenthesizedExpression) node);
                case NodeKind.FunctionCallExpression:
                    return AnaylizeFunctionCallExpression((FunctionCallExpression) node);
                case NodeKind.ErrorExpression:
                    return new TypedErrorExpression();
                default:
                    throw new InternalErrorException($"Unsupported expression '{node.Kind}'");
            }
        }

        private TypedExpression AnaylizeFunctionCallExpression(FunctionCallExpression node)
        {
            if (!BuiltinFunctions.TryLookup(node.NameToken.Value, out var function))
            {
                errorBag.ReportFunctionNotDefined(node.NameToken.TextSpan, node.NameToken.Value);
                return new TypedErrorExpression();
            }
            if (function.Symbol.Parameters.Length != node.Arguments.Length)
            {
                errorBag.ReportInvalidNumberOfParameters(node.TextSpan, function.Symbol.Parameters.Length);
                return new TypedErrorExpression();
            }
            var arguments = new List<TypedExpression>();
            for (var i = 0; i < node.Arguments.Length; ++i)
            {
                var expectedType = function.Symbol.Parameters[i].Type;
                var argument = AnalyzeExpression(node.Arguments[i].Value);
                if (argument.Type != expectedType)
                {
                    errorBag.ReportInvalidParameterType(node.TextSpan, i + 1, expectedType);
                    return new TypedErrorExpression();
                }
                arguments.Add(argument);
            }
            return new TypedFunctionCallExpression(function.Symbol, arguments.ToArray());
        }

        private TypedExpression AnalyzeVariableExpression(VariableExpression node)
        {
            if (!scope.TryLookup(node.IdentifierToken.Value, out var variable))
            {
                errorBag.ReportVariableNotDefined(node.IdentifierToken.TextSpan, node.IdentifierToken.Value);
                return new TypedErrorExpression();
            }
            return new TypedVariableExpression(variable);
        }

        private TypedExpression AnalyzeLiteralExpression(LiteralExpression node)
        {
            return new TypedLiteralExpression(node.ParsedValue);
        }

        private TypedExpression AnalyzeUnaryExpression(UnaryExpression node)
        {
            var operand = AnalyzeExpression(node.Value);
            
            if (operand.Type == TypeSymbol.Error)
                return new TypedErrorExpression();

            if (!TypedUnaryOperator.TryBind(node.Op.Type, operand.Type, out var typedOperator))
            {
                errorBag.ReportInvalidTypeUnaryOperand(node.Op.TextSpan, node.Op.Type, operand.Type);
                return new TypedErrorExpression();
            }
            
            return new TypedUnaryExpression(typedOperator, operand);
        }

        private TypedExpression AnalyzeBinaryExpression(BinaryExpression node)
        {
            var left = AnalyzeExpression(node.Left);
            var right = AnalyzeExpression(node.Right);
            
            if (left.Type == TypeSymbol.Error || right.Type == TypeSymbol.Error)
                return new TypedErrorExpression();
            
            if (!TypedBinaryOperator.TryBind(node.Op.Type, left.Type, right.Type, out var typedOperator))
            {
                errorBag.ReportInvalidTypeBinaryOperand(node.Op.TextSpan, node.Op.Type, left.Type, right.Type);
                return new TypedErrorExpression();
            }

            return new TypedBinaryExpression(left, typedOperator, right);
        }

        private TypedExpression AnalyzeParenthesizedExpression(ParenthesizedExpression node)
        {
            return AnalyzeExpression(node.Expression);
        }
    }
}
