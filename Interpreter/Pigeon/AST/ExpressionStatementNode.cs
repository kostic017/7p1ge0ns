namespace Kostic017.Pigeon.AST
{
    internal class ExpressionStatementNode : StatementNode
    {
        internal ExpressionNode expression { get; }

        internal ExpressionStatementNode(ExpressionNode expression)
        {
            this.expression = expression;
        }
        
        internal override AstNodeKind Kind => AstNodeKind.ExpressionStatement;
    }
}
