using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Kostic017.Pigeon.Symbols;
using System;
using System.Globalization;

namespace Kostic017.Pigeon
{
    class BreakLoopException : Exception { }

    class Evaluator : PigeonBaseVisitor<object>
    {
        private readonly ParseTreeProperty<PigeonType> types;

        public Evaluator(ParseTreeProperty<PigeonType> types)
        {
            this.types = types;
        }

        public override object VisitProgram([NotNull] PigeonParser.ProgramContext context)
        {
            return base.VisitProgram(context);
        }

        public override object VisitParenthesizedExpression([NotNull] PigeonParser.ParenthesizedExpressionContext context)
        {
            return VisitExpr(context.expr());
        }

        public override object VisitBoolLiteral([NotNull] PigeonParser.BoolLiteralContext context)
        {
            return bool.Parse(context.BOOL().GetText());
        }

        public override object VisitStringLiteral([NotNull] PigeonParser.StringLiteralContext context)
        {
            return context.STRING().GetText();
        }

        public override object VisitNumberLiteral([NotNull] PigeonParser.NumberLiteralContext context)
        {
            return types.Get(context) == PigeonType.Int
                ? int.Parse(context.NUMBER().GetText())
                : float.Parse(context.NUMBER().GetText(), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
        }

        public override object VisitUnaryExpression([NotNull] PigeonParser.UnaryExpressionContext context)
        {
            var operand = VisitExpr(context.expr());
            var resType = types.Get(context);;
            switch (context.op.Text)
            {
                case "+":
                    return resType == PigeonType.Int ? (int) operand : (float) operand;
                case "-":
                    return resType == PigeonType.Int ? - (int) operand : - (float) operand;
                case "!":
                    return ! (bool) operand;
                default:
                    throw new InternalErrorException($"Unsupported unary operator {context.op.Text}");
            }
        }

        public override object VisitBinaryExpression([NotNull] PigeonParser.BinaryExpressionContext context)
        {
            var left = VisitExpr(context.expr(0));
            var right = VisitExpr(context.expr(1));
            var resType = types.Get(context);

            switch (context.op.Text)
            {
                case "==":
                    return left == right;

                case "!=":
                    return left != right;

                case "&&":
                    return (bool) left && (bool) right;

                case "||":
                    return (bool) left || (bool) right;

                case "<":
                    return resType == PigeonType.Int ? (int) left < (int) right : (float) left < (float) right;

                case ">":
                    return resType == PigeonType.Int ? (int) left > (int) right : (float) left > (float) right;

                case "<=":
                    return resType == PigeonType.Int ? (int) left <= (int) right : (float) left <= (float) right;

                case ">=":
                    return resType == PigeonType.Int ? (int) left >= (int) right : (float) left >= (float) right;

                case "+":
                    if (resType == PigeonType.Int)
                        return (int) left + (int) right;
                    else if (resType == PigeonType.Float)
                        return (float) left + (float) right;
                    else
                        return left.ToString() + right.ToString();

                case "-":
                    return resType == PigeonType.Int ? (int) left - (int) right : (float) left - (float) right;

                case "*":
                    return resType == PigeonType.Int ? (int) left * (int) right : (float) left * (float) right;

                case "/":
                    return resType == PigeonType.Int ? (int) left / (int) right : (float) left / (float) right;

                case "%":
                    return (int) left % (int) right;

                default:
                    throw new InternalErrorException($"Unsupported binary operator {context.op.Text}");
            }
        }

        public override object VisitIfStatement([NotNull] PigeonParser.IfStatementContext context)
        {
            if ((bool) VisitExpr(context.expr()))
                VisitStmtBlock(context.stmtBlock(0));
            else if (context.stmtBlock(1) != null)
                VisitStmtBlock(context.stmtBlock(1));
            return null;
        }

        public override object VisitDoWhileStatement([NotNull] PigeonParser.DoWhileStatementContext context)
        {
            do
                try
                {
                    VisitStmtBlock(context.stmtBlock());
                }
                catch (BreakLoopException)
                {
                    return null;
                }
            while ((bool) VisitExpr(context.expr()));
            return null;
        }

        public override object VisitWhileStatement([NotNull] PigeonParser.WhileStatementContext context)
        {
            while ((bool) VisitExpr(context.expr()))
                try
                {
                    VisitStmtBlock(context.stmtBlock());
                }
                catch (BreakLoopException)
                {
                    return null;
                }
            return null;
        }

        public override object VisitForStatement([NotNull] PigeonParser.ForStatementContext context)
        {
            var startValue = (int) VisitExpr(context.expr(0));
            var targetValue = (int) VisitExpr(context.expr(1));
            var isIncrementing = context.dir.Text == "to";

            var i = startValue;
            while (isIncrementing ? i <= targetValue : i >= targetValue)
            {
                try
                {
                    VisitStmtBlock(context.stmtBlock());
                }
                catch (BreakLoopException)
                {
                    return null;
                }
                i += isIncrementing ? 1 : -1;
            }

            return null;
        }

        public override object VisitReturnStatement([NotNull] PigeonParser.ReturnStatementContext context)
        {
            return VisitExpr(context.expr());
        }

        public override object VisitStmtBlock([NotNull] PigeonParser.StmtBlockContext context)
        {
            foreach (var statement in context.stmt())
            {
                var r = VisitStmt(statement);
                if (statement is PigeonParser.ReturnStatementContext)
                    return r;
                if (statement is PigeonParser.ContinueStatementContext)
                    return null;
                if (statement is PigeonParser.BreakStatementContext)
                    throw new BreakLoopException()
            }
            return null;
        }

        public override object VisitExpr([NotNull] PigeonParser.ExprContext context)
        {
            return base.VisitExpr(context);
        }

        public override object VisitFunctionArgs([NotNull] PigeonParser.FunctionArgsContext context)
        {
            return base.VisitFunctionArgs(context);
        }

        public override object VisitFunctionCall([NotNull] PigeonParser.FunctionCallContext context)
        {
            return base.VisitFunctionCall(context);
        }

        public override object VisitFunctionCallExpression([NotNull] PigeonParser.FunctionCallExpressionContext context)
        {
            return base.VisitFunctionCallExpression(context);
        }

        public override object VisitFunctionCallStatement([NotNull] PigeonParser.FunctionCallStatementContext context)
        {
            return base.VisitFunctionCallStatement(context);
        }

        public override object VisitFunctionDecl([NotNull] PigeonParser.FunctionDeclContext context)
        {
            return base.VisitFunctionDecl(context);
        }

        public override object VisitFunctionParams([NotNull] PigeonParser.FunctionParamsContext context)
        {
            return base.VisitFunctionParams(context);
        }

        public override object VisitStmt([NotNull] PigeonParser.StmtContext context)
        {
            return base.VisitStmt(context);
        }

        public override object VisitTernaryExpression([NotNull] PigeonParser.TernaryExpressionContext context)
        {
            return base.VisitTernaryExpression(context);
        }

        public override object VisitVarAssign([NotNull] PigeonParser.VarAssignContext context)
        {
            return base.VisitVarAssign(context);
        }

        public override object VisitVariableAsignment([NotNull] PigeonParser.VariableAsignmentContext context)
        {
            return base.VisitVariableAsignment(context);
        }

        public override object VisitVariableExpression([NotNull] PigeonParser.VariableExpressionContext context)
        {
            return base.VisitVariableExpression(context);
        }

        /*
        private readonly Stack<Scope> scopes;
        private readonly TypedAstRoot typedAstRoot;

        internal Evaluator(TypedAstRoot typedAstRoot)
        {
            scopes = new Stack<Scope>();
            this.typedAstRoot = typedAstRoot;
        }

        internal void Evaluate()
        {
            EvaluateStatement(typedAstRoot.StatementBlock);
        }

        private void AssignValue(VariableSymbol variable, object value)
        {
            scopes.Peek().Assign(variable, value);
        }

        private object GetVariableValue(VariableSymbol variable)
        {
            return scopes.Peek().Evaluate(variable);
        }

        private void EvaluateStatementBlock(TypedStatementBlock node, Scope predefinedScope = null)
        {
            scopes.Push(predefinedScope ?? new Scope());
            foreach (var statement in node.Statements)
                EvaluateStatement(statement);
            scopes.Pop();
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

        private object EvaluateFunctionCallExpression(TypedFunctionCallExpression node)
        {
            BuiltinFunctions.TryLookup(node.Function.Name, out var builtinFunction);
            var argumentValues = new List<object>();
            foreach (var argument in node.Arguments)
            {
                argumentValues.Add(EvaluateExpression(argument));
            }
            return builtinFunction.Action(argumentValues.ToArray());
        }

        private object EvaluateVariableExpression(TypedVariableExpression node)
        {
            return GetVariableValue(node.Variable);
        }
        */
        
    }
}
