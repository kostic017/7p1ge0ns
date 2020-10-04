using System.Collections.Generic;

namespace Kostic017.Pigeon.AST
{
    class LiteralExpression : Expression
    {
        internal SyntaxToken LiteralToken { get; }
        internal object ParsedValue { get; }

        internal LiteralExpression(SyntaxToken literalToken, object parsedValue)
        {
            LiteralToken = literalToken;
            ParsedValue = parsedValue;
        }

        internal override NodeKind Kind => NodeKind.LiteralExpression;

        internal override IEnumerable<AstNode> GetChildren()
        {
            yield return new SyntaxTokenWrap(LiteralToken);
        }
    }
}
