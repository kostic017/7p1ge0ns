using Kostic017.Pigeon.Symbols;
using System.Collections.Generic;
using System.Linq;

namespace Kostic017.Pigeon.Operators
{
    class AssignmentOperator
    {
        internal PigeonType VariableType { get; }
        internal PigeonType ValueType { get; }

        internal AssignmentOperator(PigeonType variableType, PigeonType valueType)
        {
            VariableType = variableType;
            ValueType = valueType;
        }

        private bool Supports(PigeonType variableType, PigeonType valueType)
        {
            return VariableType == variableType && ValueType == valueType;
        }

        internal static bool IsAssignable(string op, PigeonType variableType, PigeonType valueType)
        {
            if (operators.TryGetValue(op, out var combinations))
                return combinations.Any(t => t.Supports(variableType, valueType));
            return false;
        }

        private static readonly Dictionary<string, AssignmentOperator[]> operators = new Dictionary<string, AssignmentOperator[]>
            {
                {
                    "=",
                    new[]
                    {
                        new AssignmentOperator(PigeonType.Int, PigeonType.Int),
                        new AssignmentOperator(PigeonType.Int, PigeonType.Float),
                        new AssignmentOperator(PigeonType.Float, PigeonType.Int),
                        new AssignmentOperator(PigeonType.Float, PigeonType.Float),
                        new AssignmentOperator(PigeonType.String, PigeonType.String),
                        new AssignmentOperator(PigeonType.Bool, PigeonType.Bool),
                    }
                },
                {
                    "+=",
                    new[]
                    {
                        new AssignmentOperator(PigeonType.Int, PigeonType.Int),
                        new AssignmentOperator(PigeonType.Int, PigeonType.Float),
                        new AssignmentOperator(PigeonType.Float, PigeonType.Int),
                        new AssignmentOperator(PigeonType.Float, PigeonType.Float),
                        new AssignmentOperator(PigeonType.String, PigeonType.Int),
                        new AssignmentOperator(PigeonType.String, PigeonType.Float),
                        new AssignmentOperator(PigeonType.String, PigeonType.Bool),
                        new AssignmentOperator(PigeonType.String, PigeonType.String),
                    }
                },
                {
                    "-=",
                    new[]
                    {
                        new AssignmentOperator(PigeonType.Int, PigeonType.Int),
                        new AssignmentOperator(PigeonType.Int, PigeonType.Float),
                        new AssignmentOperator(PigeonType.Float, PigeonType.Int),
                        new AssignmentOperator(PigeonType.Float, PigeonType.Float),
                    }
                },
                {
                    "*=",
                    new[]
                    {
                        new AssignmentOperator(PigeonType.Int, PigeonType.Int),
                        new AssignmentOperator(PigeonType.Int, PigeonType.Float),
                        new AssignmentOperator(PigeonType.Float, PigeonType.Int),
                        new AssignmentOperator(PigeonType.Float, PigeonType.Float),
                    }
                },
                {
                    "/=",
                    new[]
                    {
                        new AssignmentOperator(PigeonType.Int, PigeonType.Int),
                        new AssignmentOperator(PigeonType.Int, PigeonType.Float),
                        new AssignmentOperator(PigeonType.Float, PigeonType.Int),
                        new AssignmentOperator(PigeonType.Float, PigeonType.Float),
                    }
                },
                {
                    "%=",
                    new[]
                    {
                        new AssignmentOperator(PigeonType.Int, PigeonType.Int),
                    }
                },

            };
    }
}
