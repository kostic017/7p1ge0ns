using Kostic017.Pigeon.Symbols;

namespace Kostic017.Pigeon.TAST
{
    class TypedFunctionCallExpression : TypedExpression
    {
        internal FunctionSymbol Function { get; }
        internal TypedExpression[] Arguments { get; }

        internal TypedFunctionCallExpression(FunctionSymbol function, TypedExpression[] arguments)
        {
            Function = function;
            Arguments = arguments;
        }

        internal override TypeSymbol Type => Function.ReturnType;
        internal override NodeKind Kind => NodeKind.FunctionCallExpression;
    }
}
