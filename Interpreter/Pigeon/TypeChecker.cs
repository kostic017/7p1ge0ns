﻿using Kostic017.Pigeon.AST;
using Kostic017.Pigeon.TAST;
using System;
using System.Collections.Generic;

namespace Kostic017.Pigeon
{
    class TypeChecker
    {
        internal List<CodeError> Errors { get; }

        internal TypeChecker()
        {
            Errors = new List<CodeError>();
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
            var top = TypedUnaryOperator.Bind(node.Op.Type, value.Type);
            
            if (top == null)
            {
                ReportError(CodeErrorType.UNARY_OPERAND_INVALID_TYPE, node.Op, node.Op.Type.GetDescription(), value.Type.ToString());
                return value; // to avoid null checks later on
            }
            
            return new TypedUnaryExpression(top.Op, value);
        }

        private TypedExpression BindBinaryExpression(BinaryExpression node)
        {
            var left = BindExpression(node.Left);
            var right = BindExpression(node.Right);
            var typedOperator = TypedBinaryOperator.Bind(node.Op.Type, left.Type, right.Type);

            if (typedOperator == null)
            {
                ReportError(CodeErrorType.BINARY_OPERAND_INVALID_TYPE, node.Op, node.Op.Type.GetDescription(), left.Type.ToString(), right.Type.ToString());
                return left;
            }

            return new TypedBinaryExpression(left, typedOperator.Op, right, typedOperator.ResultType);
        }

        private void ReportError(CodeErrorType type, SyntaxToken token, params string[] data)
        {
            Errors.Add(new CodeError(type, token.StartLine, token.StartColumn, data));
        }
    }
}
