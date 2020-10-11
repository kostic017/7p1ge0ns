﻿using System;

namespace Kostic017.Pigeon.TAST
{
    class TypedBinaryExpression : TypedExpression
    {
        internal TypedExpression Left { get; }
        internal TypedBinaryOperator Op { get; }
        internal TypedExpression Right { get; }

        internal TypedBinaryExpression(TypedExpression left, TypedBinaryOperator op, TypedExpression right)
        {
            Left = left;
            Op = op;
            Right = right;
        }

        internal override Type Type => Op.ResultType;
        internal override NodeKind Kind => NodeKind.BinaryExpression;
    }
}
