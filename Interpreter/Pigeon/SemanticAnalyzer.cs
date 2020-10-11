using Kostic017.Pigeon.AST;
using Kostic017.Pigeon.Error;
using Kostic017.Pigeon.TAST;
using Kostic017.Pigeon.Variable;
using System;
using System.Linq;

namespace Kostic017.Pigeon
{
    public class SemanticAnalyzer
    {
        private VariableScope scope;
        private readonly CodeErrorBag errorBag;

        internal SemanticAnalyzer()
        {
            scope = new VariableScope(null);
            errorBag = new CodeErrorBag();
        }

        public static GlobalScope Anaylize(SyntaxTree syntaxTree)
        {
            var analyzer = new SemanticAnalyzer();
            var statementBlock = (TypedStatementBlock) analyzer.AnalyzeStatement(syntaxTree.Root.StatementBlock);
            var program = new TypedProgram(statementBlock);
            return new GlobalScope(program, analyzer.errorBag.Errors);
        }

        private TypedStatement AnalyzeStatement(Statement node)
        {
            switch (node.Kind)
            {
                case NodeKind.StatementBlock:
                    return AnalyzeStatementBlock((StatementBlock) node);
                case NodeKind.VariableDeclaration:
                    return AnalyzeVariableDeclaration((VariableDeclaration) node);
                case NodeKind.VariableAssignment:
                    return AnalyzeVariableAssignment((VariableAssignment) node);
            }
            throw new NotImplementedException();
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
                errorBag.Report(CodeErrorType.NAME_ALREADY_DEFINED, node.IdentifierToken.TextSpan, variable.Name);
          
            return new TypedVariableDeclaration(variable, value);
        }

        private TypedStatement AnalyzeVariableAssignment(VariableAssignment node)
        {
            if (!scope.TryLookup(node.IdentifierToken.Value, out var variable))
            {
                errorBag.Report(CodeErrorType.NAME_NOT_DEFINED, node.IdentifierToken.TextSpan, node.IdentifierToken.Value);
                return new TypedVariableAssignment(null, null, null);
            }
            
            if (variable.ReadOnly)
                errorBag.Report(CodeErrorType.MODIFYING_READ_ONLY_VARIABLE, node.IdentifierToken.TextSpan, node.IdentifierToken.Value);
            
            var value = AnalyzeExpression(node.Value);
            var typedOperator = TypedAssignmentOperator.Bind(node.Op.Type, variable.Type, value.Type);

            if (typedOperator == null)
                errorBag.Report(CodeErrorType.INVALID_TYPE_ASSIGNMENT, node.Op.TextSpan, variable.Name, variable.Type.ToString(), value.Type.ToString());
        
            return new TypedVariableAssignment(variable, typedOperator, value);
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
            if (scope.TryLookup(node.IdentifierToken.Value, out var variable))
                return new TypedVariableExpression(variable);
            errorBag.Report(CodeErrorType.NAME_NOT_DEFINED, node.IdentifierToken.TextSpan, node.IdentifierToken.Value);
            return new TypedLiteralExpression(0);
        }

        private TypedExpression AnalyzeLiteralExpression(LiteralExpression node)
        {
            return new TypedLiteralExpression(node.ParsedValue ?? 0);
        }

        private TypedExpression AnalyzeUnaryExpression(UnaryExpression node)
        {
            var value = AnalyzeExpression(node.Value);
            var typedOperator = TypedUnaryOperator.Bind(node.Op.Type, value.Type);
            
            if (typedOperator == null)
            {
                errorBag.Report(CodeErrorType.INVALID_TYPE_UNARY_OPERAND, node.Op.TextSpan, node.Op.Type.GetDescription(), value.Type.ToString());
                return value;
            }
            
            return new TypedUnaryExpression(typedOperator, value);
        }

        private TypedExpression AnalyzeBinaryExpression(BinaryExpression node)
        {
            var left = AnalyzeExpression(node.Left);
            var right = AnalyzeExpression(node.Right);
            var typedOperator = TypedBinaryOperator.Bind(node.Op.Type, left.Type, right.Type);

            if (typedOperator == null)
            {
                errorBag.Report(CodeErrorType.INVALID_TYPE_BINARY_OPERAND, node.Op.TextSpan, node.Op.Type.GetDescription(), left.Type.ToString(), right.Type.ToString());
                return left;
            }

            return new TypedBinaryExpression(left, typedOperator, right);
        }

        private TypedExpression AnalyzeParenthesizedExpression(ParenthesizedExpression node)
        {
            return AnalyzeExpression(node.Expression);
        }
    }
}
