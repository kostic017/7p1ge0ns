using Kostic017.Pigeon.AST;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Kostic017.Pigeon
{
    class Parser
    {
        int index;

        readonly SyntaxToken[] tokens;

        SyntaxToken Current => index < tokens.Length ? tokens[index] : tokens[tokens.Length - 1];

        internal List<CodeError> Errors { get; }

        internal Parser(SyntaxToken[] syntaxTokens)
        {
            index = 0;
            tokens = syntaxTokens.Where(token => token.Type != SyntaxTokenType.Comment && token.Type != SyntaxTokenType.BlockComment).ToArray();
            Errors = new List<CodeError>();
        }

        internal AstNode Parse()
        {
            return ParseProgram();
        }

        AstNode ParseProgram()
        {
            var stmtBlock = ParseStatementBlock();
            return new ProgramNode(stmtBlock);
        }

        StatementBlockNode ParseStatementBlock()
        {
            var statements = new List<StatementNode>();

            if (Current.Type == SyntaxTokenType.LBrace)
            {
                Match(SyntaxTokenType.LBrace);

                while (Current.Type != SyntaxTokenType.RBrace && Current.Type != SyntaxTokenType.EOF)
                    statements.Add(ParseStatement());

                if (Current.Type == SyntaxTokenType.EOF)
                    ReportError(CodeErrorType.UNTERMINATED_STATEMENT_BLOCK);

                Match(SyntaxTokenType.RBrace);
            }
            else
            {
                statements.Add(ParseStatement());
            }
            
            return new StatementBlockNode(statements.ToArray());
        }

        StatementNode ParseStatement()
        {
            switch (Current.Type)
            {
                case SyntaxTokenType.If:
                    return ParseIfStatement();
                case SyntaxTokenType.Let:
                case SyntaxTokenType.Const:
                    return ParseVariableDeclaration();
                case SyntaxTokenType.LBrace:
                    return ParseStatementBlock();
                default:
                    return ParseExpressionStatement();
            }
        }

        // if <expression> <statement_block> [else <statement_block>]
        private StatementNode ParseIfStatement()
        {
            Match(SyntaxTokenType.If);
            var conditon = ParseExpression();
            var thenBlock = ParseStatementBlock();

            if (Current.Type == SyntaxTokenType.Else)
            {
                Match(SyntaxTokenType.Else);
                var elseBlock = ParseStatementBlock();
                return new IfStatementNode(conditon, thenBlock, elseBlock);
            }

            return new IfStatementNode(conditon, thenBlock);
        }

        // (let|const) id = <expression>
        VariableDeclarationNode ParseVariableDeclaration()
        {
            var keyword = NextToken();
            var id = Match(SyntaxTokenType.ID);
            Match(SyntaxTokenType.Assign);
            var value = ParseExpression();
            return new VariableDeclarationNode(keyword, id, value);
        }

        // id = <expression>
        // id([<expression>, ...])
        // <expression>
        StatementNode ParseExpressionStatement()
        {
            var expression = ParseExpression();
            return new ExpressionStatementNode(expression);
        }

        // precedence climbing algorithm
        ExpressionNode ParseExpression(int precedence = 0)
        {
            var left = ParsePrimaryExpression();

            while (SyntaxFacts.BinOpPrec.ContainsKey(Current.Type) && SyntaxFacts.BinOpPrec[Current.Type] >= precedence)
            {
                var op = NextToken();
                var right = ParseExpression(SyntaxFacts.BinOpPrec[op.Type] + (int)SyntaxFacts.BinOpAssoc(op.Type));
                left = new BinaryExpressionNode(left, op, right);
            }

            return left;
        }

        ExpressionNode ParsePrimaryExpression()
        {
            switch (Current.Type)
            {
                case SyntaxTokenType.ID:
                    return ParseIdExpression();

                case SyntaxTokenType.IntLiteral:
                    return ParseIntLiteralExpression();

                case SyntaxTokenType.FloatLiteral:
                    return ParseFloatLiteralExpression();

                case SyntaxTokenType.StringLiteral:
                    return ParseStringLiteralExpression();

                case SyntaxTokenType.BoolLiteral:
                    return ParseBoolLiteralExpression();

                case SyntaxTokenType.LPar:
                    return ParseParenthesizedExpression();

                case SyntaxTokenType.Minus:
                case SyntaxTokenType.Plus:
                case SyntaxTokenType.Not:
                    return ParseUnaryExpression();

                default:
                    ReportError(CodeErrorType.INVALID_EXPRESSION_TERM, Current.Type.GetDescription());
                    return new LiteralExpressionNode(DummyToken(SyntaxTokenType.Illegal), "");
            }
        }

        ExpressionNode ParseIdExpression()
        {
            var token = Match(SyntaxTokenType.ID);
            return new IdentifierExpressionNode(token);
        }

        ExpressionNode ParseIntLiteralExpression()
        {
            var token = Match(SyntaxTokenType.IntLiteral);
            
            if (int.TryParse(token.Value, out int i))
                return new LiteralExpressionNode(token, i);
            
            ReportError(CodeErrorType.ILLEGAL_NUMBER, token.Value);
            return new LiteralExpressionNode(token, token.Value);
        }

        ExpressionNode ParseFloatLiteralExpression()
        {
            var token = Match(SyntaxTokenType.FloatLiteral);
            
            if (float.TryParse(token.Value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out float f))
                return new LiteralExpressionNode(token, f);
     
            ReportError(CodeErrorType.ILLEGAL_NUMBER, token.Value);
            return new LiteralExpressionNode(token, token.Value);
        }

        ExpressionNode ParseStringLiteralExpression()
        {
            var token = Match(SyntaxTokenType.StringLiteral);
            return new LiteralExpressionNode(token, token.Value);
        }

        ExpressionNode ParseBoolLiteralExpression()
        {
            var token = Match(SyntaxTokenType.BoolLiteral);
            return new LiteralExpressionNode(token, bool.Parse(token.Value));
        }

        ExpressionNode ParseParenthesizedExpression()
        {
            Match(SyntaxTokenType.LPar);
            var expression = ParseExpression();
            Match(SyntaxTokenType.RPar);
            return new ParenthesizedExpressionNode(expression);
        }

        ExpressionNode ParseUnaryExpression()
        {
            var opToken = NextToken();
            var primary = ParsePrimaryExpression();
            return new UnaryExpressionNode(opToken, primary);
        }

        SyntaxToken Match(SyntaxTokenType type)
        {
            if (Current.Type != type)
            {
                ReportError(CodeErrorType.MISSING_EXPECTED_TOKEN, type.GetDescription());
                return DummyToken(type);
            }

            return NextToken();
        }

        SyntaxToken NextToken()
        {
            var current = Current;
            ++index;
            return current;
        }

        /// <summary>
        /// Fabricates tokens so we can avoid null checks later on.
        /// </summary>
        SyntaxToken DummyToken(SyntaxTokenType type)
        {
            return new SyntaxToken(type, -1, -1, -1, -1);
        }

        void ReportError(CodeErrorType type, params string[] data)
        {
            Errors.Add(new CodeError(type, Current.StartLine, Current.StartColumn, data));
        }
    }
}
