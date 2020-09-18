using System.Collections.Generic;

namespace Kostic017.Pigeon.AST
{
    internal class WhileStatementNode : StatementNode
    {
        internal ExpressionNode Condition { get; }
        internal StatementBlockNode Body { get; }

        public WhileStatementNode(ExpressionNode condition, StatementBlockNode body)
        {
            Condition = condition;
            Body = body;
        }

        internal override SyntaxNodeKind Kind => SyntaxNodeKind.WhileStatement;

        internal override IEnumerable<AstNode> GetChildren()
        {
            yield return Condition;
            yield return Body;
        }
    }
}
