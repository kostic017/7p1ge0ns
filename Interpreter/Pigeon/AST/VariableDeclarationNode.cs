﻿using System.Collections.Generic;

namespace Kostic017.Pigeon.AST
{
    internal class VariableDeclarationNode : StatementNode
    {
        internal SyntaxToken Keyword { get; }
        internal SyntaxToken Name { get; }
        internal ExpressionNode Value { get; }

        internal VariableDeclarationNode(SyntaxToken type, SyntaxToken name)
        {
            Keyword = type;
            Name = name;
        }

        internal VariableDeclarationNode(SyntaxToken type, SyntaxToken name, ExpressionNode value)
        {
            Keyword = type;
            Name = name;
            Value = value;
        }

        internal override AstNodeKind Kind => AstNodeKind.VariableDeclaration;

        internal override IEnumerable<AstNode> GetChildren()
        {
            yield return new SyntaxTokenWrap(Keyword);
            yield return new SyntaxTokenWrap(Name);
            yield return Value;
        }
    }
}
