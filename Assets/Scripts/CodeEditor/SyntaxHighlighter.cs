using UnityEngine;

public class SyntaxHighlighter : MonoBehaviour
{
    public Color testColor;

    private Scanner scanner;

    void Start()
    {
        scanner = new Scanner();
    }

    public string Highlight(string code)
    {
        Token[] tokens = scanner.Scan(code);
        // note that iterations are from last to first
        for (int i = tokens.Length - 1; i >= 0; --i)
        {
            string color = ColorUtility.ToHtmlStringRGB(testColor);
            code = code.Insert(tokens[i].EndIndex, "</color>")
                       .Insert(tokens[i].StartIndex - 1, $"<color=#{color}>");
        }
        return code;
    }
}
