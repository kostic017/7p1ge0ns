using Kostic017.Pigeon.AST;
using Kostic017.Pigeon.Errors;
using Kostic017.Pigeon.TAST;
using Kostic017.Pigeon.Symbols;
using System;
using System.Linq;

namespace Kostic017.Pigeon
{
    public class TypeChecker
    {
        private VariableScope scope;
        private readonly CodeErrorBag errorBag;

        internal TypeChecker()
        {
            scope = new VariableScope(null);
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
            }
            throw new NotImplementedException();
        }

        private TypedStatement AnalyzeIfStatement(IfStatement node)
        {
            var condition = AnalyzeExpression(node.Condition, TypeSymbol.Bool);
            var thenBlock = AnalyzeStatementBlock(node.ThenBlock);
            var elseBlock = node.ElseBlock != null ? AnalyzeStatementBlock(node.ElseBlock) : null ;
            return new TypedIfStatement(condition, thenBlock, elseBlock);
        }

        private TypedStatement AnalyzeForStatement(ForStatement node)
        {
            if (!scope.TryLookup(node.IdentifierToken.Value, out var variable))
                errorBag.Report(CodeErrorType.NAME_NOT_DEFINED, node.IdentifierToken.TextSpan, node.IdentifierToken.Value);
            if (variable.Type != TypeSymbol.Int)
                errorBag.Report(CodeErrorType.UNEXPECTED_TYPE, node.IdentifierToken.TextSpan, TypeSymbol.Int.ToString(), variable.Type.ToString());
                
            var startValue = AnalyzeExpression(node.StartValue, TypeSymbol.Int);
            var targetValue = AnalyzeExpression(node.TargetValue, TypeSymbol.Int);
            var stepValue = node.StepValue != null ? AnalyzeExpression(node.StepValue, TypeSymbol.Int) : null;
            var direction = node.DirectionToken.Type == SyntaxTokenType.To ? LoopDirection.To : LoopDirection.Downto;
            var body = AnalyzeStatementBlock(node.Body);

            return new TypedForStatement(variable, startValue, targetValue, stepValue, direction, body);
        }

        private TypedStatement AnalyzeWhileStatement(WhileStatement node)
        {
            var condition = AnalyzeExpression(node.Condition, TypeSymbol.Bool);
            var body = AnalyzeStatementBlock(node.Body);
            return new TypedWhileStatement(condition, body);
        }

        private TypedStatement AnalyzeDoWhileStatement(DoWhileStatement node)
        {
            var body = AnalyzeStatementBlock(node.Body);
            var condition = AnalyzeExpression(node.Condition, TypeSymbol.Bool);
            return new TypedDoWhileStatement(body, condition);
        }

        private TypedStatement AnalyzeStatementBlock(StatementBlock node)
        {
            scope = new VariableScope(scope);
            var statements = node.Statements.Select(statement => AnalyzeStatement(statement)).ToArray();
            scope = scope.Parent;
            return new TypedStatementBlock(statements);
        }

        private TypedStatement AnalyzeVariableDeclaration(VariableDeclaration node)
        {
            var value = AnalyzeExpression(node.Value);
            var variable = new VariableSymbol(node.IdentifierToken.Value, value.Type, node.Keyword.Type == SyntaxTokenType.Const);
            
            if (!scope.TryDeclare(variable))
            {
                errorBag.Report(CodeErrorType.NAME_ALREADY_DEFINED, node.IdentifierToken.TextSpan, variable.Name);
                throw new NotImplementedException();
            }
          
            return new TypedVariableDeclaration(variable, value);
        }

        private TypedStatement AnalyzeVariableAssignment(VariableAssignment node)
        {
            if (!scope.TryLookup(node.IdentifierToken.Value, out var variable))
            {
                errorBag.Report(CodeErrorType.NAME_NOT_DEFINED, node.IdentifierToken.TextSpan, node.IdentifierToken.Value);
                throw new NotImplementedException();
            }
            
            if (variable.ReadOnly)
            {
                errorBag.Report(CodeErrorType.MODIFYING_READ_ONLY_VARIABLE, node.IdentifierToken.TextSpan, node.IdentifierToken.Value);
                throw new NotImplementedException();
            }
            
            var value = AnalyzeExpression(node.Value);

            if (!TypedAssignmentOperator.TryBind(node.Op.Type, variable.Type, value.Type, out var typedOperator))
            {
                errorBag.Report(CodeErrorType.INVALID_TYPE_ASSIGNMENT, node.Op.TextSpan, variable.Name, variable.Type.ToString(), value.Type.ToString());
                throw new NotImplementedException();
            }
       
            return new TypedVariableAssignment(variable, typedOperator, value);
        }

        private TypedExpression AnalyzeExpression(Expression node, TypeSymbol expectedType)
        {
            var expression = AnalyzeExpression(node);
            if (expression.Type != expectedType)
            {
                errorBag.Report(CodeErrorType.UNEXPECTED_TYPE, node.TextSpan, expectedType.ToString(), expression.Type.ToString());
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
            }
            throw new NotImplementedException();
        }

        private TypedExpression AnalyzeVariableExpression(VariableExpression node)
        {
            if (!scope.TryLookup(node.IdentifierToken.Value, out var variable))
            {
                errorBag.Report(CodeErrorType.NAME_NOT_DEFINED, node.IdentifierToken.TextSpan, node.IdentifierToken.Value);
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
                errorBag.Report(CodeErrorType.INVALID_TYPE_UNARY_OPERAND, node.Op.TextSpan, node.Op.Type.GetDescription(), operand.Type.ToString());
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
                errorBag.Report(CodeErrorType.INVALID_TYPE_BINARY_OPERAND, node.Op.TextSpan, node.Op.Type.GetDescription(), left.Type.ToString(), right.Type.ToString());
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
