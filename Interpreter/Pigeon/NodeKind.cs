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
        SyntaxTokenWrap,
        IfStatement,
        WhileStatement,
        ForStatement,
        DoWhileStatement,
        AssignmentExpression,
        VariableAssignment,
    }
}