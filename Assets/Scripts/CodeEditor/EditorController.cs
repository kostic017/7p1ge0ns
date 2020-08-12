using System.Linq;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EditorController : MonoBehaviour
{
    public Image caret;
    public TMP_Text codeField;
    public TMP_Text gutterField;

    int line;
    int column;
    List<string> code;

    void Start()
    {
        code = codeField.text.Split('\n').ToList();        
        UpdateLineNumbers();
    }

    void Update()
    {
        HandleTextInput();
        HandleSpecialInput();
        PositionCaret();
        UpdateLineNumbers();
        codeField.text = string.Join("\n", code);
    }

    void HandleTextInput()
    {
        string input = Input.inputString;
        foreach (char c in input)
        {
            
            if (c == '\b')
            {
                if (column > 0)
                {
                    code[line] = code[line].Remove(column - 1, 1);
                    --column;
                }
                else if (line > 0)
                {
                    code[line - 1] += code[line];
                    code.RemoveAt(line);
                    --line;
                }
            }
            else
            {
                if (c == '\n' || c == '\r')
                {
                    code.Insert(line, "");
                    ++line;
                }
                else
                {
                    InsertChar(c);
                }
                ++column;
            }
        }
    }

    void HandleSpecialInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (column > 0)
            {
                --column;
            }
            else
            {
                column = code[line - 1].Length - 1;
                --line;
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (column < code[line].Length - 1)
            {
                ++column;
            }
            else
            {
                column = 0;
                ++line;
            }
        }
    }

    void InsertChar(char c)
    {
        if (column < code[line].Length)
        {
            code[line] = code[line].Insert(column, c.ToString());
        }
        else
        {
            code[line] += c;
        }
    }

    void PositionCaret()
    {
        codeField.text = code[line].Substring(0, column).Replace(' ', '.');
        float caretX = codeField.preferredWidth;

        codeField.text = string.Join("\n", code.Take(line + 1)).TrimEnd('\n');
        float caretY = codeField.preferredHeight;

        caret.rectTransform.localPosition = new Vector2(caretX, codeField.rectTransform.rect.height - caretY);
    }

    void UpdateLineNumbers()
    {
        string text = "";
        for (int i = 1; i <= code.Count; ++i)
        {
            text += i + "\n";
        }
        gutterField.text = text;
    }
}
