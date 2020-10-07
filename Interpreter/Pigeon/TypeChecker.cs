using Kostic017.Pigeon.AST;
using Kostic017.Pigeon.Error;
using Kostic017.Pigeon.TAST;
using System;

namespace Kostic017.Pigeon
{
    class TypeChecker
    {
        internal CodeErrorBag ErrorBag { get; }

        internal TypeChecker()
        {
            ErrorBag = new CodeErrorBag();
        }

        public TypedExpression BindExpression(Expression node)
        {
            switch (node.Kind)
            {
                case NodeKind.LiteralExpression:
                    return BindLiteralExpression((LiteralExpression) node);
                case NodeKind.UnaryExpression:
                    return BindUnaryExpression((UnaryExpression) node);
                case NodeKind.BinaryExpression:
                    return BindBinaryExpression((BinaryExpression) node);
                case NodeKind.ParenthesizedExpression:
                    return BindParenthesizedExpression((ParenthesizedExpression) node);
            }
            throw new NotImplementedException();
        }

        private TypedExpression BindLiteralExpression(LiteralExpression node)
        {
            return new TypedLiteralExpression(node.ParsedValue ?? 0);
        }

        private TypedExpression BindUnaryExpression(UnaryExpression node)
        {
            var value = BindExpression(node.Value);
            var typedOperator = TypedUnaryOperator.Bind(node.Op.Type, value.Type);
            
            if (typedOperator == null)
            {
                ErrorBag.Report(CodeErrorType.UNARY_OPERAND_INVALID_TYPE, node.Op.TextSpan, node.Op.Type.GetDescription(), value.Type.ToString());
                return value; // to avoid null checks later on
            }
            
            return new TypedUnaryExpression(typedOperator, value);
        }

        private TypedExpression BindBinaryExpression(BinaryExpression node)
        {
            var left = BindExpression(node.Left);
            var right = BindExpression(node.Right);
            var typedOperator = TypedBinaryOperator.Bind(node.Op.Type, left.Type, right.Type);

            if (typedOperator == null)
            {
                ErrorBag.Report(CodeErrorType.BINARY_OPERAND_INVALID_TYPE, node.Op.TextSpan, node.Op.Type.GetDescription(), left.Type.ToString(), right.Type.ToString());
                return left;
            }

            return new TypedBinaryExpression(left, typedOperator, right);
        }

        private TypedExpression BindParenthesizedExpression(ParenthesizedExpression node)
        {
            return BindExpression(node.Expression);
        }
    }
}
