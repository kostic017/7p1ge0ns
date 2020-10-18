using Kostic017.Pigeon.Errors;
using Kostic017.Pigeon.Symbols;
using Kostic017.Pigeon.TAST;
using System;
using System.Collections.Generic;

namespace Kostic017.Pigeon
{
    class Evaluator
    {
        private readonly Stack<Scope> scopes;

        public Evaluator()
        {
            scopes = new Stack<Scope>();
        }

        public static void Evaluate(TypedProgram program)
        {
            var evaluator = new Evaluator();
            evaluator.EvaluateStatement(program.StatementBlock);
        }

        private void AssignValue(VariableSymbol variable, object value)
        {
            scopes.Peek().Assign(variable, value);
        }

        private object GetVariableValue(VariableSymbol variable)
        {
            return scopes.Peek().Evaluate(variable);
        }

        private void EvaluateStatement(TypedStatement node)
        {
            switch (node.Kind)
            {
                case NodeKind.StatementBlock:
                    EvaluateStatementBlock((TypedStatementBlock) node);
                    break;
                case NodeKind.IfStatement:
                    EvaluateIfStatement((TypedIfStatement) node);
                    break;
                case NodeKind.ForStatement:
                    EvaluateForStatement((TypedForStatement) node);
                    break;
                case NodeKind.WhileStatement:
                    EvaluateWhileStatement((TypedWhileStatement) node);
                    break;
                case NodeKind.DoWhileStatement:
                    EvaluateDoWhileStatement((TypedDoWhileStatement) node);
                    break;
                case NodeKind.VariableDeclaration:
                    EvaluateVariableDeclaration((TypedVariableDeclaration) node);
                    break;
                case NodeKind.VariableAssignment:
                    EvaluateVariableAssignment((TypedVariableAssignment) node);
                    break;
                default:
                    throw new InternalErrorException($"Unsupported node '{node.Kind}'");
            }
        }

        private void EvaluateStatementBlock(TypedStatementBlock node, Scope predefinedScope = null)
        {
            scopes.Push(predefinedScope ?? new Scope());
            foreach (var statement in node.Statements)
                EvaluateStatement(statement);
            scopes.Pop();
        }

        private void EvaluateIfStatement(TypedIfStatement node)
        {
            if ((bool) EvaluateExpression(node.Condition))
                EvaluateStatementBlock(node.ThenBlock);
            else if (node.ElseBlock != null)
                EvaluateStatementBlock(node.ElseBlock);
        }

        private void EvaluateForStatement(TypedForStatement node)
        {
            var startValue = (int) EvaluateExpression(node.StartValue);
            var targetValue = (int) EvaluateExpression(node.TargetValue);
            var isIncrementing = node.Direction == LoopDirection.To;

            var i = startValue;
            while (isIncrementing ? i <= targetValue : i >= targetValue)
            {
                var scope = new Scope();
                scope.Declare(node.CounterVariable, i);
                EvaluateStatementBlock(node.Body, scope);
                i += isIncrementing ? 1 : -1;
            }
        }

        private void EvaluateWhileStatement(TypedWhileStatement node)
        {
            while ((bool) EvaluateExpression(node.Condition))
                EvaluateStatementBlock(node.Body);
        }

        private void EvaluateDoWhileStatement(TypedDoWhileStatement node)
        {
            do
                EvaluateStatementBlock(node.Body);
            while ((bool) EvaluateExpression(node.Condition));
        }

        private void EvaluateVariableDeclaration(TypedVariableDeclaration node)
        {
            var value = EvaluateExpression(node.Value);
            scopes.Peek().Declare(node.Variable, value);
        }

        private void EvaluateVariableAssignment(TypedVariableAssignment node)
        {
            var newValue = EvaluateExpression(node.Value);
            var currentValue = GetVariableValue(node.Variable);
            switch (node.Op.Kind)
            {
                case AssigmentOperator.Eq:
                    AssignValue(node.Variable, newValue);
                    break;

                case AssigmentOperator.PlusEq:
                    if (node.Op.VariableType == TypeSymbol.Int)
                        AssignValue(node.Variable, (int) currentValue + (int) newValue);
                    else if (node.Op.VariableType == TypeSymbol.Float)
                        AssignValue(node.Variable, (float) currentValue + (float) newValue);
                    else
                        AssignValue(node.Variable, (string) currentValue + (string) newValue);
                    break;

                case AssigmentOperator.MinusEq:
                    if (node.Op.VariableType == TypeSymbol.Int)
                        AssignValue(node.Variable, (int) currentValue - (int) newValue);
                    else if (node.Op.VariableType == TypeSymbol.Float)
                        AssignValue(node.Variable, (float) currentValue - (float) newValue);
                    break;

                case AssigmentOperator.MulEq:
                    if (node.Op.VariableType == TypeSymbol.Int)
                        AssignValue(node.Variable, (int) currentValue * (int) newValue);
                    else if (node.Op.VariableType == TypeSymbol.Float)
                        AssignValue(node.Variable, (float) currentValue * (float) newValue);
                    break;

                case AssigmentOperator.DivEq:
                    if (node.Op.VariableType == TypeSymbol.Int)
                        AssignValue(node.Variable, (int) currentValue / (int) newValue);
                    else if (node.Op.VariableType == TypeSymbol.Float)
                        AssignValue(node.Variable, (float) currentValue / (float) newValue);
                    break;

                case AssigmentOperator.ModEq:
                    AssignValue(node.Variable, (int) currentValue / (int) newValue);
                    break;
            }
        }
    
        private object EvaluateExpression(TypedExpression node)
        {
            switch (node.Kind)
            {
                case NodeKind.LiteralExpression:
                    return ((TypedLiteralExpression) node).Value;
                case NodeKind.UnaryExpression:
                    return EvaluateUnaryExpression((TypedUnaryExpression) node);
                case NodeKind.BinaryExpression:
                    return EvaluateBinaryExpression((TypedBinaryExpression) node);
                case NodeKind.VariableExpression:
                    return EvaluateVariableExpression((TypedVariableExpression) node);
                default:
                    throw new InternalErrorException($"Unsupported expression kind {node.Kind.GetDescription()}");
            }
        }

        private object EvaluateUnaryExpression(TypedUnaryExpression node)
        {
            switch (node.Op.Kind)
            {
                case UnaryOperator.Plus:
                    return node.Type == TypeSymbol.Int
                        ? (int) EvaluateExpression(node.Operand)
                        : (float) (int) EvaluateExpression(node.Operand);
                case UnaryOperator.Minus:
                    return node.Type == TypeSymbol.Int
                        ? - (int) EvaluateExpression(node.Operand)
                        : - (float) EvaluateExpression(node.Operand);
                case UnaryOperator.Negation:
                    return ! (bool) EvaluateExpression(node.Operand);
                default:
                    throw new InternalErrorException($"Unsupported unary operator {node.Op.Kind.GetDescription()}");
            }
        }

        private object EvaluateVariableExpression(TypedVariableExpression node)
        {
            return GetVariableValue(node.Variable);
        }

        private object EvaluateBinaryExpression(TypedBinaryExpression node)
        {
            switch (node.Op.Kind)
            {
                case BinaryOperator.Equal:
                    return EvaluateExpression(node.Left) == EvaluateExpression(node.Right);

                case BinaryOperator.NotEqual:
                    return EvaluateExpression(node.Left) != EvaluateExpression(node.Right);

                case BinaryOperator.And:
                    return (bool) EvaluateExpression(node.Left) && (bool) EvaluateExpression(node.Right);

                case BinaryOperator.Or:
                    return (bool) EvaluateExpression(node.Left) || (bool) EvaluateExpression(node.Right);
                
                case BinaryOperator.LessThan:
                {
                    var left = EvaluateExpression(node.Left);
                    var right = EvaluateExpression(node.Right);
                    return node.Op.ResultType == TypeSymbol.Int
                        ? (int) left < (int) right
                        : (float) left < (float) right;
                }

                case BinaryOperator.GreaterTan:
                {
                    var left = EvaluateExpression(node.Left);
                    var right = EvaluateExpression(node.Right);
                    return node.Op.ResultType == TypeSymbol.Int
                        ? (int) left > (int) right
                        : (float) left > (float) right;
                }
                
                case BinaryOperator.LessOrEqual:
                {
                    var left = EvaluateExpression(node.Left);
                    var right = EvaluateExpression(node.Right);
                    return node.Op.ResultType == TypeSymbol.Int
                        ? (int)left <= (int)right
                        : (float)left <= (float)right;
                }

                case BinaryOperator.GreaterOrEqual:
                {
                    var left = EvaluateExpression(node.Left);
                    var right = EvaluateExpression(node.Right);
                    return node.Op.ResultType == TypeSymbol.Int
                        ? (int)left >= (int)right
                        : (float)left >= (float)right;
                }

                case BinaryOperator.Plus:
                {
                    var left = EvaluateExpression(node.Left);
                    var right = EvaluateExpression(node.Right);
                    if (node.Op.ResultType == TypeSymbol.Int)
                        return (int) left + (int) right;
                    else if (node.Op.ResultType == TypeSymbol.Float)
                        return (float) left + (float) right;
                    else
                        return left.ToString() + right.ToString();
                }

                case BinaryOperator.Minus:
                {
                    var left = EvaluateExpression(node.Left);
                    var right = EvaluateExpression(node.Right);
                    return node.Op.ResultType == TypeSymbol.Int
                        ? (int)left - (int)right
                        : (float)left - (float)right;
                }

                case BinaryOperator.Mul:
                {
                    var left = EvaluateExpression(node.Left);
                    var right = EvaluateExpression(node.Right);
                    return node.Op.ResultType == TypeSymbol.Int
                        ? (int)left * (int)right
                        : (float)left * (float)right;
                }

                case BinaryOperator.Div:
                {
                    var left = EvaluateExpression(node.Left);
                    var right = EvaluateExpression(node.Right);
                    return node.Op.ResultType == TypeSymbol.Int
                        ? (int)left / (int)right
                        : (float)left / (float)right;
                }

                case BinaryOperator.Mod:
                    return (int) EvaluateExpression(node.Left) % (int) EvaluateExpression(node.Right);

                default:
                    throw new InternalErrorException($"Unsupported binary operator {node.Op.Kind.GetDescription()}");
            }

        }
    }
}
