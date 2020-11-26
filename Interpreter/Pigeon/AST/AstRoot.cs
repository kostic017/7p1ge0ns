using System.Collections.Generic;

namespace Kostic017.Pigeon.AST
{
    class AstRoot : SyntaxNode
    {
        internal StatementBlock StatementBlock { get; }

        internal AstRoot(StatementBlock statementBlock)
        {
            StatementBlock = statementBlock;
        }

        internal override NodeKind Kind => NodeKind.AstRoot;

        internal override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return StatementBlock;
        }
    }
}
