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
        SyntaxToken[] tokens;
        List<CodeError> errors;

        SyntaxToken Current => index < tokens.Length ? tokens[index] : tokens[tokens.Length - 1];

        internal AstNode Parse(SyntaxToken[] tokens, List<CodeError> errors)
        {
            index = 0;
            this.errors = errors;
            this.tokens = tokens;
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
                EatCurrentToken();
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
                    return new IdExpressionNode(EatCurrentToken().Value.ToString());

                case SyntaxTokenType.IntLiteral:
                case SyntaxTokenType.FloatLiteral:
                case SyntaxTokenType.StringLiteral:
                case SyntaxTokenType.BoolLiteral:
                    {
                        SyntaxToken token = EatCurrentToken();
                        return new LiteralExpressionNode(token.Type, token.Value);
                    }

                case SyntaxTokenType.LPar:
                    {
                        EatCurrentToken();
                        ExpressionNode expression = ParseExpression();
                        Match(SyntaxTokenType.RPar);
                        return new ParenthesizedExpressionNode(expression);
                    }

                case SyntaxTokenType.Minus:
                case SyntaxTokenType.Plus:
                case SyntaxTokenType.Not:
                    {
                        SyntaxToken op = EatCurrentToken();
                        ExpressionNode primary = ParsePrimaryExpression();
                        return new UnaryExpressionNode(op.Type, primary);
                    }

                default:
                    Error(CodeErrorType.UNEXPECTED_TOKEN, Current.Type.PrettyPrint());
                    return new LiteralExpressionNode(SyntaxTokenType.Illegal, "");
            }
        }

        SyntaxToken Match(SyntaxTokenType type)
        {
            SyntaxToken token = tokens[index];

            if (token.Type != type)
            {
                Error(CodeErrorType.EXPECTED_OTHER_TOKEN, type.PrettyPrint(), token.Type.PrettyPrint());
                return new SyntaxToken(type);
            }

            return EatCurrentToken();
        }

        SyntaxToken EatCurrentToken()
        {
            SyntaxToken current = Current;
            ++index;
            return current;
        }

        void Error(CodeErrorType type, params string[] data)
        {
            errors.Add(new CodeError(type, Current.StartLine, Current.StartColumn, data));
            if (Current.ErrorIndex == -1)
            {
                Current.ErrorIndex = errors.Count - 1;
            }
        }
    }
}
