using Kostic017.Pigeon;
using System.Text.RegularExpressions;
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

    public string Highlight(string code, SyntaxToken[] tokens)
    {
        // Iterations are from last to first. EOF token is skipped.
        for (int i = tokens.Length - 2; i >= 0; --i)
        {
            SyntaxToken token = tokens[i];
            Color color = DetermineColor(token.Type);
            string colorValue = ColorUtility.ToHtmlStringRGB(color);

            string sufix = "</color>";
            string prefix = $"<color=#{colorValue}>";
            code = code.Insert(tokens[i].EndIndex, sufix)
                       .Insert(tokens[i].StartIndex - 1, prefix);
        }
        return code;
    }

    public string StripTags(string code)
    {
        string s = code.Replace("</color>", "");
        s = Regex.Replace(s, "<color=#.+?>", string.Empty);
        return s;
    }

    Color DetermineColor(SyntaxTokenType tokenType)
    {
        switch (tokenType)
        {
            case SyntaxTokenType.IntLiteral:
            case SyntaxTokenType.FloatLiteral:
                return numberColor;
            case SyntaxTokenType.BoolLiteral:
                return boolColor;
            case SyntaxTokenType.StringLiteral:
                return stringColor;
            case SyntaxTokenType.Comment:
                return commentColor;
            case SyntaxTokenType.BlockComment:
                return blockCommentColor;
            case SyntaxTokenType.ID:
                return identifierColor;
            default:
                if (SyntaxFacts.Types.ContainsValue(tokenType))
                {
                    return typeNameColor;
                }

                if (SyntaxFacts.Keywords.ContainsValue(tokenType))
                {
                    return keywordColor;
                }

                return normalColor;
        }
    }
}
