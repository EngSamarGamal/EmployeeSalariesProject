using System.Globalization;

public static class LanguageHelper
{
	public static bool IsEnglish =>
		CultureInfo.CurrentUICulture.TwoLetterISOLanguageName
		.Equals("en", StringComparison.OrdinalIgnoreCase);
}
