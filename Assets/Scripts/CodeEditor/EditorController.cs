using System.Linq;
using TMPro;
using UnityEngine;

public class EditorController : MonoBehaviour
{
    public TMP_Text gutter;
    public TMP_Text consoleOutput;
    public TMP_InputField codeField;

    string prevCode;
    Scanner scanner;
    SyntaxHighlighter highlighter;

    void Start()
    {
        codeField.textComponent.enableWordWrapping = false;
        scanner = new Scanner(codeField.fontAsset.tabSize);
        codeField.ActivateInputField();
        highlighter = GetComponent<SyntaxHighlighter>();
    }

    void Update()
    {
        string code = codeField.text;

        if (code == prevCode) return;
        prevCode = code;

        code = highlighter.StripTags(code);
       
        UpdateLineNumbers(code);

        consoleOutput.text = "";

        Token[] tokens = scanner.Scan(code);

        code = highlighter.Highlight(code, tokens);
        
        if (scanner.Errors.Count > 0)
        {
            consoleOutput.text = scanner.Errors[0].Message();
            if (scanner.Errors.Count > 1)
            {
                consoleOutput.text += $" (and {scanner.Errors.Count - 1} more)";
            }
        }

        // inserting rich text while the player is typing used to
        // result in caret being positioned inside rich text tags
        int pos = codeField.caretPosition;
        codeField.text = code;
        //codeField.textComponent.ForceMeshUpdate();
        codeField.caretPosition = pos;
    }

    void UpdateLineNumbers(string code)
    {
        string text = "";
        int lines = code.Count(c => c == '\n') + 1;
        
        for (int i = 1; i <= lines; ++i)
        {
            text += $"{i}\n";
        }

        gutter.text = text;
    }
}
