using Kostic017.Pigeon.Symbols;

namespace Kostic017.Pigeon.TAST
{
    class TypedErrorExpression : TypedExpression
    {
        internal override TypeSymbol Type => TypeSymbol.Error;
        internal override NodeKind Kind => NodeKind.ErrorExpression;
    }
}
