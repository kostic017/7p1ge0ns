using Kostic017.Pigeon.AST;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Kostic017.Pigeon
{
    class Parser
    {
        readonly static Dictionary<SyntaxTokenType, (int precedence, bool leftAssoc)> binOps =
            new Dictionary<SyntaxTokenType, (int, bool)>
        {
            { SyntaxTokenType.And, (0, true) },
            { SyntaxTokenType.Or, (0, true) },
            { SyntaxTokenType.Eq, (1, true) },
            { SyntaxTokenType.Neq, (1, true) },
            { SyntaxTokenType.Lt, (2, true) },
            { SyntaxTokenType.Gt, (2, true) },
            { SyntaxTokenType.Leq, (2, true) },
            { SyntaxTokenType.Geq, (2, true) },
            { SyntaxTokenType.Plus, (3, true) },
            { SyntaxTokenType.Minus, (3, true) },
            { SyntaxTokenType.Mul, (4, true) },
            { SyntaxTokenType.Div, (4, true) },
            { SyntaxTokenType.Mod, (4, true) },
            { SyntaxTokenType.Power, (5, false) },
        };

        int index;

        readonly SyntaxToken[] tokens;

        internal List<CodeError> Errors { get; }

        SyntaxToken Current => index < tokens.Length ? tokens[index] : tokens[tokens.Length - 1];

        internal Parser(SyntaxToken[] syntaxTokens)
        {
            index = 0;
            tokens = RemoveComments(syntaxTokens);
            Errors = new List<CodeError>();
        }

        private static SyntaxToken[] RemoveComments(SyntaxToken[] tokens)
        {
            return tokens.Where(token => token.Type != SyntaxTokenType.Comment && token.Type != SyntaxTokenType.BlockComment).ToArray();
        }

        internal AstNode Parse()
        {
            return ParseProgram();
        }

        private AstNode ParseProgram()
        {
            var stmtBlock = ParseStatementBlock();
            return new ProgramNode(stmtBlock);
        }

        private StatementBlockNode ParseStatementBlock()
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

        private StatementNode ParseStatement()
        {
            switch (Current.Type)
            {
                case SyntaxTokenType.LBrace:
                    return ParseStatementBlock();
                case SyntaxTokenType.Let:
                case SyntaxTokenType.Const:
                    return ParseVariableDeclaration();
                default:
                    return ParseExpressionStatement();
            }
        }

        // (let|const) id [= <expression>]
        private VariableDeclarationNode ParseVariableDeclaration()
        {
            var keyword = NextToken();
            var id = Match(SyntaxTokenType.ID);
            
            if (Current.Type == SyntaxTokenType.Assign)
            {
                Match(SyntaxTokenType.Assign);
                var value = ParseExpression();
                return new VariableDeclarationNode(keyword, id, value);
            }

            Match(SyntaxTokenType.Semicolon);
            return new VariableDeclarationNode(keyword, id);
        }

        // id = <expression>
        // id([<expression>, ...])
        // <expression>
        private StatementNode ParseExpressionStatement()
        {
            var expression = ParseExpression();
            return new ExpressionStatementNode(expression);
        }

        /// <summary>
        /// Parses expressions by precedence climbing.
        /// </summary>
        private ExpressionNode ParseExpression(int precedence = 0)
        {
            var left = ParsePrimaryExpression();

            while (binOps.ContainsKey(Current.Type) && binOps[Current.Type].precedence >= precedence)
            {
                var op = Current.Type;
                NextToken();
                var right = ParseExpression(binOps[op].precedence + (binOps[op].leftAssoc ? 1 : 0));
                left = new BinaryExpressionNode(left, op, right);
            }

            return left;
        }

        private ExpressionNode ParsePrimaryExpression()
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
                    ReportError(CodeErrorType.INVALID_EXPRESSION_TERM, Current.Type.PrettyPrint());
                    return new LiteralExpressionNode(SyntaxTokenType.Illegal, "");
            }
        }

        private ExpressionNode ParseIdExpression()
        {
            var token = Match(SyntaxTokenType.ID);
            return new IdExpressionNode(token.Value.ToString());
        }

        private ExpressionNode ParseIntLiteralExpression()
        {
            var token = Match(SyntaxTokenType.IntLiteral);
            
            if (int.TryParse(token.Value, out int i))
                return new LiteralExpressionNode(token.Type, i);
            
            ReportError(CodeErrorType.ILLEGAL_NUMBER, token.Value);
            return new LiteralExpressionNode(token.Type, token.Value);
        }

        private ExpressionNode ParseFloatLiteralExpression()
        {
            var token = Match(SyntaxTokenType.IntLiteral);
            
            if (float.TryParse(token.Value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out float f))
                return new LiteralExpressionNode(token.Type, f);
     
            ReportError(CodeErrorType.ILLEGAL_NUMBER, token.Value);
            return new LiteralExpressionNode(token.Type, token.Value);
        }

        private ExpressionNode ParseStringLiteralExpression()
        {
            var token = Match(SyntaxTokenType.StringLiteral);
            return new LiteralExpressionNode(token.Type, token.Value);
        }

        private ExpressionNode ParseBoolLiteralExpression()
        {
            var token = Match(SyntaxTokenType.BoolLiteral);
            return new LiteralExpressionNode(token.Type, bool.Parse(token.Value));
        }

        private ExpressionNode ParseParenthesizedExpression()
        {
            Match(SyntaxTokenType.LPar);
            var expression = ParseExpression();
            Match(SyntaxTokenType.RPar);
            return new ParenthesizedExpressionNode(expression);
        }

        private ExpressionNode ParseUnaryExpression()
        {
            var op = NextToken();
            var primary = ParsePrimaryExpression();
            return new UnaryExpressionNode(op.Type, primary);
        }

        private SyntaxToken Match(SyntaxTokenType type)
        {
            if (Current.Type != type)
            {
                ReportError(CodeErrorType.EXPECTED_TOKEN, type.PrettyPrint());
                return new SyntaxToken(type, -1, -1, -1, -1); // fabricate token to avoid null checks later on
            }

            return NextToken();
        }

        private SyntaxToken NextToken()
        {
            var current = Current;
            ++index;
            return current;
        }

        private void ReportError(CodeErrorType type, params string[] data)
        {
            Errors.Add(new CodeError(type, Current.StartLine, Current.StartColumn, data));
        }
    }
}
