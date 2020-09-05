namespace Kostic017.Pigeon.AST
{
    enum AstNodeKind
    {
        Program,
        StatementBlock,
        BinaryExpression,
        IdExpression,
        LiteralExpression,
        ParenthesizedExpression,
        UnaryExpression,
        VariableDeclaration,
        ExpressionStatement,
        SyntaxTokenWrap,
    }
}