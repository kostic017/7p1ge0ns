﻿using System.Collections.Generic;
using System.Linq;
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
    int lastColumn;
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
        string input = Input.inputString; // the text that the pressed keys would write into a text field
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
                    column = code[line - 1].Length;
                    code[line - 1] += code[line];
                    code.RemoveAt(line);
                    --line;
                }
            }
            else
            {
                if (c == '\n' || c == '\r')
                {
                    if (column < code[line].Length)
                    {
                        code.Insert(line + 1, code[line].Substring(column));
                        code[line] = code[line].Remove(column);
                    }
                    else
                    {
                        code.Insert(line + 1, "");
                    }
                    ++line;
                    column = 0;
                }
                else
                {
                    InsertChar(c);
                    ++column;
                }
            }
            lastColumn = column;
        }
    }

    void HandleSpecialInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (column > 0)
            {
                --column;
                lastColumn = column;
            }
            else if (line > 0)
            {
                --line;
                column = lastColumn = code[line].Length;
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (column < code[line].Length)
            {
                ++column;
                lastColumn = column;
            }
            else if (line < code.Count - 1)
            {
                ++line;
                column = lastColumn = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (line > 0)
            {
                --line;
                if (lastColumn > code[line].Length)
                {
                    column = code[line].Length;
                }
                else
                {
                    column = lastColumn;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (line < code.Count - 1)
            {
                ++line;
                if (lastColumn > code[line].Length)
                {
                    column = code[line].Length;
                }
                else
                {
                    column = lastColumn;
                }
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
        // '.' is there because empty lines and spaces are ignored from calculations
        string linesUpToCaret = string.Join("\n.", code.Take(line + 1)) + ".";
        string charsUpToCaret = code[line].Substring(0, column).Replace(' ', '.');

        codeField.text = charsUpToCaret;
        float caretX = codeField.preferredWidth;

        codeField.text = linesUpToCaret;
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
