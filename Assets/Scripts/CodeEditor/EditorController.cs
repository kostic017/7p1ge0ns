using TMPro;
using UnityEngine;

public class EditorController : MonoBehaviour
{
    public TMP_Text codeField;
    public TMP_Text gutterField;

    private string code;
    private int charIndex;
    private int lineIndex;

    void Start()
    {
        code = codeField.text;
        UpdateLineNumbers();
    }

    void Update()
    {
        HandleTextInput();
        UpdateLineNumbers();
        codeField.text = code;
    }

    void HandleTextInput()
    {
        string input = Input.inputString;
        foreach (char c in input)
        {
            
            if (c == '\b')
            {
                if (charIndex > 0)
                {
                    char deletedChar = code[charIndex - 1];
                    code = code.Remove(charIndex - 1, 1);
                    --charIndex;
                    if (deletedChar == '\n')
                    {
                        --lineIndex;
                    }
                }
            }
            else
            {
                if (c == '\n' || c == '\r')
                {
                    ++lineIndex;
                    InsertChar('\n');
                }
                else
                {
                    InsertChar(c);
                }
                ++charIndex;
            }
        }
    }

    void InsertChar(char c)
    {
        if (string.IsNullOrEmpty(code) || charIndex == code.Length)
        {
            code += c;
        }
        else
        {
            code = code.Insert(charIndex, c.ToString());
        }
    }

    void UpdateLineNumbers()
    {
        int lines = codeField.text.Split('\n').Length;

        string text = "";
        for (int i = 1; i <= lines; ++i)
        {
            text += i + "\n";
        }

        gutterField.text = text;
    }
}
