using Kostic017.Pigeon.Symbols;

namespace Kostic017.Pigeon.TAST
{
    enum LoopDirection
    {
        To,
        Downto
    }

    class TypedForStatement : TypedStatement
    {
        internal VariableSymbol CounterVariable { get; }
        internal TypedExpression StartValue { get; }
        internal TypedExpression TargetValue { get; }
        internal TypedExpression StepValue { get; }
        internal LoopDirection Direction { get; }
        internal TypedStatementBlock Body { get; }

        public TypedForStatement(VariableSymbol counterVariable, TypedExpression startValue, TypedExpression targetValue,
                                 TypedExpression stepValue, LoopDirection direction, TypedStatementBlock body)
        {
            CounterVariable = counterVariable;
            StartValue = startValue;
            TargetValue = targetValue;
            StepValue = stepValue;
            Direction = direction;
            Body = body;
        }

        internal override NodeKind Kind => NodeKind.ForStatement;
    }
}