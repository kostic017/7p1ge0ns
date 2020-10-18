using Kostic017.Pigeon.Symbols;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kostic017.Pigeon.TAST
{
    enum AssigmentOperator
    {
        Eq,
        PlusEq,
        MinusEq,
        MulEq,
        DivEq,
        ModEq,
        PowerEq,
    }

    class TypedAssignmentOperator
    {
        internal AssigmentOperator Kind { get; }
        internal TypeSymbol VariableType { get; }
        internal TypeSymbol ValueType { get; }

        internal TypedAssignmentOperator(AssigmentOperator op, TypeSymbol variableType, TypeSymbol valueType)
        {
            Kind = op;
            VariableType = variableType;
            ValueType = valueType;
        }

        private bool Supports(TypeSymbol variableType, TypeSymbol valueType)
        {
            return VariableType == variableType && ValueType == valueType;
        }

        internal static bool TryBind(SyntaxTokenType op, TypeSymbol variableType, TypeSymbol valueType, out TypedAssignmentOperator typedOperator)
        {
            typedOperator = null;
            if (combinations.TryGetValue(op, out var typedOperators))
                typedOperator = typedOperators.FirstOrDefault(t => t.Supports(variableType, valueType));
            return typedOperator != null;
        }

        private static readonly Dictionary<SyntaxTokenType, TypedAssignmentOperator[]> combinations
            = new Dictionary<SyntaxTokenType, TypedAssignmentOperator[]>
            {
                {
                    SyntaxTokenType.Eq,
                    new[]
                    {
                        new TypedAssignmentOperator(AssigmentOperator.Eq, TypeSymbol.Int, TypeSymbol.Int),
                        new TypedAssignmentOperator(AssigmentOperator.Eq, TypeSymbol.Int, TypeSymbol.Float),
                        new TypedAssignmentOperator(AssigmentOperator.Eq, TypeSymbol.Float, TypeSymbol.Int),
                        new TypedAssignmentOperator(AssigmentOperator.Eq, TypeSymbol.Float, TypeSymbol.Float),
                        new TypedAssignmentOperator(AssigmentOperator.Eq, TypeSymbol.String, TypeSymbol.String),
                        new TypedAssignmentOperator(AssigmentOperator.Eq, TypeSymbol.Bool, TypeSymbol.Bool),
                    }
                },
                {
                    SyntaxTokenType.PlusEq,
                    new[]
                    {
                        new TypedAssignmentOperator(AssigmentOperator.PlusEq, TypeSymbol.Int, TypeSymbol.Int),
                        new TypedAssignmentOperator(AssigmentOperator.PlusEq, TypeSymbol.Int, TypeSymbol.Float),
                        new TypedAssignmentOperator(AssigmentOperator.PlusEq, TypeSymbol.Float, TypeSymbol.Int),
                        new TypedAssignmentOperator(AssigmentOperator.PlusEq, TypeSymbol.Float, TypeSymbol.Float),
                        new TypedAssignmentOperator(AssigmentOperator.PlusEq, TypeSymbol.String, TypeSymbol.Int),
                        new TypedAssignmentOperator(AssigmentOperator.PlusEq, TypeSymbol.String, TypeSymbol.Float),
                        new TypedAssignmentOperator(AssigmentOperator.PlusEq, TypeSymbol.String, TypeSymbol.Bool),
                        new TypedAssignmentOperator(AssigmentOperator.PlusEq, TypeSymbol.String, TypeSymbol.String),
                    }
                },
                {
                    SyntaxTokenType.MinusEq,
                    new[]
                    {
                        new TypedAssignmentOperator(AssigmentOperator.MinusEq, TypeSymbol.Int, TypeSymbol.Int),
                        new TypedAssignmentOperator(AssigmentOperator.MinusEq, TypeSymbol.Int, TypeSymbol.Float),
                        new TypedAssignmentOperator(AssigmentOperator.MinusEq, TypeSymbol.Float, TypeSymbol.Int),
                        new TypedAssignmentOperator(AssigmentOperator.MinusEq, TypeSymbol.Float, TypeSymbol.Float),
                    }
                },
                {
                    SyntaxTokenType.MulEq,
                    new[]
                    {
                        new TypedAssignmentOperator(AssigmentOperator.MulEq, TypeSymbol.Int, TypeSymbol.Int),
                        new TypedAssignmentOperator(AssigmentOperator.MulEq, TypeSymbol.Int, TypeSymbol.Float),
                        new TypedAssignmentOperator(AssigmentOperator.MulEq, TypeSymbol.Float, TypeSymbol.Int),
                        new TypedAssignmentOperator(AssigmentOperator.MulEq, TypeSymbol.Float, TypeSymbol.Float),
                    }
                },
                {
                    SyntaxTokenType.DivEq,
                    new[]
                    {
                        new TypedAssignmentOperator(AssigmentOperator.DivEq, TypeSymbol.Int, TypeSymbol.Int),
                        new TypedAssignmentOperator(AssigmentOperator.DivEq, TypeSymbol.Int, TypeSymbol.Float),
                        new TypedAssignmentOperator(AssigmentOperator.DivEq, TypeSymbol.Float, TypeSymbol.Int),
                        new TypedAssignmentOperator(AssigmentOperator.DivEq, TypeSymbol.Float, TypeSymbol.Float),
                    }
                },
                {
                    SyntaxTokenType.ModEq,
                    new[]
                    {
                        new TypedAssignmentOperator(AssigmentOperator.ModEq, TypeSymbol.Int, TypeSymbol.Int),
                    }
                },

            };
    }
}
