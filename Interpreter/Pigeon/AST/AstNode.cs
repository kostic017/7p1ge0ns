using System.Collections.Generic;
using System.Linq;

namespace Kostic017.Pigeon.AST
{
    abstract class AstNode
    {
        /// <summary>
        /// Returns text span information about a node. It's most useful with expressions.
        /// Other nodes are "abstract" and don't contain all related tokens as children,
        /// so text span would not cover the entire node.
        /// </summary>
        internal virtual TextSpan TextSpan
        {
            get
            {
                var first = GetChildren().First().TextSpan;
                var last = GetChildren().Last().TextSpan;
                return new TextSpan(first.Line, first.Column, first.StartIndex, last.EndIndex);
            }
        }

        internal abstract NodeKind Kind { get; }
        internal abstract IEnumerable<AstNode> GetChildren();
    }
}
