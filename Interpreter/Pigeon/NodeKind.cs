namespace Kostic017.Pigeon
{
    enum NodeKind
    {
        AstRoot,
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
        ErrorStatement,
        PowerExpression,
        ExpressionStatement,
        FunctionCallExpression,
        FunctionDeclaration,
    }
}