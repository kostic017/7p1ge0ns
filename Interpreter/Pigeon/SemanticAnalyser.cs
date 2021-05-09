using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Kostic017.Pigeon.Errors;
using Kostic017.Pigeon.Symbols;
using Antlr4.Runtime.Tree;
using Kostic017.Pigeon.Operators;

namespace Kostic017.Pigeon
{
    class SemanticAnalyser : PigeonBaseListener
    {
        internal CodeErrorBag ErrorBag { get; }

        Scope scope = new Scope(null);
        readonly ParseTreeProperty<PigeonType> types = new ParseTreeProperty<PigeonType>();

        public override void EnterStmtBlock([NotNull] PigeonParser.StmtBlockContext context)
        {
            scope = new Scope(scope);
        }

        public override void ExitStmtBlock([NotNull] PigeonParser.StmtBlockContext context)
        {
            scope = scope.Parent;
        }

        public override void EnterForStatement([NotNull] PigeonParser.ForStatementContext context)
        {
            scope = new Scope(scope);
            scope.Declare(PigeonType.Int, context.ID().GetText(), false);
            CheckExprType(context.expr(0), PigeonType.Bool);
            CheckExprType(context.expr(1), PigeonType.Bool);
        }

        public override void ExitForStatement([NotNull] PigeonParser.ForStatementContext context)
        {
            scope = scope.Parent;
        }

        public override void ExitWhileStatement([NotNull] PigeonParser.WhileStatementContext context)
        {
            CheckExprType(context.expr(), PigeonType.Bool);
        }

        public override void ExitDoWhileStatement([NotNull] PigeonParser.DoWhileStatementContext context)
        {
            CheckExprType(context.expr(), PigeonType.Bool);
        }

        public override void ExitIfStatement([NotNull] PigeonParser.IfStatementContext context)
        {
            CheckExprType(context.expr(), PigeonType.Bool);
        }

        public override void ExitVarDecl([NotNull] PigeonParser.VarDeclContext context)
        {
            var name = context.ID().GetText();
            var type = types.RemoveFrom(context.expr());
            if (scope.TryLookup(name, out var _))
                ErrorBag.ReportVariableRedeclaration(context.GetTextSpan(), name);
            else
                scope.Declare(type, name, context.accessType.Text == "const");
        }

        public override void ExitVarAssign([NotNull] PigeonParser.VarAssignContext context)
        {
            var name = context.variable().ID().GetText();
            var valueType = types.RemoveFrom(context.expr());

            if (scope.TryLookup(name, out var variable))
            {
                if (variable.ReadOnly)
                    ErrorBag.ReportRedefiningReadOnlyVariable(context.GetTextSpan(), name);
                if (!AssignmentOperator.IsAssignable(context.op.Text, variable.Type, valueType))
                    ErrorBag.ReportInvalidTypeAssignment(context.GetTextSpan(), name, variable.Type, valueType);
            }
            else
                ErrorBag.ReportUndeclaredVariable(context.variable().GetTextSpan(), name);
        }

        public override void ExitBreakStatement([NotNull] PigeonParser.BreakStatementContext context)
        {
            if (!IsInLoop(context))
                ErrorBag.ReportStatementNotInLoop(context.Start.GetTextSpan(), "break");
        }

        public override void ExitContinueStatement([NotNull] PigeonParser.ContinueStatementContext context)
        {
            if (!IsInLoop(context))
                ErrorBag.ReportStatementNotInLoop(context.Start.GetTextSpan(), "continue");
        }

        public override void ExitNumberLiteral([NotNull] PigeonParser.NumberLiteralContext context)
        {
            types.Put(context, context.GetText().Contains(".") ? PigeonType.Float : PigeonType.Int);
        }

        public override void ExitStringLiteral([NotNull] PigeonParser.StringLiteralContext context)
        {
            types.Put(context, PigeonType.String);
        }

        public override void ExitBoolLiteral([NotNull] PigeonParser.BoolLiteralContext context)
        {
            types.Put(context, PigeonType.Bool);
        }

        public override void ExitParenthesizedExpression([NotNull] PigeonParser.ParenthesizedExpressionContext context)
        {
            types.Put(context, types.RemoveFrom(context.expr()));
        }
        
        public override void ExitBinaryExpression([NotNull] PigeonParser.BinaryExpressionContext context)
        {
            var left = types.RemoveFrom(context.expr(0));
            var right = types.RemoveFrom(context.expr(1));
            if (!BinaryOperator.TryGetResType(context.op.Text, left, right, out var type))
                ErrorBag.ReportInvalidTypeBinaryOperator(context.op.GetTextSpan(), context.op.Text, left, right);
            types.Put(context, type);
        }

        public override void ExitUnaryExpression([NotNull] PigeonParser.UnaryExpressionContext context)
        {
            var operandType = types.RemoveFrom(context.expr());
            if (!UnaryOperator.TryGetResType(context.op.Text, operandType , out var type))
                ErrorBag.ReportInvalidTypeUnaryOperator(context.op.GetTextSpan(), context.op.Text, type);
            types.Put(context, type);
        }

        public override void ExitTernaryExpression([NotNull] PigeonParser.TernaryExpressionContext context)
        {
            CheckExprType(context.expr(0), PigeonType.Bool);
            
            var whenTrue = types.RemoveFrom(context.expr(1));
            var whenFalse = types.RemoveFrom(context.expr(2));
            if (!TernaryOperator.TryGetResType(whenTrue, whenFalse, out var type))
                ErrorBag.ReportInvalidTypeTernaryOperator(context.GetTextSpan(), whenTrue, whenFalse);
            
            types.Put(context, type);
        }

        public override void ExitVariableExpression([NotNull] PigeonParser.VariableExpressionContext context)
        {
            var name = context.variable().ID().GetText();
            if (scope.TryLookup(name, out var variable))
                types.Put(context.variable(), variable.Type);
            else
                ErrorBag.ReportUndeclaredVariable(context.GetTextSpan(), name);
        }

        public override void ExitReturnStatement([NotNull] PigeonParser.ReturnStatementContext context)
        {
            // TODO check if expr type matches function type
        }

        private void CheckExprType(PigeonParser.ExprContext context, PigeonType expected)
        {
            var actual = types.RemoveFrom(context);
            if (actual != expected)
                ErrorBag.ReportUnexpectedType(context.GetTextSpan(), actual, expected);
        }

        private bool IsInLoop(RuleContext node)
        {
            while (node != null)
            {
                if (
                    node.Parent is PigeonParser.DoWhileStatementContext ||
                    node.Parent is PigeonParser.WhileStatementContext ||
                    node.Parent is PigeonParser.ForStatementContext
                ) return true;
                node = node.Parent;
            }
            return false;
        }

        /*
        private TypedExpression AnaylizeFunctionCallExpression(FunctionCallExpression node)
        {
            if (!BuiltinFunctions.TryLookup(node.NameToken.Value, out var function))
            {
                ErrorBag.ReportFunctionNotDefined(node.NameToken.TextSpan, node.NameToken.Value);
                return new TypedErrorExpression();
            }
            if (function.Symbol.Parameters.Length != node.Arguments.Length)
            {
                ErrorBag.ReportInvalidNumberOfParameters(node.TextSpan, function.Symbol.Parameters.Length);
                return new TypedErrorExpression();
            }
            var arguments = new List<TypedExpression>();
            for (var i = 0; i < node.Arguments.Length; ++i)
            {
                var expectedType = function.Symbol.Parameters[i].Type;
                var argument = AnalyzeExpression(node.Arguments[i].Value);
                if (argument.Type != expectedType)
                {
                    ErrorBag.ReportInvalidParameterType(node.TextSpan, i + 1, expectedType);
                    return new TypedErrorExpression();
                }
                arguments.Add(argument);
            }
            return new TypedFunctionCallExpression(function.Symbol, arguments.ToArray());
        }
        */
    }
}
