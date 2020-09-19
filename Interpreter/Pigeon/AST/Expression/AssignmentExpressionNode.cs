using System.Collections.Generic;

namespace Kostic017.Pigeon.AST
{
    internal class AssignmentExpressionNode : ExpressionNode
    {
        internal SyntaxToken IdentifierToken { get; }
        internal ExpressionNode Value { get; }

        public AssignmentExpressionNode(SyntaxToken identifierToken, ExpressionNode value)
        {
            IdentifierToken = identifierToken;
            Value = value;
        }

        internal override SyntaxNodeKind Kind => SyntaxNodeKind.AssignmentExpression;

        internal override IEnumerable<AstNode> GetChildren()
        {
            yield return new SyntaxTokenWrap(IdentifierToken);
            yield return Value;
        }
    }
}
