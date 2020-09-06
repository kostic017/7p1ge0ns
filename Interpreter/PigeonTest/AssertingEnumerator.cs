using Kostic017.Pigeon.AST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Xunit;

namespace Kostic017.Pigeon.Tests
{
    class AssertingEnumerator : IDisposable
    {
        bool assertEmpty;

        readonly IEnumerator<AstNode> enumerator;

        internal AssertingEnumerator(AstNode root, bool assertEmpty = true)
        {
            this.assertEmpty = assertEmpty;
            enumerator = Flatten(root).GetEnumerator();
        }

        internal void AssertNode(SyntaxNodeKind kind)
        {
            try
            {
                Assert.True(enumerator.MoveNext());
                Assert.Equal(kind, enumerator.Current.Kind);
            }
            catch when (MarkFailed()) // we don't really want to catch the exception
            {
            }
        }

        internal void AssertToken(SyntaxTokenType type, string value = null)
        {
            try
            {
                Assert.True(enumerator.MoveNext());
                var tokenWrap = Assert.IsType<SyntaxTokenWrap>(enumerator.Current);
                Assert.Equal(type, tokenWrap.Token.Type);
                Assert.Equal(value, tokenWrap.Token.Value);
            }
            catch when (MarkFailed())
            {
            }
        }

        internal AstNode GetNext()
        {
            Assert.True(enumerator.MoveNext());
            return enumerator.Current;
        }

        bool MarkFailed()
        {
            assertEmpty = false; // that assert would mask the original cause of failure
            return false;
        }

        public void Dispose()
        {
            if (assertEmpty)
            {
                Assert.False(enumerator.MoveNext());
            }
            enumerator.Dispose();
        }

        static IEnumerable<AstNode> Flatten(AstNode root)
        {
            var stack = new Stack<AstNode>();
            stack.Push(root);
            while (stack.Count > 0)
            {
                var node = stack.Pop();
                yield return node;
                foreach (var child in node.GetChildren().Reverse())
                {
                    stack.Push(child);
                }
            }
        }
    }
}
