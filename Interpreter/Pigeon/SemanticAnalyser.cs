using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Kostic017.Pigeon.Errors;
using Kostic017.Pigeon.Symbols;
using Kostic017.Pigeon.Operators;

namespace Kostic017.Pigeon
{
    class SemanticAnalyser : PigeonBaseListener
    {
        private Scope scope;
        private readonly CodeErrorBag errorBag;
        private readonly GlobalScope globalScope;

        internal ParseTreeProperty<PigeonType> Types { get; } = new ParseTreeProperty<PigeonType>();

        internal SemanticAnalyser(CodeErrorBag errorBag, GlobalScope globalScope)
        {
            this.errorBag = errorBag;
            this.globalScope = globalScope;
        }

        public override void EnterProgram([NotNull] PigeonParser.ProgramContext context)
        {
            scope = globalScope;
        }

        public override void EnterFunctionDecl([NotNull] PigeonParser.FunctionDeclContext context)
        {
            scope = new Scope(scope);
            globalScope.TryGetFunction(context.ID().GetText(), out var function);
            foreach (var parameter in function.Parameters)
                scope.DeclareVariable(parameter.Type, parameter.Name, parameter.ReadOnly);
        }

        public override void ExitFunctionDecl([NotNull] PigeonParser.FunctionDeclContext context)
        {
            scope = scope.Parent;
        }

        public override void ExitFunctionCall([NotNull] PigeonParser.FunctionCallContext context)
        {
            var functionName = context.ID().GetText();
            if (!globalScope.TryGetFunction(functionName, out var function))
            {
                errorBag.ReportUndeclaredFunction(context.GetTextSpan(), functionName);
                return;
            }
            var argumentCount = context.functionArgs() != null ? context.functionArgs().expr().Length : 0;
            if (argumentCount != function.Parameters.Length)
                errorBag.ReportInvalidNumberOfArguments(context.GetTextSpan(), functionName, argumentCount);
            for (var i = 0; i < argumentCount; ++i)
            {
                var argument = context.functionArgs().expr(i);
                var argumentType = Types.Get(argument);
                if (function.Parameters[i].Type != PigeonType.Any && argumentType != function.Parameters[i].Type)
                    errorBag.ReportInvalidArgumentType(argument.GetTextSpan(), i, function.Parameters[i].Type);
            }
        }

        public override void ExitFunctionCallExpression([NotNull] PigeonParser.FunctionCallExpressionContext context)
        {
            var functionName = context.functionCall().ID().GetText();
            if (globalScope.TryGetFunction(functionName, out var function))
                Types.Put(context, function.ReturnType);
            else
                errorBag.ReportUndeclaredFunction(context.GetTextSpan(), functionName);
        }

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
            scope.DeclareVariable(PigeonType.Int, context.ID().GetText(), false);
        }

        public override void ExitForStatement([NotNull] PigeonParser.ForStatementContext context)
        {
            CheckExprType(context.expr(0), PigeonType.Int);
            CheckExprType(context.expr(1), PigeonType.Int);
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

        public override void ExitVarAssign([NotNull] PigeonParser.VarAssignContext context)
        {
            var varName = context.ID().GetText();
            var varType = Types.Get(context.expr());

            if (scope.TryGetVariable(varName, out var variable))
            {
                if (variable.ReadOnly)
                    errorBag.ReportRedefiningReadOnlyVariable(context.GetTextSpan(), varName);
                if (!AssignmentOperator.IsAssignable(context.op.Text, variable.Type, varType))
                    errorBag.ReportInvalidTypeAssignment(context.GetTextSpan(), varName, variable.Type, varType);
            }
            else if (scope.IsVariableDeclaredHere(varName))
                errorBag.ReportVariableRedeclaration(context.GetTextSpan(), varName);
            else if (context.op.Text == "=")
                scope.DeclareVariable(varType, varName, IsAllUpper(varName));
            else
                errorBag.ReportUndeclaredVariable(context.GetTextSpan(), varName);
        }

        public override void ExitBreakStatement([NotNull] PigeonParser.BreakStatementContext context)
        {
            if (!IsInLoop(context))
                errorBag.ReportStatementNotInLoop(context.Start.GetTextSpan(), "break");
        }

        public override void ExitContinueStatement([NotNull] PigeonParser.ContinueStatementContext context)
        {
            if (!IsInLoop(context))
                errorBag.ReportStatementNotInLoop(context.Start.GetTextSpan(), "continue");
        }

        public override void ExitNumberLiteral([NotNull] PigeonParser.NumberLiteralContext context)
        {
            Types.Put(context, context.GetText().Contains(".") ? PigeonType.Float : PigeonType.Int);
        }

        public override void ExitStringLiteral([NotNull] PigeonParser.StringLiteralContext context)
        {
            Types.Put(context, PigeonType.String);
        }

        public override void ExitBoolLiteral([NotNull] PigeonParser.BoolLiteralContext context)
        {
            Types.Put(context, PigeonType.Bool);
        }

        public override void ExitParenthesizedExpression([NotNull] PigeonParser.ParenthesizedExpressionContext context)
        {
            Types.Put(context, Types.Get(context.expr()));
        }
        
        public override void ExitBinaryExpression([NotNull] PigeonParser.BinaryExpressionContext context)
        {
            var left = Types.Get(context.expr(0));
            var right = Types.Get(context.expr(1));
            if (!BinaryOperator.TryGetResType(context.op.Text, left, right, out var type))
                errorBag.ReportInvalidTypeBinaryOperator(context.op.GetTextSpan(), context.op.Text, left, right);
            Types.Put(context, type);
        }

        public override void ExitUnaryExpression([NotNull] PigeonParser.UnaryExpressionContext context)
        {
            var operandType = Types.Get(context.expr());
            if (!UnaryOperator.TryGetResType(context.op.Text, operandType , out var type))
                errorBag.ReportInvalidTypeUnaryOperator(context.op.GetTextSpan(), context.op.Text, type);
            Types.Put(context, type);
        }

        public override void ExitTernaryExpression([NotNull] PigeonParser.TernaryExpressionContext context)
        {
            CheckExprType(context.expr(0), PigeonType.Bool);
            
            var whenTrue = Types.Get(context.expr(1));
            var whenFalse = Types.Get(context.expr(2));
            if (!TernaryOperator.TryGetResType(whenTrue, whenFalse, out var type))
                errorBag.ReportInvalidTypeTernaryOperator(context.GetTextSpan(), whenTrue, whenFalse);
            
            Types.Put(context, type);
        }

        public override void ExitVariableExpression([NotNull] PigeonParser.VariableExpressionContext context)
        {
            var name = context.ID().GetText();
            if (scope.TryGetVariable(name, out var variable))
                Types.Put(context, variable.Type);
            else
                errorBag.ReportUndeclaredVariable(context.GetTextSpan(), name);
        }

        public override void ExitReturnStatement([NotNull] PigeonParser.ReturnStatementContext context)
        {
            var returnType = Types.Get(context.expr());
            
            RuleContext node = context;
            while (!(node is PigeonParser.FunctionDeclContext))
                node = node.Parent;
            
            var functionName = ((PigeonParser.FunctionDeclContext)node).ID().GetText();
            globalScope.TryGetFunction(functionName, out var function);
            
            if (returnType != function.ReturnType)
                errorBag.ReportUnexpectedType(context.expr().GetTextSpan(), returnType, function.ReturnType);
        }

        private void CheckExprType(PigeonParser.ExprContext context, PigeonType expected)
        {
            var actual = Types.Get(context);
            if (actual != expected)
                errorBag.ReportUnexpectedType(context.GetTextSpan(), actual, expected);
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

        private bool IsAllUpper(string input)
        {
            for (int i = 0; i < input.Length; i++)
                if (char.IsLetter(input[i]) && !char.IsUpper(input[i]))
                    return false;
            return true;
        }
    }
}
