using System.Collections.Generic;

namespace Kostic017.Pigeon.AST
{
    internal class AssignmentExpression : Expression
    {
        internal SyntaxToken IdentifierToken { get; }
        internal Expression Value { get; }

        public AssignmentExpression(SyntaxToken identifierToken, Expression value)
        {
            IdentifierToken = identifierToken;
            Value = value;
        }

        internal override NodeKind Kind => NodeKind.AssignmentExpression;

        internal override IEnumerable<AstNode> GetChildren()
        {
            yield return new SyntaxTokenWrap(IdentifierToken);
            yield return Value;
        }
    }
}
