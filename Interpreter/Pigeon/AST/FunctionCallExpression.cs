using System.Collections.Generic;

namespace Kostic017.Pigeon.AST
{
    class Argument
    {
        internal Expression Value;
        internal SyntaxToken Comma;

        internal Argument(Expression value, SyntaxToken comma = null)
        {
            Value = value;
            Comma = comma;
        }
    }

    class FunctionCallExpression : Expression
    {
        internal SyntaxToken NameToken { get; }
        internal SyntaxToken LParToken { get; }
        internal Argument[] Arguments { get; }
        internal SyntaxToken RParToken { get; }
        
        internal FunctionCallExpression(SyntaxToken name, SyntaxToken lPar, Argument[] arguments, SyntaxToken rPar)
        {
            NameToken = name;
            LParToken = lPar;
            Arguments = arguments;
            RParToken = rPar;
        }

        internal override NodeKind Kind => NodeKind.FunctionCallExpression;

        internal override IEnumerable<AstNode> GetChildren()
        {
            yield return new SyntaxTokenWrap(NameToken);
            yield return new SyntaxTokenWrap(LParToken);

            foreach (Argument parameter in Arguments)
            {
                yield return parameter.Value;
                if (parameter.Comma != null)
                    yield return new SyntaxTokenWrap(parameter.Comma);
            }

            yield return new SyntaxTokenWrap(RParToken);
        }
    }
}
