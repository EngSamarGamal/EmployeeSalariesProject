using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace App.Common.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .FirstOrDefault()?
                            .GetCustomAttribute<DisplayAttribute>()?
                            .Name ?? enumValue.ToString();
        }

        public static string GetDisplayName(this Enum value, string lang)
        {
            if (value == null)
                return string.Empty;

            var type = value.GetType();
            var enumValue = Convert.ToInt32(value);

            if (!Enum.IsDefined(type, enumValue))
                return string.Empty;

            var name = Enum.GetName(type, enumValue);
            if (name == null)
                return string.Empty;

            var field = type.GetField(name);
            if (field == null)
                return string.Empty;

            var attr = field.GetCustomAttribute<DisplayAttribute>();
            if (attr == null)
                return name;

            return lang == "ar"
                ? (attr.Description ?? attr.Name ?? name)
                : (attr.Name ?? name);
        }
    }
}
