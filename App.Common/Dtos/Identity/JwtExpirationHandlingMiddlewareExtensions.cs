using Microsoft.AspNetCore.Builder;

namespace App.Common.Dtos.Identity
{
    public static class JwtMiddlewareExtensions
    {
        public static IApplicationBuilder UseJwtExpirationHandling(this IApplicationBuilder app)
        {
            return app.UseMiddleware<JwtExpirationHandlingMiddleware>();
        }
    }
}
