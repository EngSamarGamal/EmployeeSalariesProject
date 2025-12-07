namespace App.Dashboard.api.Extensions.ServiceCollectionExtensions;

public static class UseMiddlewaresExtensions
{
    public static void UseAppMiddlewares(this WebApplication app)
    {
        app.UseCors("_myAllowSpecificOrigins");
        app.UseStaticFiles();
        app.UseHttpsRedirection();

        var supported = LocalizationHelper.GetSupportedCulturesAsString();
        var localizationOptions = new RequestLocalizationOptions()
            .SetDefaultCulture(supported[0])
            .AddSupportedCultures(supported);
        app.UseRequestLocalization(localizationOptions);

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseMiddleware<SerilogUserEnricherMiddleware>();
    }
}
