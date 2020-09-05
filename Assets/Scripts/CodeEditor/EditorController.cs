﻿using Kostic017.Pigeon;
using System.Linq;
using TMPro;
using UnityEngine;

public class EditorController : MonoBehaviour
{
    public TMP_Text gutter;
    public TMP_Text consoleOutput;
    public TMP_InputField textBox;

    string prevCode;

    SyntaxHighlighter highlighter;

    void Start()
    {
        highlighter = GetComponent<SyntaxHighlighter>();
        textBox.textComponent.enableWordWrapping = false;
        textBox.ActivateInputField();
    }

    void OnEnable()
    {
        textBox.verticalScrollbar.onValueChanged.AddListener(OnScrollValueChanged);
    }

    void OnDisable()
    {
        textBox.verticalScrollbar.onValueChanged.RemoveListener(OnScrollValueChanged);
    }

    void Update()
    {
        string code = highlighter.StripTags(textBox.text);

        if (code == prevCode) return;
        prevCode = code;

        UpdateLineNumbers(code);

        var syntaxTree = SyntaxTree.Parse(code, textBox.fontAsset.tabSize);

        code = highlighter.Highlight(code, syntaxTree.Tokens);

        UpdateErrorConsole(syntaxTree.Errors);

        // inserting rich text while the player is typing used to result in caret being positioned inside rich text tags
        int caret = textBox.caretPosition;
        textBox.text = code;
        textBox.caretPosition = caret;
    }

    void UpdateErrorConsole(CodeError[] errors)
    {
        consoleOutput.text = "";

        if (errors.Length > 0)
        {
            consoleOutput.text = errors[0].DetailedMessage;

            if (errors.Length > 1)
            {
                consoleOutput.text += $" (and {errors.Length - 1} more)";
            }
        }
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

    void OnScrollValueChanged(float value)
    {
        gutter.rectTransform.anchoredPosition = new Vector2(
            gutter.rectTransform.anchoredPosition.x,
            textBox.textComponent.rectTransform.anchoredPosition.y
        );
    }
}
