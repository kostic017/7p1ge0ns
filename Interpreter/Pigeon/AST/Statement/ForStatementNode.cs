using System.Collections.Generic;

namespace Kostic017.Pigeon.AST
{
    internal class ForStatementNode : StatementNode
    {
        internal SyntaxToken Variable { get; }
        internal ExpressionNode From { get; }
        internal SyntaxToken Dir { get; }
        internal ExpressionNode To { get; }
        internal ExpressionNode Step { get; }
        internal StatementBlockNode Body { get; }

        public ForStatementNode(SyntaxToken variable, ExpressionNode from, SyntaxToken dir, ExpressionNode to, ExpressionNode step, StatementBlockNode body)
        {
            Variable = variable;
            From = from;
            Dir = dir;
            To = to;
            Step = step;
            Body = body;
        }

        internal override SyntaxNodeKind Kind => SyntaxNodeKind.ForStatement;

        internal override IEnumerable<AstNode> GetChildren()
        {
            yield return new SyntaxTokenWrap(Variable);
            yield return From;
            yield return new SyntaxTokenWrap(Dir);
            yield return To;
            if (Step != null)
            {
                yield return Step;
            }
            yield return Body;
        }
    }
}
