using System.Collections.Generic;

namespace Kostic017.Pigeon.Errors
{
    public class CodeErrorBag
    {
        readonly List<CodeError> errors;
        public CodeError[] Errors => errors.ToArray();

        internal CodeErrorBag()
        {
            errors = new List<CodeError>();
        }

        internal void Report(CodeErrorType errorType, TextSpan textSpan, params string[] data)
        {
            errors.Add(new CodeError(errorType, textSpan, data));
        }

        
    }
}
