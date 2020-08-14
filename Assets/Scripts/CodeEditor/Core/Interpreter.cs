class Interpreter
{
    private readonly Scanner scanner;

    public Interpreter()
    {
        this.scanner = new Scanner();
    }

    public void Exec(string code)
    {
        scanner.Scan(code);
    }
}
