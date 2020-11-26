using System.Collections.Generic;

namespace Kostic017.Pigeon.AST
{
    class ForStatement : Statement
    {
        internal SyntaxToken IdentifierToken { get; }
        internal Expression StartValue { get; }
        internal SyntaxToken DirectionToken { get; }
        internal Expression TargetValue { get; }
        internal Expression StepValue { get; }
        internal StatementBlock Body { get; }

        internal ForStatement(SyntaxToken identifierToken, Expression startValue, SyntaxToken directionToken,
                              Expression targetValue, StatementBlock body)
        {
            IdentifierToken = identifierToken;
            StartValue = startValue;
            DirectionToken = directionToken;
            TargetValue = targetValue;
            Body = body;
        }

        internal override NodeKind Kind => NodeKind.ForStatement;

        internal override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return new SyntaxTokenWrap(IdentifierToken);
            yield return StartValue;
            yield return new SyntaxTokenWrap(DirectionToken);
            yield return TargetValue;
            yield return Body;
        }
    }
}
