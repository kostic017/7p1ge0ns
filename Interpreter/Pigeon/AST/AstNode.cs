namespace Kostic017.Pigeon.AST
{
    public abstract class AstNode
    {
        public abstract string AsString(int ident = 0);

        protected string Ident(int ident)
        {
            return new string(' ', ident * 4);
        }
    }
}
