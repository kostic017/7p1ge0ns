using System;
using System.Collections.Generic;

namespace Kostic017.Pigeon.AST
{
    class IfStatementNode : StatementNode
    {
        internal ExpressionNode Condition { get; }
        internal StatementBlockNode ThenBlock { get; }
        internal StatementBlockNode ElseBlock { get; }

        internal IfStatementNode(ExpressionNode condition, StatementBlockNode thenBlock, StatementBlockNode elseBlock = null)
        {
            Condition = condition;
            ThenBlock = thenBlock;
            ElseBlock = elseBlock;
        }

        internal override SyntaxNodeKind Kind => SyntaxNodeKind.IfStatement;

        internal override IEnumerable<AstNode> GetChildren()
        {
            yield return Condition;
            yield return ThenBlock;
            if (ElseBlock != null)
                yield return ElseBlock;
        }
    }
}
