using Kostic017.Pigeon.Variable;
using System;

namespace Kostic017.Pigeon.TAST
{
    class TypedVariableExpression : TypedExpression
    {
        internal VariableSymbol Variable { get; }

        internal TypedVariableExpression(VariableSymbol variable)
        {
            Variable = variable;
        }

        internal override Type Type => Variable.Type;
        internal override NodeKind Kind => NodeKind.VariableExpression;
    }
}
