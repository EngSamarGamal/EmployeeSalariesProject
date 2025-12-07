using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Context;

public class SerilogUserEnricherMiddleware
{
    private readonly RequestDelegate _next;

    public SerilogUserEnricherMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            var ip = context.Connection.RemoteIpAddress?.ToString();

            var userId = context.User?.FindFirst(ClaimTypes.Name)?.Value
                       ?? context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var userRole = context.User?.FindFirst(ClaimTypes.Role)?.Value;

            using (LogContext.PushProperty("IpAddress", ip ?? "unknown"))
            using (LogContext.PushProperty("UserName", userId ?? "anonymous"))
            using (LogContext.PushProperty("UserRole", userRole ?? "none"))
            {
                await _next(context);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error enriching Serilog context.");
            await _next(context);
        }
    }

}
