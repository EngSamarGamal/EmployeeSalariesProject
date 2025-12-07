




using App.Application.Interfaces.Repositories.EmployeesSalaryRepo;
using App.Application.Services.EmployeeSalaries;
using App.Application.Services.EmployeeSalariesService;
using App.Infrastructure.Implementation.Repositories.EmployeeSalariesRepo;

namespace App.Dashboard.api.Extensions.ServiceCollectionExtensions;

public static class AddAppServicesExtensions
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddTransient<IUnitOfWork, UnitOfWork>();
        services.AddTransient<IResponseModel, ResponseModel>();
        services.AddMemoryCache();
        services.AddTransient<IFirebasePushNotification, FirebasePushNotification>();
     
        services.AddScoped<IFileManager, FileManager>();
        services.AddScoped<IImageService, ImageService>();

     


        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IEmployeeService, EmployeeService>();

        services.AddScoped<IEmployeeSalaryService, EmployeeSalaryService>();    
        services.AddScoped<IEmployeesSalaryRepository, EmployeeSalariesRepository>();    





		return services;
    }
    //public static async Task SeedDataAsync(IServiceProvider serviceProvider)
    //{
    //    using var scope = serviceProvider.CreateScope();

    //    var cityRepository = scope.ServiceProvider.GetRequiredService<ICityRepository>();
    //    var areaRepository = scope.ServiceProvider.GetRequiredService<IAreaRepository>();
    //    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

    //    await CitySeeds.SeedAsync(cityRepository, unitOfWork);

    //    await AreaSeeds.SeedCairoAsync(areaRepository, cityRepository, unitOfWork);
    //    await AreaSeeds.SeedGizaAsync(areaRepository, cityRepository, unitOfWork);
    //    await AreaSeeds.SeedAlexandriaAsync(areaRepository, cityRepository, unitOfWork);

    //}

}
