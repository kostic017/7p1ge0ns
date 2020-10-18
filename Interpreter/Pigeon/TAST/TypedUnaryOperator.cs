using Kostic017.Pigeon.Symbols;
using System.Collections.Generic;
using System.Linq;

namespace Kostic017.Pigeon.TAST
{
    enum UnaryOperator
    {
        Plus,
        Minus,
        Negation,
    }

    class TypedUnaryOperator
    {
        internal UnaryOperator Kind { get; }
        internal TypeSymbol ResultType { get; }

        private TypedUnaryOperator(UnaryOperator op, TypeSymbol type)
        {
            Kind = op;
            ResultType = type;
        }

        private bool Supports(TypeSymbol type)
        {
            return ResultType == type;
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
                        new TypedUnaryOperator(UnaryOperator.Negation, TypeSymbol.Bool)
                    }
                }
            };
    }
}
