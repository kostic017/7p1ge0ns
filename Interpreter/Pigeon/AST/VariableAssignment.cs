using System.Collections.Generic;

namespace Kostic017.Pigeon.AST
{
    class VariableAssignment : Statement
    {
        internal SyntaxToken IdentifierToken { get; }
        internal SyntaxToken Op { get; }
        internal Expression Value { get; }

        internal VariableAssignment(SyntaxToken name, SyntaxToken op, Expression value)
        {
            IdentifierToken = name;
            Op = op;
            Value = value;
        }

        internal override NodeKind Kind => NodeKind.VariableAssignment;

        internal override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return new SyntaxTokenWrap(IdentifierToken);
            yield return new SyntaxTokenWrap(Op);
            yield return Value;
        }
    }
}
