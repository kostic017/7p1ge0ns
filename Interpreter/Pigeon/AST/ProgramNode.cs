using System.Text;

namespace Kostic017.Pigeon.AST
{
    class ProgramNode : AstNode
    {
        internal StatementNode[] statements;

        public ProgramNode(StatementNode[] statements)
        {
            this.statements = statements;
        }

        public override string AsString(int ident = 0)
        {
            StringBuilder sb = new StringBuilder();
            foreach (StatementNode node in statements)
            {
                sb.AppendLine(node.AsString());
            }
            return sb.ToString();
        }
    }
}
