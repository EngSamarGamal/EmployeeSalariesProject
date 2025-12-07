
namespace App.Dashboard.api.Extensions.ServiceCollectionExtensions;

public static class AddDbAndIdentityExtensions
{
    public static IServiceCollection AddAppDbAndIdentity(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                    config.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))
                   .EnableSensitiveDataLogging()
                   .LogTo(Console.WriteLine, LogLevel.Information));

        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddUserManager<UserManager<ApplicationUser>>()
            .AddRoles<IdentityRole>()
            .AddDefaultTokenProviders()
            .AddSignInManager<SignInManager<ApplicationUser>>();

        return services;
    }
}
