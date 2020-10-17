using Kostic017.Pigeon.Symbols;

namespace Kostic017.Pigeon.TAST
{
    abstract class TypedExpression : TypedAstNode
    {
        internal abstract TypeSymbol Type { get; }
    }
}
