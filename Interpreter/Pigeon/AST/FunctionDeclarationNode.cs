using System.Collections.Generic;

namespace Kostic017.Pigeon.AST
{
    class Parameter
    {
        internal SyntaxToken Type { get; }
        internal SyntaxToken Name { get; }

        internal Parameter(SyntaxToken type, SyntaxToken name)
        {
            Type = type;
            Name = name;
        }
    }

    class FunctionDeclarationNode : AstNode
    {
        internal SyntaxToken Type { get; }
        internal SyntaxToken Name { get; }
        internal Parameter[] Parameters { get; }

        internal FunctionDeclarationNode(SyntaxToken type, SyntaxToken name, Parameter[] parameters)
        {
            Type = type;
            Name = name;
            Parameters = parameters;
        }

        internal override NodeKind Kind => NodeKind.FunctionDeclaration;

        internal override IEnumerable<AstNode> GetChildren()
        {
            yield return new SyntaxTokenWrap(Type);
            yield return new SyntaxTokenWrap(Name);
            foreach (var parameter in Parameters)
            {
                yield return new SyntaxTokenWrap(parameter.Type);
                yield return new SyntaxTokenWrap(parameter.Name);
            }
        }
    }
}
