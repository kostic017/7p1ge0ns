using System.Collections.Generic;

namespace Kostic017.Pigeon.AST
{
    internal class VariableDeclaration : Statement
    {
        internal SyntaxToken Keyword { get; }
        internal SyntaxToken Name { get; }
        internal Expression Value { get; }

        internal VariableDeclaration(SyntaxToken type, SyntaxToken name)
        {
            Keyword = type;
            Name = name;
        }

        internal VariableDeclaration(SyntaxToken type, SyntaxToken name, Expression value)
        {
            Keyword = type;
            Name = name;
            Value = value;
        }

        internal override NodeKind Kind => NodeKind.VariableDeclaration;

        internal override IEnumerable<AstNode> GetChildren()
        {
            yield return new SyntaxTokenWrap(Keyword);
            yield return new SyntaxTokenWrap(Name);
            yield return Value;
        }
    }
}
