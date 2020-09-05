using System.Collections.Generic;

namespace Kostic017.Pigeon.AST
{
    internal class ExpressionStatementNode : StatementNode
    {
        internal ExpressionNode Expression { get; }

        internal ExpressionStatementNode(ExpressionNode expression)
        {
            Expression = expression;
        }
        
        internal override SyntaxNodeKind Kind => SyntaxNodeKind.ExpressionStatement;

        internal override IEnumerable<AstNode> GetChildren()
        {
            yield return Expression;
        }
    }
}
