namespace Assets.Scripts.Editor
{
    public class Interpreter
    {
        private Scanner scanner;

        public void Exec(string code)
        {
            scanner.Scan(code);
        }
    }
}
