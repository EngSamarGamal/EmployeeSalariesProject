using App.Dashboard.Api.Extensions.Localization; // تأكد من وجود البروڤايدر المخصص
using System.Globalization;

namespace App.Dashboard.api.Extensions.ServiceCollectionExtensions
{
    public static class AddLocalizationExtensions
    {
        public static IServiceCollection AddAppLocalization(this IServiceCollection services)
        {
            services.AddLocalization();
            services.AddDistributedMemoryCache();
            services.AddMemoryCache();
            services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();
            services.AddSingleton(typeof(IStringLocalizer<>), typeof(StringLocalizer<>));
            services.AddSingleton<IStringLocalizer>(sp =>
            {
                var factory = sp.GetRequiredService<IStringLocalizerFactory>();
                return factory.Create(null);
            });

            services.AddHttpContextAccessor();

            var supported = new[] { new CultureInfo("en"), new CultureInfo("ar") };

            services.Configure<RequestLocalizationOptions>(opts =>
            {
                opts.DefaultRequestCulture = new RequestCulture("en");
                opts.SupportedCultures = supported;
                opts.SupportedUICultures = supported;

                opts.RequestCultureProviders = new IRequestCultureProvider[]
                {
                    new LanguageHeaderRequestCultureProvider(),
                    new QueryStringRequestCultureProvider(),
                    new CookieRequestCultureProvider()
                };
            });



            return services;
        }
    }
}
