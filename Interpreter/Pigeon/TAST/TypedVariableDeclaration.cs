using Kostic017.Pigeon.TAST;
using Kostic017.Pigeon.Variable;

namespace Kostic017.Pigeon
{
    class TypedVariableDeclaration : TypedStatement
    {
        internal VariableSymbol Variable { get; }
        internal TypedExpression Value { get; }

        internal TypedVariableDeclaration(VariableSymbol variable, TypedExpression value)
        {
            Variable = variable;
            Value = value;
        }

        internal override NodeKind Kind => NodeKind.VariableDeclaration;
    }
}