using Kostic017.Pigeon.Symbols;

namespace Kostic017.Pigeon.TAST
{
    enum LoopDirection
    {
        To,
        Downto
    }

    internal class TypedForStatement : TypedStatement
    {
        internal VariableSymbol Variable { get; }
        internal TypedExpression StartValue { get; }
        internal TypedExpression TargetValue { get; }
        internal TypedExpression StepValue { get; }
        internal LoopDirection Direction { get; }
        internal TypedStatement Body { get; }

        public TypedForStatement(VariableSymbol variable, TypedExpression startValue, TypedExpression targetValue,
                                 TypedExpression stepValue, LoopDirection direction, TypedStatement body)
        {
            Variable = variable;
            StartValue = startValue;
            TargetValue = targetValue;
            StepValue = stepValue;
            Direction = direction;
            Body = body;
        }

        internal override NodeKind Kind => NodeKind.ForStatement;
    }
}