using System;

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

        internal override Type Type => Op.Type;
        internal override NodeKind Kind => NodeKind.UnaryExpression;
    }
}
