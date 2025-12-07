using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;

namespace App.Common.Enums
{
    public static class HelperLocalizer
    {
        private static readonly IStringLocalizer _localizer;
        public static string GetLocalizedDescription<TEnum>(TEnum value) where TEnum : Enum
        {
            var enumName = typeof(TEnum).Name; // e.g., "GenderEnum"
            var valueName = value.ToString(); // e.g., "General"
            var key = $"{enumName}.{valueName}";

            // Retrieve localized value from JSON
            return _localizer[key] ?? valueName;
        }
    }
}
