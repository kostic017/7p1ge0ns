using System;
using System.Collections.Generic;
using System.Linq;

namespace Kostic017.Pigeon.TAST
{
    enum BinaryOperator
    {
        Plus,
        Minus,
        Mul,
        Div,
        Mod,
        Pow,
    }

    class TypedBinaryOperator
    {
        readonly Type leftType;
        readonly Type rightType;
        internal Type ResultType { get; }
        internal BinaryOperator Op { get; }

        private TypedBinaryOperator(BinaryOperator op, Type left, Type right, Type result)
        {
            Op = op;
            ResultType = result;
            leftType = left;
            rightType = right;
        }

        private bool Supports(Type left, Type right)
        {
            return leftType == left && rightType == right;
        }

        internal static TypedBinaryOperator Bind(SyntaxTokenType op, Type leftType, Type rightType)
        {
            if (operators.TryGetValue(op, out var tops))
                return tops.FirstOrDefault(t => t.Supports(leftType, rightType));
            return null;
        }

        private static readonly Dictionary<SyntaxTokenType, TypedBinaryOperator[]> operators
            = new Dictionary<SyntaxTokenType, TypedBinaryOperator[]>
            {
                {
                    SyntaxTokenType.Plus,
                    new[]
                    {
                        new TypedBinaryOperator(BinaryOperator.Plus, typeof(int), typeof(int), typeof(int)),
                        new TypedBinaryOperator(BinaryOperator.Plus, typeof(int), typeof(float), typeof(float)),
                        new TypedBinaryOperator(BinaryOperator.Plus, typeof(float), typeof(int), typeof(float)),
                        new TypedBinaryOperator(BinaryOperator.Plus, typeof(float), typeof(float), typeof(float)),
                        new TypedBinaryOperator(BinaryOperator.Plus, typeof(int), typeof(string), typeof(string)),
                        new TypedBinaryOperator(BinaryOperator.Plus, typeof(string), typeof(int), typeof(string)),
                        new TypedBinaryOperator(BinaryOperator.Plus, typeof(float), typeof(string), typeof(string)),
                        new TypedBinaryOperator(BinaryOperator.Plus, typeof(string), typeof(float), typeof(string)),
                        new TypedBinaryOperator(BinaryOperator.Plus, typeof(bool), typeof(string), typeof(string)),
                        new TypedBinaryOperator(BinaryOperator.Plus, typeof(string), typeof(bool), typeof(string)),
                        new TypedBinaryOperator(BinaryOperator.Plus, typeof(string), typeof(string), typeof(string)),
                    }
                },
                {
                    SyntaxTokenType.Minus,
                    new[]
                    {
                        new TypedBinaryOperator(BinaryOperator.Minus, typeof(int), typeof(int), typeof(int)),
                        new TypedBinaryOperator(BinaryOperator.Minus, typeof(int), typeof(float), typeof(float)),
                        new TypedBinaryOperator(BinaryOperator.Minus, typeof(float), typeof(int), typeof(float)),
                        new TypedBinaryOperator(BinaryOperator.Minus, typeof(float), typeof(float), typeof(float)),
                    }
                },
                {
                    SyntaxTokenType.Mul,
                    new[]
                    {
                        new TypedBinaryOperator(BinaryOperator.Mul, typeof(int), typeof(int), typeof(int)),
                        new TypedBinaryOperator(BinaryOperator.Mul, typeof(int), typeof(float), typeof(float)),
                        new TypedBinaryOperator(BinaryOperator.Mul, typeof(float), typeof(int), typeof(float)),
                        new TypedBinaryOperator(BinaryOperator.Mul, typeof(float), typeof(float), typeof(float)),
                    }
                },
                {
                    SyntaxTokenType.Div,
                    new []
                    {
                        new TypedBinaryOperator(BinaryOperator.Div, typeof(int), typeof(int), typeof(int)),
                        new TypedBinaryOperator(BinaryOperator.Div, typeof(int), typeof(float), typeof(float)),
                        new TypedBinaryOperator(BinaryOperator.Div, typeof(float), typeof(int), typeof(float)),
                        new TypedBinaryOperator(BinaryOperator.Div, typeof(float), typeof(float), typeof(float)),
                    }
                },
                {
                    SyntaxTokenType.Mod,
                    new []
                    {
                        new TypedBinaryOperator(BinaryOperator.Div, typeof(int), typeof(int), typeof(int)),
                    }
                },
                {
                    SyntaxTokenType.Power,
                    new []
                    {
                        new TypedBinaryOperator(BinaryOperator.Div, typeof(int), typeof(int), typeof(int)),
                        new TypedBinaryOperator(BinaryOperator.Div, typeof(int), typeof(float), typeof(float)),
                        new TypedBinaryOperator(BinaryOperator.Div, typeof(float), typeof(int), typeof(float)),
                        new TypedBinaryOperator(BinaryOperator.Div, typeof(float), typeof(float), typeof(float)),
                    }
                }
            };
    }
}
