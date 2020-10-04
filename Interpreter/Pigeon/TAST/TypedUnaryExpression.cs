using System;

namespace Kostic017.Pigeon.TAST
{
    class TypedUnaryExpression : TypedExpression
    {
        internal UnaryOperator Op { get; }
        internal TypedExpression Value { get; }

        internal TypedUnaryExpression(UnaryOperator op, TypedExpression value) {
            Op = op;
            Value = value;
        }

        internal override Type Type => Value.Type;
        internal override NodeKind Kind => NodeKind.UnaryExpression;
    }
}
