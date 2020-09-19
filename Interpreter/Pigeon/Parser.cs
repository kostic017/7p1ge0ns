using Kostic017.Pigeon.AST;
using System;
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
        SyntaxToken Peek => index + 1 < tokens.Length ? tokens[index + 1] : tokens[tokens.Length - 1];

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
                {
                    var startToken = Current;
                    statements.Add(ParseStatement());
                    // If ParseStatement() did not consume any tokens,
                    // we need to skip the current token in order to
                    // avoid an infinite loop (e.g. for input '{ )').
                    if (startToken == Current)
                        NextToken();
                }   

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
                case SyntaxTokenType.If:
                    return ParseIfStatement();
                case SyntaxTokenType.For:
                    return ParseForStatement();
                case SyntaxTokenType.While:
                    return ParseWhileStatement();
                case SyntaxTokenType.Do:
                    return ParseDoWhileStatement();
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

        // for id = <expression> (to|downto) <expression> [step <expression>] <statement_block>
        private StatementNode ParseForStatement()
        {
            ExpressionNode step = null;
            Match(SyntaxTokenType.For);
            var id = Match(SyntaxTokenType.ID);
            Match(SyntaxTokenType.Assign);
            var from = ParseExpression();
            var dir = Match(SyntaxTokenType.To, SyntaxTokenType.Downto);
            var to = ParseExpression();
            if (Current.Type == SyntaxTokenType.Step)
            {
                Match(SyntaxTokenType.Step);
                step = ParseExpression();
            }
            var body = ParseStatementBlock();
            return new ForStatementNode(id, from, dir, to, step, body);
        }

        // while <expression> <statement_block>
        private StatementNode ParseWhileStatement()
        {
            Match(SyntaxTokenType.While);
            var condition = ParseExpression();
            var body = ParseStatementBlock();
            return new WhileStatementNode(condition, body);
        }

        // do <statement_block> while <expression>
        private StatementNode ParseDoWhileStatement()
        {
            Match(SyntaxTokenType.Do);
            var body = ParseStatementBlock();
            Match(SyntaxTokenType.While);
            var condition = ParseExpression();
            return new DoWhileStatementNode(body, condition);
        }

        // (let|const) id = <expression>
        private VariableDeclarationNode ParseVariableDeclaration()
        {
            var keyword = Match(SyntaxTokenType.Let, SyntaxTokenType.Const);
            var id = Match(SyntaxTokenType.ID);
            Match(SyntaxTokenType.Assign);
            var value = ParseExpression();
            return new VariableDeclarationNode(keyword, id, value);
        }

        // <expression>
        private StatementNode ParseExpressionStatement()
        {
            var expression = ParseExpression();
            return new ExpressionStatementNode(expression);
        }

        // precedence climbing algorithm
        private ExpressionNode ParseExpression(int precedence = 0)
        {
            if (Current.Type == SyntaxTokenType.ID && Peek.Type == SyntaxTokenType.Assign)
                return ParseAssignmentExpression();

            var left = ParsePrimaryExpression();

            while (SyntaxFacts.BinOpPrec.ContainsKey(Current.Type) && SyntaxFacts.BinOpPrec[Current.Type] >= precedence)
            {
                var op = NextToken();
                var right = ParseExpression(SyntaxFacts.BinOpPrec[op.Type] + (int)SyntaxFacts.BinOpAssoc(op.Type));
                left = new BinaryExpressionNode(left, op, right);
            }

            return left;
        }

        private ExpressionNode ParseAssignmentExpression()
        {
            var id = Match(SyntaxTokenType.ID);
            Match(SyntaxTokenType.Assign);
            var value = ParseExpression();
            return new AssignmentExpressionNode(id, value);
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
                    ReportError(CodeErrorType.INVALID_EXPRESSION_TERM, Current.Type.GetDescription());
                    return new LiteralExpressionNode(DummyToken(SyntaxTokenType.Illegal), "");
            }
        }

        private ExpressionNode ParseIdExpression()
        {
            var token = Match(SyntaxTokenType.ID);
            return new IdentifierExpressionNode(token);
        }

        private ExpressionNode ParseIntLiteralExpression()
        {
            var token = Match(SyntaxTokenType.IntLiteral);
            
            if (int.TryParse(token.Value, out int i))
                return new LiteralExpressionNode(token, i);
            
            ReportError(CodeErrorType.ILLEGAL_NUMBER, token.Value);
            return new LiteralExpressionNode(token, token.Value);
        }

        private ExpressionNode ParseFloatLiteralExpression()
        {
            var token = Match(SyntaxTokenType.FloatLiteral);
            
            if (float.TryParse(token.Value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out float f))
                return new LiteralExpressionNode(token, f);
     
            ReportError(CodeErrorType.ILLEGAL_NUMBER, token.Value);
            return new LiteralExpressionNode(token, token.Value);
        }

        private ExpressionNode ParseStringLiteralExpression()
        {
            var token = Match(SyntaxTokenType.StringLiteral);
            return new LiteralExpressionNode(token, token.Value);
        }

        private ExpressionNode ParseBoolLiteralExpression()
        {
            var token = Match(SyntaxTokenType.BoolLiteral);
            return new LiteralExpressionNode(token, bool.Parse(token.Value));
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
            var opToken = NextToken();
            var primary = ParsePrimaryExpression();
            return new UnaryExpressionNode(opToken, primary);
        }

        private SyntaxToken Match(params SyntaxTokenType[] types)
        {
            var token = NextToken();
            if (types.Any(t => t == token.Type))
                return token;
            ReportError(CodeErrorType.MISSING_EXPECTED_TOKEN, string.Join(", ", types.Select(t => t.GetDescription())));
            return DummyToken(types[0]);
        }

        private SyntaxToken NextToken()
        {
            var current = Current;
            index = Math.Min(index + 1, tokens.Length - 1);
            return current;
        }

        /// <summary>
        /// Fabricates tokens so we can avoid null checks later on.
        /// </summary>
        private SyntaxToken DummyToken(SyntaxTokenType type)
        {
            return new SyntaxToken(type, -1, -1, -1, -1);
        }

        private void ReportError(CodeErrorType type, params string[] data)
        {
            Errors.Add(new CodeError(type, Current.StartLine, Current.StartColumn, data));
        }
    }
}
