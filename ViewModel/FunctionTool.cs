using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT008_QuanLyBanHang.ViewModel
{
    public static class FunctionTool
    {
        public static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder(capacity: normalizedString.Length);

            for (int i = 0; i < normalizedString.Length; i++)
            {
                char c = normalizedString[i];
                UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                Trace.WriteLine(c.ToString() + " " + unicodeCategory.ToString());
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    if (c == 'đ')
                        c = 'd';
                    if (c == 'Đ')
                        c = 'D';
                    stringBuilder.Append(c == 'đ' ? 'd' : c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public static bool CheckContains(string s1, string s2)
        {
            s1 = RemoveDiacritics(s1);
            s2 = RemoveDiacritics(s2);

            string[] words = s2.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            foreach (var word in words)
            {
                if (!s1.Contains(word, StringComparison.OrdinalIgnoreCase))
                    return false;
            }
            return true;
        }
    }
}
