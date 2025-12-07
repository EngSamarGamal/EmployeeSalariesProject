using App.Common.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Common.Localization
{
    public static class LocalizationHelper
    {

        public static List<CultureInfo> GetSupportedCultures()
        {
            var supportedCultures = new List<CultureInfo>();
            foreach (LanguageEnum cultureEnum in Enum.GetValues(typeof(LanguageEnum)))
            {
                CultureInfo culture = new CultureInfo(cultureEnum.ToString());
                supportedCultures.Add(culture);
            }
            return supportedCultures;
        }

        public static string[] GetSupportedCulturesAsString()
        {
            var values = Enum.GetValues(typeof(LanguageEnum));
            var supportedCultures = new string[values.Length];
            int index = 0;
            foreach (LanguageEnum lang in values)
            {
                supportedCultures[index++] = lang.ToString();
            }
            return supportedCultures;

        }

    }
}
