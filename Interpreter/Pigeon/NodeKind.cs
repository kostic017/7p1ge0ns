namespace Kostic017.Pigeon
{
    enum NodeKind
    {
        Program,
        StatementBlock,
        BinaryExpression,
        VariableExpression,
        LiteralExpression,
        ParenthesizedExpression,
        UnaryExpression,
        VariableDeclaration,
        VariableAssignment,
        SyntaxTokenWrap,
        IfStatement,
        WhileStatement,
        ForStatement,
        DoWhileStatement,
        ErrorExpression,
    }
}