using System.Linq;

namespace Kostic017.Pigeon.Errors
{
    public class SyntaxErrorBag
    {
        internal CodeError[] LexerErrors { get; }
        internal CodeError[] ParserErrors { get; }
        internal CodeError[] TypeCheckerErrors { get; }

        public CodeError[] AllErrors { get; }

        internal SyntaxErrorBag(CodeErrorBag lexerBag, CodeErrorBag parserBag, CodeErrorBag typeCheckerBag)
        {
            LexerErrors = lexerBag.Errors;
            ParserErrors = parserBag.Errors;
            TypeCheckerErrors = typeCheckerBag.Errors;
            AllErrors = LexerErrors.Concat(ParserErrors).Concat(TypeCheckerErrors).ToArray();
        }
    }
}
