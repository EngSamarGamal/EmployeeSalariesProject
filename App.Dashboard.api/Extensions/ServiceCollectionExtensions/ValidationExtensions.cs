
namespace App.Dashboard.api.Extensions.ServiceCollectionExtensions
{
    public static class ValidationExtensions
    {
        public static IServiceCollection AddAppValidation(this IServiceCollection services)
        {
            services
                .AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();

            services.AddValidatorsFromAssemblies(new[]
            {
                typeof(Program).Assembly,                
            });

            return services;
        }
    }
}
