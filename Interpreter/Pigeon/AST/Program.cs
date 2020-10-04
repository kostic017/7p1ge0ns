using System.Collections.Generic;

namespace Kostic017.Pigeon.AST
{
    class Program : AstNode
    {
        internal StatementBlock StatementBlock { get; }

        internal Program(StatementBlock statementBlock)
        {
            StatementBlock = statementBlock;
        }

        internal override NodeKind Kind => NodeKind.Program;

        internal override IEnumerable<AstNode> GetChildren()
        {
            yield return StatementBlock;
        }
    }
}
