using System.Linq;
using TMPro;
using UnityEngine;

public class EditorController : MonoBehaviour
{
    public TMP_Text gutter;
    public RectTransform popup;
    public TMP_Text consoleOutput;
    public TMP_InputField textBox;

    string prevCode;
    int hoverLinkIndex = -1;

    Canvas canvas;
    Scanner scanner;
    TMP_Text popupText;
    SyntaxHighlighter highlighter;

    void Start()
    {
        scanner = new Scanner(textBox.fontAsset.tabSize);
        
        canvas = GetComponentInParent<Canvas>();
        highlighter = GetComponent<SyntaxHighlighter>();
        popupText = popup.GetComponentInChildren<TMP_Text>();

        popup.gameObject.SetActive(false);
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
        UpdatePopup();

        string code = highlighter.StripTags(textBox.text);

        if (code == prevCode) return;
        prevCode = code;

        UpdateLineNumbers(code);

        consoleOutput.text = "";

        Token[] tokens = scanner.Scan(code);

        code = highlighter.Highlight(code, tokens);

        if (scanner.Errors.Count > 0)
        {
            consoleOutput.text = scanner.Errors[0].DetailedMessage();

            if (scanner.Errors.Count > 1)
            {
                consoleOutput.text += $" (and {scanner.Errors.Count - 1} more)";
            }
        }

        // inserting rich text while the player is typing used to result in caret being positioned inside rich text tags
        int caret = textBox.caretPosition;
        textBox.text = code;
        textBox.caretPosition = caret;
    }

    void UpdatePopup()
    {
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(textBox.textComponent, Input.mousePosition, canvas.worldCamera);

        // clear previous link selection if one existed
        if ((linkIndex == -1 && hoverLinkIndex != -1) || linkIndex != hoverLinkIndex)
        {
            hoverLinkIndex = -1;
            popup.gameObject.SetActive(false);
        }

        if (linkIndex != -1 && linkIndex != hoverLinkIndex)
        {
            hoverLinkIndex = linkIndex;
            TMP_LinkInfo errorInfo = textBox.textComponent.textInfo.linkInfo[linkIndex];
            RectTransformUtility.ScreenPointToWorldPointInRectangle(textBox.textComponent.rectTransform,
                Input.mousePosition, canvas.worldCamera, out Vector3 worldPointInRectangle);
            
            popup.position = worldPointInRectangle;
            popup.gameObject.SetActive(true);
            popupText.text = scanner.Errors[int.Parse(errorInfo.GetLinkID())].Message();
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
