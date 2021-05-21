namespace Kostic017.Pigeon.Symbols
{
    class FunctionScope
    {
        private Scope scope;

        internal FunctionScope(GlobalScope globalScope)
        {
            scope = new Scope(globalScope);
        }

        internal void Assign(string name, object value, PigeonType type = null)
        {
            if (type != null && !scope.TryGetVariable(name, out _))
                scope.DeclareVariable(type, name);
            scope.Assign(name, value);
        }

        internal object Evaluate(string name)
        {
            return scope.Evaluate(name);
        }

        internal void EnterScope()
        {
            scope = new Scope(scope);
        }

        internal void ExitScope()
        {
            scope = scope.Parent;
        }
    }
}
