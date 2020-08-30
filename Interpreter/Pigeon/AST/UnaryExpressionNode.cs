﻿namespace Kostic017.Pigeon.AST
{
    class UnaryExpressionNode : ExpressionNode
    {
        internal SyntaxTokenType Op { get; }
        internal ExpressionNode Value { get; }

        internal UnaryExpressionNode(SyntaxTokenType op, ExpressionNode value)
        {
            Op = op;
            Value = value;
        }

        public override string AsString(int ident = 0)
        {
            return "(" + Op.PrettyPrint() + " " + Value.AsString() + ")";
        }
    }
}