using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Kostic017.Pigeon.Errors;
using Kostic017.Pigeon.Symbols;
using System.IO;

namespace Kostic017.Pigeon
{
    public delegate object FuncPointer(params object[] arguments);

    public class Interpreter
    {
        private readonly IParseTree tree;
        private readonly PigeonParser parser;
        private readonly CodeErrorBag errorBag;

        public Interpreter(string code, BuiltinSymbols builtinSymbols)
        {
            errorBag = new CodeErrorBag();

            var inputStream = new AntlrInputStream(code);
            var lexer = new PigeonLexer(inputStream);
            var tokenStream = new CommonTokenStream(lexer);
            parser = new PigeonParser(tokenStream);
            var errorListener = new CodeErrorListener(errorBag);
            parser.AddErrorListener(errorListener);
            tree = parser.program();

            var analyzer = new SemanticAnalyser(errorBag, builtinSymbols);
            var walker = new ParseTreeWalker();
            walker.Walk(analyzer, tree);
        }

        public void PrintTree(TextWriter writer)
        {
            tree.PrintTree(writer, parser.RuleNames);
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
}
