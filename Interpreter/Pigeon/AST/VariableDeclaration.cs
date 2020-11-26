using System.Collections.Generic;

namespace Kostic017.Pigeon.AST
{
    internal class VariableDeclaration : Statement
    {
        internal SyntaxToken Keyword { get; }
        internal SyntaxToken IdentifierToken { get; }
        internal Expression Value { get; }

        internal VariableDeclaration(SyntaxToken type, SyntaxToken name)
        {
            Keyword = type;
            IdentifierToken = name;
        }

        internal VariableDeclaration(SyntaxToken type, SyntaxToken name, Expression value)
        {
            Keyword = type;
            IdentifierToken = name;
            Value = value;
        }

        internal override NodeKind Kind => NodeKind.VariableDeclaration;

        internal override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return new SyntaxTokenWrap(Keyword);
            yield return new SyntaxTokenWrap(IdentifierToken);
            yield return Value;
        }
    }
}
