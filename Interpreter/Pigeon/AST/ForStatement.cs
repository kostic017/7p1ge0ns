using System.Collections.Generic;

namespace Kostic017.Pigeon.AST
{
    internal class ForStatement : Statement
    {
        internal SyntaxToken Variable { get; }
        internal Expression From { get; }
        internal SyntaxToken Dir { get; }
        internal Expression To { get; }
        internal Expression Step { get; }
        internal StatementBlock Body { get; }

        public ForStatement(SyntaxToken variable, Expression from, SyntaxToken dir, Expression to, Expression step, StatementBlock body)
        {
            Variable = variable;
            From = from;
            Dir = dir;
            To = to;
            Step = step;
            Body = body;
        }

        internal override NodeKind Kind => NodeKind.ForStatement;

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
