using Kostic017.Pigeon.Symbols;

namespace Kostic017.Pigeon.TAST
{
    class TypedVariableExpression : TypedExpression
    {
        internal VariableSymbol Variable { get; }

        internal TypedVariableExpression(VariableSymbol variable)
        {
            Variable = variable;
        }

        internal override TypeSymbol Type => Variable.Type;
        internal override NodeKind Kind => NodeKind.VariableExpression;
    }
}
