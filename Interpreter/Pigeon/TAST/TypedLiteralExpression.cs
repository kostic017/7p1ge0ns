using System;

namespace Kostic017.Pigeon.TAST
{
    class TypedLiteralExpression : TypedExpression
    {
        internal object Value { get; }

        public TypedLiteralExpression(object value)
        {
            Value = value;
        }

        internal override Type Type => Value.GetType();
        internal override NodeKind Kind => NodeKind.LiteralExpression;
    }
}
