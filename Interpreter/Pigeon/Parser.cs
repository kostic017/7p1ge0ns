﻿using Kostic017.Pigeon.AST;
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
                case SyntaxTokenType.LBrace:
                    return ParseStatementBlock();
                case SyntaxTokenType.Let:
                case SyntaxTokenType.Const:
                    return ParseVariableDeclaration();
                default:
                    return ParseExpressionStatement();
            }
        }

        // (let|const) id = <expression>
        VariableDeclarationNode ParseVariableDeclaration()
        {
            var keyword = NextToken();
            var id = Match(SyntaxTokenType.ID);
            Match(SyntaxTokenType.Assign);
            var value = ParseExpression();
            Match(SyntaxTokenType.Semicolon);
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

        /// <summary>
        /// Parses expressions by precedence climbing.
        /// </summary>
        ExpressionNode ParseExpression(int precedence = 1)
        {
            var left = ParsePrimaryExpression();

            while (GetOperatorPrecedence(Current.Type) > 0 && GetOperatorPrecedence(Current.Type) >= precedence)
            {
                var op = NextToken();
                var right = ParseExpression(GetOperatorPrecedence(op.Type) + IsLeftAssoc(op.Type));
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
                    ReportError(CodeErrorType.INVALID_EXPRESSION_TERM, Current.Type.PrettyPrint());
                    return new LiteralExpressionNode(DummyToken(SyntaxTokenType.Illegal), "");
            }
        }

        ExpressionNode ParseIdExpression()
        {
            var token = Match(SyntaxTokenType.ID);
            return new IdExpressionNode(token);
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
            var token = Match(SyntaxTokenType.IntLiteral);
            
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
                ReportError(CodeErrorType.EXPECTED_TOKEN, type.PrettyPrint());
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
        /// Fabricates tokens so we could avoid null checks later on.
        /// </summary>
        SyntaxToken DummyToken(SyntaxTokenType type)
        {
            return new SyntaxToken(type, -1, -1, -1, -1);
        }

        int GetOperatorPrecedence(SyntaxTokenType op)
        {
            switch (op)
            {
                case SyntaxTokenType.And:
                case SyntaxTokenType.Or:
                    return 1;
                case SyntaxTokenType.Eq:
                case SyntaxTokenType.Neq:
                    return 2;
                case SyntaxTokenType.Lt:
                case SyntaxTokenType.Gt:
                case SyntaxTokenType.Leq:
                case SyntaxTokenType.Geq:
                    return 3;
                case SyntaxTokenType.Plus:
                case SyntaxTokenType.Minus:
                    return 4;
                case SyntaxTokenType.Mul:
                case SyntaxTokenType.Div:
                case SyntaxTokenType.Mod:
                    return 5;
                case SyntaxTokenType.Power:
                    return 6;
                default:
                    return 0;
            }
        }

        int IsLeftAssoc(SyntaxTokenType op)
        {
            return op != SyntaxTokenType.Power ? 1 : 0;
        }

        void ReportError(CodeErrorType type, params string[] data)
        {
            Errors.Add(new CodeError(type, Current.StartLine, Current.StartColumn, data));
        }
    }
}
