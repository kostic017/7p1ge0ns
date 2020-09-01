using Kostic017.Pigeon.AST;
using System.Collections.Generic;

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

        internal Parser(SyntaxToken[] tokens)
        {
            index = 0;
            this.tokens = tokens;
            Errors = new List<CodeError>();
        }

        internal AstNode Parse()
        {
            return ParseExpression();
        }

        /// <summary>
        /// Parses expressions by precedence climbing.
        /// </summary>
        ExpressionNode ParseExpression(int precedence = 0)
        {
            ExpressionNode left = ParsePrimaryExpression();

            while (binOps.ContainsKey(Current.Type) && binOps[Current.Type].precedence >= precedence)
            {
                SyntaxTokenType op = Current.Type;
                NextToken();
                ExpressionNode right = ParseExpression(binOps[op].precedence + (binOps[op].leftAssoc ? 1 : 0));
                left = new BinaryExpressionNode(left, op, right);
            }

            return left;
        }

        ExpressionNode ParsePrimaryExpression()
        {
            switch (Current.Type)
            {
                case SyntaxTokenType.ID:
                    return new IdExpressionNode(NextToken().Value.ToString());

                case SyntaxTokenType.IntLiteral:
                case SyntaxTokenType.FloatLiteral:
                case SyntaxTokenType.StringLiteral:
                case SyntaxTokenType.BoolLiteral:
                    {
                        SyntaxToken token = NextToken();
                        return new LiteralExpressionNode(token.Type, token.Value);
                    }

                case SyntaxTokenType.LPar:
                    {
                        NextToken();
                        ExpressionNode expression = ParseExpression();
                        Match(SyntaxTokenType.RPar);
                        return new ParenthesizedExpressionNode(expression);
                    }

                case SyntaxTokenType.Minus:
                case SyntaxTokenType.Plus:
                case SyntaxTokenType.Not:
                    {
                        SyntaxToken op = NextToken();
                        ExpressionNode primary = ParsePrimaryExpression();
                        return new UnaryExpressionNode(op.Type, primary);
                    }

                default:
                    ReportError(CodeErrorType.INVALID_EXPRESSION_TERM, Current.Type.PrettyPrint());
                    return new LiteralExpressionNode(SyntaxTokenType.Illegal, "");
            }
        }

        SyntaxToken Match(SyntaxTokenType type)
        {
            if (Current.Type != type)
            {
                ReportError(CodeErrorType.EXPECTED_TOKEN, type.PrettyPrint());
                return new SyntaxToken(type, -1, -1, -1, -1); // return dummy token to avoid null checks later on
            }

            return NextToken();
        }
        
        SyntaxToken NextToken()
        {
            SyntaxToken current = Current;
            ++index;
            return current;
        }

        void ReportError(CodeErrorType type, params string[] data)
        {
            Errors.Add(new CodeError(type, Current.StartLine, Current.StartColumn, data));
        }
    }
}
