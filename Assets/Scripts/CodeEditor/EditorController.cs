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
    Vector2 textDim;
    List<string> code;

    void Start()
    {
        textDim = codeField.GetPreferredValues(".");
        code = codeField.text.Split('\n').ToList();
        caret.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, textDim.x);
        caret.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, textDim.y);
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
        caret.rectTransform.localPosition = new Vector2(
            column * textDim.x + caret.rectTransform.rect.width * 0.5f,
            codeField.rectTransform.rect.height - line * textDim.y - caret.rectTransform.rect.height * 0.5f
        );
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
