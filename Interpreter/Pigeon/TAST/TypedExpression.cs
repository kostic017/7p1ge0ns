using System;

namespace Kostic017.Pigeon.TAST
{
    abstract class TypedExpression : TypedAstNode
    {
        internal abstract Type Type { get; }
    }
}
