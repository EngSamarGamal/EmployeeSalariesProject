using Microsoft.Extensions.Primitives;

namespace App.Dashboard.Api.Extensions.Localization
{
    public sealed class LanguageHeaderRequestCultureProvider : RequestCultureProvider
    {
        public const string HeaderName = "language"; // نفس الهيدر في Swagger

        public override Task<ProviderCultureResult?> DetermineProviderCultureResult(HttpContext httpContext)
        {
            if (!httpContext.Request.Headers.TryGetValue(HeaderName, out StringValues v) || StringValues.IsNullOrEmpty(v))
                return Task.FromResult<ProviderCultureResult?>(null);

            var raw = v.ToString().Trim().ToLowerInvariant();

            // طبّع القيم إلى en-US / ar-EG (مطابقة للـ SupportedCultures)
            var culture = raw switch
            {
                "ar" or "ar-eg" => "ar-EG",
                "en" or "en-us" => "en-US",
                _ => null
            };

            return Task.FromResult(culture is null ? null : new ProviderCultureResult(culture, culture));
        }
    }
}
