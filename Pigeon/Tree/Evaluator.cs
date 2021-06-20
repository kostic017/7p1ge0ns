using Antlr4.Runtime.Misc;
using Kostic017.Pigeon.Symbols;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Kostic017.Pigeon
{
    class BreakLoopException : Exception { }
    class ContinueLoopException : Exception { }
    
    class FuncReturnValueException : Exception
    {
        internal object Value { get; }

        internal FuncReturnValueException(object value)
        {
            Value = value;
        }
    }

    class Evaluator : PigeonBaseVisitor<object>
    {
        private readonly SemanticAnalyser analyser;
        private readonly Stack<FunctionScope> functionScopes = new Stack<FunctionScope>();

        internal Evaluator(SemanticAnalyser analyser)
        {
            this.analyser = analyser;
        }

        public override object VisitProgram([NotNull] PigeonParser.ProgramContext context)
        {
            functionScopes.Push(new FunctionScope(analyser.GlobalScope));

            foreach (var stmt in context.stmt())
                VisitStmt(stmt);
            
            return null;
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
            return context.STRING().GetText().Trim('"');
        }

        public override object VisitNumberLiteral([NotNull] PigeonParser.NumberLiteralContext context)
        {
            if (analyser.Types.Get(context) == PigeonType.Int)
                return int.Parse(context.NUMBER().GetText());
            return float.Parse(context.NUMBER().GetText(), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
        }

        public override object VisitUnaryExpression([NotNull] PigeonParser.UnaryExpressionContext context)
        {
            var operand = VisitExpr(context.expr());
            var resType = analyser.Types.Get(context);
            switch (context.op.Text)
            {
                case "+":
                    if (resType == PigeonType.Int)
                        return (int)operand;
                    return (float)operand;
                case "-":
                    if (resType == PigeonType.Int)
                        return -(int)operand;
                    return -(float)operand;
                case "!":
                    return !(bool)operand;
                default:
                    throw new InternalErrorException($"Unsupported unary operator {context.op.Text}");
            }
        }

        public override object VisitBinaryExpression([NotNull] PigeonParser.BinaryExpressionContext context)
        {
            var left = VisitExpr(context.expr(0));
            var right = VisitExpr(context.expr(1));
            var resType = analyser.Types.Get(context);

            var areBothInt =
                analyser.Types.Get(context.expr(0)) == PigeonType.Int &&
                analyser.Types.Get(context.expr(1)) == PigeonType.Int;
            
            switch (context.op.Text)
            {
                case "==":
                    return left.Equals(right);

                case "!=":
                    return !left.Equals(right);

                case "&&":
                    return (bool)left && (bool)right;

                case "||":
                    return (bool)left || (bool)right;

                case "<":
                    if (areBothInt)
                        return (int)left < (int)right;
                    return Convert.ToSingle(left) < Convert.ToSingle(right);

                case ">":
                    if (areBothInt)
                        return (int)left > (int)right;
                    return Convert.ToSingle(left) > Convert.ToSingle(right);

                case "<=":
                    if (areBothInt)
                        return (int)left <= (int)right;
                    return Convert.ToSingle(left) <= Convert.ToSingle(right);

                case ">=":
                    if (areBothInt)
                        return (int)left >= (int)right;
                    return Convert.ToSingle(left) >= Convert.ToSingle(right);

                case "+":
                    if (resType == PigeonType.Int)
                        return (int)left + (int)right;
                    else if (resType == PigeonType.Float)
                        return Convert.ToSingle(left) + Convert.ToSingle(right);
                    else
                        return left.ToString() + right.ToString();

                case "-":
                    if (resType == PigeonType.Int)
                        return (int)left - (int)right;
                    return Convert.ToSingle(left) - Convert.ToSingle(right);

                case "*":
                    if (resType == PigeonType.Int)
                        return (int)left * (int)right;
                    return Convert.ToSingle(left) * Convert.ToSingle(right);

                case "/":
                    if (resType == PigeonType.Int)
                        return (int)left / (int)right;
                    return Convert.ToSingle(left) / Convert.ToSingle(right);

                case "%":
                    return (int)left % (int)right;

                default:
                    throw new InternalErrorException($"Unsupported binary operator {context.op.Text}");
            }
        }

        public override object VisitFunctionCallExpression([NotNull] PigeonParser.FunctionCallExpressionContext context)
        {
            return VisitFunctionCall(context.functionCall());
        }

        public override object VisitTernaryExpression([NotNull] PigeonParser.TernaryExpressionContext context)
        {
            var condition = VisitExpr(context.expr(0));
            var whenTrue = VisitExpr(context.expr(1));
            var whenFalse = VisitExpr(context.expr(2));
            return (bool) condition ? whenTrue : whenFalse;
        }

        public override object VisitVariableExpression([NotNull] PigeonParser.VariableExpressionContext context)
        {
            return functionScopes.Peek().Evaluate(context.ID().GetText());
        }

        public override object VisitIfStatement([NotNull] PigeonParser.IfStatementContext context)
        {
            if ((bool) VisitExpr(context.expr()))
                VisitStmtBlock(context.stmtBlock(0));
            else if (context.stmtBlock(1) != null)
                VisitStmtBlock(context.stmtBlock(1));
            return null;
        }

        public override object VisitStmtBlock([NotNull] PigeonParser.StmtBlockContext context)
        {
            if (ShouldCreateScope(context))
                functionScopes.Peek().EnterScope();
            foreach (var statement in context.stmt())
            {
                var r = VisitStmt(statement);
                if (statement is PigeonParser.ContinueStatementContext)
                    throw new ContinueLoopException();
                if (statement is PigeonParser.BreakStatementContext)
                    throw new BreakLoopException();
                if (statement is PigeonParser.ReturnStatementContext)
                    throw new FuncReturnValueException(r);
            }
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
                catch (ContinueLoopException)
                {
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
                catch (ContinueLoopException)
                {
                }
            return null;
        }

        public override object VisitForStatement([NotNull] PigeonParser.ForStatementContext context)
        {
            var startValue = (int) VisitExpr(context.expr(0));
            var targetValue = (int) VisitExpr(context.expr(1));
            var isIncrementing = context.dir.Text == "to";

            functionScopes.Peek().EnterScope();

            var i = startValue;
            Assign(context.ID().GetText(), i, PigeonType.Int);
            
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
                catch (ContinueLoopException)
                {
                }
                i += isIncrementing ? 1 : -1;
                Assign(context.ID().GetText(), i);
            }

            functionScopes.Peek().ExitScope();
            return null;
        }

        public override object VisitFunctionCallStatement([NotNull] PigeonParser.FunctionCallStatementContext context)
        {
            return VisitFunctionCall(context.functionCall());
        }

        public override object VisitReturnStatement([NotNull] PigeonParser.ReturnStatementContext context)
        {
            return context.expr() != null ? VisitExpr(context.expr()) : null;
        }

        public override object VisitVariableAssignmentStatement([NotNull] PigeonParser.VariableAssignmentStatementContext context)
        {
            return VisitVarAssign(context.varAssign());
        }

        public override object VisitVarAssign([NotNull] PigeonParser.VarAssignContext context)
        {
            var name = context.ID().GetText();
            var type = analyser.Types.Get(context.expr());
            var value = VisitExpr(context.expr());
            var currentValue = functionScopes.Peek().Evaluate(name);

            switch (context.op.Text)
            {
                case "=":
                    Assign(name, value, type);
                    break;

                case "+=":
                    if (type == PigeonType.Int)
                        Assign(name, (int) currentValue + (int) value);
                    else if (type == PigeonType.Float)
                        Assign(name, (float) currentValue + (float) value);
                    else
                        Assign(name, (string) currentValue + (string) value);
                    break;

                case "-=":
                    if (type == PigeonType.Int)
                        Assign(name, (int) currentValue - (int) value);
                    else if (type == PigeonType.Float)
                        Assign(name, (float) currentValue - (float) value);
                    break;

                case "*=":
                    if (type == PigeonType.Int)
                        Assign(name, (int) currentValue * (int) value);
                    else if (type == PigeonType.Float)
                        Assign(name, (float) currentValue * (float) value);
                    break;

                case "/=":
                    if (type == PigeonType.Int)
                        Assign(name, (int) currentValue / (int) value);
                    else if (type == PigeonType.Float)
                        Assign(name, (float) currentValue / (float) value);
                    break;

                case "%=":
                    Assign(name, (int) currentValue / (int) value);
                    break;
            }

            return null;
        }

        public override object VisitFunctionCall([NotNull] PigeonParser.FunctionCallContext context)
        {
            analyser.GlobalScope.TryGetFunction(context.ID().GetText(), out var function);

            var argValues = new List<object>();

            if (context.functionArgs() != null)
                foreach (var arg in context.functionArgs().expr())
                    argValues.Add(VisitExpr(arg));

            if (function.FuncBody is FuncPointer fp)
                return fp(argValues.ToArray());

            var funcBody = (PigeonParser.StmtBlockContext)function.FuncBody;
            functionScopes.Push(new FunctionScope(analyser.GlobalScope));
            
            for (var i = 0; i < argValues.Count; ++i)
                Assign(function.Parameters[i].Name, argValues[i], function.Parameters[i].Type);

            try
            {
                return VisitStmtBlock(funcBody);
            }
            catch (FuncReturnValueException e)
            {
                return e.Value;
            }
            finally
            {
                functionScopes.Pop();
            }
        }

        public override object VisitStmt([NotNull] PigeonParser.StmtContext context)
        {
            if (context is PigeonParser.IfStatementContext ctxi)
                return VisitIfStatement(ctxi);
            if (context is PigeonParser.DoWhileStatementContext ctxd)
                return VisitDoWhileStatement(ctxd);
            if (context is PigeonParser.WhileStatementContext ctxw)
                return VisitWhileStatement(ctxw);
            if (context is PigeonParser.ForStatementContext ctxf)
                return VisitForStatement(ctxf);
            if (context is PigeonParser.FunctionCallStatementContext ctxfu)
                return VisitFunctionCallStatement(ctxfu);
            if (context is PigeonParser.ReturnStatementContext ctxr)
                return VisitReturnStatement(ctxr);
            if (context is PigeonParser.VariableAssignmentStatementContext ctxv)
                return VisitVariableAssignmentStatement(ctxv);
            if (context is PigeonParser.BreakStatementContext)
                return null;
            if (context is PigeonParser.ContinueStatementContext)
                return null;
            throw new InternalErrorException($"Unsupported statement type {context.GetType().Name}");
        }

        public override object VisitExpr([NotNull] PigeonParser.ExprContext context)
        {
            if (context is PigeonParser.ParenthesizedExpressionContext ctxp)
                return VisitParenthesizedExpression(ctxp);
            if (context is PigeonParser.BoolLiteralContext ctxb)
                return VisitBoolLiteral(ctxb);
            if (context is PigeonParser.StringLiteralContext ctxs)
                return VisitStringLiteral(ctxs);
            if (context is PigeonParser.NumberLiteralContext ctxn)
                return VisitNumberLiteral(ctxn);
            if (context is PigeonParser.UnaryExpressionContext ctxu)
                return VisitUnaryExpression(ctxu);
            if (context is PigeonParser.BinaryExpressionContext ctxbi)
                return VisitBinaryExpression(ctxbi);
            if (context is PigeonParser.FunctionCallExpressionContext ctxf)
                return VisitFunctionCallExpression(ctxf);
            if (context is PigeonParser.TernaryExpressionContext ctxt)
                return VisitTernaryExpression(ctxt);
            if (context is PigeonParser.VariableExpressionContext ctxv)
                return VisitVariableExpression(ctxv);
            throw new InternalErrorException($"Unsupported expression type {context.GetType().Name}");
        }

        private void Assign(string name, object value, PigeonType type = null)
        {
            functionScopes.Peek().Assign(name, value, type);
        }

        private bool ShouldCreateScope(PigeonParser.StmtBlockContext context)
        {
            return !(context.Parent is PigeonParser.ForStatementContext);
        }
        
    }
}
