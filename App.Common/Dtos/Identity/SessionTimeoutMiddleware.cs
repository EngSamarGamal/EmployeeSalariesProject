using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Common.Dtos.Identity
{
    public class SessionTimeoutMiddleware
    {
        private readonly RequestDelegate _next;

        public SessionTimeoutMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Only check for authenticated routes (not Login/Account controllers)
            var path = context.Request.Path.Value?.ToLower();

            // Skip for login or static files
            if (path == "/" ||
                path.Contains("/lang/set") ||
                path.Contains("/accounts/login") ||
                path.Contains("/accounts/logout") ||
                path.Contains("/accounts/forgotpassword") ||
                path.Contains("/accounts/resetpassword") ||
                path.Contains("/accounts/register")||
                path.Contains("/home/error") ||
                path.Contains("/css") ||
                path.Contains("/js") ||
                path.Contains("/images") ||
                path.Contains("/lib") ||
                path.StartsWith("/api"))
                {
                await _next(context);
                return;
            }

            //// Check if the user is authenticated
            //if (context.User?.Identity?.IsAuthenticated == true)
            //{
            //    // Check if the session expired (no user data in session)
            //    var userSession = context.Session.GetString("UserSession");
            //    if (string.IsNullOrEmpty(userSession))
            //    {
            //        await context.SignOutAsync();
            //       context.Response.Redirect("/Accounts/Login?sessionExpired=true");                   
            //        return;
            //    }
            //}
            var userSession = context.Session.GetString("UserSession");
            if (string.IsNullOrEmpty(userSession))
            {
                await context.SignOutAsync();
                context.Response.Redirect("/Accounts/Login?sessionExpired=true");
                return;
            }
            await _next(context);
        }
    }
}
