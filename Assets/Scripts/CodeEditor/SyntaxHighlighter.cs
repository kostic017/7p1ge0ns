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
        // note that iterations are from last to first
        for (int i = tokens.Length - 1; i >= 0; --i)
        {
            SyntaxToken token = tokens[i];
            Color color = DetermineColor(token.Type);
            string colorValue = ColorUtility.ToHtmlStringRGB(color);

            string sufix = "</color>";
            string prefix = $"<color=#{colorValue}>";

            if (token.ErrorIndex > -1)
            {
                sufix += "</u></link>";
                prefix = $"<link={token.ErrorIndex}><u>" + prefix;
            }

            code = code.Insert(tokens[i].EndIndex, sufix)
                       .Insert(tokens[i].StartIndex - 1, prefix);
        }
        return code;
    }

    public string StripTags(string code)
    {
        string s = code.Replace("</color>", "")
                       .Replace("</u></link>", "");
        s = Regex.Replace(s, "<color=#.+?>", string.Empty);
        s = Regex.Replace(s, @"<link=\d+?><u>", string.Empty);
        return s;
    }

    Color DetermineColor(SyntaxTokenType tokenType)
    {
        switch (tokenType)
        {
            case SyntaxTokenType.IntConst:
            case SyntaxTokenType.FloatConst:
                return numberColor;
            case SyntaxTokenType.BoolConst:
                return boolColor;
            case SyntaxTokenType.StringConst:
                return stringColor;
            case SyntaxTokenType.Comment:
                return commentColor;
            case SyntaxTokenType.BlockComment:
                return blockCommentColor;
            case SyntaxTokenType.ID:
                return identifierColor;
            default:
                if (Lexer.types.ContainsValue(tokenType))
                {
                    return typeNameColor;
                }

                if (Lexer.keywords.ContainsValue(tokenType))
                {
                    return keywordColor;
                }

                return normalColor;
        }
    }
}
