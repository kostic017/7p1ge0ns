namespace Kostic017.Pigeon
{
    enum NodeKind
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