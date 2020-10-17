using System.Collections.Generic;
using System.Linq;

namespace Kostic017.Pigeon.AST
{
    class SyntaxTokenWrap : AstNode
    {
        internal SyntaxToken Token { get; }

        internal SyntaxTokenWrap(SyntaxToken token)
        {
            Token = token;
        }

        internal override TextSpan TextSpan
        {
            get
            {
                return Token.TextSpan;
            }
        }

        internal override NodeKind Kind => NodeKind.SyntaxTokenWrap;
        internal override IEnumerable<AstNode> GetChildren() => Enumerable.Empty<AstNode>();
    }
}
