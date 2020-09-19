namespace Kostic017.Pigeon.AST
{
    enum SyntaxNodeKind
    {
        Program,
        StatementBlock,
        BinaryExpression,
        IdentifierExpression,
        LiteralExpression,
        ParenthesizedExpression,
        UnaryExpression,
        VariableDeclaration,
        ExpressionStatement,
        SyntaxTokenWrap,
        IfStatement,
        WhileStatement,
        ForStatement,
        DoWhileStatement,
        AssignmentExpression,
    }
}