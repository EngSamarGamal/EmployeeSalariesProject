
namespace App.Dashboard.api.Extensions.ServiceCollectionExtensions;

public static class AddControllersExtensions
{
    public static IServiceCollection AddAppControllers(this IServiceCollection services)
    {
        services.AddControllers(o => o.Filters.Add<CultureFilter>())
                .AddNewtonsoftJson();
        return services;
    }
}
