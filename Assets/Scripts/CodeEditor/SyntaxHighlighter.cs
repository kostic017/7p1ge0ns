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

    public string Highlight(string code, Token[] tokens)
    {
        // note that iterations are from last to first
        for (int i = tokens.Length - 1; i >= 0; --i)
        {
            Token token = tokens[i];
            Color color = DetermineColor(token.Type);
            string colorValue = ColorUtility.ToHtmlStringRGB(color);

            string sufix = "</color>";
            string prefix = $"<color=#{colorValue}>";

            if (token.Error > -1)
            {
                sufix += "</u></link>";
                prefix = $"<link={token.Error}><u>" + prefix;
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

    Color DetermineColor(TokenType tokenType)
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
