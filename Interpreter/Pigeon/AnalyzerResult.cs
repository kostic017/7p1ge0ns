using Kostic017.Pigeon.Error;
using Kostic017.Pigeon.TAST;

namespace Kostic017.Pigeon
{
    public class AnalyzerResult
    {
        internal TypedProgram Root { get; }
        public CodeError[] Errors { get; }

        internal AnalyzerResult(TypedProgram root, CodeError[] errors)
        {
            Root = root;
            Errors = errors;
        }
    }
}
