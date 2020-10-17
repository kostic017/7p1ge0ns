using Kostic017.Pigeon.Errors;
using Kostic017.Pigeon.Symbols;

namespace Kostic017.Pigeon.TAST
{
    class TypedLiteralExpression : TypedExpression
    {
        internal object Value { get; }

        internal TypedLiteralExpression(object value)
        {
            Value = value;
            if (value is bool)
                Type = TypeSymbol.Bool;
            else if (value is int)
                Type = TypeSymbol.Int;
            else if (value is float)
                Type = TypeSymbol.Float;
            else if (value is string)
                Type = TypeSymbol.String;
            else
                throw new InternalErrorException($"Unexpected literal '{value}' of type '{value.GetType()}'");
        }

        internal override TypeSymbol Type { get; }
        internal override NodeKind Kind => NodeKind.LiteralExpression;
    }
}
