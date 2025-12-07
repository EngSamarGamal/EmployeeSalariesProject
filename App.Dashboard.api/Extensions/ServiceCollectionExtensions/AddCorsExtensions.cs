namespace App.Dashboard.api.Extensions.ServiceCollectionExtensions
{
    public static class AddCorsExtensions
    {
        public static IServiceCollection AddAppCors(this IServiceCollection services, IConfiguration config)
        {
            var raw = config["Cors_Origins"] ?? string.Empty;
            var origins = raw.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            Console.WriteLine("CORS Origins: " + string.Join(", ", origins)); 

            services.AddCors(options =>
            {
                options.AddPolicy("_myAllowSpecificOrigins", policy =>
                {
                    //خليها كدا دلوقتى هبقى اظبتها
                    policy.WithOrigins(
                             "http://localhost:3000",
                             "http://localhost:3001",
                             "https://homes-dashboard.vercel.app",
                             "http://sbtechnology-001-site43.atempurl.com",
                             "https://sbtechnology-001-site43.atempurl.com",
							 "https://localhost:7201")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });

            return services;
        }
    }
}



    

