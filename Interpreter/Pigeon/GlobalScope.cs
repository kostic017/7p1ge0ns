using Kostic017.Pigeon.Error;
using Kostic017.Pigeon.TAST;

namespace Kostic017.Pigeon
{
    public class GlobalScope
    {
        internal TypedProgram Root { get; }
        public CodeError[] Errors { get; }

        internal GlobalScope(TypedProgram root, CodeError[] errors)
        {
            Root = root;
            Errors = errors;
        }
    }
}
