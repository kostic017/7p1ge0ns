using Kostic017.Pigeon.Symbols;
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
        internal TypeSymbol Type { get; }

        private TypedUnaryOperator(UnaryOperator op, TypeSymbol type)
        {
            Op = op;
            Type = type;
        }

        private bool Supports(TypeSymbol type)
        {
            return Type == type;
        }

        internal static bool TryBind(SyntaxTokenType op, TypeSymbol operandType, out TypedUnaryOperator typedOperator)
        {
            typedOperator = null;
            if (combinations.TryGetValue(op, out var typedOperators))
                typedOperator = typedOperators.FirstOrDefault(t => t.Supports(operandType));
            return typedOperator != null;
        }

        private static readonly Dictionary<SyntaxTokenType, TypedUnaryOperator[]> combinations
            = new Dictionary<SyntaxTokenType, TypedUnaryOperator[]>
            {
                {
                    SyntaxTokenType.Plus,
                    new[]
                    {
                        new TypedUnaryOperator(UnaryOperator.Plus, TypeSymbol.Int),
                        new TypedUnaryOperator(UnaryOperator.Plus, TypeSymbol.Float),
                    }
                },
                {
                    SyntaxTokenType.Minus,
                    new[]
                    {
                        new TypedUnaryOperator(UnaryOperator.Minus, TypeSymbol.Int),
                        new TypedUnaryOperator(UnaryOperator.Minus, TypeSymbol.Float)
                    }
                },
                {
                    SyntaxTokenType.Not,
                    new[]
                    {
                        new TypedUnaryOperator(UnaryOperator.Not, TypeSymbol.Bool)
                    }
                }
            };
    }
}
