using Kostic017.Pigeon.Errors;
using Kostic017.Pigeon.TAST;

namespace Kostic017.Pigeon
{
    public class AnalysisResult
    {
        internal TypedProgram Root { get; }
        public CodeError[] Errors { get; }

        internal AnalysisResult(TypedProgram root, CodeError[] errors)
        {
            Root = root;
            Errors = errors;
        }
    }
}
