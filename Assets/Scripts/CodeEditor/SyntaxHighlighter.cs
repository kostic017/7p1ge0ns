using UnityEngine;

public class SyntaxHighlighter : MonoBehaviour
{
    public Color normalColor;

    public Color boolColor;
    public Color numberColor;
    public Color stringColor;

    public Color keywordColor;
    public Color typeNameColor;
    public Color identifierColor;

    public Color commentColor;
    public Color blockCommentColor;

    public string Highlight(string code, Token[] tokens)
    {
        // note that iterations are from last to first
        for (int i = tokens.Length - 1; i >= 0; --i)
        {
            Color color = DetermineColor(tokens[i].Type);
            string colorValue = ColorUtility.ToHtmlStringRGB(color);
            code = code.Insert(tokens[i].EndIndex, "</color>")
                       .Insert(tokens[i].StartIndex - 1, $"<color=#{colorValue}>");
        }
        return code;
    }

    private Color DetermineColor(TokenType tokenType)
    {
        switch (tokenType)
        {
            case TokenType.IntConst:
            case TokenType.FloatConst:
                return numberColor;
            case TokenType.BoolConst:
                return boolColor;
            case TokenType.StringConst:
                return stringColor;
            case TokenType.Comment:
                return commentColor;
            case TokenType.BlockComment:
                return blockCommentColor;
            case TokenType.Identifier:
                return identifierColor;
            default:
                if (Scanner.types.ContainsValue(tokenType))
                {
                    return typeNameColor;
                }

                if (Scanner.keywords.ContainsValue(tokenType))
                {
                    return keywordColor;
                }

                return normalColor;
        }
    }
}
