using System.Collections.Generic;

namespace Kostic017.Pigeon.AST
{
    class ProgramNode : AstNode
    {
        internal List<StatementNode> Statements { get; }

        internal ProgramNode(List<StatementNode> statements)
        {
            Statements = statements;
        }

        internal override AstNodeKind Kind() => AstNodeKind.Program;
    }
}
