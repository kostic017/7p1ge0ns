namespace Kostic017.Pigeon
{
    class DummyToken : SyntaxToken
    {
        public DummyToken(SyntaxTokenType type) : base(type, new TextSpan(-1, -1, -1, -1))
        {       
        }
    }
}
