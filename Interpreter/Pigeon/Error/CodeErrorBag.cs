using System.Collections.Generic;

namespace Kostic017.Pigeon.Error
{
    public class CodeErrorBag
    {
        public List<CodeError> Errors { get; }

        public CodeErrorBag()
        {
            Errors = new List<CodeError>();
        }

        internal void Report(CodeErrorType errorType, TextSpan textSpan, params string[] data)
        {
            Errors.Add(new CodeError(errorType, textSpan, data));
        }
    }
}
