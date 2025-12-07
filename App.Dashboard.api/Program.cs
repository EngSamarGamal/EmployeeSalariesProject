using App.Dashboard.api.Extensions.ServiceCollectionExtensions;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddSerilogLogging();
builder.Services
    .AddHttpClient()
    .AddAppCors(builder.Configuration)
    .AddAppControllers()
    .AddAppSwagger()
    .AddAppLocalization()
    .AddAppDbAndIdentity(builder.Configuration)
    .AddAppAuthentication(builder.Configuration)
    .AddAppAuthorization()
    .AddAppServices()
    .AddAppValidation();



var app = builder.Build();



app.UseHttpsRedirection();

app.UseStaticFiles();

var loc = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(loc.Value);

app.UseRouting();
app.UseCors("_myAllowSpecificOrigins");

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<SerilogUserEnricherMiddleware>();

app.UseAppSwaggerUI();

app.MapControllers();

await app.RunAsync();


