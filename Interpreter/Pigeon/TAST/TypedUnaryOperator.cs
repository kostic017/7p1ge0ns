using System;
using System.Collections.Generic;

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
        readonly HashSet<Type> supportedTypes;

        private TypedUnaryOperator(UnaryOperator op, params Type[] types)
        {
            Op = op;
            supportedTypes = new HashSet<Type>(types);
        }

        private bool Supports(Type type)
        {
            return supportedTypes.Contains(type);
        }

        private static readonly Dictionary<SyntaxTokenType, TypedUnaryOperator> operators
            = new Dictionary<SyntaxTokenType, TypedUnaryOperator>
            {
                { SyntaxTokenType.Plus, new TypedUnaryOperator(UnaryOperator.Plus, typeof(int), typeof(float)) },
                { SyntaxTokenType.Minus, new TypedUnaryOperator(UnaryOperator.Minus, typeof(int), typeof(float)) },
                { SyntaxTokenType.Not, new TypedUnaryOperator(UnaryOperator.Not, typeof(bool)) }
            };

        internal static TypedUnaryOperator Bind(SyntaxTokenType op, Type operandType)
        {
            if (operators.TryGetValue(op, out var typedOperator))
                if (typedOperator.Supports(operandType))
                    return typedOperator;
            return null;
        }
    }
}
