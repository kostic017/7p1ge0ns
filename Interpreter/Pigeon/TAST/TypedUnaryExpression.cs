using Kostic017.Pigeon.Symbols;

namespace Kostic017.Pigeon.TAST
{
    class TypedUnaryExpression : TypedExpression
    {
        internal TypedUnaryOperator Op { get; }
        internal TypedExpression Operand { get; }

        internal TypedUnaryExpression(TypedUnaryOperator op, TypedExpression operand) {
            Op = op;
            Operand = operand;
        }

        internal override TypeSymbol Type => Op.ResultType;
        internal override NodeKind Kind => NodeKind.UnaryExpression;
    }
}
