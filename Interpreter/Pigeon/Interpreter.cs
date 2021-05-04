namespace Kostic017.Pigeon
{
    public static class Interpreter
    {
/*
        public static AnalysisResult Analyze(string code, int tabSize = 4)
        {
            var lexer = new MyLexer(code, tabSize);
            var tokens = lexer.Lex();
            var parser = new MyParser(tokens);
            var ast = parser.Parse();
            var typeChecker = new TypeChecker(ast);
            var typedAst = typeChecker.Anaylize();
            var errorBag = new SyntaxErrorBag(lexer.ErrorBag, parser.ErrorBag, typeChecker.ErrorBag);
            return new AnalysisResult(tokens, ast, typedAst, errorBag);
        }

        public static void Evaluate(AnalysisResult analysisResult)
        {
            if (analysisResult.Errors.AllErrors.Length > 0)
                throw new IllegalUsageException("There were errors in this analysis result");
            var evaluator = new Evaluator(analysisResult.TypedAstRoot);
            evaluator.Evaluate();
        }
*/
    }
}
