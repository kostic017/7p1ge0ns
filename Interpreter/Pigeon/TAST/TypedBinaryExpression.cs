using System;

namespace Kostic017.Pigeon.TAST
{


    class TypedBinaryExpression : TypedExpression
    {
        internal TypedExpression Left { get; }
        internal BinaryOperator Op { get; }
        internal TypedExpression Right { get; }
        readonly Type resultType;

        public TypedBinaryExpression(TypedExpression left, BinaryOperator op, TypedExpression right, Type resultType)
        {
            Left = left;
            Op = op;
            Right = right;
            this.resultType = resultType;
        }

        internal override Type Type => resultType;
        internal override NodeKind Kind => NodeKind.BinaryExpression;
    }
}
