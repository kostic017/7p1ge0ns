using System;
using System.ComponentModel;
using System.Reflection;

namespace Kostic017.Pigeon
{
    public static class SyntaxTokenPrettyPrint
    {
        public static string PrettyPrint(this SyntaxTokenType tokenType)
        {
            FieldInfo field = tokenType.GetType().GetField(tokenType.ToString());
            if (!(Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attr))
            {
                return tokenType.ToString();
            }
            return attr.Description;
        }
    }
}
