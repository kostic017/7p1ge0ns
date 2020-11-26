using Kostic017.Pigeon.AST;
using Kostic017.Pigeon.Errors;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Kostic017.Pigeon
{
    class Parser
    {
        private int index;
        
        private readonly SyntaxToken[] tokens;
        
        private SyntaxToken Current => index < tokens.Length ? tokens[index] : tokens[tokens.Length - 1];
        private SyntaxToken Peek => index + 1 < tokens.Length ? tokens[index + 1] : tokens[tokens.Length - 1];

        internal CodeErrorBag ErrorBag { get; }

        internal Parser(SyntaxToken[] syntaxTokens)
        {
            index = 0;
            tokens = syntaxTokens.Where(token => token.Type != SyntaxTokenType.Comment && token.Type != SyntaxTokenType.BlockComment).ToArray();
            ErrorBag = new CodeErrorBag();
        }

        internal AstRoot Parse()
        {
            var stmtBlock = ParseStatementBlock();
            
            if (Current.Type != SyntaxTokenType.EOF)
                ErrorBag.ReportLeftoverTokensFound(Current.TextSpan);
            
            return new AstRoot(stmtBlock);
        }

        private StatementBlock ParseStatementBlock()
        {
            var statements = new List<Statement>();

            if (Current.Type == SyntaxTokenType.LBrace)
            {
                Match(SyntaxTokenType.LBrace);

                while (Current.Type != SyntaxTokenType.RBrace && Current.Type != SyntaxTokenType.EOF)
                {
                    var stmt = ParseStatement();
                    if (stmt == null) break;
                    statements.Add(stmt);
                }   

                if (Current.Type == SyntaxTokenType.EOF)
                    ErrorBag.ReportUnterminatedStatementBlock(Current.TextSpan);

                Match(SyntaxTokenType.RBrace);
            }
            else
            {
                var stmt = ParseStatement();
                if (stmt != null)
                    statements.Add(stmt);
            }
            
            return new StatementBlock(statements.ToArray());
        }

        private Statement ParseStatement()
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
                case SyntaxTokenType.ID:
                    if (Peek.Type == SyntaxTokenType.LPar)
                        return new ExpressionStatement(ParseFunctionCallExpression());
                    if (SyntaxFacts.AssignmentOperators.Contains(Peek.Type))
                        return ParseVariableAssignment();
                    break;
            }
            if (Current.Type != SyntaxTokenType.Illegal)
                ErrorBag.ReportUnexpectedToken(Current.TextSpan, Current.Type);
            return null;
        }

        // if <expression> <statement_block> [else <statement_block>]
        private Statement ParseIfStatement()
        {
            Match(SyntaxTokenType.If);
            var conditon = ParseExpression();
            var thenBlock = ParseStatementBlock();

            if (Current.Type == SyntaxTokenType.Else)
            {
                Match(SyntaxTokenType.Else);
                var elseBlock = ParseStatementBlock();
                return new IfStatement(conditon, thenBlock, elseBlock);
            }

            return new IfStatement(conditon, thenBlock);
        }

        // for id = <expression> (to|downto) <expression> <statement_block>
        private Statement ParseForStatement()
        {
            Match(SyntaxTokenType.For);
            var identifierToken = Match(SyntaxTokenType.ID);
            Match(SyntaxTokenType.Eq);
            var startValue = ParseExpression();
            var directionToken = Match(SyntaxTokenType.To, SyntaxTokenType.Downto);
            var targetValue = ParseExpression();
            var body = ParseStatementBlock();
            return new ForStatement(identifierToken, startValue, directionToken, targetValue, body);
        }

        // while <expression> <statement_block>
        private Statement ParseWhileStatement()
        {
            Match(SyntaxTokenType.While);
            var condition = ParseExpression();
            var body = ParseStatementBlock();
            return new WhileStatement(condition, body);
        }

        // do <statement_block> while <expression>
        private Statement ParseDoWhileStatement()
        {
            Match(SyntaxTokenType.Do);
            var body = ParseStatementBlock();
            Match(SyntaxTokenType.While);
            var condition = ParseExpression();
            return new DoWhileStatement(body, condition);
        }

        // (let|const) id = <expression>
        private VariableDeclaration ParseVariableDeclaration()
        {
            var keyword = Match(SyntaxTokenType.Let, SyntaxTokenType.Const);
            var id = Match(SyntaxTokenType.ID);
            Match(SyntaxTokenType.Eq);
            var value = ParseExpression();
            return new VariableDeclaration(keyword, id, value);
        }

        // id (=|+=|-=|*=|/=|%=|^=) <expression>
        private VariableAssignment ParseVariableAssignment()
        {
            var id = Match(SyntaxTokenType.ID);
            var op = Match(SyntaxFacts.AssignmentOperators);
            var value = ParseExpression();
            return new VariableAssignment(id, op, value);
        }

        // precedence climbing algorithm
        private Expression ParseExpression(int precedence = 0)
        {
            var left = ParsePrimaryExpression();

            while (SyntaxFacts.BinOpPrec.ContainsKey(Current.Type) && SyntaxFacts.BinOpPrec[Current.Type] >= precedence)
            {
                var op = NextToken();
                var right = ParseExpression(SyntaxFacts.BinOpPrec[op.Type] + 1);
                left = new BinaryExpression(left, op, right);
            }

            return left;
        }

        private Expression ParsePrimaryExpression()
        {
            switch (Current.Type)
            {
                case SyntaxTokenType.ID:
                    if (Peek.Type == SyntaxTokenType.LPar)
                        return ParseFunctionCallExpression();
                    return ParseVarableExpression();

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
                    ErrorBag.ReportInvalidExpressionTerm(Current.TextSpan, Current.Type);
                    return new ErrorExpression();
            }
        }

        private Expression ParseFunctionCallExpression()
        {
            var id = Match(SyntaxTokenType.ID);
            var parameters = new List<Argument>();
            
            var lPar = Match(SyntaxTokenType.LPar);

            while (Current.Type != SyntaxTokenType.RPar && Current.Type != SyntaxTokenType.EOF)
            {
                var value = ParseExpression();
                
                if (Current.Type == SyntaxTokenType.Comma)
                {
                    var comma = Match(SyntaxTokenType.Comma);
                    parameters.Add(new Argument(value, comma));
                }
                else
                {
                    parameters.Add(new Argument(value));
                }
            }

            var rPar = Match(SyntaxTokenType.RPar);

            return new FunctionCallExpression(id, lPar, parameters.ToArray(), rPar);
        }

        private Expression ParseVarableExpression()
        {
            var token = Match(SyntaxTokenType.ID);
            return new VariableExpression(token);
        }

        private Expression ParseIntLiteralExpression()
        {
            var token = Match(SyntaxTokenType.IntLiteral);
            if (int.TryParse(token.Value, out int i))
                return new LiteralExpression(token, i);
            ErrorBag.ReportUnparseableNumber(token.TextSpan, token.Value);
            return new LiteralExpression(token, token.Value);
        }

        private Expression ParseFloatLiteralExpression()
        {
            var token = Match(SyntaxTokenType.FloatLiteral);
            if (float.TryParse(token.Value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out float f))
                return new LiteralExpression(token, f);
            ErrorBag.ReportUnparseableNumber(token.TextSpan, token.Value);
            return new LiteralExpression(token, token.Value);
        }

        private Expression ParseStringLiteralExpression()
        {
            var token = Match(SyntaxTokenType.StringLiteral);
            return new LiteralExpression(token, token.Value);
        }

        private Expression ParseBoolLiteralExpression()
        {
            var token = Match(SyntaxTokenType.BoolLiteral);
            return new LiteralExpression(token, bool.Parse(token.Value));
        }

        private Expression ParseParenthesizedExpression()
        {
            var leftParen = Match(SyntaxTokenType.LPar);
            var expression = ParseExpression();
            var rightParen = Match(SyntaxTokenType.RPar);
            return new ParenthesizedExpression(leftParen, expression, rightParen);
        }

        private Expression ParseUnaryExpression()
        {
            var opToken = NextToken();
            var primary = ParsePrimaryExpression();
            return new UnaryExpression(opToken, primary);
        }

        private SyntaxToken Match(params SyntaxTokenType[] types)
        {
            var token = NextToken();
            if (types.Any(t => t == token.Type))
                return token;
            ErrorBag.ReportMissingExpectedToken(token.TextSpan, types);
            return new DummyToken(types[0]);
        }

        private SyntaxToken NextToken()
        {
            var current = Current;
            index = Math.Min(index + 1, tokens.Length - 1);
            return current;
        }
    }
}
