using System.Collections.Generic;

namespace Kostic017.Pigeon.AST
{
    internal class DoWhileStatementNode : StatementNode
    {
        internal StatementBlockNode Body { get; }
        internal ExpressionNode Condition { get; }

        public DoWhileStatementNode(StatementBlockNode body, ExpressionNode condition)
        {
            Body = body;
            Condition = condition;
        }

        internal override SyntaxNodeKind Kind => SyntaxNodeKind.DoWhileStatement;

        internal override IEnumerable<AstNode> GetChildren()
        {
            yield return Body;
            yield return Condition;
        }
    }
}
