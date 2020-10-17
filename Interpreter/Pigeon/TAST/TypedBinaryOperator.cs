using Kostic017.Pigeon.Symbols;
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
        And,
        Or,
        Eq,
        Neq,
    }

    class TypedBinaryOperator
    {
        readonly TypeSymbol leftType;
        readonly TypeSymbol rightType;
        internal TypeSymbol ResultType { get; }
        internal BinaryOperator Op { get; }

        private TypedBinaryOperator(BinaryOperator op, TypeSymbol left, TypeSymbol right, TypeSymbol result)
        {
            Op = op;
            ResultType = result;
            leftType = left;
            rightType = right;
        }

        private bool Supports(TypeSymbol left, TypeSymbol right)
        {
            return leftType == left && rightType == right;
        }

        internal static bool TryBind(SyntaxTokenType op, TypeSymbol leftType, TypeSymbol rightType, out TypedBinaryOperator typedOperator)
        {
            typedOperator = null;
            if (combinations.TryGetValue(op, out var typedOperators))
                typedOperator = typedOperators.FirstOrDefault(t => t.Supports(leftType, rightType));
            return typedOperator != null;
        }

        private static readonly Dictionary<SyntaxTokenType, TypedBinaryOperator[]> combinations
            = new Dictionary<SyntaxTokenType, TypedBinaryOperator[]>
            {
                {
                    SyntaxTokenType.Plus,
                    new[]
                    {
                        new TypedBinaryOperator(BinaryOperator.Plus, TypeSymbol.Int, TypeSymbol.Int, TypeSymbol.Int),
                        new TypedBinaryOperator(BinaryOperator.Plus, TypeSymbol.Int, TypeSymbol.Float, TypeSymbol.Float),
                        new TypedBinaryOperator(BinaryOperator.Plus, TypeSymbol.Float, TypeSymbol.Int, TypeSymbol.Float),
                        new TypedBinaryOperator(BinaryOperator.Plus, TypeSymbol.Float, TypeSymbol.Float, TypeSymbol.Float),
                        new TypedBinaryOperator(BinaryOperator.Plus, TypeSymbol.Int, TypeSymbol.String, TypeSymbol.String),
                        new TypedBinaryOperator(BinaryOperator.Plus, TypeSymbol.String, TypeSymbol.Int, TypeSymbol.String),
                        new TypedBinaryOperator(BinaryOperator.Plus, TypeSymbol.Float, TypeSymbol.String, TypeSymbol.String),
                        new TypedBinaryOperator(BinaryOperator.Plus, TypeSymbol.String, TypeSymbol.Float, TypeSymbol.String),
                        new TypedBinaryOperator(BinaryOperator.Plus, TypeSymbol.Bool, TypeSymbol.String, TypeSymbol.String),
                        new TypedBinaryOperator(BinaryOperator.Plus, TypeSymbol.String, TypeSymbol.Bool, TypeSymbol.String),
                        new TypedBinaryOperator(BinaryOperator.Plus, TypeSymbol.String, TypeSymbol.String, TypeSymbol.String),
                    }
                },
                {
                    SyntaxTokenType.Minus,
                    new[]
                    {
                        new TypedBinaryOperator(BinaryOperator.Minus, TypeSymbol.Int, TypeSymbol.Int, TypeSymbol.Int),
                        new TypedBinaryOperator(BinaryOperator.Minus, TypeSymbol.Int, TypeSymbol.Float, TypeSymbol.Float),
                        new TypedBinaryOperator(BinaryOperator.Minus, TypeSymbol.Float, TypeSymbol.Int, TypeSymbol.Float),
                        new TypedBinaryOperator(BinaryOperator.Minus, TypeSymbol.Float, TypeSymbol.Float, TypeSymbol.Float),
                    }
                },
                {
                    SyntaxTokenType.Mul,
                    new[]
                    {
                        new TypedBinaryOperator(BinaryOperator.Mul, TypeSymbol.Int, TypeSymbol.Int, TypeSymbol.Int),
                        new TypedBinaryOperator(BinaryOperator.Mul, TypeSymbol.Int, TypeSymbol.Float, TypeSymbol.Float),
                        new TypedBinaryOperator(BinaryOperator.Mul, TypeSymbol.Float, TypeSymbol.Int, TypeSymbol.Float),
                        new TypedBinaryOperator(BinaryOperator.Mul, TypeSymbol.Float, TypeSymbol.Float, TypeSymbol.Float),
                    }
                },
                {
                    SyntaxTokenType.Div,
                    new []
                    {
                        new TypedBinaryOperator(BinaryOperator.Div, TypeSymbol.Int, TypeSymbol.Int, TypeSymbol.Int),
                        new TypedBinaryOperator(BinaryOperator.Div, TypeSymbol.Int, TypeSymbol.Float, TypeSymbol.Float),
                        new TypedBinaryOperator(BinaryOperator.Div, TypeSymbol.Float, TypeSymbol.Int, TypeSymbol.Float),
                        new TypedBinaryOperator(BinaryOperator.Div, TypeSymbol.Float, TypeSymbol.Float, TypeSymbol.Float),
                    }
                },
                {
                    SyntaxTokenType.Mod,
                    new []
                    {
                        new TypedBinaryOperator(BinaryOperator.Div, TypeSymbol.Int, TypeSymbol.Int, TypeSymbol.Int),
                    }
                },
                {
                    SyntaxTokenType.Power,
                    new []
                    {
                        new TypedBinaryOperator(BinaryOperator.Div, TypeSymbol.Int, TypeSymbol.Int, TypeSymbol.Int),
                        new TypedBinaryOperator(BinaryOperator.Div, TypeSymbol.Int, TypeSymbol.Float, TypeSymbol.Float),
                        new TypedBinaryOperator(BinaryOperator.Div, TypeSymbol.Float, TypeSymbol.Int, TypeSymbol.Float),
                        new TypedBinaryOperator(BinaryOperator.Div, TypeSymbol.Float, TypeSymbol.Float, TypeSymbol.Float),
                    }
                },
                {
                    SyntaxTokenType.And,
                    new []
                    {
                        new TypedBinaryOperator(BinaryOperator.And, TypeSymbol.Bool, TypeSymbol.Bool, TypeSymbol.Bool),
                    }
                },
                {
                    SyntaxTokenType.Or,
                    new []
                    {
                        new TypedBinaryOperator(BinaryOperator.Or, TypeSymbol.Bool, TypeSymbol.Bool, TypeSymbol.Bool),
                    }
                },
                {
                    SyntaxTokenType.EqEq,
                    new []
                    {
                        new TypedBinaryOperator(BinaryOperator.Eq, TypeSymbol.Int, TypeSymbol.Int, TypeSymbol.Bool),
                        new TypedBinaryOperator(BinaryOperator.Eq, TypeSymbol.Int, TypeSymbol.Float, TypeSymbol.Bool),
                        new TypedBinaryOperator(BinaryOperator.Eq, TypeSymbol.Float, TypeSymbol.Int, TypeSymbol.Bool),
                        new TypedBinaryOperator(BinaryOperator.Eq, TypeSymbol.Float, TypeSymbol.Float, TypeSymbol.Bool),
                        new TypedBinaryOperator(BinaryOperator.Eq, TypeSymbol.Bool, TypeSymbol.Bool, TypeSymbol.Bool),
                        new TypedBinaryOperator(BinaryOperator.Eq, TypeSymbol.String, TypeSymbol.String, TypeSymbol.Bool),
                    }
                },
                {
                    SyntaxTokenType.Neq,
                    new []
                    {
                        new TypedBinaryOperator(BinaryOperator.Neq, TypeSymbol.Int, TypeSymbol.Int, TypeSymbol.Bool),
                        new TypedBinaryOperator(BinaryOperator.Neq, TypeSymbol.Int, TypeSymbol.Float, TypeSymbol.Bool),
                        new TypedBinaryOperator(BinaryOperator.Neq, TypeSymbol.Float, TypeSymbol.Int, TypeSymbol.Bool),
                        new TypedBinaryOperator(BinaryOperator.Neq, TypeSymbol.Float, TypeSymbol.Float, TypeSymbol.Bool),
                        new TypedBinaryOperator(BinaryOperator.Neq, TypeSymbol.Bool, TypeSymbol.Bool, TypeSymbol.Bool),
                        new TypedBinaryOperator(BinaryOperator.Neq, TypeSymbol.String, TypeSymbol.String, TypeSymbol.Bool),
                    }
                },
            };
    }
}
