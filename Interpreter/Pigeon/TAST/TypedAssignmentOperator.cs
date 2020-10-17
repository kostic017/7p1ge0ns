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
        internal AssigmentOperator Op { get; }
        internal Type VariableType { get; }
        internal Type ValueType { get; }

        internal TypedAssignmentOperator(AssigmentOperator op, Type variableType, Type valueType)
        {
            Op = op;
            VariableType = variableType;
            ValueType = valueType;
        }

        private bool Supports(Type variableType, Type valueType)
        {
            return VariableType == variableType && ValueType == valueType;
        }

        internal static TypedAssignmentOperator Bind(SyntaxTokenType op, Type variableType, Type valueType)
        {
            if (combinations.TryGetValue(op, out var typedOperators))
                return typedOperators.FirstOrDefault(t => t.Supports(variableType, valueType));
            return null;
        }

        private static readonly Dictionary<SyntaxTokenType, TypedAssignmentOperator[]> combinations
            = new Dictionary<SyntaxTokenType, TypedAssignmentOperator[]>
            {
                {
                    SyntaxTokenType.Eq,
                    new[]
                    {
                        new TypedAssignmentOperator(AssigmentOperator.Eq, typeof(int), typeof(int)),
                        new TypedAssignmentOperator(AssigmentOperator.Eq, typeof(int), typeof(float)),
                        new TypedAssignmentOperator(AssigmentOperator.Eq, typeof(float), typeof(int)),
                        new TypedAssignmentOperator(AssigmentOperator.Eq, typeof(float), typeof(float)),
                        new TypedAssignmentOperator(AssigmentOperator.Eq, typeof(string), typeof(string)),
                        new TypedAssignmentOperator(AssigmentOperator.Eq, typeof(bool), typeof(bool)),
                    }
                },
                {
                    SyntaxTokenType.PlusEq,
                    new[]
                    {
                        new TypedAssignmentOperator(AssigmentOperator.PlusEq, typeof(int), typeof(int)),
                        new TypedAssignmentOperator(AssigmentOperator.PlusEq, typeof(int), typeof(float)),
                        new TypedAssignmentOperator(AssigmentOperator.PlusEq, typeof(float), typeof(int)),
                        new TypedAssignmentOperator(AssigmentOperator.PlusEq, typeof(float), typeof(float)),
                        new TypedAssignmentOperator(AssigmentOperator.PlusEq, typeof(string), typeof(string)),
                    }
                },
                {
                    SyntaxTokenType.MinusEq,
                    new[]
                    {
                        new TypedAssignmentOperator(AssigmentOperator.MinusEq, typeof(int), typeof(int)),
                        new TypedAssignmentOperator(AssigmentOperator.MinusEq, typeof(int), typeof(float)),
                        new TypedAssignmentOperator(AssigmentOperator.MinusEq, typeof(float), typeof(int)),
                        new TypedAssignmentOperator(AssigmentOperator.MinusEq, typeof(float), typeof(float)),
                    }
                },
                {
                    SyntaxTokenType.MulEq,
                    new[]
                    {
                        new TypedAssignmentOperator(AssigmentOperator.MulEq, typeof(int), typeof(int)),
                        new TypedAssignmentOperator(AssigmentOperator.MulEq, typeof(int), typeof(float)),
                        new TypedAssignmentOperator(AssigmentOperator.MulEq, typeof(float), typeof(int)),
                        new TypedAssignmentOperator(AssigmentOperator.MulEq, typeof(float), typeof(float)),
                    }
                },
                {
                    SyntaxTokenType.DivEq,
                    new[]
                    {
                        new TypedAssignmentOperator(AssigmentOperator.DivEq, typeof(int), typeof(int)),
                        new TypedAssignmentOperator(AssigmentOperator.DivEq, typeof(int), typeof(float)),
                        new TypedAssignmentOperator(AssigmentOperator.DivEq, typeof(float), typeof(int)),
                        new TypedAssignmentOperator(AssigmentOperator.DivEq, typeof(float), typeof(float)),
                    }
                },
                {
                    SyntaxTokenType.ModEq,
                    new[]
                    {
                        new TypedAssignmentOperator(AssigmentOperator.ModEq, typeof(int), typeof(int)),
                    }
                },
                {
                    SyntaxTokenType.PowerEq,
                    new[]
                    {
                        new TypedAssignmentOperator(AssigmentOperator.PowerEq, typeof(int), typeof(int)),
                        new TypedAssignmentOperator(AssigmentOperator.PowerEq, typeof(int), typeof(float)),
                        new TypedAssignmentOperator(AssigmentOperator.PowerEq, typeof(float), typeof(int)),
                        new TypedAssignmentOperator(AssigmentOperator.PowerEq, typeof(float), typeof(float)),
                    }
                },

            };
    }
}
