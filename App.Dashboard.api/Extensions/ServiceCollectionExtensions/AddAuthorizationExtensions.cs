
namespace App.Dashboard.api.Extensions.ServiceCollectionExtensions;

public static class AddAuthorizationExtensions
{
    public static IServiceCollection AddAppAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy("SuperAdmin", p => p.RequireRole(RoleEnum.SuperAdmin.ToString()));
            options.AddPolicy("Admin", p => p.RequireRole(RoleEnum.Admin.ToString()));
            options.AddPolicy("DefaultAdmin", p => p.RequireRole(RoleEnum.DefaultAdmin.ToString()));
            options.AddPolicy("User", p => p.RequireRole(RoleEnum.User.ToString()));
        });

        return services;
    }
}
