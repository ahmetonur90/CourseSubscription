using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace CourseSubscription.Core.Util
{
    public class Utilise
    {
        public static string ToJson(object param)
        {
            return JsonConvert.SerializeObject(param, Formatting.Indented);
        }

        public static string ConvertToENChar(string text)
        {
            return String.Join("", text.Normalize(NormalizationForm.FormD)
            .Where(c => char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)).Trim();
        }
    }
}
