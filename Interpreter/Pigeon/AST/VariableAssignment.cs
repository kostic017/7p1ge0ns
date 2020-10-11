﻿using System.Collections.Generic;

namespace Kostic017.Pigeon.AST
{
    class VariableAssignment : Statement
    {
        internal SyntaxToken NameToken { get; }
        internal SyntaxToken Op { get; }
        internal Expression Value { get; }

        internal VariableAssignment(SyntaxToken name, SyntaxToken op, Expression value)
        {
            NameToken = name;
            Op = op;
            Value = value;
        }

        internal override NodeKind Kind => NodeKind.VariableAssignment;

        internal override IEnumerable<AstNode> GetChildren()
        {
            yield return new SyntaxTokenWrap(NameToken);
            yield return new SyntaxTokenWrap(Op);
            yield return Value;
        }
    }
}
