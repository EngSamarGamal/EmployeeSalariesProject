using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

namespace App.Api.Services
{
    public static class FireBaseConfigurationsService
    {
        public static IServiceCollection AddFireBaseConfigurations(this IServiceCollection services,
                                                                   IWebHostEnvironment webEnvironment)
        {
            var filePath = Path.Combine(webEnvironment.ContentRootPath, "FireBaseConfigurations.json");
            FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromFile(filePath)
            });

            return services;
        }
    }
}
