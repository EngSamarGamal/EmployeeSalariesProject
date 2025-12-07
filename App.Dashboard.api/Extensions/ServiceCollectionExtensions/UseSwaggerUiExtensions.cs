namespace App.Dashboard.api.Extensions.ServiceCollectionExtensions;

public static class UseSwaggerUiExtensions
{
    
    public static void UseAppSwaggerUI(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/LEXT/swagger.json", "LEXT APIs");

            c.SwaggerEndpoint("/swagger/Mobile/swagger.json", "Mobile APIs");

            c.RoutePrefix = "swagger";
        });
    }

}
