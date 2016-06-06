using System;
using System.Linq;
using System.Text;

namespace Phaka.Azure.ResourceManager.Activities
{
    public class LanguageUtility
    {
        public static string ToEnglishList<T>()
        {
            return ToEnglishList(Enum.GetNames(typeof(T)));
        }

        public static string ToEnglishList(Type enumType)
        {
            if (enumType == null) throw new ArgumentNullException(nameof(enumType));
            return ToEnglishList(Enum.GetNames(enumType));
        }

        public static string ToEnglishList(string[] names)
        {
            if (names == null) throw new ArgumentNullException(nameof(names));
            var builder = new StringBuilder();
            for (int i = 0; i < names.Length-1; i++)
            {
                if (builder.Length > 0)
                {
                    builder.Append(", ");
                }
                builder.Append(names[i]);
            }

            if (names.Length > 1)
            {
                builder.Append(" and ");
            }

            if (names.Length > 0)
            {
                builder.Append(names[names.Length - 1]);
            }

            return builder.ToString();
        }
    }
}