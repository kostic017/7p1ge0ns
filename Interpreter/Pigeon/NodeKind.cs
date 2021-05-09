namespace Kostic017.Pigeon
{
    enum NodeKind
    {
        Error,
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
        PowerExpression,
        ExpressionStatement,
        FunctionCallExpression,
        FunctionDeclaration,
        ContinueStatement,
    }
}