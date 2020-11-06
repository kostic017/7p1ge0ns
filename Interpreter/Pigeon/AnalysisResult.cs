using Kostic017.Pigeon.Errors;
using Kostic017.Pigeon.TAST;

namespace Kostic017.Pigeon
{
    public class AnalysisResult
    {
        internal TypedProgram Program { get; }
        public CodeError[] Errors { get; }

        internal AnalysisResult(TypedProgram root, CodeError[] errors)
        {
            Program = root;
            Errors = errors;
        }
    }
}
