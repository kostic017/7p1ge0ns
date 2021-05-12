using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Kostic017.Pigeon.Errors;
using System.IO;

namespace Kostic017.Pigeon
{
    public class Interpreter
    {
        private readonly IParseTree tree;
        private readonly PigeonParser parser;
        private readonly CodeErrorBag errorBag;

        public Interpreter(string code)
        {
            errorBag = new CodeErrorBag();
            var inputStream = new AntlrInputStream(code);
            var lexer = new PigeonLexer(inputStream);
            var tokenStream = new CommonTokenStream(lexer);
            parser = new PigeonParser(tokenStream);
            var errorListener = new CodeErrorListener(errorBag);
            parser.AddErrorListener(errorListener);
            tree = parser.program();
            var walker = new ParseTreeWalker();
            var analyzer = new SemanticAnalyser(errorBag);
            walker.Walk(analyzer, tree);
        }

        public void PrintTree(TextWriter writer)
        {
            tree.PrintTree(writer, parser.RuleNames);
        }

        public string ToStringTree()
        {
            return tree.ToStringTree();
        }

        public void PrintErrors(TextWriter writer)
        {
            foreach (var error in errorBag)
                writer.WriteLine(error.ToString());
        }

        //public static void Evaluate(AnalysisResult analysisResult)
        //{
        //    if (analysisResult.Errors.AllErrors.Length > 0)
        //        throw new IllegalUsageException("There were errors in this analysis result");
        //    var evaluator = new Evaluator(analysisResult.TypedAstRoot);
        //    evaluator.Evaluate();
        //}
    }

    class CodeErrorListener : BaseErrorListener
    {
        private readonly CodeErrorBag errorBag;

        public CodeErrorListener(CodeErrorBag errorBag)
        {
            this.errorBag = errorBag;
        }

        public override void SyntaxError([NotNull] IRecognizer recognizer, [Nullable] IToken offendingSymbol, int line, int charPositionInLine, [NotNull] string msg, [Nullable] RecognitionException e)
        {
            errorBag.Report(msg, new TextSpan(offendingSymbol.Line, offendingSymbol.Column, offendingSymbol.StartIndex, offendingSymbol.StopIndex));
        }
    }
}
