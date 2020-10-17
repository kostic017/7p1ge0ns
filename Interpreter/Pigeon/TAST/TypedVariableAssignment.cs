using Kostic017.Pigeon.Symbols;

namespace Kostic017.Pigeon.TAST
{
    class TypedVariableAssignment : TypedStatement
    {
        internal VariableSymbol Variable { get; }
        internal TypedAssignmentOperator Op { get; }
        internal TypedExpression Value { get; }

        internal TypedVariableAssignment(VariableSymbol variable, TypedAssignmentOperator op, TypedExpression value)
        {
            Variable = variable;
            Op = op;
            Value = value;
        }

        internal override NodeKind Kind => NodeKind.VariableAssignment;
    }
}
