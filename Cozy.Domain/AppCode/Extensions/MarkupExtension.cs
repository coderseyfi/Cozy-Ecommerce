using System.Linq;
using System.Text.RegularExpressions;

namespace Cozy.Domain.AppCode.Extensions
{
    public static partial class Extension
    {
        public static string ToPlainText(this string text)
        {
            text = Regex.Replace(text, "<[^>]*>", "");
            return text;
        }

        public static string ToEllipse(this string text, int len)
        {
            if (!string.IsNullOrWhiteSpace(text) && text.Length > len)
            {
                text = text.Substring(0, len);
            }
            return text;
        }

        public static string ToSlug(this string context)
        {
            if (string.IsNullOrWhiteSpace(context))
                return null;

            // c# & sql => csharp-and-sql

            context = context.Replace("Ü", "u").Replace("ü", "u")
                .Replace("İ", "i").Replace("I", "i").Replace("ı", "i")
                .Replace("Ş", "s").Replace("ş", "s")
                .Replace("Ö", "o").Replace("ö", "o")
                .Replace("Ç", "c").Replace("ç", "c")
                .Replace("Ğ", "g").Replace("ğ", "g")
                .Replace("Ə", "e").Replace("ə", "e")
                .Replace(" ", "-").Replace("?", "").Replace("/", "")
                .Replace("\\", "").Replace(".", "").Replace("'", "").Replace("#", "sharp").Replace("%", "")
                .Replace("*", "").Replace("!", "").Replace("@", "").Replace("+", "")
                .ToLower().Trim();

            context = Regex.Replace(context, @"\&+", "and");
            context = Regex.Replace(context, @"[^a-z0-9]+", "-");
            context = Regex.Replace(context, @"-+", "-");
            context = context.Trim('-');

            return context;
        }

        public static string ToJsonArray(this int[] array)
        {
            if (array == null || array.Length == 0)
            {
                return "[]";
            }

            return $"[{string.Join(",", array)}]";
        }

    }
}