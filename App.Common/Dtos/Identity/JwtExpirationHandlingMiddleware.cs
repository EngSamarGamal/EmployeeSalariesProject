using App.Common.Response;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

public class JwtExpirationHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public JwtExpirationHandlingMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);

            if (context.Response.HasStarted)
                return;

            // 401 من UseAuthorization
            if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
            {
                context.Response.ContentType = "application/json";
                var payload = ResponseModel.Error("Unauthorized. Please log in again.", 401);

                var json = JsonConvert.SerializeObject(payload, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

                await context.Response.WriteAsync(json);
                return;
            }

            if (context.Response.StatusCode == StatusCodes.Status500InternalServerError)
            {
                context.Response.ContentType = "application/json";
                var payload = ResponseModel.Error("Internal server error. Please try again later.", 500);

                var json = JsonConvert.SerializeObject(payload, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

                await context.Response.WriteAsync(json);
                return;
            }
        }
        catch
        {
            if (context.Response.HasStarted) throw;

            context.Response.Clear();
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            var payload = ResponseModel.Error("Internal server error. Please try again later.", 500);

            var json = JsonConvert.SerializeObject(payload, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            await context.Response.WriteAsync(json);
        }
    }
}
