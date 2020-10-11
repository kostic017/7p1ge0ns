using System;
using System.Collections.Generic;
using System.Linq;

namespace Kostic017.Pigeon.TAST
{
    enum UnaryOperator
    {
        Plus,
        Minus,
        Not,
    }

    class TypedUnaryOperator
    {
        internal UnaryOperator Op { get; }
        internal Type Type { get; }

        private TypedUnaryOperator(UnaryOperator op, Type type)
        {
            Op = op;
            Type = type;
        }

        private bool Supports(Type type)
        {
            return Type == type;
        }

        internal static TypedUnaryOperator Bind(SyntaxTokenType op, Type operandType)
        {
            if (combinations.TryGetValue(op, out var typedOperators))
                return (typedOperators.FirstOrDefault(t => t.Supports(operandType)));
            return null;
        }

        private static readonly Dictionary<SyntaxTokenType, TypedUnaryOperator[]> combinations
            = new Dictionary<SyntaxTokenType, TypedUnaryOperator[]>
            {
                {
                    SyntaxTokenType.Plus,
                    new[]
                    {
                        new TypedUnaryOperator(UnaryOperator.Plus, typeof(int)),
                        new TypedUnaryOperator(UnaryOperator.Plus, typeof(float)),
                    }
                },
                {
                    SyntaxTokenType.Minus,
                    new[]
                    {
                        new TypedUnaryOperator(UnaryOperator.Minus, typeof(int)),
                        new TypedUnaryOperator(UnaryOperator.Minus, typeof(float))
                    }
                },
                {
                    SyntaxTokenType.Not,
                    new[]
                    {
                        new TypedUnaryOperator(UnaryOperator.Not, typeof(bool))
                    }
                }
            };
    }
}
