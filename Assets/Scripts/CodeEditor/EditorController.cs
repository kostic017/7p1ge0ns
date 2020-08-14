using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EditorController : MonoBehaviour
{
    public int tabSize = 4;
    
    public float blinkRate = 0.7f;
    public float lastInputRate = 0.05f;
    
    public Image caret;

    public RectTransform codeEditor;

    public TMP_Text codeTextField;
    public TMP_Text gutterTextField;
    public TMP_Text consoleOutput;

    int line;
    int column;
    int lastColumn;
    
    float blinkTimer;
    float lastInputTime;
    
    List<string> code;

    Scanner scanner;
    InputManager inputManager;
    SyntaxHighlighter highlighter;

    void Start()
    {
        scanner = new Scanner();
        inputManager = GetComponent<InputManager>();
        highlighter = GetComponent<SyntaxHighlighter>();
        code = codeTextField.text.Split('\n').ToList();        
        UpdateLineNumbers();
    }

    void Update()
    {
        HandleTextInput();
        HandleSpecialInput();
        PositionCaret();
        Scroll();
        BlinkCaret();
        UpdateLineNumbers();

        consoleOutput.text = "";
        string codeText = string.Join("\n", code);

        try
        {
            Token[] tokens = scanner.Scan(codeText);
            codeText = highlighter.Highlight(codeText, tokens);
        } catch (ScannerException e)
        {
            consoleOutput.text = e.Message;
        }


        codeTextField.text = codeText;
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
                    lastInputTime = Time.time;
                    code[line] = code[line].Remove(column - 1, 1);
                    --column;
                }
                else if (line > 0)
                {
                    lastInputTime = Time.time;
                    column = code[line - 1].Length;
                    code[line - 1] += code[line];
                    code.RemoveAt(line);
                    --line;
                }
            }
            else if (c == '\n' || c == '\r')
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
                lastInputTime = Time.time;
            }
            else
            {
                lastInputTime = Time.time;

                if (column < code[line].Length)
                {
                    code[line] = code[line].Insert(column, c.ToString());
                }
                else
                {
                    code[line] += c;
                }

                ++column;
            }
            lastColumn = column;
        }
    }

    void HandleSpecialInput()
    {
        if (inputManager.CheckKey(KeyCode.LeftArrow))
        {
            if (column > 0)
            {
                --column;
                lastColumn = column;
                lastInputTime = Time.time;
            }
            else if (line > 0)
            {
                --line;
                column = lastColumn = code[line].Length;
                lastInputTime = Time.time;
            }
        }
        else if (inputManager.CheckKey(KeyCode.RightArrow))
        {
            if (column < code[line].Length)
            {
                ++column;
                lastColumn = column;
                lastInputTime = Time.time;
            }
            else if (line < code.Count - 1)
            {
                ++line;
                column = lastColumn = 0;
                lastInputTime = Time.time;
            }
        }
        else if (inputManager.CheckKey(KeyCode.UpArrow))
        {
            if (line > 0)
            {
                --line;
                lastInputTime = Time.time;
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
        else if (inputManager.CheckKey(KeyCode.DownArrow))
        {
            if (line < code.Count - 1)
            {
                ++line;
                lastInputTime = Time.time;
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
        else if (inputManager.CheckKey(KeyCode.Delete))
        {
            if (column < code[line].Length)
            {
                code[line] = code[line].Remove(column, 1);
                lastInputTime = Time.time;
            }
            else if (line < code.Count - 1)
            {
                code[line] += code[line + 1];
                code.RemoveAt(line + 1);
                lastInputTime = Time.time;
            }
        }
        else if (inputManager.CheckKey(KeyCode.Tab))
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                if (code[line].StartsWith(new string(' ', tabSize)))
                {
                    code[line] = code[line].Remove(0, tabSize);
                    column -= tabSize;
                }
            }
            else
            {
                code[line] = code[line].Insert(column, new string(' ', tabSize));
                column += tabSize;
            }
            lastInputTime = Time.time;
        }
    }

    void PositionCaret()
    {
        // '.' is there because empty lines and spaces are ignored from calculations
        string linesUpToCaret = string.Join("\n.", code.Take(line + 1)) + ".";
        string charsUpToCaret = code[line].Substring(0, column).Replace(' ', '.');

        codeTextField.text = charsUpToCaret;
        float caretX = codeTextField.preferredWidth;

        codeTextField.text = linesUpToCaret;
        float caretY = codeTextField.preferredHeight;

        caret.rectTransform.position = codeTextField.rectTransform.position;
        caret.rectTransform.localPosition += Vector3.right * caretX;
        caret.rectTransform.localPosition += Vector3.up * (codeTextField.rectTransform.rect.height - caretY);
    }

    void Scroll()
    {
        if (caret.rectTransform.position.y < consoleOutput.rectTransform.rect.height)
        {
            codeEditor.position += Vector3.up * caret.rectTransform.rect.height;
        }
        if (caret.rectTransform.position.y > Screen.height)
        {
            codeEditor.position += Vector3.down * caret.rectTransform.rect.height;
        }
    }

    void BlinkCaret()
    {
        blinkTimer += Time.deltaTime;
        if (Time.time - lastInputTime < lastInputRate)
        {
            blinkTimer = 0;
            caret.enabled = true;    
        }
        else if (blinkTimer >= blinkRate)
        {
            blinkTimer = 0;
            caret.enabled = !caret.enabled;
        }
    }

    void UpdateLineNumbers()
    {
        string text = "";
        for (int i = 1; i <= code.Count; ++i)
        {
            text += i + "\n";
        }
        gutterTextField.text = text;
    }
}
